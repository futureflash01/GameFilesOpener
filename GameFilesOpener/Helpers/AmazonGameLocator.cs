using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace GameFilesOpener.Helpers
{
    public class AmazonGameLocator : IGameLocator
    {
        public bool CanHandle(ShortcutInfo info) => info.IsUrl && info.Content.Contains("amazon-games://");

        public string GetGamePath(ShortcutInfo info)
        {
            SQLitePCL.Batteries.Init();

            var match = Regex.Match(info.Content, @"amazon-games://play/([^\r\n\s]+)", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                throw new Exception("Could not extract Game ID from Amazon shortcut.");
            }

            string gameId = match.Groups[1].Value;

            // Navigate to the SQLite DB. From my testing, this is always located on the C: drive in the LocalAppData folder
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Amazon Games\Data\Games\Sql\GameInstallInfo.sqlite");

            if (!File.Exists(dbPath))
            {
                throw new FileNotFoundException("Amazon Games database not found.");
            }

            // Query the database for the InstallDirectory
            // 'InstallDirectory' is a column in the 'DbSet' table containing, well... of the game. We cross-reference it with the 'Id' column, which contains the GameID we extracted earlier and voila! Game path retrieved!
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                // Standard SQL query, nothing fancy
                command.CommandText = "SELECT InstallDirectory FROM DbSet WHERE Id = $id";
                command.Parameters.AddWithValue("$id", gameId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetString(0);
                    }
                }
            }

            throw new DirectoryNotFoundException("Game not found in Amazon database or directory is missing.");
        }
    }
}