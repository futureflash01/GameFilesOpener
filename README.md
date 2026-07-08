# GameFilesOpener (Formerly SteamGameFolderOpener)

This program adds an **"Open Game Files"** button when you right-click a game's desktop shortcut.

It opens the actual game folder instantly, so there is no more digging through multiple library folders or hidden system directories. It's perfect for those who hate manually navigating to their game files, such as myself.

### Features
- Adds a clean **"Open Game Files"** option to the right-click menu on any supported game's desktop shortcut.
- Works seamlessly with **FIVE** major gaming platforms: Steam, Epic Games, Amazon Games, Xbox / Microsoft Apps, and GOG Galaxy.
- Works with **multiple libraries** across different drives.
- Lightweight, single file to both install and uninstall, meaning there are no background tasks or unnecessary services that slow down your PC.

### Installation

1. Download the latest `GameFilesOpener Application` from the [Releases page](https://github.com/futureflash01/GameFilesOpener/releases).
2. Open the program and click **Install**.
3. Done! Now go right-click any supported desktop shortcut and you'll see the new option.

### IMPORTANT NOTE:
If you're on Windows 11, you may have to click 'Show more options' after right-clicking a shortcut.

### To Uninstall:
Open the program again and click **Uninstall**. Even if you deleted the program, just re-download it any time and the Uninstall option will still show regardless.

---

## How It Works in Detail (VERY Technical Deep Dive)

This section is VERY boring but explains exactly what happens behind the scenes when you use the tool. The program uses smart routing to figure out what type of shortcut you clicked, and then acts accordingly.

### 1. Adding the Context Menu
When you click **Install**, the program copies itself to your Local AppData folder and writes a few entries into the Windows Registry. It registers under:
- `HKEY_CURRENT_USER\Software\Classes\InternetShortcut\shell\GameShortcutOpener` (For web-based `.url` shortcuts)
- `HKEY_CURRENT_USER\Software\Classes\SystemFileAssociations\.url\shell\GameShortcutOpener` (A backup entry for `.url` files)
- `HKEY_CURRENT_USER\Software\Classes\lnkfile\shell\GameShortcutOpener` (For standard `.lnk` desktop shortcuts)

This tells Windows: “When someone right-clicks a `.url` or `.lnk` file, show my menu item and run my exe with the file path as an argument”.

### 2. Identifying the Shortcut Type
When you click **"Open Game Files"**, the program checks if the file ends with `.url` or `.lnk`.
- **.url files** (used by Steam, Epic Games, and Amazon) are tiny text files containing custom web protocols. The program reads the text inside to see which launcher it belongs to.
- **.lnk files** (used by Xbox and GOG) are binary desktop shortcuts. The program uses the Windows Script Host to read hidden properties like arguments and icon locations inside the file.

### 3. Finding the Actual Game Folder (The Smart Part)
Here is exactly how the program parses the libraries and manifests for every single launcher to find your game:

#### 🔵 Steam
- It extracts the **AppID** from the text inside the `.url` shortcut (the number after `rungameid/` or `run/`) using a regular expression.
- It reads your Steam installation path from the registry (`HKEY_CURRENT_USER\Software\Valve\Steam\SteamPath`).
- It opens Steam’s master library file: `steamapps\libraryfolders.vdf`.
- This file contains **every Steam library folder** you have (even ones on other drives).
- For each library, it looks for a file called `appmanifest_{AppID}.acf`.
- Inside that file, it finds the `installdir` value (the actual folder name).
- It builds the full path: `library-folder\steamapps\common\installdir`.
- As soon as it finds a folder that actually exists on disk, it opens it in Explorer.

#### ⚫ Epic Games Launcher
- It extracts the **Game ID** from the shortcut's `com.epicgames.launcher://apps/` URL by grabbing everything up until the first `%3A` character.
- It finds where Epic Games is installed by reading the `AppDataPath` key in the Registry (`HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Epic Games\EpicGamesLauncher`).
- In that folder, it opens the `Manifests` directory, which contains a bunch of `.item` (JSON) files.
- Because the file names are random, it reads every single `.item` file until it finds the one where the `CatalogNamespace` matches the Game ID extracted earlier.
- It reads the `ManifestLocation` property from that file, removes the hidden `.egstore` folder from the path, formats the slashes, and opens the folder.

#### 🟠 Amazon Games
- It uses a regular expression to extract the **Game ID** from the `amazon-games://play/` URL in the shortcut.
- It navigates to Amazon's hidden SQLite database located at `%LOCALAPPDATA%\Amazon Games\Data\Games\Sql\GameInstallInfo.sqlite`.
- It opens a connection to this database and runs an SQL query on the `DbSet` table.
- It looks for the row where the `Id` column matches the Game ID, reads the `InstallDirectory` column, and opens that folder.

#### 🟣 GOG Galaxy
- It uses a Windows script tool to read the `.lnk` shortcut's hidden arguments.
- It uses a regular expression to extract the `gameId=` value from those arguments.
- It simply looks up that specific Game ID in the Windows Registry at `HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\GOG.com\Games\{gameId}`.
- It reads the `path` value from that registry key and opens it.

#### 🟢 Xbox / Microsoft Apps
- Xbox games are notoriously difficult to track via registry keys or manifests.
- Instead, the program reads the `iconLocation` property embedded inside the `.lnk` shortcut file.
- The icon for an Xbox game is always stored in the game's `Content` folder (e.g., `E:\XboxGames\RandomGame\Content\Logo.ico`).
- Since we don't care about the icon, the program extracts the file path up to the `Content\` folder and opens it, as that's where the game is installed (e.g., `E:\XboxGames\RandomGame\Content`).

---

## License

This project is licensed under the **MIT License** — see [LICENSE](https://github.com/futureflash01/GameFilesOpener/blob/master/LICENSE.txt) for details.

You are free to use, modify, and distribute this code, as long as you keep the original copyright notice.

---

## Author

Made with ❤️ by [**FutureFlash**](https://youtube.com/@FutureFlash)

If this tool helped you, feel free to star the repo or drop a comment!

---

**Keywords**: steam open game files, steam desktop shortcut context menu, open steam library folder, steam game location, steam right click menu, steam shortcut open files, steam game folder quick access, epic games launcher folder opener, amazon games installation path, open gog galaxy game files, xbox app game folder location, universal game file locator, find epic games install folder, multi-launcher game files opener
