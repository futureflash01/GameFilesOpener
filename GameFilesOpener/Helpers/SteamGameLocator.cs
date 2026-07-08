using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace GameFilesOpener.Helpers
{
    public class SteamGameLocator : IGameLocator
    {
        // Constants moved from the old AppConfig
        private const string SteamRegex = @"URL=steam://(?:rungameid|run)/(\d+)";
        private const string LibraryFoldersRegex = @"""path""\s+""([^""]+)""";
        private const string SteamPathRegistry = @"HKEY_CURRENT_USER\Software\Valve\Steam";
        private const string SteamDefaultPath = @"C:\Program Files (x86)\Steam";

        public bool CanHandle(ShortcutInfo info) => info.IsUrl && info.Content.Contains("steam://");

        // This is where part of the magic happens. This somewhat complex method takes in the file path of the URL shortcut, reads the content, extracts the AppID using a regex, then finds the installation path of the game using the AppID and opens that folder.
        public string GetGamePath(ShortcutInfo info)
        {
            var regexMatch = Regex.Match(info.Content, SteamRegex, RegexOptions.IgnoreCase);

            if (!regexMatch.Success)
            {
                throw new Exception("Could not extract AppID from Steam shortcut.");
            }

            string appId = regexMatch.Groups[1].Value;
            string? steamPath = Registry.GetValue(SteamPathRegistry, "SteamPath", SteamDefaultPath)?.ToString();

            if (string.IsNullOrEmpty(steamPath) || !Directory.Exists(steamPath))
            {
                throw new DirectoryNotFoundException("Steam installation not found.");
            }

            string steamLibraryFolders = Path.Combine(steamPath, "steamapps", "libraryfolders.vdf");

            if (!File.Exists(steamLibraryFolders))
            {
                throw new FileNotFoundException("Steam libraryfolders.vdf not found.");
            }

            // Read the 'libraryfolders.vdf' file to get ALL Steam Library folder paths, including the default one, and check each for the presence of the game's 'appmanifest' file.
            var steamLibraries = new[] { steamPath }.Concat(Regex.Matches(File.ReadAllText(steamLibraryFolders), LibraryFoldersRegex).Cast<Match>().Select(m => m.Groups[1].Value.Replace("\\\\", "\\"))).Distinct().ToList();

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
                    return finalSteamGamePath;
                }
            }

            throw new DirectoryNotFoundException("Could not locate game in Steam libraries.");
        }
    }
}