namespace Supercell.Laser.Logic.Home
{
    using System;
    using System.Collections.Immutable;
    using System.Numerics;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Supercell.Laser.Logic.Command.Home;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Home.Quest;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Logic.Message.Account.Auth;
    using Supercell.Laser.Titan.DataStream;

    [JsonObject(MemberSerialization.OptIn)]
    public class ClientHome
    {
        public const int DAILYOFFERS_COUNT = 6;

        public static readonly int[] GoldPacksPrice = new int[]
        {
            20, 50, 140, 280
        };

        public static readonly int[] GoldPacksAmount = new int[]
        {
            300, 800, 2400, 4200
        };

        [JsonProperty] public long HomeId;
        [JsonProperty] public int ThumbnailId;
        [JsonProperty] public int NameColorId;
        [JsonProperty] public int CharacterId;

        [JsonProperty] public List<OfferBundle> OfferBundles;

        [JsonProperty] public int TrophiesReward;
        [JsonProperty] public int TokenReward;
        [JsonProperty] public int StarTokenReward;
        [JsonProperty] public int StarPointsGained;
        [JsonProperty] public int CoinsGained;
        [JsonProperty] public int GemsGained;
        [JsonProperty] public int PowerPlayTrophiesReward;

        [JsonProperty] public BigInteger BrawlPassProgress;
        [JsonProperty] public BigInteger PremiumPassProgress;
        [JsonProperty] public int BrawlPassTokens;
        [JsonProperty] public bool HasPremiumPass;
        [JsonProperty] public BigInteger OldBrawlPassProgress;
        [JsonProperty] public BigInteger OldPremiumPassProgress;
        [JsonProperty] public int OldBrawlPassTokens;     
        [JsonProperty] public bool OldHasPremiumPass;   
        [JsonProperty] public List<int> UnlockedEmotes;

        [JsonProperty] public int Experience;
        [JsonProperty] public int TokenDoublers;

        [JsonProperty] public int TrophyRoadProgress;
        [JsonProperty] public Quests Quests;
        [JsonProperty] public NotificationFactory NotificationFactory;        
        [JsonProperty] public List<int> UnlockedSkins;
        [JsonProperty] public int[] SelectedSkins;
        [JsonProperty] public int PowerPlayGamesPlayed;
        [JsonProperty] public int PowerPlayScore;
        [JsonProperty] public int PowerPlayHighestScore;
        [JsonProperty] public int BattleTokens;
        [JsonProperty] public DateTime BattleTokensRefreshStart;        
        [JsonProperty] public DateTime DevEndTime;
        [JsonProperty] public DateTime PremiumEndTime;        
        [JsonProperty] public int DevLevel;
        [JsonProperty] public int PremiumLevel;
        [JsonProperty] public DateTime CreatorEndTime;
        [JsonProperty] public DateTime BanEndTime;
        [JsonProperty] public string BanReason;
        [JsonProperty] public DateTime MuteEndTime;
        [JsonProperty] public string MuteReason;
        [JsonProperty] public List<long> ReportsIds;
        [JsonProperty] public bool BlockFriendRequests;
        [JsonProperty] public bool HasIP;
        [JsonProperty] public string FirstIpAddress;
        [JsonProperty] public string IpAddress;
        [JsonProperty] public string Device;
        [JsonProperty] public string AndroidId;
        [JsonProperty] public List<string> OffersClaimed;
        [JsonProperty] public string Day;
        [JsonProperty] public int Theme;
        [JsonProperty] public int DayStreak;
        [JsonProperty] public int? LoadCode;
        [JsonProperty] public DateTime CooldownChangeName { get; set; } = new DateTime(1970, 1, 1);
        [JsonProperty] public int CostChangeName;
        [JsonProperty] public bool LobbyInfo;
        
        [JsonProperty] public DateTime CooldownKickedClan { get; set; } = new DateTime(1970, 1, 1);
        
        [JsonProperty] public string CustomLobbyInfo;
        
        [JsonProperty] public DateTime CooldownQuests;
        
        [JsonProperty] public int CreatorLevel;
        [JsonProperty] public int SocialAge = 1;

        [JsonIgnore] public EventData[] Events;        
        
        public PlayerThumbnailData Thumbnail => DataTables.Get(DataType.PlayerThumbnail).GetDataByGlobalId<PlayerThumbnailData>(ThumbnailId);
        public NameColorData NameColor => DataTables.Get(DataType.NameColor).GetDataByGlobalId<NameColorData>(NameColorId);
        public CharacterData Character => DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(CharacterId);

        public HomeMode HomeMode;
        public string ClientVersion;

        [JsonProperty] public DateTime LastVisitHomeTime;
        [JsonProperty] public DateTime LastRotateDate;

        [JsonIgnore] public bool ShouldUpdateDay;
        [JsonIgnore] public DateTime BattleStartTime;

        public ClientHome()
        {
            ThumbnailId = GlobalId.CreateGlobalId(28, 0);
            NameColorId = GlobalId.CreateGlobalId(43, 0);
            CharacterId = GlobalId.CreateGlobalId(16, 0);
            SelectedSkins = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            OfferBundles = new List<OfferBundle>();
            OffersClaimed = new List<string>();
            ReportsIds = new List<long>();
            UnlockedSkins = new List<int>();
            LastVisitHomeTime = DateTime.UnixEpoch;

            TrophyRoadProgress = 1;
            
            Theme = 17;
            
            CostChangeName = 0;
                                    
            LoadCode = null;

            BrawlPassProgress = 1;
            PremiumPassProgress = 1;
            OldBrawlPassProgress = 1;
            OldPremiumPassProgress = 1;
            
            LobbyInfo = true;
            
            PowerPlayScore = 0;

            UnlockedEmotes = new List<int>();
            BattleTokens = 500;
            BattleTokensRefreshStart = new();
            if (NotificationFactory == null)
            {
                NotificationFactory = new NotificationFactory();
            }

        }
        
        public class OfferJsonItem
        {
            public string Type { get; set; }
            public int Count { get; set; } = 1;
            public int BrawlerID { get; set; } = 0;
            public int Extra { get; set; }
        }

        public class OfferJsonBundle
        {
            public string Title { get; set; }
            public bool IsDailyDeals { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public string BackgroundExportName { get; set; }
            public int Currency { get; set; }
            public int Cost { get; set; }
            public int OldCost { get; set; }
            public int SaleStyle { get; set; }
            public int SaleAmount { get; set; }
            public string Claim { get; set; }
            public bool IsTrue { get; set; }
            public bool IsLobbyPopupOffer { get; set; }
            public List<OfferJsonItem> Items { get; set; }
        }
        
        public void GenerateNotification(
            DateTime OfferStart, DateTime OfferEnd,
            int _ID, string _MessageEntry, int _Count, 
            int _brawlerId, int _skin, string Claim)
        {
            if (TimerMath(OfferStart, OfferEnd) > -1 && !OffersClaimed.Contains(Claim)){
                OffersClaimed.Add(Claim);
                HomeMode.Home.NotificationFactory.Add(new Notification
                {
                    Id = _ID,
                    MessageEntry = _MessageEntry,
                    DonationCount = _Count,
                    brawlerId = _brawlerId,
                    skin = _skin,
                });
            }
        }
        
        public void UpdateNotification()
        {
           /* if (HasPremiumPass && !SelectedSkins.Contains(29000211))
            {
                GenerateNotification(
                    new DateTime(2025, 7, 31, 11, 0, 0), new DateTime(2025, 8, 31, 0, 0, 0),
                    94, "Compensation for lost skin: Trixie Colette", 1,
                    0, 211, "trixiecolettelost"
                );
            }
            
            if (HasPremiumPass && !SelectedSkins.Contains(29000213))
            {
                GenerateNotification(
                    new DateTime(2025, 7, 31, 11, 0, 0), new DateTime(2025, 8, 31, 0, 0, 0),
                    94, "Compensation for lost skin: Poco Starr", 1,
                    0, 213, "pocostarrlost"
                );
            }*/

        }

        public void HomeVisited()
        {

            RotateShopContent(DateTime.UtcNow, OfferBundles.Count == 0);
            LastVisitHomeTime = DateTime.UtcNow;
            //Quests = null;            
            UpdateOfferBundles();
            UpdateNotification();

            string Today = LastVisitHomeTime.ToString("d");
            if (Today != Day)
            {
                Day = Today;
            }

            if (Quests == null && TrophyRoadProgress >= 11)
            {
                Quests = new Quests();
                Quests.AddRandomQuests(HomeMode.Avatar.Heroes, 6);
            }
        }

        /*public void Tick()
        {
            LastVisitHomeTime = DateTime.UtcNow;
            TokenReward = 0;
            TrophiesReward = 0;
            StarTokenReward = 0;
            StarPointsGained = 0;
            CoinsGained = 0;
            PowerPlayTrophiesReward = 0;
        }*/

