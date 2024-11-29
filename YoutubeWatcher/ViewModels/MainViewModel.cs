using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YoutubeWatcher.Services;
using YoutubeWatcher.Views;

namespace YoutubeWatcher.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string _url = App.YoutubeUrl;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    private MainWindow? Window;
    
    [RelayCommand]
    private void GoBack() => Window?.webView.CoreWebView2.GoBack();
    
    [RelayCommand]
    private void GoForward() => Window?.webView.CoreWebView2.GoForward();
    
    [RelayCommand]
    private void Refresh() => Window?.webView.CoreWebView2.Reload();

    [RelayCommand]
    private void ClearStatus() => StatusMessage = string.Empty;

    [RelayCommand]
    private void CopyStatus() => Clipboard.SetText(StatusMessage);

    public async Task Initialize()
    {
        try
        {
            Window = (Application.Current.MainWindow as MainWindow)!;

            // Инициализируем WebView2 с созданным окружением
            await Window.webView.EnsureCoreWebView2Async(null);

            // Устанавливаем начальный URL для YouTube
            Window.webView.Source = new Uri(Url);
            
            try
            {
                await VPNService.StartEmbeddedVPN(App.VpnConfigPath);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка при запуске VPN: {ex.Message}";
                if (ex.Message.Contains("Не удается найти указанный файл."))
                {
                    StatusMessage += """
                                     
                                     
                                     Почему это могло произойти:
                                     1. Приложение запущено без прав администратора
                                     2. На устройстве не установлен OpenVPN (дистрибутивы можно найти в папке приложения Dist)
                                     3. В пользовательской переменной среды "Path" не указан путь к OpenVPN\bin (самая частая ошибка)
                                     """;
                }
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка инициализации: {ex.Message}";
        }
    }
}
