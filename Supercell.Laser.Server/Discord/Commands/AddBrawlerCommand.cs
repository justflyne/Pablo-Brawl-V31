namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Command.Home;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Logic.Message.Account.Auth;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Networking.Session;

    public class UnlockBrawler : CommandModule<CommandContext>
    {
        [Command("addbrawler")]
        public static string UnlockBrawlerCommand(string playerTag, int brawlerId)
        {
            if (!playerTag.StartsWith("#"))
            {
                return "Invalid player tag. Make sure it starts with '#'.";
            }

            long playerId = LogicLongCodeGenerator.ToId(playerTag);
            Account account = Accounts.Load(playerId);

            if (account == null)
            {
                return $"User with tag {playerTag} not found.";
            }

            try
            {
                if (account.Avatar.HasHero(16000000 + brawlerId))
                {
                    return $"The player already owns the brawler with ID {brawlerId}.";
                }

                CharacterData character = DataTables
                    .Get(16) // DataType.Character
                    .GetDataWithId<CharacterData>(brawlerId);

                if (character == null)
                {
                    return $"Brawler with ID {brawlerId} not found.";
                }

                Notification n = new()
            {
                Id = 93,
                brawlerId = brawlerId,
                MessageEntry =
                    $"<c6>Спасибо за поддержку сервера!</c>"
            };

            account.Home.NotificationFactory.Add(n);

            LogicAddNotificationCommand acm = new() { Notification = n };

            AvailableServerCommandMessage asm = new() { Command = acm };

            if (Sessions.IsSessionActive(playerId))
            {
                Session session = Sessions.GetSession(playerId);
                session.GameListener.SendTCPMessage(asm);
            }

                return $"Боец {character.Name} успешно разблокирован для {playerTag}.";
            }
            catch (Exception ex)
            {
                return $"An error occurred while unlocking the brawler: {ex.Message}";
            }
        }
    }
}