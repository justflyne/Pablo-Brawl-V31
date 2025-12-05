namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Command.Home;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Networking.Session;
    public class GiveDev : CommandModule<CommandContext>
    {
        [Command("givedev")]
        public static string GiveDevCommand(
            [CommandParameter(Remainder = true)] string parameters
        )
        {
            string[] parts = parameters.Split(' ');
            if (parts.Length != 2)
            {
                return "Usage: !giveprem [TAG] [DURATION_IN_MONTHS]";
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

            if (!int.TryParse(parts[1], out int durationMonths) || durationMonths <= 0)
            {
                return "Invalid duration. Please provide a positive number of months.";
            }

            Account account = Accounts.Load(id);
            if (account == null)
            {
                return $"Could not find player with ID {parts[0]}.";
            }

            if (account.Home.DevEndTime < DateTime.UtcNow)
            {
                account.Home.DevEndTime = DateTime.UtcNow.AddMonths(durationMonths);
            }
            else
            {
                account.Home.DevEndTime = account.Home.DevEndTime.AddMonths(durationMonths);
            }

            account.Avatar.DevLevel = 1;

            string formattedDate = account.Home.DevEndTime.ToString("dd'th of' MMMM yyyy");

            Notification n = new()
            {
                Id = 89,
                DonationCount = 999999,
                MessageEntry =
                    $"<c6>DEV-Статус активирован/продлён до {account.Home.DevEndTime} UTC! ({formattedDate})</c>"
            };

            account.Home.NotificationFactory.Add(n);

            LogicAddNotificationCommand acm = new() { Notification = n };

            AvailableServerCommandMessage asm = new() { Command = acm };

            if (Sessions.IsSessionActive(id))
            {
                Session session = Sessions.GetSession(id);
                session.GameListener.SendTCPMessage(asm);
            }                        

            string d = sc ? LogicLongCodeGenerator.ToCode(id) : parts[0];
            return $"Успешно: выдали разработчика для {d} активирован до {account.Home.DevEndTime} UTC! ({formattedDate})";
        }
    } 
}