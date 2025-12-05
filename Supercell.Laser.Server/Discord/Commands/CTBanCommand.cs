namespace Supercell.Laser.Server.Discord.Commands
{
    using System.Net;
    using NetCord.Services.Commands;
    using Supercell.Laser.Server.Settings;
    public class CTBan : CommandModule<CommandContext>
    {
        [Command("ctban")]
        public static string BanCtCommand([CommandParameter(Remainder = true)] string country)
        {
            if (!Configuration.Instance.antiddos)
            {
                return "Anti-DDoS is disabled. Enable it in config.json to use this command.";
            }

            if (IsCountryBanned(country))
            {
                return $"Страна {country} уже заблокирован.";
            }

            try
            {
                File.AppendAllText("ban_country.txt", country + Environment.NewLine);
                return $"Страна {country} была заблокирована.";
            }
            catch (Exception ex)
            {
                return $"Failed to ban country: {ex.Message}";
            }
        }
        private static bool IsCountryBanned(string country)
        {
            if (!File.Exists("ban_country.txt"))
            {
                return false;
            }

            string[] bannedCountries = File.ReadAllLines("ban_country.txt");
            return bannedCountries.Contains(country);
        }
    }
}