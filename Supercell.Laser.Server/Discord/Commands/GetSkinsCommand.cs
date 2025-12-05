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

    public class ListSkinCommand : CommandModule<CommandContext>
    {
        [Command("listskin")]
        public static string GetSkinsCommand(string playerTag)
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
                // Получаем все разблокированные скины игрока
                var unlockedSkins = account.Home.UnlockedSkins;

                if (unlockedSkins.Count == 0)
                {
                    return $"Игрок {playerTag} не имеет разблокированных скинов.";
                }

                // Формируем строку для вывода
                string skinsList = "Разблокированные скины:\n";
                foreach (var skinId in unlockedSkins)
                {
                    SkinData skin = DataTables
                        .Get(DataType.Skin)
                        .GetDataWithId<SkinData>(skinId);

                    if (skin != null)
                    {
                        skinsList += $"- {skin.Name} (ID: {skinId})\n";
                    }
                }

                return skinsList;
            }
            catch (Exception ex)
            {
                return $"Произошла ошибка при получении списка скинов: {ex.Message}";
            }
        }
    }
}