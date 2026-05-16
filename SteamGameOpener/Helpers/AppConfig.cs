using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace SteamGameOpener.Helpers
{
    public class AppConfig
    {
        // If you want a thorough and detailed explanation of how this whole program works, take a look at the README.md file in the repo.
        // It contains a detailed behind-the-scenes summary of the very moment you click the context menu, to the moment the game files are opened.
        public string SteamRegex { get; set; } = @"URL=steam://(?:rungameid|run)/(\d+)";
        public string LibraryFoldersRegex { get; set; } = @"""path""\s+""([^""]+)""";
        public string SteamPathRegistry { get; set; } = @"HKEY_CURRENT_USER\Software\Valve\Steam";
        public string SteamDefaultPath { get; set; } = @"C:\Program Files (x86)\Steam";

        // Below code ensures only one instance of AppConfig is used throughout the application. If the instance is null, it will create a new one with the default values.
        // This is for future proofing purposes. This makes it possible for values to be loaded without changing the rest of the code.
        private static AppConfig? _instance;
        public static AppConfig Instance => _instance ?? new AppConfig();
    }
}
