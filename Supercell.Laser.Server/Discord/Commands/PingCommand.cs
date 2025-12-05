namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    public class Ping : CommandModule<CommandContext>
    {
        [Command("ping")]
        public static string Pong() => "Понг!";
    }
}