        public int TimerMath(DateTime timer_start, DateTime timer_end)
        {
            {
                TimeZoneInfo moscowTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");
                DateTime timer_now = TimeZoneInfo.ConvertTime(DateTime.UtcNow, moscowTimeZone);
                if (timer_now > timer_start)
                {
                    if (timer_now < timer_end)
                    {
                        int time_sec = (int)(timer_end - timer_now).TotalSeconds;
                        return time_sec;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
        }       
        public void Tick()
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo mskZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");
            DateTime nowMsk = TimeZoneInfo.ConvertTimeFromUtc(utcNow, mskZone);

            LastVisitHomeTime = nowMsk;

            while (ShouldAddTokens())
            {
                BattleTokensRefreshStart = BattleTokensRefreshStart.AddMinutes(30);
                BattleTokens = Math.Min(500, BattleTokens + 30);

                if (BattleTokens == 500)
                {
                    BattleTokensRefreshStart = new DateTime();
                    break;
                }
            }
            RotateShopContent(nowMsk, OfferBundles.Count == 0);
        }

        public int GetbattleTokensRefreshSeconds()
        {
            if (BattleTokensRefreshStart == new DateTime())
            {
                return -1;
            }
            return (int)BattleTokensRefreshStart.AddMinutes(30).Subtract(DateTime.UtcNow).TotalSeconds;
        }
        public bool ShouldAddTokens()
        {
            if (BattleTokensRefreshStart == new DateTime())
            {
                return false;
            }
            return GetbattleTokensRefreshSeconds() < 1;
        }

        public void PurchaseOffer(int index)
        {
            if (index < 0 || index >= OfferBundles.Count) return;

            OfferBundle bundle = OfferBundles[index];
            if (bundle.Purchased) return;

            if (bundle.Currency == 0)
            {
                if (!HomeMode.Avatar.UseDiamonds(bundle.Cost)) return;
            }
            else if (bundle.Currency == 1)
            {
                if (!HomeMode.Avatar.UseGold(bundle.Cost)) return;
            }
            else if (bundle.Currency == 3)
            {
                if (!HomeMode.Avatar.UseStarPoints(bundle.Cost)) return;
            }

            bundle.Purchased = true;

            if (bundle.Claim == "debug")
            {
                ;
            }
            else
            {
                OffersClaimed.Add(bundle.Claim);
            }


            LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
            Random rand = new Random();

            foreach (Offer offer in bundle.Items)
            {
                if (offer.Type == ShopItem.BrawlBox || offer.Type == ShopItem.FreeBox)
                {
                    for (int x = 0; x < offer.Count; x++)
                    {
                        DeliveryUnit unit = new DeliveryUnit(10);
                        HomeMode.SimulateGatcha(unit);
                        if (x + 1 != offer.Count)
                        {
                            command.Execute(HomeMode);
                        }
                        command.DeliveryUnits.Add(unit);
                    }
                }
                else if (offer.Type == ShopItem.HeroPower)
                {
                    DeliveryUnit unit = new DeliveryUnit(100);
                    GatchaDrop reward = new GatchaDrop(6);
                    reward.DataGlobalId = offer.ItemDataId;
                    reward.Count = offer.Count;
                    unit.AddDrop(reward);
                    command.DeliveryUnits.Add(unit);
                }
                else if (offer.Type == ShopItem.BigBox)
                {
                    for (int x = 0; x < offer.Count; x++)
                    {
                        DeliveryUnit unit = new DeliveryUnit(12);
                        HomeMode.SimulateGatcha(unit);
                        if (x + 1 != offer.Count)
                        {
                            command.Execute(HomeMode);
                        }
                        command.DeliveryUnits.Add(unit);
                    }
                }
                else if (offer.Type == ShopItem.MegaBox)
                {
                    for (int x = 0; x < offer.Count; x++)
                    {
                        DeliveryUnit unit = new DeliveryUnit(11);
                        HomeMode.SimulateGatcha(unit);
                        if (x + 1 != offer.Count)
                        {
                            command.Execute(HomeMode);
                        }
                        command.DeliveryUnits.Add(unit);
                    }
                }
                else if (offer.Type == ShopItem.OmegaBox)
                {
                    for (int x = 0; x < offer.Count; x++)
                    {
                        DeliveryUnit unit = new DeliveryUnit(11);
                        unit.TypeBox = 2;
                        HomeMode.SimulateGatcha(unit);
                        if (x + 1 != offer.Count)
                        {
                            command.Execute(HomeMode);
                        }
                        command.DeliveryUnits.Add(unit);
                    }
                }
                else if (offer.Type == ShopItem.Skin)
                {
                    DeliveryUnit unit = new DeliveryUnit(100);
                    GatchaDrop reward = new GatchaDrop(9);
                    reward.SkinGlobalId = GlobalId.CreateGlobalId(29, offer.SkinDataId);
                    unit.AddDrop(reward);
                    command.DeliveryUnits.Add(unit);
                }
                else if (offer.Type == ShopItem.Gems)
                {
                    DeliveryUnit unit = new DeliveryUnit(100);
                    GatchaDrop reward = new GatchaDrop(8);
                    reward.Count = offer.Count;
                    unit.AddDrop(reward);
                    command.DeliveryUnits.Add(unit);
                }
                else if (offer.Type == ShopItem.Coin)
                {
                    DeliveryUnit unit = new DeliveryUnit(100);
                    GatchaDrop reward = new GatchaDrop(7);
                    reward.Count = offer.Count;
                    unit.AddDrop(reward);
                    command.DeliveryUnits.Add(unit);
                }
                else if (offer.Type == ShopItem.GuaranteedHero)
                {
                    DeliveryUnit unit = new DeliveryUnit(100);
                    GatchaDrop reward = new GatchaDrop(1);
                    reward.DataGlobalId = offer.ItemDataId;
                    reward.Count = 1;
                    unit.AddDrop(reward);
                    command.DeliveryUnits.Add(unit);
                }
                else if (offer.Type == ShopItem.CoinDoubler)
                {
                    DeliveryUnit unit = new DeliveryUnit(100);
                    GatchaDrop reward = new GatchaDrop(2);
                    reward.Count = offer.Count;
                    unit.AddDrop(reward);
                    command.DeliveryUnits.Add(unit);
                }
                else if (offer.Type == ShopItem.EmoteBundle)
                {
                    DeliveryUnit unit = new DeliveryUnit(100);
                    List<int> Emotes_All = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 256, 257, 258, 259, 260, 261, 262, 263, 264, 265, 266, 267, 268, 269, 270, 271, 272, 273, 274, 275, 276, 277, 278, 279, 280, 281, 282, 283, 284, 285, 286, 287, 288, 289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300 };
                    List<int> Emotes_Locked = Emotes_All.Except(UnlockedEmotes).OrderBy(x => Guid.NewGuid()).Take(3).ToList(); ;

                    foreach (int x in Emotes_Locked)
                    {
                        GatchaDrop reward = new GatchaDrop(11);
                        reward.Count = 1;
                        reward.PinGlobalId = 52000000 + x;
                        unit.AddDrop(reward);
                        UnlockedEmotes.Add(x);
                    }
                    command.DeliveryUnits.Add(unit);
                }
                else if (offer.Type == ShopItem.RandomEmotes)
                {
                    DeliveryUnit unit = new DeliveryUnit(100);
                    List<int> Emotes_All = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 256, 257, 258, 259, 260, 261, 262, 263, 264, 265, 266, 267, 268, 269, 270, 271, 272, 273, 274, 275, 276, 277, 278, 279, 280, 281, 282, 283, 284, 285, 286, 287, 288, 289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300 };
                    List<int> Emotes_Locked = Emotes_All.Except(UnlockedEmotes).OrderBy(x => Guid.NewGuid()).Take(3).ToList(); ;

                    foreach (int x in Emotes_Locked)
                    {
                        GatchaDrop reward = new GatchaDrop(11);
                        reward.Count = 1;
                        reward.PinGlobalId = 52000000 + x;
                        unit.AddDrop(reward);
                        UnlockedEmotes.Add(x);
                    }
                    command.DeliveryUnits.Add(unit);
                }
                else if (offer.Type == ShopItem.Emote)
                {
                    DeliveryUnit unit = new DeliveryUnit(100);
                    GatchaDrop reward = new GatchaDrop(11);
                    reward.Count = 1;
                    reward.PinGlobalId = 52000155;
                    unit.AddDrop(reward);
                    command.DeliveryUnits.Add(unit);
                }
                else if (offer.Type == ShopItem.GuaranteedBox)
                {
                    var baseRarityPools = new Dictionary<int, List<int>>()
                    {
                           { 0, new List<int> { 1, 2, 3, 7, 8, 9, 14, 22, 30, 27 } }, // Common
                           { 1, new List<int> { 10, 6, 13, 24 } }, // Rare
                           { 2, new List<int> { 4, 18, 19, 34, 25 } }, // Super Rare 
                           { 3, new List<int> { 16, 20, 26, 29, 43, 15, 36 } }, // Epic
                           { 4, new List<int> { 37, 11, 17, 21, 32, 31 } }, // Mega Epic 
                           { 5, new List<int> { 12, 5, 28, 23 } }, // Legendary
                           { 6, new List<int> { 35, 38 } } // Chromatic
                    };

                    if (baseRarityPools.TryGetValue(offer.SkinDataId, out var pool))
                    {
                        var availableHeroes = pool.Where(heroId => !HomeMode.HasHeroUnlocked(16000000 + heroId)).ToList();

                        if (availableHeroes.Count > 0)
                        {
                            int randomItemId = availableHeroes[new Random().Next(availableHeroes.Count)];
                            var unit = new DeliveryUnit(100);
                            var reward = new GatchaDrop(1)
                            {
                                DataGlobalId = 16000000 + randomItemId,
                                Count = 1
                            };
                            unit.AddDrop(reward);
                            command.DeliveryUnits.Add(unit);
                        }
                        else
                        {
                           // если бойца нет то можно что то дать чтобы не крашнулось к хуям если автоматическая защита не сработает
                        }
                    }
                }
                else
                {
                    // todo...
                }

                command.Execute(HomeMode);


            }           
            UpdateOfferBundles();
            AvailableServerCommandMessage message = new AvailableServerCommandMessage();
            message.Command = command;
            HomeMode.GameListener.SendMessage(message);            
        }

