using System.IO;

namespace Wyke;

public static class NoteStorage
{
    private static readonly string SavePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Wyke", "note.txt");

    public static string Load()
    {
        if (!File.Exists(SavePath))
            return string.Empty;
        try { return File.ReadAllText(SavePath); }
        catch { return string.Empty; }
    }

    public static void Save(string text)
    {
        var dir = Path.GetDirectoryName(SavePath)!;
        Directory.CreateDirectory(dir);
        File.WriteAllText(SavePath, text);
    }
}
