namespace Supercell.Laser.Server.Discord.Commands
{
    using System.Threading.Tasks;
    using NetCord.Services.Commands;
    using Supercell.Laser.Server.Database.Cache;

    public class SaveCache : CommandModule<CommandContext>
    {
        [Command("savecache")]
        public async Task<string> savecache()
        {
            await Task.Run(() => AccountCache.SaveAll());
            await Task.Run(() => AllianceCache.SaveAll());

            return "Весь кэш игроков и клубов сохранены в базе данных.";
        }
    }
}