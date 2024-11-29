using System.Security.Principal;
using System.Windows;
using YoutubeWatcher.Services;
using YoutubeWatcher.ViewModels;
using YoutubeWatcher.Views;

namespace YoutubeWatcher;

public partial class App
{
    private static MainWindow m_window { get; } = new();
    public static MainWindow GetMainWindow() => m_window;
    public static MainViewModel GetViewModel() => (m_window.DataContext as MainViewModel)!;
    
    public const string Password = "8285398";
    public const string Username = "freevpn4you";
    public const string LogFilePath = "./debug.log";
    public const string VpnConfigPath = "./tcp.ovpn";
    public const string YoutubeUrl = "https://www.youtube.com/";

    protected override void OnExit(ExitEventArgs e)
    {
        VPNService.StopVPN();
        base.OnExit(e);
    }
    
    public static bool IsRunningAsAdministrator()
    {
        // Get current Windows user
        var windowsIdentity = WindowsIdentity.GetCurrent();
        // Get current Windows user principal
        WindowsPrincipal windowsPrincipal = new(windowsIdentity);
        // Return TRUE if user is in role "Administrator"
        return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    private void App_OnStartup(object sender, StartupEventArgs e)
    {
        MainWindow = m_window;
        m_window.Show();
    }
}