        private void RotateShopContent(DateTime time, bool isNewAcc)
        {
            /*if (OfferBundles.Select(bundle => bundle.IsDailyDeals).ToArray().Length > 6)
            {
                OfferBundles.RemoveAll(bundle => bundle.IsDailyDeals);
            }*/
            bool IsUpdated = false;
            int offLen = OfferBundles.Count;
            OfferBundles.RemoveAll(offer => offer.EndTime <= time);
            bool WELCOME_OFFER = true;
            bool SHOULD_FREE3 = false;
            bool SHOULD_FREE4 = false;
            bool WELCOME_BP = true;
            bool WELCOME_100 = false;
            bool TROPHIES_1000 = true;

            foreach (OfferBundle o in OfferBundles)
            {
                if (o.Title == "Welcome 170")
                {
                    WELCOME_BP = false;
                }
                if (o.Title == "FIRST 100 PLAYER OFFER!")
                {
                    WELCOME_100 = false;
                }
                if (o.Title == "<cff0800>У<cff1000>р<cff1800>а<cff2000>а<cff2900>а<cff3100>а<cff3900>а<cff4100>а<cff4a00>а<cff5200>а<cff5a00>а<cff6200>а<cff6a00>а<cff7300>а<cff7b00>а<cff8300>а<cff8b00>а<cff9400>а<cff9c00>а<cffa400>а<cffac00>а<cffb400>а<cffbd00>а<cffc500>а<cfecd00>а<cffd500>а<cffde00>а<cffe600>а<cffee00>а<cfff600>а<cfffe00>а<cf6ff00>а<ceeff00>а<ce6ff00>а<cdeff00>а<cd5ff00>!<ccdff00>!<cc5ff00>!<cbdff00>!<cb4ff00> <cacff00>С<ca4ff00>б<c9cff00>р<c94ff00>о<c8bff00>с<c83ff00> <c7bff00>С<c73ff00>е<c6aff00>з<c62ff00>о<c5aff00>н<c52ff00>а<c4aff00>!<c41ff00>!<c39ff00>!<c31fe00>!<c29ff00>!<c20ff00>!<c18ff00>!<c10ff00>!<c08ff00>!<c01ff00>!</c>")
                {
                    WELCOME_OFFER = false;
                }
                if (o.Title == "Congrats on 1000 trophies!")
                {
                    TROPHIES_1000 = false;
                }
            }
                        
            if (WELCOME_OFFER)
            {
                OfferBundle bundle = new OfferBundle();
                bundle.Title = "Welcome offer";
                bundle.IsDailyDeals = false;
                bundle.EndTime = DateTime.UtcNow.AddDays(14); // tomorrow at 8:00 utc (11:00 MSK)
                bundle.BackgroundExportName = "offer_legendary";
                Offer megaBoxOffer = new Offer(ShopItem.MegaBox, 1);
                bundle.Items.Add(megaBoxOffer);
                bundle.Cost = 0;
                bundle.Currency = 0;
                //OfferBundles.Add(bundle);
            }
            if (SHOULD_FREE3)
            {
                OfferBundle bundle = new OfferBundle();
                bundle.Title = "<cff2400>F<cff4800>r<cff6d00>e<cfe9100>e<cffb600> <cffda00>O<cfffe00>f<cffff00>f<cdaff00>e<cb6ff00>r<c91ff00> <c6dfe00>#<c48ff00>7</c>";
                bundle.IsDailyDeals = false;
                bundle.EndTime = DateTime.UtcNow.AddDays(14); // tomorrow at 8:00 utc (11:00 MSK)
                bundle.BackgroundExportName = "offer_special";
                Offer megaBoxOffer = new Offer(ShopItem.MegaBox, 2);
                bundle.Items.Add(megaBoxOffer);
                bundle.Cost = 0;
                bundle.Currency = 0;
                OfferBundles.Add(bundle);
            }
            if (SHOULD_FREE4 && !SelectedSkins.Contains(29000178))
            {
                OfferBundle bundle = new OfferBundle();
                bundle.Title = "<cff1f00>F<cff3f00>i<cff5f00>n<cff7f00>a<cff9f00>l<cffbf00> <cffdf00>F<cffff00>r<cdfff00>e<cbfff00>e<c9fff00> <c7fff00>O<c5fff00>f<c3fff00>f<c1fff00>e<c00ff00>r</c>";
                bundle.IsDailyDeals = false;
                bundle.EndTime = DateTime.UtcNow.AddDays(14); // tomorrow at 8:00 utc (11:00 MSK)
                bundle.BackgroundExportName = "offer_legendary";
                Offer megaBoxOffer = new Offer(ShopItem.Skin, 1);
                //megaBoxOffer.ItemDataId = 178;
                megaBoxOffer.SkinDataId = 178;
                bundle.Items.Add(megaBoxOffer);
                bundle.Cost = 0;
                bundle.Currency = 0;
                OfferBundles.Add(bundle);
            }
            if (SHOULD_FREE3 || SHOULD_FREE4)
            {
                ShouldUpdateDay = true;
            }
            if (WELCOME_BP || WELCOME_100 || TROPHIES_1000)
            {
                ShouldUpdateDay = false;
            }
            IsUpdated = OfferBundles.Count != offLen;
            if (isNewAcc || DateTime.UtcNow.Hour >= 8) // Daily deals refresh at 08:00 AM UTC
            {
                //LastRotateDate = new DateTime();
                if (LastRotateDate < DateTime.UtcNow.Date)
                {
                    IsUpdated = true;
                    OfferBundles.RemoveAll(offer => offer.IsDailyDeals);
                    LastRotateDate = DateTime.UtcNow.Date;
                    UpdateDailyOfferBundles();
                    UpdateDailySkins();
                    LoadOffersFromJson();
                    
                    PowerPlayGamesPlayed = 0;
                    ReportsIds = new List<long>();
                    if (Quests != null && DateTime.Now >= CooldownQuests)
                    {
                        CooldownQuests = DateTime.Now.AddDays(3);
                        Quests.QuestList.RemoveAll(bundle => bundle.IsDailyQuest);
                        Quests.AddRandomQuests(HomeMode.Avatar.Heroes, Quests.QuestList.Count >= 18 ? 8 : 10);
                    }
                }
            }
            if (OfferBundles == null)
            {
                UpdateDailyOfferBundles();
                UpdateDailySkins();
                LoadOffersFromJson();
                
                IsUpdated = true;
            }
            else if (OfferBundles.Count == 0 || OfferBundles.All(b => b.IsJsonOffer))
            {
                UpdateDailyOfferBundles();
                UpdateDailySkins();
                LoadOffersFromJson();
                
                IsUpdated = true;
            }
            if (IsUpdated)
            {
                LogicDayChangedCommand newday = new()
                {
                    Home = this
                };
                newday.Home.Events = Events;
                AvailableServerCommandMessage eventupdated = new()
                {
                    Command = newday,
                };
                HomeMode.GameListener.SendMessage(eventupdated);
            }
        }
        
        private static readonly TimeZoneInfo MoscowZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");

private DateTime GetMoscowTime()
{
    return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, MoscowZone);
}

private DateTime ParseMoscowTime(string input)
{
    DateTime parsed = DateTime.Parse(input);
    
    if (parsed.Kind == DateTimeKind.Unspecified)
    {
        return TimeZoneInfo.ConvertTimeToUtc(parsed, MoscowZone);
    }
    
    if (parsed.Kind == DateTimeKind.Local)
    {
        return TimeZoneInfo.ConvertTimeToUtc(parsed, TimeZoneInfo.Local);
    }
    
    return TimeZoneInfo.ConvertTimeFromUtc(parsed, MoscowZone);
}

        private void UpdateOfferBundles()
        {
            string offersFilePath = "/root/PabloBrawl/prod31/Supercell.Laser.Server/JSON/offers.json";
        if (File.Exists(offersFilePath))
        {
            LoadOffersFromJson();
        }
        else
        {
            Console.WriteLine("offers.json не найден, фикси давай бля");
        }

            //OfferBundles.RemoveAll(bundle => bundle.IsTrue);
        }
        
