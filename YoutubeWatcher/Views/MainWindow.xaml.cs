using System.Windows;
using System.Windows.Input;
using YoutubeWatcher.Services;
using YoutubeWatcher.ViewModels;

namespace YoutubeWatcher.Views;

public partial class MainWindow
{
    public MainWindow() => InitializeComponent();

    // ReSharper disable once AsyncVoidMethod
    private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainViewModel vm) return;
        await vm.Initialize();
        await Logger.Log("Инициализация прокси-сервера...");
        
        if (!App.IsRunningAsAdministrator())
            await Logger.Log("Если YouTube будет тормозить, запустите приложение от имени администратора, чтобы обеспечить стабильную работу прокси-сервера");
    }

    private void Window_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key is Key.OemTilde)
        {
            LogRow.Height = new GridLength(LogRow.Height.Value > 0 ? 0 : 120);
        }
    }

    private void SearchBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key is Key.Enter)
        {
            LogBox.Focus();
            QueryBox.Focus();
        }
    }
}