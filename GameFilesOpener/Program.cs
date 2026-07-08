// Program.cs
using GameFilesOpener.Helpers;
using System;
using System.Windows.Forms;

namespace GameFilesOpener
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0 && (args[0].EndsWith(".url", StringComparison.OrdinalIgnoreCase) || args[0].EndsWith(".lnk", StringComparison.OrdinalIgnoreCase)))
            {
                // Let the new GameLocator figure out what to do with the shortcut
                GameLocator.HandleShortcut(args[0]);
                return;
            }

            Application.Run(new MainForm());
        }
    }
}