using Microsoft.Win32;
using System;
using System.Text.RegularExpressions;

namespace GameFilesOpener.Helpers
{
    public class GogGameLocator : IGameLocator
    {
        public bool CanHandle(ShortcutInfo info) => info.IsLnk && info.Arguments.Contains("gameId=");

        public string GetGamePath(ShortcutInfo info)
        {
            // Literally that simple: Extract the GameId from the arguments
            // In a GOG shortcut, there is actually a game path argument, but it's not always reliable and if any GOG game is moved, the path will be invalid. So instead, we extract the GameId and use that to find the installation path in the Registry.
            var match = Regex.Match(info.Arguments, @"gameId=(.*?)(?:\s|$)");
            if (!match.Success)
            {
                throw new Exception("Could not extract Game ID from GOG shortcut.");
            }

            string gameId = match.Groups[1].Value;

            // Navigate to the Registry to find the final game path
            string registryKey = $@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\GOG.com\Games\{gameId}";
            string? gamePath = Registry.GetValue(registryKey, "path", null)?.ToString();

            if (string.IsNullOrEmpty(gamePath))
            {
                throw new Exception("GOG game installation path not found in registry.");
            }

            return gamePath;
        }
    }
}