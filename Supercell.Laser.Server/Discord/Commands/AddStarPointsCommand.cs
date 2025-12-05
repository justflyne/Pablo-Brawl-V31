namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Message.Account.Auth;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Networking.Session;

    public class AddStarPoints : CommandModule<CommandContext>
    {
        [Command("addstarpoints")]
        public static string ExecuteAddStarPoints(
            [CommandParameter(Remainder = true)] string playerIdAndPoints
        )
        {
            string[] parts = playerIdAndPoints.Split(' ');
            if (
                parts.Length != 2
                || !parts[0].StartsWith("#")
                || !int.TryParse(parts[1], out int points)
                || points <= 0
            )
            {
                return "Usage: !addstarpoints [TAG] [amount] (amount must be positive)";
            }

            long lowID = LogicLongCodeGenerator.ToId(parts[0]);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Could not find player with ID {parts[0]}.";
            }

            // Добавляем звёздные очки
            account.Avatar.StarPoints += points;

            if (Sessions.IsSessionActive(lowID))
            {
                Session session = Sessions.GetSession(lowID);
                session.GameListener.SendTCPMessage(
                    new AuthenticationFailedMessage()
                    {
                        Message =
                            $"Твой аккаунт был обновлен! Теперь у тебя добавлено {points} звёздных очков."
                    }
                );
                Sessions.Remove(lowID);
            }

            return $"Добавлено {points} звёздных очков для аккаунта с айди {parts[0]}. Новый баланс: {account.Avatar.StarPoints}.";
        }
    }
}