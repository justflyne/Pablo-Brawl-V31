namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Message.Account.Auth;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Logic.Command.Home;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Networking.Session;
    using System;

    public class Mute : CommandModule<CommandContext>
    {
        [Command("mute")]
        public static string MuteCommand(string playerId, string durationDaysStr, [CommandParameter(Remainder = true)] string reason)
        {
            if (!playerId.StartsWith("#"))
            {
                return "Invalid player ID. Make sure it starts with '#'.";
            }

            if (!int.TryParse(durationDaysStr, out int durationDays) || durationDays <= 0)
            {
                return "Invalid duration. Please provide a positive number of days.";
            }

            long lowID = LogicLongCodeGenerator.ToId(playerId);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Could not find player with ID {playerId}.";
            }
            
            if (account.Home.MuteEndTime < DateTime.UtcNow)
            {
                account.Home.MuteEndTime = DateTime.UtcNow.AddDays(durationDays);
            }
            else
            {
                account.Home.MuteEndTime = account.Home.MuteEndTime.AddDays(durationDays);
            }

            account.Avatar.IsCommunityBanned = true;
            
            if (string.IsNullOrWhiteSpace(reason))
            {
                reason = "Не указана"; 
            }

            account.Home.MuteReason = reason;
            DateTime muteEndTime = account.Home.MuteEndTime;
            
            Notification notification = new()
            {                          
                Id = 81,
                MessageEntry = $"Социальные функции были для тебя ограничены на {durationDays} дней. (до {muteEndTime:yyyy-MM-dd HH:mm:ss} UTC)\nПричина: {reason}"
            };
            account.Home.NotificationFactory.Add(notification);

            LogicAddNotificationCommand acm = new() { Notification = notification };
            AvailableServerCommandMessage asm = new() { Command = acm };

            if (Sessions.IsSessionActive(lowID))
            {
                Session session = Sessions.GetSession(lowID);
                session.GameListener.SendTCPMessage(asm);
            }

            return $"Юзер с айди {playerId} был замучен на {durationDays} дней. Причина: {reason}.";
        }
    }
}