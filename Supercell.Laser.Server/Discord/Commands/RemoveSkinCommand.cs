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

    public class RemoveSkin : CommandModule<CommandContext>
    {
        [Command("removeskin")]
        public static string RemoveSkinCommand(string playerTag, int skinId)
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
                string fullSkinId = skinId.ToString();
                if (fullSkinId.Length == 3) 
                {
                    fullSkinId = "29000" + fullSkinId;
                }
                else if (fullSkinId.Length == 2) 
                {
                    fullSkinId = "290000" + fullSkinId;
                }
                else if (fullSkinId.Length == 1)
                {
                    fullSkinId = "2900000" + fullSkinId;
                }
                else
                {
                    return "Неверный формат ID скина.";
                }

                int finalSkinId = int.Parse(fullSkinId);              

                if (!account.Home.UnlockedSkins.Contains(finalSkinId))
                {
                    return $"Юзер не владеет скином с айди {finalSkinId}.";
                }

                SkinData skin = DataTables
                    .Get(DataType.Skin)
                    .GetDataWithId<SkinData>(finalSkinId);

                if (skin == null)
                {
                    return $"Skin with ID {finalSkinId} not found.";
                }

                account.Home.UnlockedSkins.Remove(skin.GetGlobalId());


                if (Sessions.IsSessionActive(playerId))
                {
                    Session session = Sessions.GetSession(playerId);
                    session.GameListener.SendTCPMessage(
                        new Supercell.Laser.Logic.Message.Account.Auth.AuthenticationFailedMessage
                        {
                            Message = $"Скин {skin.Name} был заблокирован!"
                        }
                    );
                    Sessions.Remove(playerId);
                }

                return $"Скин {skin.Name} успешно был заблокирован для {playerTag}.";
            }
            catch (Exception ex)
            {
                return $"An error occurred while locking the skin: {ex.Message}";
            }
        }
    }
}