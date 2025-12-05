namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    public class ResetGems: CommandModule<CommandContext>
    {
        [Command("resetgems")]
        public static string ResetGemsCommand()
        {
            long accid = Accounts.GetMaxAvatarId();
            long lastAccId = Accounts.GetMaxAvatarId();

            for (int accId = 1; accId <= lastAccId; accId++)
            {
                Account thisacc = Accounts.LoadNoCache(accId);                
                if (thisacc == null) continue;
                thisacc.Avatar.Diamonds = 0;
                Accounts.Save(thisacc);
            }                            

            return "Количество гемов были сброшены для каждого пользователя.";
        }
    }
}