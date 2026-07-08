using Microsoft.Win32;

namespace GameFilesOpener.Helpers
{
    public static class RegistryHelper
    {
        // The names and values used in this class are crucial, as that's the only way Windows recognizes them as valid context menu items.
        // Changing any of these values will result in the context menu item not appearing at all.
        public static string MenuName => "GameShortcutOpener";
        public static string MenuDisplayText => "Open Game Files"; // Updated since it supports more than just Steam now!

        // Keys for .url shortcuts (Steam, Epic Games, Amazon)
        public static string GetUrlInstallKey() => $@"Software\Classes\InternetShortcut\shell\{MenuName}";
        public static string GetUrlBackupKey() => $@"Software\Classes\SystemFileAssociations\.url\shell\{MenuName}";

        // Key for .lnk shortcuts (GOG, Xbox, Standard Windows Shortcuts)
        public static string GetLnkInstallKey() => $@"Software\Classes\lnkfile\shell\{MenuName}";

        public static bool IsInstalled() =>
            Registry.CurrentUser.OpenSubKey(GetUrlInstallKey()) != null ||
            Registry.CurrentUser.OpenSubKey(GetUrlBackupKey()) != null ||
            Registry.CurrentUser.OpenSubKey(GetLnkInstallKey()) != null;

        public static void Register(string exePath)
        {
            // Register for standard .url web shortcuts
            RegisterKey(GetUrlInstallKey(), exePath);

            // Register backup for .url
            RegisterKey(GetUrlBackupKey(), exePath);

            // Register for standard .lnk desktop shortcuts
            RegisterKey(GetLnkInstallKey(), exePath);
        }

        private static void RegisterKey(string keyPath, string exePath)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(keyPath))
            {
                key.SetValue("", MenuDisplayText);
                key.SetValue("Icon", $"\"{exePath}\",0");

                // The MUIVerb, or, the Multilingual User Interface Verb, is a Windows registry entry that defines the display name of a custom context menu item.
                // This is another crucial value that can't be changed. It's the only way Windows will recognize it as a valid key.
                key.SetValue("MUIVerb", MenuDisplayText);
            }

            // The "command" registry key is what actually executes when the context menu item is clicked.
            using (var cmd = Registry.CurrentUser.CreateSubKey(keyPath + @"\command"))
            {
                cmd.SetValue("", $"\"{exePath}\" \"%1\"");
            }
        }

        public static void Unregister()
        {
            Registry.CurrentUser.DeleteSubKeyTree(GetUrlInstallKey(), false);
            Registry.CurrentUser.DeleteSubKeyTree(GetUrlBackupKey(), false);
            Registry.CurrentUser.DeleteSubKeyTree(GetLnkInstallKey(), false);
        }
    }
}