        private void LoadOffersFromJson()
{
    try
    {
        string jsonPath = "/root/PabloBrawl/prod31/Supercell.Laser.Server/JSON/offers.json";
        string json = File.ReadAllText(jsonPath);
        List<OfferJsonBundle> bundles = JsonConvert.DeserializeObject<List<OfferJsonBundle>>(json);
        HashSet<string> jsonClaims = new HashSet<string>(bundles.Select(b => b.Claim));
        HashSet<string> claimsToRemove = new HashSet<string>();
        
        OfferBundles.RemoveAll(bundle => !jsonClaims.Contains(bundle.Claim) && !bundle.IsDailySkins && !bundle.IsDailyDeals);

        foreach (var incomingBundle in bundles)
        {
            DateTime nowMoscow = GetMoscowTime();
            DateTime startTime = ParseMoscowTime(incomingBundle.StartTime);
            DateTime endTime = ParseMoscowTime(incomingBundle.EndTime);

            if (endTime < nowMoscow) continue;

            bool hasUnlockedBrawler = false;
            bool hasUnlockedSkin = false;
            bool hasMaxLvlBrawler = false;
            float discountMultiplier = 1.0f;

            List<Offer> validItems = new List<Offer>();

            foreach (var item in incomingBundle.Items)
            {
                ShopItem shopItemType = Enum.Parse<ShopItem>(item.Type);
                int brawlerGlobalId = 16000000 + item.BrawlerID;
                int skinShopId = 29000000 + item.Extra;
                
            if (shopItemType == ShopItem.HeroPower)
            {                
                Hero hero = HomeMode.Avatar.GetHero(brawlerGlobalId);

                if (hero != null && (hero.PowerLevel >= 8 || hero.PowerPoints >= 1410))
                {
                    hasMaxLvlBrawler = true;
                    Console.WriteLine("пидарас с 9 лвл обнаружен");
                    continue;
                }
            }

            if (shopItemType == ShopItem.GuaranteedHero && HomeMode.HasHeroUnlocked(brawlerGlobalId))
            {
                hasUnlockedBrawler = true;
                continue;
            }

            if (shopItemType == ShopItem.Skin && UnlockedSkins.Contains(skinShopId))
            {
                hasUnlockedSkin = true;
                continue;
            }    

            Offer offer = new Offer(shopItemType, item.Count, brawlerGlobalId, item.Extra);

            validItems.Add(offer);
        }

            if (validItems.Count == 0)
            {
                claimsToRemove.Add(incomingBundle.Claim);
                continue;
            }

            if (hasUnlockedBrawler && hasUnlockedSkin && hasMaxLvlBrawler)
            {
                discountMultiplier *= 0.65f;                
            }
            else if (hasUnlockedBrawler && hasUnlockedSkin)
            {
                discountMultiplier *= 0.7f;
            }
            else if (hasUnlockedBrawler && hasMaxLvlBrawler)
            {
                discountMultiplier *= 0.75f;
            }
            else if (hasUnlockedBrawler)
            {
                discountMultiplier *= 0.8f;
            }
            else if (hasUnlockedSkin)
            {
                discountMultiplier *= 0.9f;
            }
            else if (hasMaxLvlBrawler)
            {
                discountMultiplier *= 0.95f;
            }

            int newCost = (int)(incomingBundle.Cost * discountMultiplier);
            int newOldCost = (int)(incomingBundle.OldCost * discountMultiplier);            

            var existingBundle = OfferBundles.FirstOrDefault(b => b.Claim == incomingBundle.Claim);

            if (existingBundle != null)
            {
                existingBundle.Title = incomingBundle.Title;
                existingBundle.IsDailyDeals = incomingBundle.IsDailyDeals;
                existingBundle.BackgroundExportName = incomingBundle.BackgroundExportName;
                existingBundle.Currency = incomingBundle.Currency;
                existingBundle.Cost = newCost;
                existingBundle.OldCost = newOldCost;
                existingBundle.SaleStyle = incomingBundle.SaleStyle;
                existingBundle.SaleAmount = incomingBundle.SaleAmount;
                existingBundle.IsJsonOffer = true;
                existingBundle.IsTrue = incomingBundle.IsTrue;
                existingBundle.IsLobbyPopupOffer = incomingBundle.IsLobbyPopupOffer;
                existingBundle.Items = validItems;
                existingBundle.Purchased = startTime > nowMoscow || OffersClaimed.Contains(existingBundle.Claim);
            }
            else
            {
                OfferBundle newBundle = new OfferBundle
                {
                    Title = incomingBundle.Title,
                    IsDailyDeals = incomingBundle.IsDailyDeals,
                    StartTime = startTime,
                    EndTime = endTime,
                    BackgroundExportName = incomingBundle.BackgroundExportName,
                    Currency = incomingBundle.Currency,
                    Cost = newCost,
                    OldCost = newOldCost,
                    SaleStyle = incomingBundle.SaleStyle,
                    SaleAmount = incomingBundle.SaleAmount,
                    IsJsonOffer = true,
                    IsTrue = incomingBundle.IsTrue,
                    IsLobbyPopupOffer = incomingBundle.IsLobbyPopupOffer,
                    Claim = incomingBundle.Claim,
                    Items = validItems,
                    Purchased = startTime > nowMoscow || OffersClaimed.Contains(incomingBundle.Claim)
                };

                OfferBundles.Add(newBundle);
            }
        }
        
        OfferBundles.RemoveAll(bundle => claimsToRemove.Contains(bundle.Claim));
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при загрузке акций: {ex.Message}");
    }
}

        public void GenerateOffer(
            DateTime OfferStart,
            DateTime OfferEnd,
            int Count,
            int BrawlerID,
            int Extra,
            ShopItem Item,
            int Cost,
            int OldCost,
            int Currency,
            string Claim,
            string Title,
            string BGR
            )
        {

            OfferBundle bundle = new OfferBundle();
            bundle.IsDailyDeals = false;
            bundle.IsTrue = true;
            bundle.EndTime = OfferEnd;
            bundle.Cost = Cost;
            bundle.OldCost = OldCost;
            bundle.Currency = Currency;
            bundle.Claim = Claim;
            bundle.Title = Title;
            bundle.BackgroundExportName = BGR;

            if (OffersClaimed.Contains(bundle.Claim))
            {
                bundle.Purchased = true;
            }
            if (TimerMath(OfferStart, OfferEnd) == -1)
            {
                bundle.Purchased = true;
            }
            if (HomeMode.HasHeroUnlocked(16000000 + BrawlerID))
            {
                bundle.Purchased = true;
            }

            Offer offer = new Offer(Item, Count, (16000000 + BrawlerID), Extra);
            bundle.Items.Add(offer);

            OfferBundles.Add(bundle);
        }
        
        public void GenerateOffer2(
            DateTime OfferStart,
            DateTime OfferEnd,
            int Count,
            int BrawlerID,
            int Extra,
            ShopItem Item,
            int Count2,
            int BrawlerID2,
            int Extra2,
            ShopItem Item2,
            int Cost,
            int OldCost,
            int Currency,
            string Claim,
            string Title,
            string BGR
            )
        {

            OfferBundle bundle = new OfferBundle();
            bundle.IsDailyDeals = false;
            bundle.IsTrue = true;
            bundle.EndTime = OfferEnd;
            bundle.Cost = Cost;
            bundle.OldCost = OldCost;
            bundle.Currency = Currency;
            bundle.Claim = Claim;
            bundle.Title = Title;
            bundle.BackgroundExportName = BGR;

            if (OffersClaimed.Contains(bundle.Claim))
            {
                bundle.Purchased = true;
            }
            if (TimerMath(OfferStart, OfferEnd) == -1)
            {
                bundle.Purchased = true;
            }
            if (HomeMode.HasHeroUnlocked(16000000 + BrawlerID))
            {
                bundle.Purchased = true;
            }

            Offer offer = new Offer(Item, Count, (16000000 + BrawlerID), Extra);
            bundle.Items.Add(offer);
            Offer offer2 = new Offer(Item2, Count2, (16000000 + BrawlerID2), Extra2);
            bundle.Items.Add(offer2);

            OfferBundles.Add(bundle);
        }

        public void GenerateOffer3(
            DateTime OfferStart,
            DateTime OfferEnd,
            int Count,
            int BrawlerID,
            int Extra,
            ShopItem Item,
            int Count2,
            int BrawlerID2,
            int Extra2,
            ShopItem Item2,
            int Count3,
            int BrawlerID3,
            int Extra3,
            ShopItem Item3,
            int Cost,
            int OldCost,
            int Currency,
            string Claim,
            string Title,
            string BGR
            )
        {

            OfferBundle bundle = new OfferBundle();
            bundle.IsDailyDeals = false;
            bundle.IsTrue = true;
            bundle.EndTime = OfferEnd;
            bundle.Cost = Cost;
            bundle.OldCost = OldCost;
            bundle.Currency = Currency;
            bundle.Claim = Claim;
            bundle.Title = Title;
            bundle.BackgroundExportName = BGR;

            if (OffersClaimed.Contains(bundle.Claim))
            {
                bundle.Purchased = true;
            }
            if (TimerMath(OfferStart, OfferEnd) == -1)
            {
                bundle.Purchased = true;
            }
            if (HomeMode.HasHeroUnlocked(16000000 + BrawlerID))
            {
                bundle.Purchased = true;
            }

            Offer offer = new Offer(Item, Count, (16000000 + BrawlerID), Extra);
            bundle.Items.Add(offer);
            Offer offer2 = new Offer(Item2, Count2, (16000000 + BrawlerID2), Extra2);
            bundle.Items.Add(offer2);
            Offer offer3 = new Offer(Item3, Count3, (16000000 + BrawlerID3), Extra3);
            bundle.Items.Add(offer3);

            OfferBundles.Add(bundle);
        }

