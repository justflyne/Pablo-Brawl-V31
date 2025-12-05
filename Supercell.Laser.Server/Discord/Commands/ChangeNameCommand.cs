namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Avatar.Structures;
    using Supercell.Laser.Logic.Battle;
    using Supercell.Laser.Logic.Battle.Structures;
    using Supercell.Laser.Logic.Club;
    using Supercell.Laser.Logic.Command;
    using Supercell.Laser.Logic.Command.Avatar;
    using Supercell.Laser.Logic.Command.Home;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Friends;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Home.Quest;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Logic.Listener;
    using Supercell.Laser.Logic.Message;
    using Supercell.Laser.Logic.Message.Account;
    using Supercell.Laser.Logic.Message.Account.Auth;
    using Supercell.Laser.Logic.Message.Battle;
    using Supercell.Laser.Logic.Message.Club;
    using Supercell.Laser.Logic.Message.Friends;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Logic.Message.Latency;
    using Supercell.Laser.Logic.Message.Ranking;
    using Supercell.Laser.Logic.Message.Security;
    using Supercell.Laser.Logic.Message.Team;
    using Supercell.Laser.Logic.Message.Team.Stream;
    using Supercell.Laser.Logic.Message.Udp;
    using Supercell.Laser.Logic.Stream.Entry;
    using Supercell.Laser.Logic.Team;
    using Supercell.Laser.Logic.Team.Stream;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Logic;
    using Supercell.Laser.Server.Logic.Game;
    using Supercell.Laser.Server.Networking;
    using Supercell.Laser.Server.Networking.Security;
    using Supercell.Laser.Server.Networking.Session;
    using Supercell.Laser.Server.Networking.UDP.Game;
    using Supercell.Laser.Server.Settings;

    public class ChangeNameCommand : CommandModule<CommandContext>
    {
        [Command("changename")]
        public static string ChangeName([CommandParameter(Remainder = true)] string playerIdAndName)
        {
            string[] parts = playerIdAndName.Split(' ');
            if (
                parts.Length < 2
                || !parts[0].StartsWith("#")
            )
            {
                return "Usage: !changename [TAG] [NewName]";
            }

            string playerId = parts[0];
            string newName = string.Join(" ", parts.Skip(1));

            if (!playerId.StartsWith("#"))
            {
                return "Invalid player ID. Make sure it starts with '#'.";
            }

            long lowID = LogicLongCodeGenerator.ToId(playerId);

            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Could not find player with ID {playerId}.";
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                return "The new name cannot be empty or contain only whitespace.";
            }

            // Изменяем имя игрока
            account.Avatar.Name = newName;

            // Проверка на альянс и обновление имени в альянсе
            if (account.Avatar.AllianceId >= 0)
            {
                Alliance alliance = Alliances.Load(account.Avatar.AllianceId);
                if (alliance != null)
                {
                    AllianceMember member = alliance.GetMemberById(account.AccountId);
                    if (member != null)
                    {
                        // Обновляем имя участника альянса
                        member.DisplayData.Name = newName;
                    }
                }
            }

            // Проверка на активную сессию и отправка уведомления
            if (Sessions.IsSessionActive(lowID))
            {
                Session session = Sessions.GetSession(lowID);
                session.GameListener.SendTCPMessage(
                    new AuthenticationFailedMessage()
                    {
                        Message = "Твое имя было изменено!"
                    }
                );
                Sessions.Remove(lowID);
            }

            return $"Юзер с айди {playerId} был изменен никнейм на {newName}.";
        }
    }
}