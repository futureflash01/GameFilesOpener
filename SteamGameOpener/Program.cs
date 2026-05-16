using SteamGameOpener.Helpers;
using System;
using System.Windows.Forms;

namespace SteamGameOpener
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0 && args[0].EndsWith(".url", StringComparison.OrdinalIgnoreCase))
            {
                SteamGameLocator.OpenGameFiles(args[0]);
                return;
            }

            Application.Run(new MainForm());
        }
    }
}