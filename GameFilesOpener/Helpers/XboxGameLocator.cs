using System;

namespace GameFilesOpener.Helpers
{
    public class XboxGameLocator : IGameLocator
    {
        public bool CanHandle(ShortcutInfo info) => info.IsLnk && info.IconLocation.Contains("Content\\", StringComparison.OrdinalIgnoreCase);

        public string GetGamePath(ShortcutInfo info)
        {
            // WARNING: Very boring and long explanation ahead. TL;DR: Xbox games are a pain to locate, and reading a certain registry key is not reliable, leading me to reverse engineer the '.lnk' file itself
            // For Xbox games, there was really no good way to find a libray/manifests file that contains the install directories. And when I found some, Xbox itself is unreliable and sometimes installs the game in 2 drives at the same time... I wish I was joking...
            // The only reliable way I could find was to read the 'iconLocation' property from the .lnk file itself.
            // The 'iconLocation' contains the path to the shortcut's icon (e.g. E:\XboxGames\Asphalt Legends\Content\Logo.ico)
            // We just need to extract everything up to the 'Content\' folder, as that is where the game is installed. The shortcut's icon is always located in the 'Content' folder, so we can reliably use that to find the game installation path.

            int index = info.IconLocation.IndexOf("Content\\", StringComparison.OrdinalIgnoreCase);

            if (index > 0)
            {
                // The '+7' is the length of the word 'Content', as it has seven letters. Since we're using String.Substring(), we have to pass an index.
                return info.IconLocation.Substring(0, index + 7);
            }

            throw new Exception("Could not parse Xbox game path from shortcut icon.");
        }
    }
}