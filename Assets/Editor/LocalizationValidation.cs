using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Localization;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Validates the dictionaries used by Resources.Load and blocks incomplete builds.
public sealed class LocalizationValidation : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    public void OnPreprocessBuild(BuildReport report) => Validate();

    [MenuItem("Tools/Localization/Validate translations")]
    public static void Validate()
    {
        var errors = new List<string>();
        var dictionaries = new Dictionary<string, Dictionary<string, string>>();
        foreach (string language in new[] { "ru", "en", "tr" })
        {
            var data = Resources.Load<LocalizationData>("Localization/" + language);
            var values = new Dictionary<string, string>();
            dictionaries.Add(language, values);
            if (data == null)
            {
                errors.Add("Missing dictionary: " + language);
                continue;
            }
            foreach (var entry in data.Entries)
            {
                if (string.IsNullOrWhiteSpace(entry.ID) || string.IsNullOrWhiteSpace(entry.Text))
                {
                    errors.Add(language + ": empty key or text: " + entry.ID);
                    continue;
                }
                if (values.ContainsKey(entry.ID)) errors.Add(language + ": duplicate key: " + entry.ID);
                else values.Add(entry.ID, entry.Text);
                try
                {
                    var indices = Placeholders(entry.Text);
                    int count = indices.Count == 0 ? 0 : indices.Max() + 1;
                    if (count > 100) throw new FormatException("Too many arguments");
                    string.Format(CultureInfo.InvariantCulture, entry.Text,
                        Enumerable.Repeat<object>(1, count).ToArray());
                }
                catch (FormatException exception)
                {
                    errors.Add(language + ": invalid format " + entry.ID + ": " + exception.Message);
                }
            }
        }
        var allKeys = new HashSet<string>(dictionaries.Values.SelectMany(d => d.Keys));
        foreach (var key in allKeys)
        {
            HashSet<int> expected = null;
            foreach (var pair in dictionaries)
            {
                if (!pair.Value.TryGetValue(key, out var value))
                {
                    errors.Add(pair.Key + ": missing key " + key);
                    continue;
                }
                var indices = Placeholders(value);
                if (expected == null) expected = indices;
                else if (!expected.SetEquals(indices)) errors.Add(pair.Key + ": mismatched arguments: " + key);
            }
        }
        // Check stored scene/prefab bindings, tutorial keys, and explicit code references.
        foreach (var path in Directory.GetFiles("Assets/MedivalTycoon", "*", SearchOption.AllDirectories))
        {
            if (!path.EndsWith(".unity") && !path.EndsWith(".prefab") && !path.EndsWith(".cs")) continue;
            string text = File.ReadAllText(path);
            var matches = path.EndsWith(".cs")
                ? Regex.Matches(text, "(?:Get|Format|Bind)\\([^\\r\\n]*?\"([a-z][A-Za-z0-9_.]+)\"")
                : Regex.Matches(text, @"(?m)^\s*(?:_key|MessageKey):\s*([A-Za-z0-9_.]+)");
            foreach (Match match in matches)
                if (!allKeys.Contains(match.Groups[1].Value)) errors.Add(path + ": unknown key " + match.Groups[1].Value);
        }
        if (errors.Count > 0) throw new BuildFailedException(string.Join("\n", errors));
        Debug.Log("Localization validated: " + allKeys.Count + " keys in ru/en/tr.");
    }

    private static HashSet<int> Placeholders(string text)
    {
        // Strip escaped braces before extracting composite-format argument indices.
        text = text.Replace("{{", "").Replace("}}", "");
        return new HashSet<int>(Regex.Matches(text, @"\{(\d+)(?:[,}:])")
            .Cast<Match>().Select(m => int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture)));
    }
}
