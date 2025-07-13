using System.Runtime.CompilerServices;
using System.Text;

namespace ClienteRegalosDulces.Helpers
{
    public static class Logger
    {
        private static string Modulo = "datahub";
        private static string LogsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        public static bool IsDebug { get; set; } = false;

        public static void LogError(string message, Exception ex)
        {
            if (!Directory.Exists(LogsDirectory))
                Directory.CreateDirectory(LogsDirectory);

            var date = DateTime.Today.ToString("yyyyMMdd");
            var logPath = Path.Combine(LogsDirectory, $"{Modulo}-error-{date}.log");

            using (var sw = new StreamWriter(logPath, true, Encoding.UTF8))
            {
                sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} -> {message}");
                if (ex != null)
                {
                    sw.WriteLine($"Exception Message: {ex.Message}");
                    if (ex.InnerException != null)
                        sw.WriteLine($"InnerException: {ex.InnerException.Message}");
                }
            }
        }

        public static void LogEx(this Exception ex, [CallerMemberName] string caller = "")
        {
            var rootEx = ex;
            while (rootEx.InnerException != null)
                rootEx = rootEx.InnerException;

            LogError($"Error {caller}:", rootEx);
        }
    }
}
