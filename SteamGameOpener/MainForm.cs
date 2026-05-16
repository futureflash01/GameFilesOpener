using SteamGameOpener.Helpers;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamGameOpener
{
    public partial class MainForm : Form
    {
        // This is the path where the program's EXE will be stored. It's in the Local AppData (%LOCALAPPDATA%) folder, so it won't require admin privileges to install or uninstall.
        private readonly string ApplicationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SteamGameOpener");
        private readonly string EXEPath;

        public MainForm()
        {
            InitializeComponent();
            EXEPath = Path.Combine(ApplicationPath, "SteamGameOpener.exe");
        }

        private void CheckInstallStatus()
        {
            // This boolean is declared here to ensure that the registry is read every time we check the status, rather than relying on a cached value sotred in the Singleton instance
            bool isInstalled = RegistryHelper.IsInstalled();

            actionButton.Text = isInstalled ? "Uninstall" : "Install";
            actionButton.BackColor = isInstalled ? Color.IndianRed : Color.MediumSeaGreen;
            actionButton.ForeColor = Color.White;
        }

        private void Install()
        {
            // This method is pretty self-explanatory. It creates the necessary folders, copies the EXE to the AppData/Local (%LOCALAPPDATA%) folder, then registers the context menu item in the registry.
            try
            {
                Directory.CreateDirectory(ApplicationPath);
                string currentPath = Application.ExecutablePath;

                if (!currentPath.Equals(EXEPath, StringComparison.OrdinalIgnoreCase))
                {
                    File.Copy(currentPath, EXEPath, true);
                }

                RegistryHelper.Register(EXEPath);

                statusLabel.ForeColor = Color.ForestGreen;
                statusLabel.Text = "Installed successfully! Right-click any Steam desktop shortcut and click 'Open Steam Game Files'";
                SystemSounds.Beep.Play();
            }

            catch (Exception ex)
            {
                statusLabel.ForeColor = Color.IndianRed;
                statusLabel.Text = "Installation Failed:\r\n" + ex.Message;
                SystemSounds.Beep.Play();
            }
        }

        private void Uninstall()
        {
            // Unregistering the context menu before deleting the files, just in case something goes wrong during the process.
            // This way, the user won't have a broken context menu item that does nothing when clicked.
            try
            {
                RegistryHelper.Unregister();

                if (File.Exists(EXEPath))
                {
                    File.Delete(EXEPath);
                }

                if (Directory.Exists(ApplicationPath))
                {
                    Directory.Delete(ApplicationPath, true);
                }

                statusLabel.ForeColor = Color.ForestGreen;
                statusLabel.Text = "Uninstalled successfully! All the program's files have been deleted.";
                SystemSounds.Beep.Play();
            }

            catch (Exception ex)
            {
                statusLabel.ForeColor = Color.IndianRed;
                statusLabel.Text = "Uninstallation Failed:\r\n" + ex.Message;
                SystemSounds.Beep.Play();
            }
        }

        private void actionButton_Click(object sender, EventArgs e)
        {
            // Very stupid and lazy way handle install/uninstall logic, but it works so I'm just gonna keep it.
            // And plus, the CheckInstallStatus() method does 90% of the work anyway.
            if (actionButton.Text == "Install")
            {
                Install();
            }

            else
            {
                Uninstall();
            }

            CheckInstallStatus();
        }

        private void madeByLabel_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = "https://youtube.com/@FutureFlash", UseShellExecute = true });
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Could I have cleared the status label in the designer? Yes.
            // But did I? No. So deal with it.
            statusLabel.Text = "";
            CheckInstallStatus();
        }
    }
}