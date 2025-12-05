namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Logic.Message.Account.Auth;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Networking.Session;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Command.Home;
    using Supercell.Laser.Logic.Message.Home;

    public class UnlockSkin : CommandModule<CommandContext>
    {
        [Command("addskin")]
        public static string UnlockSkinCommand(string playerTag, int skinId)
        {
            if (!playerTag.StartsWith("#"))
            {
                return "Невалидный тэг игрока. Убедись что тэг начинается с '#'.";
            }

            long playerId = LogicLongCodeGenerator.ToId(playerTag);
            Account account = Accounts.Load(playerId);

            if (account == null)
            {
                return $"User with tag {playerTag} not found.";
            }

            try
            {
                if (account.Home.UnlockedSkins.Contains(skinId))
                {
                    return $"Юзер уже владеет скином с айди {skinId}.";
                }

                SkinData skin = DataTables
                    .Get(DataType.Skin)
                    .GetDataWithId<SkinData>(skinId);

                if (skin == null)
                {
                    return $"Skin with ID {skinId} not found.";
                }
                                       
                Notification n = new()
            {
                Id = 94,
                skin = skinId,
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

                return $"Скин {skin.Name} успешно был разблокирован для {playerTag}.";
            }
            catch (Exception ex)
            {
                return $"An error occurred while unlocking the skin: {ex.Message}";
            }
        }
    }
}