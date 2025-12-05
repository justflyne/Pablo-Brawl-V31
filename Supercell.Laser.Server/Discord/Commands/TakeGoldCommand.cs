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

    public class TakeGold : CommandModule<CommandContext>
    {
        [Command("takegold")]
        public static string ExecuteTakeGold(
            [CommandParameter(Remainder = true)] string playerIdAndGoldAmount
        )
        {
            string[] parts = playerIdAndGoldAmount.Split(' ');
            if (
                parts.Length != 2
                || !parts[0].StartsWith("#")
                || !int.TryParse(parts[1], out int goldAmount)
                || goldAmount <= 0
            )
            {
                return "Usage: !removegold [TAG] [amount] (amount must be positive)";
            }
            
            long id = 0;

            long lowID = LogicLongCodeGenerator.ToId(parts[0]);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Could not find player with ID {parts[0]}.";
            }

            if (account.Avatar.Gold < goldAmount)
            {
                return $"Недостаточно золота! У игрока с ID {parts[0]} только {account.Avatar.Gold} монет.";
            }

            // Уменьшаем количество золота
            account.Avatar.Gold -= goldAmount;

            Notification n = new()
            {
                Id = 81,
                MessageEntry =
                    $"У вас было списано {goldAmount} монет."
            };

            account.Home.NotificationFactory.Add(n);

            LogicAddNotificationCommand acm = new() { Notification = n };

            AvailableServerCommandMessage asm = new() { Command = acm };

            if (Sessions.IsSessionActive(id))
            {
                Session session = Sessions.GetSession(id);
                session.GameListener.SendTCPMessage(asm);
            }

            return $"У аккаунта с айди {parts[0]} отнято {goldAmount} монет. Новый баланс: {account.Avatar.Gold}.";
        }
    }
}