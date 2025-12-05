namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Message.Account.Auth;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Networking.Session;
    public class AddTrophies : CommandModule<CommandContext>
    {
        [Command("addtrophies")]
        public static string addtrophies(
            [CommandParameter(Remainder = true)] string playerIdAndTrophyCount
        )
        {
            string[] parts = playerIdAndTrophyCount.Split(' ');
            if (
                parts.Length != 3
                || !parts[0].StartsWith("#")
                || !int.TryParse(parts[1], out int trophyCount)
                || !int.TryParse(parts[2], out int brawlerid)
            )
            {
                return "Usage: !addtrophies [TAG] [amount] [brawlerid]";
            }

            long lowID = LogicLongCodeGenerator.ToId(parts[0]);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Could not find player with ID {parts[0]}.";
            }

            account.Avatar.GiveTrophies(trophyCount, brawlerid);

            if (Sessions.IsSessionActive(lowID))
            {
                Session session = Sessions.GetSession(lowID);
                session.GameListener.SendTCPMessage(
                    new AuthenticationFailedMessage()
                    {
                        Message =
                            $"Твой аккаунт был обновлен! Теперь у тебя {trophyCount} трофеев на бойце с айди {brawlerid}!"
                    }
                );
                Sessions.Remove(lowID);
            }

            return $"Установлено {trophyCount} трофеев для каждого бойца для аккаунта с айди {parts[0]}.";
        }
    }
}