namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Server.Database.Cache;
    using Supercell.Laser.Server.Networking.Session;
    using System;

    public class Maintenance : CommandModule<CommandContext>
    {
        [Command("maintenance")]
        public static string MaintenanceCommand()
        {
            try
            {
                Sessions.StartShutdown();
                AccountCache.SaveAll();
                AllianceCache.SaveAll();

                AccountCache.Started = false;
                AllianceCache.Started = false;

                return "Технический перерыв начался. Все данные сохранены.";
            }
            catch (Exception ex)
            {
                return $"Ошибка при запуске тех. перерыва: {ex.Message}";
            }
        }
    }
}