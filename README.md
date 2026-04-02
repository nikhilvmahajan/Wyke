# Wyke

A tiny, always-available notepad that lives in your Windows system tray. Inspired by the Mac app [Tyke](https://tyke.app/).

Think of it as a sticky note that's always one keypress away — no opening apps, no creating files, no saving. Just press a shortcut, jot something down, and get back to work.

---

## What it does

- Pops open a small floating notepad in the top-right corner of your screen
- Press **Ctrl + Shift + Space** from anywhere to show or hide it instantly
- Everything you type is saved automatically — nothing is ever lost, even if you restart your PC
- Stays on top of other windows so it never gets buried
- Sits quietly in your system tray (bottom-right of your taskbar) when not in use
- Starts automatically when Windows starts

---

## Download & Install

1. Go to the [Releases](../../releases) page
2. Download `Wyke.exe`
3. Double-click it — no installation needed, it just runs

> Wyke does not require any additional software to be installed.

---

## How to use

| Action | How |
|---|---|
| Show / hide the notepad | **Ctrl + Shift + Space** |
| Move the notepad | Click and drag it anywhere on screen |
| Resize the notepad | Drag the bottom-right corner |
| Right-click the tray icon | Access menu: Show/Hide, Always on Top, Exit |
| Toggle always-on-top | Right-click tray icon → Always on Top |
| Quit the app | Right-click tray icon → Exit |

Your notes are saved automatically as you type. You don't need to do anything.

---

## Where is my note saved?

Your note is stored as a plain text file at:

```
C:\Users\YourName\AppData\Roaming\Wyke\note.txt
```

You can open this file in any text editor if you ever want to back it up or copy it somewhere else.

---

## Does it start automatically?

Yes — Wyke adds itself to your Windows startup list the first time you run it, so it's always ready when you boot up. If you'd rather it didn't, you can remove it from **Task Manager → Startup apps**.

---

## Built with

- C# / WPF
- .NET 8
- Windows 10 / 11

---

## License

MIT — free to use, modify, and distribute.
