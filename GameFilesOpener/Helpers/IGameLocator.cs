using System;
using System.Collections.Generic;
using System.Text;

namespace GameFilesOpener.Helpers
{
    public class ShortcutInfo
    {
        public string Path { get; set; } = string.Empty;
        public bool IsUrl { get; set; }
        public bool IsLnk { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Arguments { get; set; } = string.Empty;
        public string IconLocation { get; set; } = string.Empty;
    }

    public interface IGameLocator
    {
        bool CanHandle(ShortcutInfo info);
        string GetGamePath(ShortcutInfo info);
    }
}