namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Utils;

    public class ChangeUserCredentials : CommandModule<CommandContext>
    {
        [Command("changecredentials")]
        public static string ChangeUserCredentialsCommand(
            [CommandParameter(Remainder = true)] string input
        )
        {
            string[] parts = input.Split(' ');
            if (parts.Length != 3)
            {
                return "Usage: !changecredentials [TAG] [newUsername] [newPassword]";
            }

            long id = 0;
            bool sc = false;

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

            string newUsername = parts[1];
            string newPassword = parts[2];

            bool success = DatabaseHelper.ExecuteNonQuery(
                "UPDATE users SET username = @username, password = @password WHERE id = @id",
                ("@username", newUsername),
                ("@password", newPassword),
                ("@id", id)
            );

            if (!success)
            {
                return $"Failed to update credentials for player with ID {parts[0]}.";
            }

            string d = sc ? LogicLongCodeGenerator.ToCode(id) : parts[0];
            return $"Данные для {d} обновлены: Юзернейм = {newUsername}, Пароль = {newPassword}";
        }
    }
}