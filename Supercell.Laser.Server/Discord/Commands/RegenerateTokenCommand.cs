namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Message.Account.Auth;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Utils;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Networking.Session;
    using System;

    public class RegenerateToken : CommandModule<CommandContext>
    {
        [Command("regentoken")]
        public static string RegenerateTokenCommand([CommandParameter(Remainder = true)] string playerId)
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

            account.Avatar.PassToken = Helpers.RandomString(40);

            if (Sessions.IsSessionActive(lowID))
            {
                Session session = Sessions.GetSession(lowID);
                session.GameListener.SendTCPMessage(
                    new AuthenticationFailedMessage { Message = "Твой аккаунт был обновлен!" }
                );
                Sessions.Remove(lowID);
            }

            return $"У юзера с айди {playerId} был обновлен токен!";
        }
    }
}