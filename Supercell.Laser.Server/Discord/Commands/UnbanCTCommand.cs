namespace Supercell.Laser.Server.Discord.Commands
{
    using System.Diagnostics;
    using System.Net;
    using NetCord.Services.Commands;

    public class UnBanCT : CommandModule<CommandContext>
    {
        [Command("ctunban")]
        public static string UnbanCtCommand([CommandParameter(Remainder = true)] string countrys)
        {            

            if (!IsCountryBanned(countrys))
            {
                return $"Страна {countrys} не была заблокирована.";
            }

            try
            {
                RemoveCountryFromBlacklist(countrys);

                return $"Страна {countrys} была разблокирована.";
            }
            catch (Exception ex)
            {
                return $"Failed to unban country: {ex.Message}";
            }
        }

        private static bool IsCountryBanned(string countrys)
        {
            if (!File.Exists("ban_country.txt"))
            {
                return false;
            }

            string[] bannedCountries = File.ReadAllLines("ban_country.txt");
            return bannedCountries.Contains(countrys);
        }        

        private static void RemoveCountryFromBlacklist(string countrys)
        {
            if (!File.Exists("ban_country.txt"))
            {
                return;
            }

            string[] bannedCountries = File.ReadAllLines("ban_country.txt");
            File.WriteAllLines("ban_country.txt", bannedCountries.Where(country => country != countrys));
        }
    }
}