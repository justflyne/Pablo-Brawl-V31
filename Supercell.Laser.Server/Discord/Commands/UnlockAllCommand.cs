namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Logic.Message.Account.Auth;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Networking.Session;
    public class UnlockAll : CommandModule<CommandContext> // TODO: REWRITE THIS SHITTY FUCKING COMMAND
    {
        [Command("unlockall")]
        public static string UnlockAllCommand([CommandParameter(Remainder = true)] string playerId)
        {
            if (!playerId.StartsWith("#"))
            {
                return "Невалидный тэг игрока. Убедись что тэг начинается с '#'.";
            }

            long id = LogicLongCodeGenerator.ToId(playerId);
            Account account = Accounts.Load(id);

            if (account == null)
            {
                return $"Юзер с айди {playerId} не найден.";
            }

            try
            {
                account.Avatar.AddDiamonds(99999);
                account.Avatar.StarPoints += 99999;
                account.Avatar.AddGold(99999);

                List<int> allBrawlers =
                    new()
                    {
                        0,
                        1,
                        2,
                        3,
                        4,
                        5,
                        6,
                        7,
                        8,
                        9,
                        10,
                        11,
                        12,
                        13,
                        14,
                        15,
                        16,
                        17,
                        18,
                        19,
                        20,
                        21,
                        22,
                        23,
                        24,
                        25,
                        26,
                        27,
                        28,
                        29,
                        30,
                        31,
                        32,
                        34,
                        35,
                        36,
                        37,
                        38                    
                    };

                foreach (int brawlerId in allBrawlers)
                {
                    if (brawlerId == 0)
                    {
                        CharacterData character = DataTables
                            .Get(16)
                            .GetDataWithId<CharacterData>(0);
                        if (character == null)
                            continue;

                        CardData card = DataTables
                            .Get(23)
                            .GetData<CardData>(character.Name + "_unlock");
                        if (card == null)
                            continue;

                        account.Avatar.UnlockHero(character.GetGlobalId(), card.GetGlobalId());

                        Hero hero = account.Avatar.GetHero(character.GetGlobalId());
                        if (hero != null)
                        {
                            hero.PowerPoints = 860;
                            hero.PowerLevel = 8;
                            hero.HasStarpower = true;

                            CardData starPower1 = DataTables
                                .Get(23)
                                .GetData<CardData>(character.Name + "_unique");
                            CardData starPower2 = DataTables
                                .Get(23)
                                .GetData<CardData>(character.Name + "_unique_2");
                            CardData starPower3 = DataTables
                                .Get(23)
                                .GetData<CardData>(character.Name + "_unique_3");

                            if (starPower1 != null)
                                account.Avatar.Starpowers.Add(starPower1.GetGlobalId());
                            if (starPower2 != null)
                                account.Avatar.Starpowers.Add(starPower2.GetGlobalId());
                            if (starPower3 != null && !starPower3.LockedForChronos)
                                account.Avatar.Starpowers.Add(starPower3.GetGlobalId());

                            string[] gadgets = { "GrowBush", "Shield", "Heal", "Jump", "ShootAround", "DestroyPet", "PetSlam", "Slow", "Push", "Dash", "SpeedBoost", "BurstHeal", "Spin", "Teleport", "Immunity", "Trail", "Totem", "Grab", "Swing", "Vision", "Regen", "HandGun", "Promote", "Sleep", "Slow", "Reload", "Fake", "Trampoline", "Explode", "Blink", "PoisonTrigger", "Barrage", "Focus", "MineTrigger", "Reload", "Seeker", "Meteor", "HealPotion", "Stun", "TurretBuff", "StaticDamage" };

                            string characterName =
                                char.ToUpper(character.Name[0]) + character.Name.Substring(1);
                            foreach (string gadgetName in gadgets)
                            {
                                CardData gadget = DataTables
                                    .Get(23)
                                    .GetData<CardData>(characterName + "_" + gadgetName);
                                if (gadget != null)
                                    account.Avatar.Starpowers.Add(gadget.GetGlobalId());
                            }
                        }
                        continue;
                    }

                    if (!account.Avatar.HasHero(16000000 + brawlerId))
                    {
                        CharacterData character = DataTables
                            .Get(16)
                            .GetDataWithId<CharacterData>(brawlerId);
                        if (character == null)
                            continue;

                        CardData card = DataTables
                            .Get(23)
                            .GetData<CardData>(character.Name + "_unlock");
                        if (card == null)
                            continue;

                        account.Avatar.UnlockHero(character.GetGlobalId(), card.GetGlobalId());

                        Hero hero = account.Avatar.GetHero(character.GetGlobalId());
                        if (hero != null)
                        {
                            hero.PowerPoints = 860;
                            hero.PowerLevel = 8;
                            hero.HasStarpower = true;

                            CardData starPower1 = DataTables
                                .Get(23)
                                .GetData<CardData>(character.Name + "_unique");
                            CardData starPower2 = DataTables
                                .Get(23)
                                .GetData<CardData>(character.Name + "_unique_2");
                            CardData starPower3 = DataTables
                                .Get(23)
                                .GetData<CardData>(character.Name + "_unique_3");

                            if (starPower1 != null)
                                account.Avatar.Starpowers.Add(starPower1.GetGlobalId());
                            if (starPower2 != null)
                                account.Avatar.Starpowers.Add(starPower2.GetGlobalId());
                            if (starPower3 != null && !starPower3.LockedForChronos)
                                account.Avatar.Starpowers.Add(starPower3.GetGlobalId());

                            string[] gadgets =
                            {
                                "GrowBush",
                                "Shield",
                                "Heal",
                                "Jump",
                                "ShootAround",
                                "DestroyPet",
                                "PetSlam",
                                "Slow",
                                "Push",
                                "Dash",
                                "SpeedBoost",
                                "BurstHeal",
                                "Spin",
                                "Teleport",
                                "Immunity",
                                "Trail",
                                "Totem",
                                "Grab",
                                "Swing",
                                "Vision",
                                "Regen",
                                "HandGun",
                                "Promote",
                                "Sleep",
                                "Slow",
                                "Reload",
                                "Fake",
                                "Trampoline",
                                "Explode",
                                "Blink",
                                "PoisonTrigger",
                                "Barrage",
                                "Focus",
                                "MineTrigger",
                                "Reload",
                                "Seeker",
                                "Meteor",
                                "HealPotion",
                                "Stun",
                                "TurretBuff",
                                "StaticDamage"
                            };

                            string characterName =
                                char.ToUpper(character.Name[0]) + character.Name.Substring(1);
                            foreach (string gadgetName in gadgets)
                            {
                                CardData gadget = DataTables
                                    .Get(23)
                                    .GetData<CardData>(characterName + "_" + gadgetName);
                                if (gadget != null)
                                    account.Avatar.Starpowers.Add(gadget.GetGlobalId());
                            }
                        }
                    }
                }

                List<string> skins =
                    new()
                    {
                        "Witch",
                        "Rockstar",
                        "Beach",
                        "Pink",
                        "Panda",
                        "White",
                        "Hair",
                        "Gold",
                        "Rudo",
                        "Bandita",
                        "Rey",
                        "Knight",
                        "Caveman",
                        "Dragon",
                        "Summer",
                        "Summertime",
                        "Pheonix",
                        "Greaser",
                        "GirlPrereg",
                        "Box",
                        "Santa",
                        "Chef",
                        "Boombox",
                        "Wizard",
                        "Reindeer",
                        "GalElf",
                        "Hat",
                        "Footbull",
                        "Popcorn",
                        "Hanbok",
                        "Cny",
                        "Valentine",
                        "WarsBox",
                        "Nightwitch",
                        "Cart",
                        "Shiba",
                        "GalBunny",
                        "Ms",
                        "GirlHotrod",
                        "Maple",
                        "RR",
                        "Mecha",
                        "MechaWhite",
                        "MechaNight",
                        "FootbullBlue",
                        "Outlaw",
                        "Hogrider",
                        "BoosterDefault",
                        "Shark",
                        "HoleBlue",
                        "BoxMoonFestival",
                        "WizardRed",
                        "Pirate",
                        "GirlWitch",
                        "KnightDark",
                        "DragonDark",
                        "DJ",
                        "Wolf",
                        "Brown",
                        "Total",
                        "Sally",
                        "Leonard",
                        "SantaRope",
                        "Gift",
                        "GT",
                        "Virus",
                        "BoosterVirus",
                        "Gamer",
                        "Valentines",
                        "Koala",
                        "BearKoala",
                        "AgentP",
                        "Football",
                        "Arena",
                        "Tanuki",
                        "Horus",
                        "ArenaPSG",
                        "DarkBunny",
                        "College",
                        "Bazaar",
                        "RedDragon",
                        "Constructor",
                        "Hawaii",
                        "Barbking",
                        "Trader",
                        "StationSummer",
                        "Silver",
                        "Bank",
                        "Retro",
                        "Ranger",
                        "Tracksuit",
                        "Knight",
                        "RetroAddon"
                    };

                foreach (Hero hero in account.Avatar.Heroes)
                {
                    CharacterData c = DataTables
                        .Get(DataType.Character)
                        .GetDataByGlobalId<CharacterData>(hero.CharacterId);
                    string cn = c.Name;
                    foreach (string name in skins)
                    {
                        SkinData s = DataTables.Get(DataType.Skin).GetData<SkinData>(cn + name);
                        if (s != null && !account.Home.UnlockedSkins.Contains(s.GetGlobalId()))
                        {
                            account.Home.UnlockedSkins.Add(s.GetGlobalId());
                        }
                    }
                }
                if (Sessions.IsSessionActive(id))
                {
                    Session session = Sessions.GetSession(id);
                    session.GameListener.SendTCPMessage(
                        new AuthenticationFailedMessage
                        {
                            Message =
                                "Твой аккаунт был обновлен, и были разблокированы все персонажи и улучшены на максимум!"
                        }
                    );
                    Sessions.Remove(id);
                }

                return $"Успешно открыли и улучшили на максимум для аккаунта {playerId}.";
            }
            catch (Exception ex)
            {
                return $"An error occurred while unlocking content: {ex.Message}";
            }
        }
    }

}