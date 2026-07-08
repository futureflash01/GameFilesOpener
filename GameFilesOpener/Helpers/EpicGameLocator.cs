using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace GameFilesOpener.Helpers
{
    public class EpicGameLocator : IGameLocator
    {
        public bool CanHandle(ShortcutInfo info) => info.IsUrl && info.Content.Contains("com.epicgames.launcher://");

        public string GetGamePath(ShortcutInfo info)
        {
            // Epic Games is the most annoying one to deal with. It's tedious and could theoritically take the longest, deedipng on how many games are installed.
            // But basically here is what a shortcut looks like. In this case, the Fall Guys shortcut: 'com.epicgames.launcher://apps/50118b7f954e450f8823df1614b24e80%3A38ec4849ea4f4de6aa7b6fb0f2d278e1%3A0a2d9f6403244d12969e11da6713137b?action=launch&silent=true'
            // Anything ast the 'com.epicgames.launcher://apps/' and up until the first '%3A' character is the Game ID. Which in this case would be '50118b7f954e450f8823df1614b24e80'
            var match = Regex.Match(info.Content, @"apps/(.*?)%3A", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                throw new Exception("Could not extract Game ID from Epic Games shortcut.");
            }

            string catalogNamespace = match.Groups[1].Value;

            string registryKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Epic Games\EpicGamesLauncher";
            string? appDataPath = Registry.GetValue(registryKey, "AppDataPath", null)?.ToString();

            if (string.IsNullOrEmpty(appDataPath) || !Directory.Exists(appDataPath))
            {
                throw new DirectoryNotFoundException("Epic Games Launcher data path not found in registry.");
            }

            string manifestsPath = Path.Combine(appDataPath, "Manifests");
            if (!Directory.Exists(manifestsPath)) throw new DirectoryNotFoundException("Epic Manifests folder missing.");

            foreach (string file in Directory.GetFiles(manifestsPath, "*.item"))
            {
                string jsonContent = File.ReadAllText(file);

                if (jsonContent.Contains($"\"CatalogNamespace\": \"{catalogNamespace}\""))
                {
                    var pathMatch = Regex.Match(jsonContent, @"\""ManifestLocation\"":\s*\""([^\""]+)\""");
                    if (pathMatch.Success)
                    {
                        // On top of Epic Games being a massive back pain, the actual given game path has both forward and backward slashes and just looks like a mess
                        // Escaping this was a nightmare, but I got around it by just replacing all forward slashes with backward slashes, and then replacing any double backward slashes with a single backward slash. It works, but it's not pretty.
                        string rawPath = pathMatch.Groups[1].Value;
                        return rawPath.Replace(".egstore", "").Replace("/", "\\").Replace("\\\\", "\\");
                    }
                }
            }

            throw new DirectoryNotFoundException("Game installation not found in Epic manifests.");
        }
    }
}