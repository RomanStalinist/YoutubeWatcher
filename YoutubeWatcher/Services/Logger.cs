using System.IO;

namespace YoutubeWatcher.Services;

public static class Logger
{
    static Logger()
    {
        _ = Log("Инициализация консоли...");
    }
    
    public static async Task Log(object? data)
    {
        try
        {
            await using var writer = new StreamWriter(App.LogFilePath, true);
            var msg = $"{DateTime.Now}: {data}";
            Console.WriteLine(msg);
            App.GetViewModel().StatusMessage += $"{msg}\r\n";
            await writer.WriteLineAsync(msg);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка записи сообщения: {ex.Message}");
        }
    }
}