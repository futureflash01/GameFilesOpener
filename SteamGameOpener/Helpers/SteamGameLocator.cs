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
        // This is where part of the magic happens. This somewhat complex method takes in the file path of the URL shortcut, reads the content, extracts the AppID using a regex, then finds the installation path of the game using the AppID and opens that folder.
        public static void OpenGameFiles(string urlFilePath)
        {
            try
            {
                // Read the content of the .url file and extract the AppID using the regex defined in AppConfig.
                string urlFileContent = File.ReadAllText(urlFilePath);
                var regexMatch = Regex.Match(urlFileContent, AppConfig.Instance.SteamRegex, RegexOptions.IgnoreCase);

                if (!regexMatch.Success)
                {
                    // This error isn't really an error, but more of a user feedback message. If the user opens an Epic Games Launcher shortcut, this error will also appear
                    MessageBox.Show("This shortcut is either invalid or it belongs to another launcher, such as Epic Games.", "Invalid Shortcut!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        // This is where the rest of the magic happens. I mean after all, it does take two to tango!
        private static string? FindGameInstallationPath(string appId)
        {
            var appConfig = AppConfig.Instance;
            string steamPath = Registry.GetValue(appConfig.SteamPathRegistry, "SteamPath", appConfig.SteamDefaultPath)?.ToString() ?? appConfig.SteamDefaultPath;

            // String formatting to ensure the path can be read correctly
            steamPath = steamPath.Replace('/', '\\');

            string steamLibraryFolders = Path.Combine(steamPath, "steamapps", "libraryfolders.vdf");

            if (!File.Exists(steamLibraryFolders))
            {
                return null;
            }

            // Read the 'libraryfolders.vdf' file to get ALL Steam Library folder paths, including the default one, and check each for the presence of the game's 'appmanifest' file.
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
                    // Combine the library path with the 'common' folder and the game's installation directory to get the final path to the game's files.
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