namespace Supercell.Laser.Server.Discord.Commands
{
    using System;
    using NetCord.Services.Commands;
    using Supercell.Laser.Server.Settings;

    public class DeviceBan : CommandModule<CommandContext>
    {
        [Command("devban")]
        public static string DeviceBanCommand([CommandParameter(Remainder = true)] string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                return "Invalid DeviceId format.";
            }

            if (IsDeviceBanned(deviceId))
            {
                return $"Устройство с айди {deviceId} уже заблокирован.";
            }

            try
            {
                File.AppendAllText("banned_devices.txt", deviceId + Environment.NewLine);
                return $"Устройство с айди {deviceId} было заблокировано.";
            }
            catch (Exception ex)
            {
                return $"Failed to ban device: {ex.Message}";
            }
        }

        private static bool IsDeviceBanned(string deviceId)
        {
            if (!File.Exists("banned_devices.txt"))
            {
                return false;
            }

            string[] bannedDevices = File.ReadAllLines("banned_devices.txt");
            return bannedDevices.Contains(deviceId);
        }
    }
}