        private void UpdateDailySkins()
        {
            List<string> skins = new() { "Witch", "Rockstar", "Beach", "Pink", "Panda", "White", "Hair", "Gold", "Rudo", "Bandita", "Rey", "Knight", "Caveman", "Dragon", "Summer", "Summertime", "Pheonix", "Greaser", "GirlPrereg", "Box", "Santa", "Chef", "Boombox", "Wizard", "Reindeer", "GalElf", "Hat", "Footbull", "Popcorn", "Hanbok", "Cny", "Valentine", "WarsBox", "Nightwitch", "Cart", "Shiba", "GalBunny", "Ms", "GirlHotrod", "Maple", "RR", "Mecha", "MechaWhite", "MechaNight", "FootbullBlue", "Outlaw", "Hogrider", "BoosterDefault", "Shark", "HoleBlue", "BoxMoonFestival", "WizardRed", "Pirate", "GirlWitch", "KnightDark", "DragonDark", "DJ", "Wolf", "Brown", "Total", "Sally", "Leonard", "SantaRope", "Gift", "GT", "SniperDefaultAddonBee", "SniperLadyBug", "SniperLadyBugAddonBee", "Virus", "BoosterVirus", "HoleStreetNinja", "Gamer", "Valentines", "Koala", "BearKoala", "TurretDefault", "AgentP", "Football", "Arena", "Tanuki", "Horus", "ArenaPSG", "DarkBunny", "College", "TurretTanuki", "TotemDefault", "Bazaar", "RedDragon", "Constructor", "Hawaii", "Barbking", "Trader", "StationSummer", "Silver", "SniperMonster", "BombMonster", "SniperMonsterAddonBee", "Bank", "Retro", "Ranger", "Tracksuit" };
            List<int> skis = new();
            List<int> starss = new();
            foreach (Hero h in HomeMode.Avatar.Heroes)
            {
                CharacterData c = DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(h.CharacterId);
                string cn = c.Name;
                foreach (string name in skins)
                {
                    SkinData s = DataTables.Get(DataType.Skin).GetData<SkinData>(cn + name);
                    if (s != null)
                    {
                        if (UnlockedSkins.Contains(s.GetGlobalId())) continue;
                        if (s.Name == "RocketGirlRanger") continue;
                        if (s.Name == "PowerLevelerKnight") continue;
                        if (s.Name == "BlowerTrader") continue;
                        if (s.Name == "BlowerTrader") continue;
                        if (s.CostLegendaryTrophies > 1)
                        {
                            starss.Add(s.GetGlobalId());
                            continue;
                        }
                        if (!s.Name.EndsWith("Gold"))
                            skis.Add(s.GetGlobalId());
                        else
                        {
                            string sss = s.Name.Replace("Gold", "Silver");
                            SkinData sc = DataTables.Get(DataType.Skin).GetData<SkinData>(sss);
                            if (sc == null)
                            {
                                skis.Add(s.GetGlobalId());
                                continue;
                            }
                            if (UnlockedSkins.Contains(sc.GetGlobalId()))
                            {
                                skis.Add(sc.GetGlobalId());
                            }
                        }
                    }
                }
            }


            Random random = new Random();
            int[] selectedElements = new int[Math.Min(skis.Count, 8)];
            for (int i = 0; i < Math.Min(skis.Count, 8); i++)
            {
                int randomIndex;
                do
                {
                    randomIndex = random.Next(0, skis.Count);
                } while (selectedElements.Contains(skis[randomIndex]));

                selectedElements[i] = skis[randomIndex];
            }

            foreach (int bbbbbb in selectedElements)
            {
                SkinData skin = DataTables.Get(DataType.Skin).GetDataByGlobalId<SkinData>(bbbbbb);
                OfferBundle bundle = new OfferBundle();
                bundle.IsDailyDeals = false;
                bundle.IsDailySkins = true;
                bundle.EndTime = DateTime.UtcNow.Date.AddDays(1).AddHours(8); // tomorrow at 8:00 utc (11:00 MSK)
                if (skin.CostGems > 0)
                {
                    int discountedCost = (int)Math.Floor(skin.CostGems * 0.85);
                    int roundedCost = (discountedCost / 10) * 10 - 1; 
                    bundle.Currency = 0;
                    bundle.Cost = roundedCost;
                    bundle.OldCost = (skin.CostGems - 1);
                }
                else if (skin.CostCoins > 0)
                {
                    bundle.Currency = 1;
                    bundle.Cost = (skin.CostCoins);
                }
                else
                {
                    continue;
                }

                Offer offer = new Offer(ShopItem.Skin, 1);
                offer.SkinDataId = GlobalId.GetInstanceId(bbbbbb);
                bundle.Items.Add(offer);
                OfferBundles.Add(bundle);
            }
            int[] selectedStElements = new int[Math.Min(starss.Count, 3)];
            for (int i = 0; i < Math.Min(starss.Count, 3); i++)
            {
                int randomIndex;
                do
                {
                    randomIndex = random.Next(0, starss.Count);
                } while (selectedStElements.Contains(starss[randomIndex]));

                selectedStElements[i] = starss[randomIndex];
            }
            foreach (int bbbbbb in selectedStElements)
            {
                SkinData skin = DataTables.Get(DataType.Skin).GetDataByGlobalId<SkinData>(bbbbbb);
                OfferBundle bundle = new OfferBundle();
                bundle.Currency = 3;
                bundle.IsDailyDeals = false;
                bundle.IsDailySkins = true;
                bundle.EndTime = DateTime.UtcNow.Date.AddDays(1).AddHours(8); // tomorrow at 8:00 utc (11:00 MSK)
                bundle.Cost = skin.CostLegendaryTrophies;

                Offer offer = new Offer(ShopItem.Skin, 1);
                offer.SkinDataId = GlobalId.GetInstanceId(bbbbbb);
                bundle.Items.Add(offer);
                OfferBundles.Add(bundle);
            }
        }

        private void UpdateDailyOfferBundles()
        {
            OfferBundles = new List<OfferBundle>();
            OfferBundles.Add(GenerateDailyGift());
            /*List<OfferBundle> dailyOffers = GenerateAllDailyOffers();
            if (dailyOffers != null)
            {
                OfferBundles.AddRange(dailyOffers);
            }*/

            List<Hero> unlockedHeroes = HomeMode.Avatar.Heroes;
            List<Hero> PossibleHeroes = new();
            foreach (Hero h in unlockedHeroes)
            {
                if (h.PowerLevel == 8) continue;
                if (h.PowerPoints >= 1410) continue;
                PossibleHeroes.Add(h);
            }
            Random random = new Random();
            bool shouldPowerPoints = true;
            bool hasMG = false;
            int offcount = 0;            
            for (int i = 1; i < DAILYOFFERS_COUNT; i++)
            {
                if (PossibleHeroes.Count == 0) break;
                offcount++;
                if (!hasMG && (random.Next(0, 100) > 33))
                {
                    i++;
                    offcount++;
                    hasMG = true;
                    OfferBundle a = GenerateDailyOffer(false, null);
                    if (a != null)
                    {
                        OfferBundles.Add(a);
                    }
                }
                int inds = random.Next(0, PossibleHeroes.Count);

                Hero brawler = PossibleHeroes[inds];
                PossibleHeroes.Remove(PossibleHeroes[inds]);
                OfferBundle dailyOffer = GenerateDailyOffer(shouldPowerPoints, brawler);                
                if (dailyOffer != null)
                {
                    if (!shouldPowerPoints) shouldPowerPoints = dailyOffer.Items[0].Type != ShopItem.HeroPower;
                    OfferBundles.Add(dailyOffer);
                }
            }
            if (offcount < 5 && !hasMG)
            {
                OfferBundle dailyOffer = GenerateDailyOffer(false, null);
                if (dailyOffer != null)
                {
                    OfferBundles.Add(dailyOffer);
                }
            }            
        }

        private OfferBundle GenerateDailyGift()
        {
            OfferBundle bundle = new OfferBundle();
            bundle.IsDailyDeals = true;
            bundle.EndTime = DateTime.UtcNow.Date.AddDays(1).AddHours(8); // Завтра в 8:00 UTC (11:00 MSK)

            Random random = new Random();
            int type = random.Next(0, 3); // 0 - FreeBox, 1 - MegaBox, 2 - Coins, 3 - PowerPoints                        

            switch (type)
            {
                case 0: // Бесплатный бокс
                    Offer freeBoxOffer = new Offer(ShopItem.FreeBox, 1);
                    bundle.Items.Add(freeBoxOffer);
                    bundle.Cost = 0;
                    break;
                case 1: // Мегабокс
                    Offer megaBoxOffer = new Offer(ShopItem.MegaBox, 1);
                    bundle.Items.Add(megaBoxOffer);
                    bundle.Cost = 0;
                    break;
                case 2: // Монеты
                    int coinAmount = random.Next(50, 201);
                    Offer coinOffer = new Offer(ShopItem.Coin, coinAmount);
                    bundle.Items.Add(coinOffer);
                    bundle.Cost = 0;
                    break;                
                }
            return bundle;
        }

