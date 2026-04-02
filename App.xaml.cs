using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace Wyke;

public partial class App : Application
{
    private NotifyIcon _trayIcon = null!;
    private HotkeyManager _hotkeyManager = null!;
    private MainWindow _mainWindow = null!;
    private ToolStripMenuItem _alwaysOnTopItem = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        if (!StartupManager.IsEnabled())
            StartupManager.Enable();

        _mainWindow = new MainWindow();
        _mainWindow.Show();

        _hotkeyManager = new HotkeyManager(_mainWindow);
        _hotkeyManager.HotkeyPressed += ToggleWindow;

        SetupTrayIcon();
    }

    private void SetupTrayIcon()
    {
        _trayIcon = new NotifyIcon
        {
            Icon = CreateTrayIcon(),
            Visible = true,
            Text = "Wyke — Click to show/hide"
        };

        _trayIcon.DoubleClick += (_, _) => ToggleWindow();

        var menu = new ContextMenuStrip();
        menu.Font = new Font("Segoe UI", 9f);

        var showHideItem = new ToolStripMenuItem("Show / Hide");
        showHideItem.Click += (_, _) => ToggleWindow();
        menu.Items.Add(showHideItem);

        menu.Items.Add(new ToolStripSeparator());

        _alwaysOnTopItem = new ToolStripMenuItem("Always on Top") { Checked = true };
        _alwaysOnTopItem.Click += (_, _) =>
        {
            _mainWindow.IsAlwaysOnTop = !_mainWindow.IsAlwaysOnTop;
            _alwaysOnTopItem.Checked = _mainWindow.IsAlwaysOnTop;
        };
        menu.Items.Add(_alwaysOnTopItem);

        menu.Items.Add(new ToolStripSeparator());

        var exitItem = new ToolStripMenuItem("Exit");
        exitItem.Click += (_, _) =>
        {
            _mainWindow.RequestExit();
            Shutdown();
        };
        menu.Items.Add(exitItem);

        _trayIcon.ContextMenuStrip = menu;
    }

    private void ToggleWindow()
    {
        if (_mainWindow.IsVisible)
        {
            _mainWindow.Hide();
        }
        else
        {
            _mainWindow.Show();
            _mainWindow.Activate();
        }
    }

    private static Icon CreateTrayIcon()
    {
        using var bmp = new Bitmap(16, 16);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.Clear(Color.FromArgb(0x1A, 0x7F, 0x64));
        using var font = new Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Bold);
        using var brush = new SolidBrush(Color.White);
        using var sf = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };
        g.DrawString("W", font, brush, new RectangleF(0, 0, 16, 16), sf);
        var hIcon = bmp.GetHicon();
        return Icon.FromHandle(hIcon);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _hotkeyManager?.Dispose();
        _trayIcon?.Dispose();
        base.OnExit(e);
    }
}
