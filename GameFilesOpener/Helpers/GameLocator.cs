using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using File = System.IO.File;

namespace GameFilesOpener.Helpers
{
    public static class GameLocator
    {
        private static readonly List<IGameLocator> Locators = new List<IGameLocator>
        {
            new SteamGameLocator(),
            new EpicGameLocator(),
            new AmazonGameLocator(),
            new GogGameLocator(),
            new XboxGameLocator()
        };

        public static void HandleShortcut(string shortcutPath)
        {
            try
            {
                var info = new ShortcutInfo { Path = shortcutPath };

                if (shortcutPath.EndsWith(".url", StringComparison.OrdinalIgnoreCase))
                {
                    info.IsUrl = true;
                    info.Content = File.ReadAllText(shortcutPath);
                }
                else if (shortcutPath.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase))
                {
                    info.IsLnk = true;
                    // Use Windows Script Host to read the '.lnk' file. This is required because '.lnk' files are binary and cannot be read as text
                    WshShell shell = new WshShell();
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

                    info.Arguments = shortcut.Arguments ?? "";
                    info.IconLocation = shortcut.IconLocation ?? "";
                }
                else
                {
                    MessageBox.Show("This shortcut is invalid, unsupported, or a standard shortcut unassociated with a specific game launcher.", "Unsupported Desktop Shortcut", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Polymorphism in action: Ask each locator if they support this shortcut.
                foreach (var locator in Locators)
                {
                    if (locator.CanHandle(info))
                    {
                        string gamePath = locator.GetGamePath(info);

                        if (!string.IsNullOrEmpty(gamePath) && Directory.Exists(gamePath))
                        {
                            OpenFolder(gamePath);
                            return;
                        }

                        MessageBox.Show("Game path found, but the directory no longer exists.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                MessageBox.Show("This shortcut is invalid, unsupported, or a standard shortcut unassociated with a specific game launcher.", "Unsupported Desktop Shortcut", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error determining shortcut type: {ex.Message}", "Game Files Opener", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void OpenFolder(string path)
        {
            Process.Start("explorer.exe", $"\"{path}\"");
        }
    }
}