        private OfferBundle GenerateDailyOffer(bool shouldPowerPoints, Hero brawler)
        {
            OfferBundle bundle = new OfferBundle();
            bundle.IsDailyDeals = true;
            bundle.EndTime = DateTime.UtcNow.Date.AddDays(1).AddHours(8); // tomorrow at 8:00 utc (11:00 MSK)

            Random random = new Random();
            int type = shouldPowerPoints ? 0 : 1; // getting a type

            switch (type)
            {
                case 0: // Power points

                    //for (int i = 0; i < Math.Min(PossibleHeroes.Count, 5))


                    int count = random.Next(15, 100) + 1;
                    Offer offer = new Offer(ShopItem.HeroPower, count, brawler.CharacterId);

                    bundle.Items.Add(offer);
                    bundle.Cost = count * 2;
                    bundle.Currency = 1;

                    break;
                case 1: // mega box
                    Offer megaBoxOffer = new Offer(ShopItem.MegaBox, 1);
                    bundle.Items.Add(megaBoxOffer);
                    bundle.Cost = 40;
                    bundle.OldCost = 80;
                    bundle.Currency = 0;
                    break;                
            }

            return bundle;
        }
        private int RoundToNearestEndingWith9(int price)
        {

            int tens = price / 10;
            int remainder = price % 10;

            if (remainder < 9)
            {
                return tens * 10 - (10 - 9);
            }
            else
            {
                return (tens + 1) * 10 - 1; // 187 → 190 - 1 = 189
            }
        }
        private List<OfferBundle> GenerateAllDailyOffers()
        {
            var random = new Random();
            var endTime = DateTime.UtcNow.Date.AddDays(1).AddHours(8);
            List<OfferBundle> allBundles = new List<OfferBundle>();

            OfferBundle cp = new OfferBundle
            {
                IsDailyDeals = false,
                IsDailyOffer = true,
                EndTime = endTime,
                Currency = 0,
                Title = "ОСОБАЯ АКЦИЯ"
            };

            int[] coinOptions = { 300, 350, 400, 450, 500, 600, 750, 900, 1000, 1500, 2000 };
            int[] powerOptions = { 100, 150, 200, 250, 300, 350, 400, 450, 500, 750, 1000 };

            int offerType = random.Next(4); // 0,1,2,3 → [0,1,2] = 75%, [3] = 25%

            bool addCoins = false;
            bool addPower = false;

            if (offerType == 0 || offerType == 1)
            {
                addCoins = true;
                addPower = true;
            }
            else if (offerType == 2)
            {
                addPower = true;                
            }
            else
            {
                addCoins = true;
            }

            int selectedCoin = addCoins ? coinOptions[random.Next(coinOptions.Length)] : 0;
            int selectedPower = 0;

            Offer coinOffer = null;
            Offer powerOffer = null;
            
            if (addCoins)
            {
                coinOffer = new Offer(ShopItem.Coin, selectedCoin);
                cp.Items.Add(coinOffer);                
            }

            if (addPower)
            {
                do
                {
                    selectedPower = powerOptions[random.Next(powerOptions.Length)];
                } while (selectedPower > selectedCoin);

                powerOffer = new Offer(ShopItem.WildcardPower, selectedPower);
                cp.Items.Add(powerOffer);                
            }
            if (addCoins && !addPower)
            {
                cp.BackgroundExportName = "offer_coins";
            }
            else if (!addCoins && addPower)
            {
                cp.BackgroundExportName = "offer_generic";
            }
            else
            {
                cp.BackgroundExportName = "offer_generic";
            }

            int totalPriceInDiamonds;
            int discountedPrice;

            if (addCoins && addPower)
            {
                totalPriceInDiamonds = (int)(selectedCoin * 0.10) + (int)(selectedPower * 0.45);
                discountedPrice = (int)(totalPriceInDiamonds * 0.75f);                
            }
            else if (addCoins)
            {
                totalPriceInDiamonds = (int)(selectedCoin * 0.10);
                discountedPrice = (int)(totalPriceInDiamonds * 0.85f);                
            }
            else if (addPower)
            {
                totalPriceInDiamonds = (int)(selectedPower * 0.45);
                discountedPrice = (int)(totalPriceInDiamonds * 0.80f);                
            }
            else
            {
                throw new InvalidOperationException("Пустая акция");
            }

            discountedPrice = RoundToNearestEndingWith9(discountedPrice);
            totalPriceInDiamonds = RoundToNearestEndingWith9(totalPriceInDiamonds);

            cp.Cost = discountedPrice;
            cp.OldCost = totalPriceInDiamonds;

            allBundles.Add(cp);                        

            OfferBundle box = new OfferBundle
            {
                IsDailyDeals = false,
                IsDailyOffer = true,
                EndTime = endTime,
                Currency = 0,
                Title = "ОСОБАЯ АКЦИЯ",
                BackgroundExportName = "offer_generic"
            };

            int boxCount = random.Next(1, 4); // от 1 до 3 ящиков
            int pricePerBox = 80;
            float discount = 0.75f;

            int finalPrice = (int)(pricePerBox * discount * boxCount);

            box.Cost = (int)(finalPrice - 1);
            box.OldCost = (int)(boxCount * pricePerBox - 1);

            var boxOffer = new Offer(ShopItem.MegaBox, boxCount);
            box.Items.Add(boxOffer);

            allBundles.Add(box);

            OfferBundle guaranteedBox = new OfferBundle
            {
                IsDailyDeals = false,
                IsDailyOffer = true,
                EndTime = endTime,
                Currency = 0,
                Title = "ОСОБАЯ АКЦИЯ"
            };

            int boxRarity = random.Next(3, 6); // 3 - Epic, 4 - Mega Epic, 5 - Legendary
            int baseCost = boxRarity switch
            {
                3 => 149,
                4 => 349,
                5 => 699,
                _ => 350
            };
    
            int discountedCost = baseCost switch
            {
                149 => 89,
                349 => 209,
                699 => 419,
                _ => 350
            };

            var guaranteedBoxOffer = new Offer(ShopItem.GuaranteedBox, 1)
            {
                SkinDataId = GlobalId.GetInstanceId(boxRarity)
            };

            guaranteedBox.Items.Add(guaranteedBoxOffer);
            guaranteedBox.Cost = discountedCost;
            guaranteedBox.OldCost = baseCost;

            allBundles.Add(guaranteedBox);

            OfferBundle brawler = new OfferBundle
            {
                IsDailyDeals = false,
                IsDailyOffer = true,
                EndTime = endTime,                
                Currency = 0,
                Title = "ОСОБАЯ АКЦИЯ"
            };

            var baseRarityPools = new Dictionary<int, List<int>>()
            {
                { 3, new List<int> { 15, 16, 20, 26, 29, 36 } }, // Epic
                { 4, new List<int> { 11, 17, 21, 31, 32, 37 } },     // Mega Epic
                { 5, new List<int> { 5, 12, 23, 28 } },               // Legendary
            };

            List<int> availableCharacterIds = baseRarityPools.Values.SelectMany(x => x).ToList();

            List<int> unlockableCharacters = availableCharacterIds
                .Where(id => !HomeMode.HasHeroUnlocked(GlobalId.CreateGlobalId(16, id)))
                .ToList();

            if (unlockableCharacters.Count > 0)
            {
                int selectedCharacterId = unlockableCharacters[random.Next(unlockableCharacters.Count)];
                CharacterData selectedCharacter = DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(selectedCharacterId);

                int basePrice = 0;
                int discountedBPrice = 0;

                foreach (var rarityPair in baseRarityPools)
                {
                    if (rarityPair.Value.Contains(selectedCharacterId))
                    {
                        switch (rarityPair.Key)
                        {
                            case 3:
                                basePrice = 149; 
                                discountedBPrice = 89;                                
                                break;
                            case 4:
                                basePrice = 349; 
                                discountedBPrice = 209; 
                                break;
                            case 5:
                                basePrice = 699; 
                                discountedBPrice = 419; 
                                break;
                        }
                        break;
                    }
                }

                var brawlerOffer = new Offer(ShopItem.GuaranteedHero, 1)
                {
                    ItemDataId = GlobalId.CreateGlobalId(16, selectedCharacterId)
                };
                brawler.Items.Add(brawlerOffer);
                brawler.Cost = discountedBPrice;
                brawler.OldCost = basePrice;

                string[] skinSuffixes = new[]
                {
                    "Witch", "Rockstar", "Beach", "Pink", "Panda", "White", "Hair", "Rudo", "Bandita",
                    "Rey", "Knight", "Caveman", "Dragon", "Summer", "Summertime", "Pheonix", "Greaser",
                    "GirlPrereg", "Box", "Santa", "Chef", "Boombox", "Wizard", "Reindeer", "GalElf",
                    "Hat", "Footbull", "Popcorn", "Hanbok", "Cny", "Valentine", "WarsBox", "Nightwitch",
                    "Cart", "Shiba", "GalBunny", "Ms", "GirlHotrod", "Maple", "RR", "Mecha", "MechaWhite",
                    "MechaNight", "FootbullBlue", "Outlaw", "Hogrider", "BoosterDefault", "Shark",
                    "HoleBlue", "BoxMoonFestival", "WizardRed", "Pirate", "GirlWitch", "KnightDark",
                    "DragonDark", "DJ", "Wolf", "Brown", "Total", "Sally", "Leonard", "SantaRope", "Gift",
                    "GT", "SniperDefaultAddonBee", "SniperLadyBug", "Virus", "BoosterVirus", "HoleStreetNinja",
                    "Gamer", "Valentines", "Koala", "BearKoala", "TurretDefault", "AgentP", "Football",
                    "Arena", "Tanuki", "Horus", "ArenaPSG", "DarkBunny", "College", "TurretTanuki",
                    "TotemDefault", "Bazaar", "RedDragon", "Constructor", "Hawaii", "Barbking", "Trader",
                    "StationSummer", "SniperMonster", "BombMonster"
                };

                SkinData selectedSkin = null;

                foreach (string suffix in skinSuffixes)
                {
                    string potentialSkinName = selectedCharacter.Name + suffix;
                    SkinData s = DataTables.Get(DataType.Skin).GetData<SkinData>(potentialSkinName);

                    if (s == null) continue;
                    int globalId = s.GetGlobalId();

                    if (UnlockedSkins.Contains(globalId)) continue;
                    if (new[] {"RocketGirlRanger", "PowerLevelerKnight", "BlowerTrader"}.Contains(s.Name)) continue;
                    if (s.CostLegendaryTrophies > 1) continue;
                    if (s.Name.EndsWith("Gold") || s.Name.EndsWith("Silver")) continue;

                    selectedSkin = s;
                    break;
                }

                if (selectedSkin != null)
                {
                    int skinGlobalId = selectedSkin.GetGlobalId();
                    int baseSkinPrice = selectedSkin.CostGems;

                    int discountedSPrice = baseSkinPrice switch
                    {
                        29 => 19,
                        49 => 29,
                        79 => 49,
                        149 => 79,
                        299 => 149,
                        _ => (int)(baseSkinPrice * 0.5f)
                    };

                    var skinOffer = new Offer(ShopItem.Skin, 1)
                    {
                        SkinDataId = GlobalId.GetInstanceId(skinGlobalId)
                    };
                    brawler.Items.Add(skinOffer);
                    brawler.Cost += (int)(discountedSPrice + 1);
                    brawler.OldCost += (int)(baseSkinPrice + 1);
                }

                allBundles.Add(brawler);
            }


            OfferBundle pinPack = new OfferBundle
            {
                IsDailyDeals = false,
                IsDailyOffer = true,
                EndTime = endTime,
                Currency = 0,
                Title = "ОСОБАЯ АКЦИЯ"
            };        

            var pinPackOffer = new Offer(ShopItem.RandomEmotes, 1);
            pinPack.Items.Add(pinPackOffer);
            pinPack.Cost = 29;
            pinPack.OldCost = 49;

            allBundles.Add(pinPack);

            return allBundles;
        }
        
