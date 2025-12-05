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

    public class AddGold : CommandModule<CommandContext>
    {
        [Command("addgold")]
        public static string ExecuteAddGold(
            [CommandParameter(Remainder = true)] string playerIdAndGoldAmount
        )
        {
            string[] parts = playerIdAndGoldAmount.Split(' ');
            if (
                parts.Length != 2
                || !parts[0].StartsWith("#")
                || !int.TryParse(parts[1], out int goldAmount)
            )
            {
                return "Usage: !addgold [TAG] [amount]";
            }

            long lowID = LogicLongCodeGenerator.ToId(parts[0]);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Could not find player with ID {parts[0]}.";
            }

            // Добавляем золото к текущему балансу
            Notification nGold = new()
            {
                Id = 90,
                DonationCount = goldAmount,
                MessageEntry = $"<c6>Ваши {goldAmount} монеты, спасибо за поддержку сервера!</c>"
            };
            account.Home.NotificationFactory.Add(nGold);
            LogicAddNotificationCommand acmGold = new() { Notification = nGold };
            AvailableServerCommandMessage asmGold = new();
            asmGold.Command = acmGold;

            if (Sessions.IsSessionActive(lowID))
            {
                Session sessionGold = Sessions.GetSession(lowID);
                sessionGold.GameListener.SendTCPMessage(asmGold);
            }

            return $"Добавлено {goldAmount} золота для аккаунта с айди {parts[0]}.";
        }
    }
}