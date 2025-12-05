namespace Supercell.Laser.Server.Discord
{
    using NetCord;
    using NetCord.Gateway;
    using NetCord.Services;
    using NetCord.Services.Commands;
    using Supercell.Laser.Server.Settings;

    public class CustomLogger
    {
        public void Log(LogSeverity severity, string message, Exception exception = null)
        {
            string formattedMessage = $"[DISCORD] {message}";

            if (severity == LogSeverity.Info)
            {
                Console.WriteLine(formattedMessage);
            }
            else
            {
                Console.WriteLine($"[{severity}] {formattedMessage}");
            }
        }
    }

    public class DiscordBot
    {
        private readonly CustomLogger _logger = new();
        private GatewayClient _client;

        public async Task StartAsync()
        {
            _client = new GatewayClient(
                new BotToken(Configuration.Instance.BotToken),
                new GatewayClientConfiguration()
                {
                    Intents =
                        GatewayIntents.GuildMessages
                        | GatewayIntents.DirectMessages
                        | GatewayIntents.MessageContent,
                }
            );

            CommandService<CommandContext> commandService = new();
            commandService.AddModules(typeof(DiscordBot).Assembly);

            _client.MessageCreate += async message =>
            {
                try
                {
                    if (message.ChannelId != Configuration.Instance.ChannelId)
                        return;

                    if (!message.Content.StartsWith('!') || message.Author.IsBot)
                        return;

                    IExecutionResult result = await commandService.ExecuteAsync(
                        prefixLength: 1,
                        new CommandContext(message, _client)
                    ).ConfigureAwait(false);

                    if (result is IFailResult failResult)
                    {
                        await message.ReplyAsync(failResult.Message).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(LogSeverity.Error, ex.Message, ex);
                }
            };

            _client.Log += message =>
            {
                _logger.Log(message.Severity, message.Message, message.Exception);
                return default;
            };

            await _client.StartAsync().ConfigureAwait(false);
        }
    }
}
