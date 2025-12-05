namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Newtonsoft.Json;
    using Supercell.Laser.Logic.Message.Account.Auth;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Networking.Session;
    using System;

    public class Ban : CommandModule<CommandContext>
    {
        [Command("ban")]
        public static string BanCommand(string playerId, string durationDaysStr, [CommandParameter(Remainder = true)] string reason)
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

            if (account.Home.BanEndTime < DateTime.UtcNow)
            {
                account.Home.BanEndTime = DateTime.UtcNow.AddDays(durationDays);
            }
            else
            {
                account.Home.BanEndTime = account.Home.BanEndTime.AddDays(durationDays);
            }
            
            if (durationDays >= 2000)
            {
                account.Home.BanEndTime = DateTime.MaxValue;
            }

            account.Avatar.Banned = true;
            
            if (string.IsNullOrWhiteSpace(reason))
            {
                reason = "Не указана"; // Если причина не задана, устанавливаем дефолтное значение
            }

            account.Home.BanReason = reason;

            if (Sessions.IsSessionActive(lowID))
            {
                Session session = Sessions.GetSession(lowID);
                session.GameListener.SendTCPMessage(
                    new AuthenticationFailedMessage
                {
                    Message = durationDays >= 2000
                        ? $"Твой аккаунт был заблокирован НАВСЕГДА!\nПричина: {reason}\nЕсли не согласны с наказанием, обратитесь в поддержку: @pbs_manager."
                        : $"Твой аккаунт был заблокирован на {durationDays} дней!\nПричина: {reason}\nЕсли не согласны с наказанием, обратитесь в поддержку: @pbs_manager."
                });
                Sessions.Remove(lowID);
            }

            return $"Юзер с айди {playerId} был заблокирован на {durationDays} дней. Причина: {reason}.";
        }
    }
}