using GameFilesOpener.Helpers;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameFilesOpener
{
    public partial class MainForm : Form
    {
        // This is the path where the program's EXE will be stored. It's in the Local AppData (%LOCALAPPDATA%) folder, so it won't require admin privileges to install or uninstall.
        private readonly string ApplicationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GameFilesOpener");
        private readonly string EXEPath;

        public MainForm()
        {
            InitializeComponent();
            EXEPath = Path.Combine(ApplicationPath, "GameFilesOpener.exe");
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
            try
            {
                // Create the LocalAppData directory if it doesn't exist
                if (!Directory.Exists(ApplicationPath))
                {
                    Directory.CreateDirectory(ApplicationPath);
                }

                // Copy the main '.exe' file to the LocalAppData folder
                string currentExe = Process.GetCurrentProcess().MainModule.FileName;
                File.Copy(currentExe, EXEPath, true);

                // Extract the embedded SQLite DLL and the 'Microsoft.Data.Sqlite' NuGet package's main DLL file into that same folder
                // This part is only used for the Amazon Games launcher, as it uses an SQLite database to store information about the game library
                string sqliteMicrosoftDataPath = Path.Combine(ApplicationPath, "Microsoft.Data.Sqlite.dll");
                string sqliteMainPath = Path.Combine(ApplicationPath, "e_sqlite3.dll");

                File.WriteAllBytes(sqliteMicrosoftDataPath, Properties.Resources.Microsoft_Data_Sqlite);
                File.WriteAllBytes(sqliteMainPath, Properties.Resources.e_sqlite3);

                // Register the context menus
                RegistryHelper.Register(EXEPath);

                statusLabel.ForeColor = Color.ForestGreen;
                statusLabel.Text = "Installation complete! Context menus are ready. You can safely delete this downloaded file, or keep it to use as an uninstaller.";
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
            try
            {
                // Unregistering the context menu before deleting the files, just in case something goes wrong during the process.
                // This way, the user won't have a broken context menu item that does nothing when clicked.
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
                statusLabel.Text = "Uninstalled successfully! All files and context menus have been deleted and unregistered!";
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