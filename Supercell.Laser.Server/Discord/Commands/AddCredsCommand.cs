namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Utils;

    public class AddUserCredentials : CommandModule<CommandContext>
    {
        [Command("addcreds")]
        public static string AddUserCredentialsCommand(
            [CommandParameter(Remainder = true)] string input
        )
        {
            string[] parts = input.Split(' ');
            if (parts.Length != 2)
            {
                return "Usage: !addcreds [TAG] [Password]";
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

            string newPassword = parts[1];

            bool success = DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO users (password, id) VALUES (@password, @id);",
                ("@password", newPassword),
                ("@id", id)
            );

            if (!success)
            {
                return $"Failed to update credentials for player with ID {parts[0]}.";
            }

            string d = sc ? LogicLongCodeGenerator.ToCode(id) : parts[0];
            return $"Данные для {d} добавлены. Пароль = {newPassword}";
        }
    }
}