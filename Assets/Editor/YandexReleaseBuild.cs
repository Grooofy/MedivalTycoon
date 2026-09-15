using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;

public static class YandexReleaseBuild
{
    // CLI: -batchmode -buildTarget WebGL -executeMethod YandexReleaseBuild.Build
    public static void Build()
    {
        string output = Environment.GetEnvironmentVariable("MEDIVAL_WEBGL_OUTPUT");
        if (string.IsNullOrEmpty(output))
            throw new InvalidOperationException("Set MEDIVAL_WEBGL_OUTPUT to a new output directory.");
        if (Directory.Exists(output) && Directory.EnumerateFileSystemEntries(output).Any())
            throw new InvalidOperationException("Build output directory must be empty.");
        var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = output,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        });
        if (report.summary.result != BuildResult.Succeeded)
            throw new InvalidOperationException("WebGL build failed: " + report.summary.result);
        File.WriteAllText(output + ".summary.txt", "Result: " + report.summary.result +
            "\nBytes: " + report.summary.totalSize + "\nWarnings: " + report.summary.totalWarnings +
            "\nErrors: " + report.summary.totalErrors + "\nDuration: " + report.summary.totalTime);
    }
}
