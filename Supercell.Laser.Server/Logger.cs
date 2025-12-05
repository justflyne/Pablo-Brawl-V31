namespace Supercell.Laser.Server
{
    using System;
    using System.Text;
    using Supercell.Laser.Titan.Debug;

    public static class Logger
    {

        private static StringBuilder FileLogger;
        public static void Print(string log)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[DEBUG] " + log);
        }

        public static void Init()
        {
            FileLogger = new StringBuilder();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Debugger.SetListener(new DebuggerListener());
        }

        public static void Warning(string log)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[WARNING] " + log);
        }

        public static void Error(string log)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[ERROR] " + log);
        }
        public static void Log(string log)
        {
            FileLogger.Append($"[{DateTime.Now}][WARN!!] " + log + "\n");
            File.AppendAllText("./log.txt", FileLogger.ToString());
            FileLogger.Clear();
        }

        public static void BLog(string log)
        {
            FileLogger.Append($"[{DateTime.Now}] " + log + "\n");
            File.AppendAllText("./battles.txt", FileLogger.ToString());
            FileLogger.Clear();
        }
        
        public static void ACLog(string log)
        {
            FileLogger.Append($"[{DateTime.Now}] " + log + "\n");
            File.AppendAllText("./anticheat.txt", FileLogger.ToString());
            FileLogger.Clear();
        }
        
        public static void VLog(string log)
        {
            FileLogger.Append($"[{DateTime.Now}] " + log + "\n");
            File.AppendAllText("./qvalues.txt", FileLogger.ToString());
            FileLogger.Clear();
        }

        public static void HandleReport(string log)
        {
            FileLogger.Append($"[{DateTime.Now}] " + log + "\n");
            File.AppendAllText("./reports.txt", FileLogger.ToString());
            FileLogger.Clear();
        }
    }

    public class DebuggerListener : IDebuggerListener
    {
        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[LOGIC] Ошибка: " + message);
        }

        public void Print(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[LOGIC] Инфо: " + message);
        }

        public void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[LOGIC] Внимание: " + message);
        }
    }
}
