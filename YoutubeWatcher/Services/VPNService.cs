namespace YoutubeWatcher.Services;

using System.Diagnostics;
using System.Threading.Tasks;

public static class VPNService
{
    private static Process? _vpnProcess;

    /// <summary>
    /// Запускает встроенный VPN-сервер с указанной конфигурацией.
    /// </summary>
    public static async Task StartEmbeddedVPN(string config)
    {
        StopVPN();

        // Запуск встроенного VPN сервиса
        _vpnProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "openvpn",
                Arguments = $"--config \"{config}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Verb = "runas"
            }
        };
        
        _vpnProcess.Start();
        
        await Task.Delay(8000);

        await using var writer = _vpnProcess.StandardInput;
        if (writer.BaseStream.CanWrite)
        {
            await writer.WriteLineAsync(App.Username);
            await Task.Delay(2000);
            await writer.WriteLineAsync(App.Password);
        }
    }

    /// <summary>
    /// Останавливает VPN-сервер.
    /// </summary>
    public static void StopVPN()
    {
        if (_vpnProcess is null || _vpnProcess.HasExited) return;
        _vpnProcess.Kill();
        _vpnProcess = null;
    }
}