        public void LogicDailyData(ByteStream encoder, DateTime utcNow)
        {

            encoder.WriteVInt(utcNow.Year * 1000 + utcNow.DayOfYear); // 0x78d4b8
            encoder.WriteVInt(utcNow.Hour * 3600 + utcNow.Minute * 60 + utcNow.Second); // 0x78d4cc
            encoder.WriteVInt(HomeMode.Avatar.Trophies); // 0x78d4e0 Player Trophies
            encoder.WriteVInt(HomeMode.Avatar.HighestTrophies); // 0x78d4f4 highest trophy
            encoder.WriteVInt(HomeMode.Avatar.HighestTrophies); // highest trophy again?
            encoder.WriteVInt(TrophyRoadProgress); // Trophy Road Reward
            encoder.WriteVInt(Experience + 1909); // Player Experience

            ByteStreamHelper.WriteDataReference(encoder, Thumbnail); // Player Profile Icon
            ByteStreamHelper.WriteDataReference(encoder, NameColorId); // Player Name Color

            encoder.WriteVInt(18); // Played game modes
            for (int i = 0; i < 18; i++)
            {
                encoder.WriteVInt(i);
            }

            encoder.WriteVInt(39); // Selected Skins Dictionary
            for (int i = 0; i < 39; i++)
            {
                encoder.WriteVInt(29);
                try
                {
                    encoder.WriteVInt(SelectedSkins[i]);
                }
                catch
                {
                    encoder.WriteVInt(0);
                    SelectedSkins = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                }
            }

            encoder.WriteVInt(UnlockedSkins.Count); // Unlocked Skins array
            foreach (int s in UnlockedSkins)
            {
                ByteStreamHelper.WriteDataReference(encoder, s);
            }

            encoder.WriteVInt(0);
            
            encoder.WriteVInt(0); // leaderboard region
            encoder.WriteVInt(HomeMode.Avatar.HighestTrophies); // 122
            encoder.WriteVInt(0); //tokens used in battles
            encoder.WriteVInt(0);
            encoder.WriteBoolean(true);
            encoder.WriteVInt(TokenDoublers); // Remaining Token Doubler
            DateTime psinasexico = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow")
            );
            DateTime SeasonTargetDateTime = new DateTime(2025, 10, 21, 12, 0, 0, 0);
            DateTime BpSeasonTargetDateTime = new DateTime(2025, 10, 21, 12, 0, 0, 0);
            DateTime PpSeasonTargetDateTime = new DateTime(2025, 10, 5, 12, 0, 0, 0);
            TimeSpan SeasonTimeDifference = SeasonTargetDateTime - psinasexico;
            TimeSpan BpSeasonTimeDifference = BpSeasonTargetDateTime - psinasexico;
            TimeSpan PpSeasonTimeDifference = PpSeasonTargetDateTime - psinasexico;
            int SeasonTimeRemaining = (int)SeasonTimeDifference.TotalSeconds;
            int BpSeasonTimeRemaining = (int)BpSeasonTimeDifference.TotalSeconds;
            int PpSeasonTimeRemaining = (int)PpSeasonTimeDifference.TotalSeconds;
            
            encoder.WriteVInt(SeasonTimeRemaining); // SEСON TIME
            encoder.WriteVInt(PpSeasonTimeRemaining); // power play timer
            encoder.WriteVInt(BpSeasonTimeRemaining); // 🦼 помощь нужна
            
            DateTime nowMoscow = GetMoscowTime();            
            int remainingSeconds = (int)(HomeMode.Home.CooldownChangeName - nowMoscow).TotalSeconds;

            encoder.WriteVInt(0);
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);

            encoder.WriteBoolean(false);
            encoder.WriteBoolean(false);
            encoder.WriteBoolean(true);
            encoder.WriteBoolean(false);
            encoder.WriteVInt(2); // Shop Token Doubler Related
            encoder.WriteVInt(2);
            encoder.WriteVInt(2);
            encoder.WriteVInt(CostChangeName); //name change cost
            encoder.WriteVInt(remainingSeconds); //name change timer

            encoder.WriteVInt(OfferBundles.Count); // Shop offers at 0x78e0c4
            foreach (OfferBundle offerBundle in OfferBundles)
            {
                offerBundle.Encode(encoder);
            }

            encoder.WriteVInt(0);

            encoder.WriteVInt(BattleTokens); // 0x78e228 Battle tokens
            encoder.WriteVInt(GetbattleTokensRefreshSeconds()); // 0x78e23c Time till Bonus Tokens
            encoder.WriteVInt(0); // 0x78e250
            encoder.WriteVInt(0); // 0x78e3a4 Tickets?
            encoder.WriteVInt(0); // 0x78e3a4

            ByteStreamHelper.WriteDataReference(encoder, Character); // Selected Brawler
            
            encoder.WriteString("RU");
            //encoder.WriteString("<cff1c00>T<cff3800>G<cff5500>:<cff7100> <cff8d00>@<cffaa00>p<cffc600>a<cffe200>b<cffff00>l<ce2ff00>o<cc6ff00>_<ca9ff00>s<c8dff00>e<c71ff00>r<c54ff00>v<c38ff00>e<c1cff00>r<c00ff00>s</c>");
            encoder.WriteString(HomeMode.Avatar.SupportedCreator);
            //encoder.WriteString(HomeMode.Avatar.Region);   
            
            string communityFilePath = "/root/PabloBrawl/prod31/Supercell.Laser.Server/JSON/events.json";
            if (!File.Exists(communityFilePath))
            {
                Console.WriteLine("[402] Файл events.json не найден!");
            }
            string communityJson = File.ReadAllText(communityFilePath);            
            JObject communityData = JObject.Parse(communityJson);
            int enabledCommunityEvent = 0;
            if (communityData["CommunityEvent"] != null && int.TryParse(communityData["CommunityEvent"].ToString(), out int parsedEnabledCMEvent))
            {
                enabledCommunityEvent = parsedEnabledCMEvent;
            }                        

