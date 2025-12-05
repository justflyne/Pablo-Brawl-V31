namespace Supercell.Laser.Server
{
    using System.Drawing;
    using System.Diagnostics;
    using Supercell.Laser.Server.Handler;
    using Supercell.Laser.Server.Settings;

    static class Program
    {
        public const string SERVER_VERSION = "30.242";
        public const string BUILD_TYPE = "Production";

        private static void Main(string[] args)
        {
            Console.Title = "BrawlStars - server emulator v" + SERVER_VERSION + " Build: " + BUILD_TYPE;
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);

            Colorful.Console.WriteWithGradient(
                @"
    ____                      __   _____ __
   / __ )_________ __      __/ /  / ___// /_____ ___________
  / __  / ___/ __ `/ | /| / / /   \__ \/ __/ __ `/ ___/ ___/
 / /_/ / /  / /_/ /| |/ |/ / /   ___/ / /_/ /_/ / /  (__  )
/_____/_/   \__,_/ |__/|__/_/   /____/\__/\__,_/_/  /____/

       " + "\n\n\n", Color.Fuchsia, Color.Cyan, 8);

            Logger.Init();
            Configuration.Instance = Configuration.LoadFromFile("config.json");

            Resources.InitDatabase();
            Resources.InitDiscord();
            Resources.InitLogic();
            Resources.InitNetwork();
            //RunBotScript("/root/PabloBrawl/abot.py");
            //RunBotScript("/root/PabloBrawl/bot.py");
            //RunBotScript("/root/PabloBrawl/donate.py");
            //RunBotScript("/root/PabloBrawl/logger.py");

            Logger.Print("Сервер запущен! Начинаем играть Pablo Brawl!");

            ExitHandler.Init();
            CmdHandler.Start();
        }
        
       private static void RunBotScript(string scriptPath)
       {
           try
           {
               var process = new Process
               {
                   StartInfo = new ProcessStartInfo
                   {
                       FileName = "python3",
                       Arguments = scriptPath,
                       RedirectStandardOutput = true,
                       UseShellExecute = false,
                       CreateNoWindow = true
                   }
               };
                
               process.Start();
               Logger.Print($"Запуск бота: {scriptPath}");
           }
           catch (Exception ex)
           {
               Logger.Print($"Ошибка при запуске бота {scriptPath}: {ex.Message}");
           }
       }
    }
}
