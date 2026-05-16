using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SteamGameOpener.Helpers
{
    public static class SteamGameLocator
    {
        public static void OpenGameFiles(string urlFilePath)
        {
            try
            {
                string urlFileContent = File.ReadAllText(urlFilePath);
                var regexMatch = Regex.Match(urlFileContent, AppConfig.Instance.SteamRegex, RegexOptions.IgnoreCase);

                if (!regexMatch.Success)
                {
                    MessageBox.Show("Not a valid Steam game shortcut.", "Steam Game Opener", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string appId = regexMatch.Groups[1].Value;
                string? gamePath = FindGameInstallationPath(appId);

                if (!string.IsNullOrEmpty(gamePath) && Directory.Exists(gamePath))
                {
                    Process.Start("explorer.exe", $"\"{gamePath}\"");
                }

                else
                {
                    //MessageBox.Show($"Game {appId} not found or not installed.", "Steam Game Opener", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MessageBox.Show(gamePath + "\r\n;;;;\r\n");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Steam Game Opener", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string? FindGameInstallationPath(string appId)
        {
            var appConfig = AppConfig.Instance;
            string steamPath = Registry.GetValue(appConfig.SteamPathRegistry, "SteamPath", appConfig.SteamDefaultPath)?.ToString() ?? appConfig.SteamDefaultPath;

            steamPath = steamPath.Replace('/', '\\');

            string steamLibraryFolders = Path.Combine(steamPath, "steamapps", "libraryfolders.vdf");

            if (!File.Exists(steamLibraryFolders))
            {
                return null;
            }

            var steamLibraries = new[] { steamPath }.Concat(Regex.Matches(File.ReadAllText(steamLibraryFolders), appConfig.LibraryFoldersRegex).Cast<Match>().Select(m => m.Groups[1].Value.Replace("\\\\", "\\"))).Distinct().ToList();

            foreach (var library in steamLibraries)
            {
                string acfManifestFile = Path.Combine(library, "steamapps", $"appmanifest_{appId}.acf");

                if (!File.Exists(acfManifestFile))
                {
                    continue;
                }

                string acfContent = File.ReadAllText(acfManifestFile);
                var directoryMatch = Regex.Match(acfContent, @"""installdir""\s+""([^""]+)""");

                if (directoryMatch.Success)
                {
                    string finalSteamGamePath = Path.Combine(library, "steamapps", "common", directoryMatch.Groups[1].Value);

                    if (Directory.Exists(finalSteamGamePath))
                    {
                        return finalSteamGamePath;
                    }
                }
            }

            return null;
        }
    }
}