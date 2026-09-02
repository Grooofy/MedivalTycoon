using UnityEngine;
using System;
using System.Collections.Generic;

namespace Localization
{
    [Serializable]
    public struct LocalizationEntry
    {
        public string ID;
        [TextArea(3, 10)] public string Text;
    }

    [CreateAssetMenu(fileName = "LocalizationData", menuName = "Settings/Localization")]
    public class LocalizationData : ScriptableObject
    {
        public List<LocalizationEntry> Entries = new List<LocalizationEntry>();

        public Dictionary<string, string> GetDictionary()
        {
            var dict = new Dictionary<string, string>();
            foreach (var entry in Entries)
            {
                if (!string.IsNullOrEmpty(entry.ID) && !dict.ContainsKey(entry.ID))
                {
                    dict.Add(entry.ID, entry.Text);
                }
            }
            return dict;
        }
    }
}
