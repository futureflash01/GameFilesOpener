using Microsoft.Win32;
using SteamGameOpener.Helpers;

namespace SteamGameOpener.Helpers
{
    public static class RegistryHelper
    {
        // The names and values used in this class are crucial, as that's the only way Windows recognizes them as valid context menu items.
        // Changing any of these values will result in the context menu item not appearing at all.
        public static string MenuName => "SteamGameOpener";
        public static string MenuDisplayText => "Open Steam Game Files";

        public static string GetInstallKey() => $@"Software\Classes\InternetShortcut\shell\{MenuName}";
        // Adding a backup entry ensures that the context menu item appears consistently across different Windows versions.
        public static string GetBackupKey() => $@"Software\Classes\SystemFileAssociations\.url\shell\{MenuName}";

        public static bool IsInstalled() => Registry.CurrentUser.OpenSubKey(GetInstallKey()) != null || Registry.CurrentUser.OpenSubKey(GetBackupKey()) != null;

        public static void Register(string exePath)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(GetInstallKey()))
            {
                key.SetValue("", MenuDisplayText);
                key.SetValue("Icon", $"\"{exePath}\",0");
                // The MUIVerb, or, the Multilingual User Interface Verb, is a Windows registry entry that defines the display name of a custom context menu item.
                // This is another crucial value that can't be changed. It's the only way Windows will recognize it as a valid key.
                key.SetValue("MUIVerb", MenuDisplayText);
            }

            // The "command" registry key is what actually executes when the context menu item is clicked.
            // Since Steam desktop shortcuts are '.url' files and not actual desktop shortcuts ('.lnk' files), we need to pass in the URL of the game as an argument to our application
            using (var cmd = Registry.CurrentUser.CreateSubKey(GetInstallKey() + @"\command"))
            {
                cmd.SetValue("", $"\"{exePath}\" \"%1\"");
            }

            using (var key = Registry.CurrentUser.CreateSubKey(GetBackupKey()))
            {
                key.SetValue("", MenuDisplayText);
                key.SetValue("MUIVerb", MenuDisplayText);
            }

            using (var cmd = Registry.CurrentUser.CreateSubKey(GetBackupKey() + @"\command"))
            {
                cmd.SetValue("", $"\"{exePath}\" \"%1\"");
            }
        }

        public static void Unregister()
        {
            Registry.CurrentUser.DeleteSubKeyTree(GetInstallKey(), false);
            Registry.CurrentUser.DeleteSubKeyTree(GetBackupKey(), false);
        }
    }
}