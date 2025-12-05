namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Message.Account.Auth;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Networking.Session;
    public class ClearAllNotif : CommandModule<CommandContext>
    {
        [Command("clearallnotif")]
        public static string clearallnotif(
            [CommandParameter(Remainder = true)] string playerIdAndTrophyCount
        )
        {
            string[] parts = playerIdAndTrophyCount.Split(' ');
            if (
                parts.Length != 1
                || !parts[0].StartsWith("#")
            )
            {
                return "Usage: !clearallnotif [TAG]";
            }

            long lowID = LogicLongCodeGenerator.ToId(parts[0]);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Could not find player with ID {parts[0]}.";
            }

            account.Home.NotificationFactory.ClearAllNotif();

            if (Sessions.IsSessionActive(lowID))
            {
                Session session = Sessions.GetSession(lowID);
                session.GameListener.SendTCPMessage(
                    new AuthenticationFailedMessage()
                    {
                        Message =
                            $"Твой аккаунт был обновлен!"
                    }
                );
                Sessions.Remove(lowID);
            }

            return $"Были удалены всё нотифы у аккаунта с айди {parts[0]}.";
        }
    }
}