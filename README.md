# Steam Game Folder Opener

This program adds a **"Open Steam Game Files"**, button when you right click a Steam game's desktop shortcut.

It opens the actual game folder, so there is no more digging through `steamapps/common/` or multiple library folders.
It's perfect for Steam users who hate manually navigating to their game files, such as myself.

### Features
- Adds a clean **"Open Steam Game Files"** option to the right-click menu on any Steam game's desktop shortcut
- Works with **multiple Steam libraries** , even on different drives
- Lightweight, single file to both install and uninstall, meaning there are no background tasks or unnnecessary services that slow down your PC

### Installation

1. Download the latest `SteamGameOpener.exe` from the [Releases page](https://github.com/futureflash01/SteamGameFolderOpener/releases)
2. Open the program and click **Install**
3. Done! Now go right-click any Steam desktop shortcut and you'll see the new option.

### IMPORTANT NOTE:
If you're on Windows 11, you may have to click 'Show more options' after right-clicking a shortcut

### To Uninstall:
Open the program again and click **Uninstall**. Even if you deleted the program, just re-download it any time and the Uninstall option will still show regardless

---

## How It Works in Detail (VERY Technical Deep Dive)

This section is VERY boring but explains exactly what happens behind the scenes when you use the tool.

### 1. Adding the Context Menu
When you click **Install**, the program writes a few entries into the Windows Registry under:
- `HKEY_CURRENT_USER\Software\Classes\InternetShortcut\shell\SteamGameOpener`
- and a backup entry under `SystemFileAssociations\.url\shell\SteamGameOpener`

This tells Windows: “When someone right-clicks a `.url` file, show my menu item and run my exe with the file path as an argument.”

### 2. When You Right-Click a Steam Shortcut
Steam desktop shortcuts are actually `.url` files (tiny text files containing `steam://rungameid/123456`).

When you click **"Open Steam Game Files"**:
- Windows launches `SteamGameOpener.exe` and passes the full path to the clicked `.url` file.
- The program reads the file and uses a regular expression to extract the **Steam App ID** (the number after `rungameid/` or `run/`).

### 3. Finding the Actual Game Folder (The Smart Part)

This is the most useful part:

- It reads your Steam installation path from the registry (`HKEY_CURRENT_USER\Software\Valve\Steam\SteamPath`).
- It opens Steam’s master library file: `steamapps\libraryfolders.vdf`
- This file contains **every Steam library folder** you have (even ones on other drives).
- For each library, it looks for a file called `appmanifest_{AppID}.acf`
- Inside that file it finds the `installdir` value (the actual folder name).
- It builds the full path: `library-folder\steamapps\common\installdir`
- As soon as it finds a folder that actually exists on disk, it opens it in Explorer.

This is why it works perfectly even if you have 5 different Steam libraries.

---

## License

This project is licensed under the **MIT License** — see [LICENSE](LICENSE) for details.

You are free to use, modify, and distribute this code, as long as you keep the original copyright notice.

---

## Author

Made with ❤️ by [**FutureFlash**](https://youtube.com/@FutureFlash)

If this tool helped you, feel free to star the repo or drop a comment!

---

**Keywords**: steam open game files, steam desktop shortcut context menu, open steam library folder, steam game location, steam right click menu, steam shortcut open files, steam game folder quick access
