namespace Supercell.Laser.Server
{
    using System;
    using System.Threading.Tasks;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Listener;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Logic;
    using Supercell.Laser.Server.Logic.Game;
    using Supercell.Laser.Server.Message;
    using Supercell.Laser.Server.Networking;
    using Supercell.Laser.Server.Networking.Session;
    using Supercell.Laser.Server.Settings;

    internal static class Resources
    {
        /// <summary>
        /// Initializes the databases
        /// </summary>
        public static void InitDatabase()
        {
            Accounts.Init();
            Alliances.Init();
        }

        /// <summary>
        /// Initializes the logic part of server
        /// </summary>
        public static void InitLogic()
        {
            Fingerprint.Load();
            DataTables.Load();
            Events.Init();
            Sessions.Init();
            Leaderboards.Init();
            // Battles.Init();
            // Matchmaking.Init();
            Teams.Init();
        }

        /// <summary>
        /// Initializes the network part of server
        /// </summary>
        public static void InitNetwork()
        {
            Processor.Init();
            Connections.Init();
            LogicServerListener.Instance = new ServerListener();
            //UDPGateway.Init("0.0.0.0", 6969);
            TCPGateway.Init("0.0.0.0", Configuration.Instance.Port);
        }

        /// <summary>
        /// Initializes the Discord bot
        /// </summary>
        public static void InitDiscord()
        {
            try
            {
                if (Configuration.Instance.BotToken == "YOUR_BOT_TOKEN_HERE" ||
                    Configuration.Instance.ChannelId == 1234567890123456789)
                {
                    Console.WriteLine("[DISCORD] To enable Discord integration, configure BotToken and ChannelId in config.json");
                    return;
                }

                if (!string.IsNullOrEmpty(Configuration.Instance.BotToken))
                {
                    var discordBot = new Discord.DiscordBot();
                    Task.Run(discordBot.StartAsync).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DISCORD] Failed to initialize Discord bot: {ex.Message}");
                Console.WriteLine("[DISCORD] Continuing without Discord integration...");
            }
        }                        
    }
}