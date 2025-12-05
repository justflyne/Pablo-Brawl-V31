namespace Supercell.Laser.Server.Handler
{
    using Supercell.Laser.Server.Database.Cache;
    using Supercell.Laser.Server.Networking.Session;

    internal static class ExitHandler
    {
        public static void Exit(object sender, ConsoleCancelEventArgs e)
        {
            Logger.Print("Выключение сервера... Пожалуйста подождите.");
            Sessions.StartShutdown();

            AccountCache.SaveAll();
            AllianceCache.SaveAll();

            AccountCache.Started = false;
            AllianceCache.Started = false;

            Console.WriteLine("Сервер в режиме технического перерыва, нажмите любую кнопку для полного выключения.");
            Console.ReadLine();

            Environment.Exit(0);
        }

        public static void Init()
        {
            Console.CancelKeyPress += ExitHandler.Exit;
        }
    }
}