            encoder.WriteVInt(12); // IntValueEntry
            {
                encoder.WriteInt(3);
                encoder.WriteInt(TokenReward); // tokens

                encoder.WriteInt(4);
                encoder.WriteInt(TrophiesReward); // trophies
                
                encoder.WriteInt(6);
                encoder.WriteInt(0); // demo account

                encoder.WriteInt(8);
                encoder.WriteInt(StarPointsGained); // star points

                encoder.WriteInt(7);
                encoder.WriteInt(HomeMode.Avatar.DoNotDisturb ? 1 : 0); // do not disturb

                encoder.WriteInt(9);
                encoder.WriteInt(1); // idk

                encoder.WriteInt(10);
                encoder.WriteInt(PowerPlayTrophiesReward); // power ply trophird
                
                encoder.WriteInt(14);
                encoder.WriteInt(CoinsGained);
                
                encoder.WriteInt(15);
                encoder.WriteInt(0);
                
                encoder.WriteInt(17);
                encoder.WriteInt(HomeMode.Avatar.DisabledTeamChat ? 1 : 0); // disable team chat
                
                encoder.WriteInt(18);
                encoder.WriteInt(enabledCommunityEvent); // cybersport button
                
                encoder.WriteInt(20);
                encoder.WriteInt(GemsGained);
            }            

            TokenReward = 0;
            TrophiesReward = 0;
            StarTokenReward = 0;
            StarPointsGained = 0;
            CoinsGained = 0;
            GemsGained = 0;
            PowerPlayTrophiesReward = 0;

            encoder.WriteVInt(0); // CoolDownEntry

            encoder.WriteVInt(1); // BrawlPassSeasonData
            {
                encoder.WriteVInt(3); //season id
                encoder.WriteVInt(BrawlPassTokens); //collected tokens
                //encoder.WriteVInt(PremiumPassProgress);
                encoder.WriteBoolean(HasPremiumPass); //is purchased
                encoder.WriteVInt(0); //collected tier

                if (encoder.WriteBoolean(true)) // Track 9
                {
                    encoder.WriteLongLong128(PremiumPassProgress);
                }
                if (encoder.WriteBoolean(true)) // Track 10
                {
                    encoder.WriteLongLong128(BrawlPassProgress);
                }
            }

            encoder.WriteVInt(1);
            {
                encoder.WriteVInt(2);
                encoder.WriteVInt(PowerPlayScore);
            }

            if (Quests != null)
            {
                encoder.WriteBoolean(true);
                Quests.Encode(encoder);
            }
            else
            {
                encoder.WriteBoolean(true);
                encoder.WriteVInt(0);
            }

            encoder.WriteBoolean(true); // Emojis Bool
            encoder.WriteVInt(UnlockedEmotes.Count);
            foreach (int i in UnlockedEmotes)
            {
                encoder.WriteVInt(52);
                encoder.WriteVInt(i);
                encoder.WriteVInt(1);
                encoder.WriteVInt(1);
                encoder.WriteVInt(1);
            }
        }

        public void LogicConfData(ByteStream encoder, DateTime utcNow)
        {
            encoder.WriteVInt(utcNow.Year * 1000 + utcNow.DayOfYear); // Shop Timestamp
            encoder.WriteVInt(100); // Brawl Box Tokens
            encoder.WriteVInt(10); // Big Box Tokens
            encoder.WriteVInt(30); // big box cost
            encoder.WriteVInt(3); // multiplier big box
            encoder.WriteVInt(80); // mega box cost
            encoder.WriteVInt(10); // multiplier megabox
            encoder.WriteVInt(20); // token doubler cost
            encoder.WriteVInt(1000); // token doubler count
            encoder.WriteVInt(550);
            encoder.WriteVInt(0);
            encoder.WriteVInt(999900);

            encoder.WriteVInt(0); // Array

            encoder.WriteVInt(9); // event slot array
            /*for (int i = 1; i <= 9; i++)
                encoder.WriteVInt(i);*/
            encoder.WriteVInt(1); //gem grab
            encoder.WriteVInt(2); //swowdown
            encoder.WriteVInt(3); //daily events
            encoder.WriteVInt(4); //team events
            encoder.WriteVInt(5); //duo showdown
            encoder.WriteVInt(6); //team events 2
            encoder.WriteVInt(7); //special events
            encoder.WriteVInt(8); //solo events
            encoder.WriteVInt(9); //power play

            encoder.WriteVInt(Events.Length);
            foreach (EventData data in Events)
            {
                data.IsSecondary = false;
                data.Encode(encoder);
            }

            encoder.WriteVInt(Events.Length);
            foreach (EventData data in Events)
            {
                data.IsSecondary = true;
                data.EndTime.AddSeconds((int)(data.EndTime - DateTime.Now).TotalSeconds);
                data.Encode(encoder);
            }

            encoder.WriteVInt(8);
            {
                encoder.WriteVInt(20);
                encoder.WriteVInt(35);
                encoder.WriteVInt(75);
                encoder.WriteVInt(140);
                encoder.WriteVInt(290);
                encoder.WriteVInt(480);
                encoder.WriteVInt(800);
                encoder.WriteVInt(1250);
            }

            encoder.WriteVInt(8);
            {
                encoder.WriteVInt(1);
                encoder.WriteVInt(2);
                encoder.WriteVInt(3);
                encoder.WriteVInt(4);
                encoder.WriteVInt(5);
                encoder.WriteVInt(10);
                encoder.WriteVInt(15);
                encoder.WriteVInt(20);
            }

            encoder.WriteVInt(3); // Tickets Price
            {
                encoder.WriteVInt(10);
                encoder.WriteVInt(30);
                encoder.WriteVInt(80);
            }

            encoder.WriteVInt(3); // Tickets Amount
            {
                encoder.WriteVInt(6);
                encoder.WriteVInt(20);
                encoder.WriteVInt(60);
            }

            ByteStreamHelper.WriteIntList(encoder, GoldPacksPrice);
            ByteStreamHelper.WriteIntList(encoder, GoldPacksAmount);

            encoder.WriteVInt(2);
            encoder.WriteVInt(500); // Max Battle Tokens
            encoder.WriteVInt(20); // Tokens Gained in Refresh

            encoder.WriteVInt(8640);
            encoder.WriteVInt(10);
            encoder.WriteVInt(5);

            encoder.WriteBoolean(false);
            encoder.WriteBoolean(false);
            encoder.WriteBoolean(false);

            encoder.WriteVInt(50);
            encoder.WriteVInt(604800);

            encoder.WriteBoolean(true); // Box boolean

            DateTime psina = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
            DateTime characterTargetDateTime = new DateTime(2025, 03, 15, 8, 0, 0);
            TimeSpan characterTimeDifference = characterTargetDateTime - psina;
            int characterSecondsRemaining = (int)characterTimeDifference.TotalSeconds;

            encoder.WriteVInt(1); // timer for 1 brawler

            encoder.WriteVInt(16); // csv
            encoder.WriteVInt(43); // brawler id
            encoder.WriteInt(characterSecondsRemaining); // timer
            encoder.WriteInt(1); // idk
                        
            string eventsFilePath = "/root/PabloBrawl/prod31/Supercell.Laser.Server/JSON/events.json";
            if (!File.Exists(eventsFilePath))
            {
                Console.WriteLine("[402] Файл events.json не найден!");
            }
            string eventsJson = File.ReadAllText(eventsFilePath);            
            JObject eventsData = JObject.Parse(eventsJson);
            int enabledTokensEvent = 0;
            int enabledCoinsEvent = 0;
            if (eventsData["TokensEvent"] != null && int.TryParse(eventsData["TokensEvent"].ToString(), out int parsedEnabledTEvent))
            {
                enabledTokensEvent = parsedEnabledTEvent;
            }
            if (eventsData["CoinsEvent"] != null && int.TryParse(eventsData["CoinsEvent"].ToString(), out int parsedEnabledCEvent))
            {
                enabledCoinsEvent = parsedEnabledCEvent;
            }
                        
            encoder.WriteVInt(12); // IntValueEntries
            {
                encoder.WriteInt(1);
                encoder.WriteInt(GlobalId.CreateGlobalId(41, Theme)); // theme

                encoder.WriteInt(5);
                encoder.WriteInt(0); // закрытие магазина
                
                encoder.WriteInt(6);
                encoder.WriteInt(0); // запретить боксы
                
                encoder.WriteInt(16);
                encoder.WriteInt(1); // вкладка видео в новостях
                
                encoder.WriteInt(15);
                encoder.WriteInt(0); // коды автора
                
                encoder.WriteInt(42);
                encoder.WriteInt(0); // коробки с банками
                
                encoder.WriteInt(46);
                encoder.WriteInt(1); // подсказки
                
                encoder.WriteInt(50);
                encoder.WriteInt(6); // подсказки
                             
                encoder.WriteInt(14);
                encoder.WriteInt(enabledTokensEvent); // ивент удвоители жетонов
                
                encoder.WriteInt(31);
                encoder.WriteInt(enabledCoinsEvent); // лавина монет
                
                encoder.WriteInt(17);
                encoder.WriteInt(1); // активация лунных банок

                encoder.WriteInt(37);
                encoder.WriteInt(0); // отруб бп
            }
        }

        public void Encode(ByteStream encoder)
        {
            DateTime utcNow = DateTime.UtcNow;

            LogicDailyData(encoder, utcNow);
            LogicConfData(encoder, utcNow);

            encoder.WriteVInt(0);
            encoder.WriteVInt(0);

            encoder.WriteLong(HomeId);
            NotificationFactory.Encode(encoder);

            encoder.WriteVInt(0);
            encoder.WriteBoolean(false);
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);
        }
    }
}
