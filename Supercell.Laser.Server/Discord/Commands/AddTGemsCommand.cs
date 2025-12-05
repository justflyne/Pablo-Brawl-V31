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

    public class AddTGems : CommandModule<CommandContext>
    {
        [Command("addtgems")]
        public static string AddTGemsCommand(string playerId, int gemAmount, [CommandParameter(Remainder = true)] string messageText)
        {
            if (!playerId.StartsWith("#"))
            {
                return "❌ Invalid player ID. Must start with '#'.";
            }

            if (gemAmount <= 0)
            {
                return "❌ Invalid gem amount. Please provide a positive number.";
            }

            long lowID = LogicLongCodeGenerator.ToId(playerId);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"❌ Could not find player with ID {playerId}.";
            }

            if (string.IsNullOrWhiteSpace(messageText))
            {
                messageText = "Не указано"; // дефолтное значение
            }

            Notification nGems = new()
            {
                Id = 89,
                DonationCount = gemAmount,
                MessageEntry = messageText
            };

            account.Home.NotificationFactory.Add(nGems);

            LogicAddNotificationCommand addCommand = new() { Notification = nGems };
            AvailableServerCommandMessage serverMsg = new() { Command = addCommand };

            if (Sessions.IsSessionActive(lowID))
            {
                Session session = Sessions.GetSession(lowID);
                session.GameListener.SendTCPMessage(serverMsg);
            }

            return $"✅ Игроку {playerId} выдано {gemAmount} гемов. Сообщение: {messageText}";
        }
    }
}