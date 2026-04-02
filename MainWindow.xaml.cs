using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace Wyke;

public partial class MainWindow : Window
{
    private readonly DispatcherTimer _saveTimer;
    private readonly DispatcherTimer _boundsTimer;
    private bool _exitRequested;
    private bool _boundsRestored;

    private static readonly string BoundsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Wyke", "bounds.txt");

    public bool IsAlwaysOnTop
    {
        get => Topmost;
        set => Topmost = value;
    }

    public void RequestExit() => _exitRequested = true;

    public MainWindow()
    {
        InitializeComponent();

        _saveTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
        _saveTimer.Tick += (_, _) =>
        {
            _saveTimer.Stop();
            NoteStorage.Save(NoteTextBox.Text);
        };

        _boundsTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(400) };
        _boundsTimer.Tick += (_, _) =>
        {
            _boundsTimer.Stop();
            SaveBounds();
        };

        Loaded += (_, _) =>
        {
            RestorePosition();
            NoteTextBox.Text = NoteStorage.Load();
            NoteTextBox.CaretIndex = NoteTextBox.Text.Length;
            NoteTextBox.Focus();
        };

        LocationChanged += (_, _) => DeferSaveBounds();
        SizeChanged += (_, _) => DeferSaveBounds();
    }

    private void RestorePosition()
    {
        if (TryLoadBounds(out var r))
        {
            // Clamp to ensure it's on a visible monitor
            var workArea = SystemParameters.WorkArea;
            Left = Math.Max(workArea.Left, Math.Min(r.Left, workArea.Right - r.Width));
            Top = Math.Max(workArea.Top, Math.Min(r.Top, workArea.Bottom - r.Height));
            Width = Math.Max(MinWidth, Math.Min(r.Width, workArea.Width));
            Height = Math.Max(MinHeight, Math.Min(r.Height, workArea.Height));
        }
        else
        {
            PositionTopRight();
        }
        _boundsRestored = true;
    }

    private void PositionTopRight()
    {
        var workArea = SystemParameters.WorkArea;
        Left = workArea.Right - Width - 24;
        Top = workArea.Top + 24;
    }

    private void DeferSaveBounds()
    {
        if (!_boundsRestored) return;
        _boundsTimer.Stop();
        _boundsTimer.Start();
    }

    private void SaveBounds()
    {
        if (WindowState != WindowState.Normal) return;
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(BoundsPath)!);
            File.WriteAllText(BoundsPath, $"{Left},{Top},{Width},{Height}");
        }
        catch { /* ignore */ }
    }

    private static bool TryLoadBounds(out Rect r)
    {
        r = default;
        if (!File.Exists(BoundsPath)) return false;
        try
        {
            var parts = File.ReadAllText(BoundsPath).Split(',');
            if (parts.Length != 4) return false;
            r = new Rect(double.Parse(parts[0]), double.Parse(parts[1]),
                         double.Parse(parts[2]), double.Parse(parts[3]));
            return r.Width > 0 && r.Height > 0;
        }
        catch { return false; }
    }

    private void NoteTextBox_TextChanged(object sender,
        System.Windows.Controls.TextChangedEventArgs e)
    {
        _saveTimer.Stop();
        _saveTimer.Start();
    }

    private void Border_MouseLeftButtonDown(object sender,
        System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
            DragMove();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (_exitRequested)
        {
            _saveTimer.Stop();
            NoteStorage.Save(NoteTextBox.Text);
            SaveBounds();
            base.OnClosing(e);
        }
        else
        {
            e.Cancel = true;
            Hide();
        }
    }
}
