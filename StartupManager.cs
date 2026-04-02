using Microsoft.Win32;

namespace Wyke;

public static class StartupManager
{
    private const string RegKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string AppName = "Wyke";

    public static void Enable()
    {
        using var key = Registry.CurrentUser.OpenSubKey(RegKey, writable: true);
        var exePath = Environment.ProcessPath
            ?? System.Reflection.Assembly.GetExecutingAssembly().Location;
        key?.SetValue(AppName, exePath);
    }

    public static void Disable()
    {
        using var key = Registry.CurrentUser.OpenSubKey(RegKey, writable: true);
        key?.DeleteValue(AppName, throwOnMissingValue: false);
    }

    public static bool IsEnabled()
    {
        using var key = Registry.CurrentUser.OpenSubKey(RegKey);
        return key?.GetValue(AppName) is not null;
    }
}
