namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Command.Home;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Networking.Session;

    public class TakeStarPoints : CommandModule<CommandContext>
    {
        [Command("takestarpoints")]
        public static string ExecuteTakeStarPoints(
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
                return "Usage: !takestarpoints [TAG] [amount] (amount must be positive)";
            }
            
            long id = 0;

            long lowID = LogicLongCodeGenerator.ToId(parts[0]);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Could not find player with ID {parts[0]}.";
            }

            if (account.Avatar.StarPoints < points)
            {
                return $"Недостаточно звёздных очков! У игрока с ID {parts[0]} только {account.Avatar.StarPoints} очков.";
            }

            // Уменьшаем звёздные очки
            account.Avatar.StarPoints -= points;

            Notification n = new()
            {
                Id = 81,
                MessageEntry =
                    $"У вас было списано {points} звёздных очков."
            };

            account.Home.NotificationFactory.Add(n);

            LogicAddNotificationCommand acm = new() { Notification = n };

            AvailableServerCommandMessage asm = new() { Command = acm };

            if (Sessions.IsSessionActive(id))
            {
                Session session = Sessions.GetSession(id);
                session.GameListener.SendTCPMessage(asm);
            }

            return $"У аккаунта с айди {parts[0]} отнято {points} звёздных очков. Новый баланс: {account.Avatar.StarPoints}.";
        }
    }
}