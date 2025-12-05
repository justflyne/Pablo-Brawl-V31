namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Message.Account.Auth;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Networking.Session;
    using System;

    public class Unban : CommandModule<CommandContext>
    {
        [Command("unban")]
        public static string UnbanCommand([CommandParameter(Remainder = true)] string playerId)
        {
            if (!playerId.StartsWith("#"))
            {
                return "Невалидный тэг игрока. Убедись что тэг начинается с '#'.";
            }

            long lowID = LogicLongCodeGenerator.ToId(playerId);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Юзер с айди {playerId} не был найден.";
            }

            // Сбрасываем BanEndTime при разблокировке
            account.Home.BanEndTime = DateTime.MinValue;  // или DateTime.UtcNow, если хотите сбросить на текущее время

            account.Avatar.Banned = false;

            if (Sessions.IsSessionActive(lowID))
            {
                Session session = Sessions.GetSession(lowID);
                session.GameListener.SendTCPMessage(
                    new AuthenticationFailedMessage { Message = "Твой аккаунт был обновлен!" }
                );
                Sessions.Remove(lowID);
            }

            return $"Юзер с айди {playerId} был разблокирован.";
        }
    }
}