namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    
    public class ChangeTheme : CommandModule<CommandContext>
    {
        [Command("changetheme")]
        public static string ChangeThemeCommand(int themeValue)
        {
            if (themeValue < 1 || themeValue > 36)
                return "Ошибка: Значение темы должно быть от 1 до 36.";

            long lastAccId = Accounts.GetMaxAvatarId();

            for (int accId = 1; accId <= lastAccId; accId++)
            {
                Account thisacc = Accounts.LoadNoCache(accId);
                if (thisacc == null) continue;

                thisacc.Home.Theme = themeValue;                
                Accounts.Save(thisacc);
            }

            return $"Тема была изменена на {themeValue} для каждого пользователя.";
        }
    }
}