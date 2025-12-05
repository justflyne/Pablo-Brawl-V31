namespace Supercell.Laser.Server.Commands
{
    using NetCord.Services.Commands;
    public class Help : CommandModule<CommandContext>
    {
        [Command("help")]
        public static string HelpCommand()
        {
            return "# Available Commands:\n"
                + "!help - показывает всё доступные команды\n"
                + "!status - показывает статус сервера\n"
                + "!ping - отвечает понг\n"
                + "!savecache - сохраняет кэш игроков и клубов\n"
                + "!unlockall - разблокирует ВСЁ на аккаунте (!unlockall [TAG])\n"
                + "!giveprem - выдаёт премиум для аккаунта (!giveprem [TAG] [DURATION])\n"
                + "!removeprem - убрать премиум для аккаунта (!removeprem [TAG])\n"
                + "!ban - заблокировать аккаунт (!ban [TAG])\n"
                + "!unban - разблокировать аккаунт (!unban [TAG])\n"
                + "!mute - замутить аккаунт (!mute [TAG])\n"
                + "!unmute - размутить аккаунт (!unmute [TAG])\n"
                + "!resetseason - сбрасывает трофейный сезон\n"
                + "!resettrophies - обнулить трофеи аккаунту (!resettrophies [TAG)\n"
                + "!changename - меняет ник пользователя (!changename [TAG] [newName])\n"
                + "!banip - блокирует IP (!banip [IP])\n"
                + "!unbanip - разблокирует IP (!unbanip [IP])\n"
                + "!battles - отправляет логи всех боёв\n"
                + "!reports - отправляет логи всех репортов в чате\n"
                + "!userinfo - показывает информацию о пользователе (!userinfo [TAG])\n"
                + "!changecredentials - меняет ник и пароль юзера (!changecredentials [TAG] [newUsername] [newPassword])\n"
                + "!addtrophies - устанавливает трофеи для определенного бойца (!addtrophies [TAG] [Trophies] [BrawlerId])\n"
                + "!addgems - выдаёт гемы пользователю (!addgems [TAG] [DonationCount])\n"
                + "!takegems - снять гемы пользователю (!takegems [TAG] [Count])\n"
                + "!addgold - добавить монеты пользователю (!addgold [TAG] [Count])\n"
                + "!takegold - снять монеты пользователю (!takegold [TAG] [Count])\n"
                + "!addstarpoints - добавить старпоинты пользователю (!addstarpoints [TAG] [Count])\n"
                + "!takestarpoints - снять старпоинты пользователю (!takestarpoints [TAG] [Count])\n"                
                + "!addbrawler - выдать бойца для аккаунта (!addbrawler [TAG] [ID])\n"
                + "!addskin - выдать скин для аккаунта (!addskin [TAG] [ID])\n"  
                + "!resetbrawlpass - обнулить всем Brawl Pass\n"       
                + "!maintenance - включить тех.перерыв";
        }
    }
}