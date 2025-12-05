namespace Supercell.Laser.Server.Discord.Commands
{
    using System.Diagnostics;
    using NetCord.Services.Commands;
    using Supercell.Laser.Server.Database.Cache;
    using Supercell.Laser.Server.Logic.Game;
    using Supercell.Laser.Server.Networking.Session;
    using Supercell.Laser.Server.Settings;
    public class Status : CommandModule<CommandContext>
    {
        [Command("status")]
        public static string status()
        {
            long megabytesUsed = Process.GetCurrentProcess().PrivateMemorySize64 / (1024 * 1024);
            DateTime startTime = Process.GetCurrentProcess().StartTime;
            DateTime now = DateTime.Now;

            TimeSpan uptime = now - startTime;

            string formattedUptime = string.Format(
                "{0}{1}{2}{3}",
                uptime.Days > 0 ? $"{uptime.Days} дней, " : string.Empty,
                uptime.Hours > 0 || uptime.Days > 0 ? $"{uptime.Hours} часов, " : string.Empty,
                uptime.Minutes > 0 || uptime.Hours > 0
                  ? $"{uptime.Minutes} минут, "
                  : string.Empty,
                uptime.Seconds > 0 ? $"{uptime.Seconds} секунд" : string.Empty
            );

            return "# Статус Сервера\n"
                + $"Серверная версия игры: v29.258\n"
                + $"Билд сервера: v1.2 from 17.01.2025\n"
                + $"SHA Ресурсы: {Fingerprint.Sha}\n"
                + $"Сервер: Production\n"
                + $"Время сервера: {now} UTC\n"
                + $"Онлайн: {Sessions.Count}\n"
                + $"Используемая память: {megabytesUsed} MB\n"
                + $"Запущен: {formattedUptime}\n"
                + $"Кэшировано аккаунтов: {AccountCache.Count}\n"
                + $"Кэшировано клубов: {AllianceCache.Count}\n"
                + $"Кэшировано румы: {Teams.Count}\n";
        }
    }
}