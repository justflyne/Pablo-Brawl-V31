namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Avatar.Structures;
    using Supercell.Laser.Logic.Club;
    using Supercell.Laser.Logic.Command;
    using Supercell.Laser.Logic.Command.Avatar;
    using Supercell.Laser.Logic.Command.Home;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Friends;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Logic.Listener;
    using Supercell.Laser.Logic.Message;
    using Supercell.Laser.Logic.Message.Account;
    using Supercell.Laser.Logic.Message.Account.Auth;
    using Supercell.Laser.Logic.Message.Club;
    using Supercell.Laser.Logic.Message.Friends;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Logic.Message.Security;
    using Supercell.Laser.Logic.Message.Team;
    using Supercell.Laser.Logic.Message.Team.Stream;
    using Supercell.Laser.Logic.Message.Udp;
    using Supercell.Laser.Logic.Stream;
    using Supercell.Laser.Logic.Stream.Entry;
    using Supercell.Laser.Logic.Team;
    using Supercell.Laser.Logic.Team.Stream;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Cache;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Logic;
    using Supercell.Laser.Server.Logic.Game;
    using Supercell.Laser.Server.Networking;
    using Supercell.Laser.Server.Networking.Security;
    using Supercell.Laser.Server.Networking.Session;
    using Supercell.Laser.Server.Networking.UDP.Game;

    public class RemovePremium : CommandModule<CommandContext>
    {
        [Command("removeprem")]
        public static string RemovePremiumCommand(
            [CommandParameter(Remainder = true)] string parameters
        )
        {
            string[] parts = parameters.Split(' ');
            if (parts.Length != 1)
            {
                return "Usage: !removeprem [TAG]";
            }

            long id = 0;
            bool sc = false;

            // Определение ID игрока
            if (parts[0].StartsWith('#'))
            {
                id = LogicLongCodeGenerator.ToId(parts[0]);
            }
            else
            {
                sc = true;
                if (!long.TryParse(parts[0], out id))
                {
                    return "Invalid player ID format.";
                }
            }

            Account account = Accounts.Load(id);
            if (account == null)
            {
                return $"Could not find player with ID {parts[0]}.";
            }

            account.Home.PremiumEndTime = DateTime.MinValue;
            account.Avatar.PremiumLevel = 0;
            account.Home.NameColorId = GlobalId.CreateGlobalId(43, 0);

            string displayId = sc ? LogicLongCodeGenerator.ToCode(id) : parts[0];
            return $"Успешно: премиум для {displayId} был удалён.";
        }
    }
}