namespace Supercell.Laser.Server.Message
{
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System;
    using System.Collections.Generic;
    using System.IO;   
    using System.Text.RegularExpressions; 
    using MySql.Data.MySqlClient;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Net;
    using System.Net.Sockets;
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Avatar.Structures;
    using Supercell.Laser.Logic.Battle;
    using Supercell.Laser.Logic.Battle.Structures;
    using Supercell.Laser.Logic.Club;
    using Supercell.Laser.Logic.Command;
    using Supercell.Laser.Logic.Command.Avatar;
    using Supercell.Laser.Logic.Command.Home;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Friends;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Home.Quest;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Logic.Listener;
    using Supercell.Laser.Logic.Message;
    using Supercell.Laser.Logic.Message.Account;
    using Supercell.Laser.Logic.Message.Account.Auth;
    using Supercell.Laser.Logic.Message.Battle;
    using Supercell.Laser.Logic.Message.Club;
    using Supercell.Laser.Logic.Message.Friends;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Logic.Message.Latency;
    using Supercell.Laser.Logic.Message.Ranking;
    using Supercell.Laser.Logic.Message.Security;
    using Supercell.Laser.Logic.Message.Team;
    using Supercell.Laser.Logic.Message.Team.Stream;
    using Supercell.Laser.Logic.Message.Udp;
    using Supercell.Laser.Logic.Message.Debug;
    using Supercell.Laser.Logic.Stream.Entry;
    using Supercell.Laser.Logic.Team;
    using Supercell.Laser.Logic.Team.Stream;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Logic;
    using Supercell.Laser.Server.Logic.Game;
    using Supercell.Laser.Server.Networking;
    using Supercell.Laser.Server.Networking.Security;
    using Supercell.Laser.Server.Networking.Session;
    using Supercell.Laser.Server.Networking.UDP.Game;
    using Supercell.Laser.Server.Settings;
    using Supercell.Laser.Titan.Debug;

    public class MessageManager
    {
        public Connection Connection { get; }

        public HomeMode HomeMode;

        public CommandManager CommandManager;

        private DateTime LastKeepAlive;

        public MessageManager(Connection connection)
        {
            Connection = connection;
            LastKeepAlive = DateTime.UtcNow;
        }


        public bool IsAlive()
        {
            return (int)(DateTime.UtcNow - LastKeepAlive).TotalSeconds < 30;
        }
        public string GetPingIconByMs(int ms)
        {
            string str = "▂   ";
            if (ms <= 75)
            {
                str = "▂▄▆█";
            }
            else if (ms <= 125)
            {
                str = "▂▄▆ ";
            }
            else if (ms <= 300)
            {
                str = "▂▄  ";
            }
            return str;
        }
                        
        public class LobbyInfoData
        {            
            public string LobbyData { get; set; }            
        }       
        
        public void ShowLobbyInfo()
        {
            string jsonPath = "/root/PabloBrawl/prod31/Supercell.Laser.Server/JSON/serverinfo.json";
            string customTemplate = null;
            LobbyInfoData lobbyData = null;

            if (!string.IsNullOrEmpty(HomeMode.Home.CustomLobbyInfo))
            {
                customTemplate = HomeMode.Home.CustomLobbyInfo;
            }
            else
            {
                try
                {
                    if (File.Exists(jsonPath))
                    {
                        string jsonContent = File.ReadAllText(jsonPath);
                        lobbyData = JsonConvert.DeserializeObject<LobbyInfoData>(jsonContent);
                        if (!string.IsNullOrEmpty(lobbyData?.LobbyData))
                        {
                            customTemplate = lobbyData.LobbyData;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading JSON: {ex.Message}");
                }
            }
    
            string pingIcon = GetPingIconByMs(Connection.Ping);
            string pingInfo = $"{pingIcon} ({Connection.Ping}ms)";
            string hasPremium = HomeMode.Home.PremiumEndTime >= DateTime.UtcNow ? "Есть" : "Нету";
            DateTime now = DateTime.Now;                        
            DateTime startTime = Process.GetCurrentProcess().StartTime;
            TimeSpan uptime = now - startTime;

            string formattedUptime = string.Format(
                "{0}{1}{2}{3}",
                uptime.Days > 0 ? $"{uptime.Days} дней, " : string.Empty,
                uptime.Hours > 0 || uptime.Days > 0 ? $"{uptime.Hours} часов, " : string.Empty,
                uptime.Minutes > 0 || uptime.Hours > 0 ? $"{uptime.Minutes} минут, " : string.Empty,
                uptime.Seconds > 0 ? $"{uptime.Seconds} секунд" : string.Empty
            );

            if (string.IsNullOrEmpty(customTemplate))
            {
                customTemplate = $"Pablo Brawl\n<cff1c00>T<cff3800>G<cff5500>:<cff7100> <cff8d00>@<cffaa00>p<cffc600>a<cffe200>b<cffff00>l<ce2ff00>o<cc6ff00>_<ca9ff00>s<c8dff00>e<c71ff00>r<c54ff00>v<c38ff00>e<c1cff00>r<c00ff00>s</c>\nПинг: {pingInfo}\nОнлайн: {Sessions.Count}\nВремя: {now}\nПремиум: {hasPremium}\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
            }                        

            string processedMessage = customTemplate
                .Replace("{pingInfo}", pingInfo)
                .Replace("{onlineCount}", Sessions.Count.ToString())
                .Replace("{currentTime}", now.ToString())
                .Replace("{hasPremium}", hasPremium)
                .Replace("{uptime}", formattedUptime);

            string finalMessage = HomeMode.Home.LobbyInfo 
                ? processedMessage 
                : new string('\n', 200);

            var lobbyInfoPacket = new LobbyInfoMessage
            {
                LobbyData = $"{finalMessage}\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n",
                PlayersCount = 0
            };

            Connection.Send(lobbyInfoPacket);
        }

        public void ReceiveMessage(GameMessage message)
        {
            string techFilePath = "/root/PabloBrawl/prod31/Supercell.Laser.Server/JSON/serverinfo.json";
            string techJson = File.ReadAllText(techFilePath);
            JObject techData = JObject.Parse(techJson);

            bool maintenance = techData["Maintenance"]?.ToObject<bool>() ?? false;
            switch (message.GetMessageType())
            {
                case 10100:
                    ClientHelloReceived((ClientHelloMessage)message);
                    break;
                case 10101:
                    LoginReceived((AuthenticationMessage)message);
                    break;
                case 10107:
                    ClientCapabilitesReceived((ClientCapabilitiesMessage)message);
                    //Connection.Send(new StartLatencyTestRequestMessage());
                    break;
                case 10108:
                    LastKeepAlive = DateTime.UtcNow;
                    Connection.Send(new KeepAliveServerMessage());
                    ShowLobbyInfo();
                    if (maintenance || Sessions.Maintenance)
                    {
                        Connection.Send(new ShutdownStartedMessage());
                        Connection.Avatar.BattleStartTime = new DateTime();
                        
                        Thread.Sleep(1000 * 10);
                        
                        Connection.Send(new AuthenticationFailedMessage()
                        {
                            ErrorCode = 1,
                            Message = "Прости, на сервере ведутся технические работы. Попробуй, пожалуйста, позже."
                        });
                        return;
                    }
                    if (!Sessions.IsSessionActive(HomeMode.Avatar.AccountId))
                    {
                        Sessions.Create(HomeMode, Connection);
                    }
                    break;
                case 10110:
                    ShowLobbyInfo();
                    break;
                case 10119:
                    ReportAllianceStreamReceived((ReportAllianceStreamMessage)message);
                    break;
                case 10212:
                    ChangeName((ChangeAvatarNameMessage)message);
                    break;
                case 10177:
                    ClientInfoReceived((ClientInfoMessage)message);
                    break;
                case 10501:
                    AcceptFriendReceived((AcceptFriendMessage)message);
                    break;
                case 10502:
                    AddFriendReceived((AddFriendMessage)message);
                    break;
                case 10504:
                    AskForFriendListReceived((AskForFriendListMessage)message);
                    break;
                case 10506:
                    RemoveFriendReceived((RemoveFriendMessage)message);
                    break;
                case 10576:
                    SetBlockFriendRequestsReceived((SetBlockFriendRequestsMessage)message);
                    break;
                case 10555:
                    break;
                case 10767:
                    DebugGemsMessage((DebugGemsMessage)message);
                    break;
                case 10768:
                    DebugGoldMessage((DebugGoldMessage)message);
                    break;
                case 10769:
                    DebugSPMessage((DebugSPMessage)message);
                    break;
                case 10770:
                    DebugAllMessage((DebugAllMessage)message);
                    break;
                case 10771:
                    DebugPremMessage((DebugPremMessage)message);
                    break;
                case 10772:
                    DebugNTMessage((DebugNTMessage)message);
                    break;
                case 10773:
                    DebugTropMessage((DebugTropMessage)message);
                    break;
                case 14101:
                    GoHomeReceived((GoHomeMessage)message);
                    break;
                case 14102:
                    EndClientTurnReceived((EndClientTurnMessage)message);
                    break;
                case 14103:
                    MatchmakeRequestReceived((MatchmakeRequestMessage)message);
                    break;
                case 14104:
                    StartSpectateReceived((StartSpectateMessage)message);
                    break;
                case 14106:
                    CancelMatchMaking((CancelMatchmakingMessage)message);
                    break;
                case 14107:
                    StopSpectateReceived((StopSpectateMessage)message);
                    break;
                case 14109:
                    GoHomeFromOfflinePractiseReceived((GoHomeFromOfflinePractiseMessage)message);
                    break;
                case 14110:
                    AskForBattleEndReceived((AskForBattleEndMessage)message);
                    break;
                case 14113:
                    GetPlayerProfile((GetPlayerProfileMessage)message);
                    break;
                case 14114:
                    BattleLogMessageReceived((BattleLogMessage)message);
                    break;
                case 14166:
                    break;
                case 14177:
                    PlayAgainReceived((PlayAgainMessage)message);
                    break;
                case 14277:
                    GetSeasonRewardsReceived((GetSeasonRewardsMessage)message);
                    break;
                case 14301:
                    CreateAllianceReceived((CreateAllianceMessage)message);
                    break;
                case 14302:
                    AskForAllianceDataReceived((AskForAllianceDataMessage)message);
                    break;
                case 14303:
                    AskForJoinableAllianceListReceived((AskForJoinableAllianceListMessage)message);
                    break;
                case 14305:
                    JoinAllianceReceived((JoinAllianceMessage)message);
                    break;
                case 14306:
                    ChangeAllianceMemberRoleReceived((ChangeAllianceMemberRoleMessage)message);
                    break;
                case 14307:
                    KickAllianceMemberReceived((KickAllianceMemberMessage)message);
                    break;
                case 14308:
                    LeaveAllianceReceived((LeaveAllianceMessage)message);
                    break;
                case 14315:
                    ChatToAllianceStreamReceived((ChatToAllianceStreamMessage)message);
                    break;
                case 14316:
                    ChangeAllianceSettingsReceived((ChangeAllianceSettingsMessage)message);
                    break;
                case 14330:
                    SendAllianceMailMessage((SendAllianceMailMessage)message);
                    break;
                case 14350:
                    TeamCreateReceived((TeamCreateMessage)message);
                    break;
                case 14353:
                    TeamLeaveReceived((TeamLeaveMessage)message);
                    break;
                case 14354:
                    TeamChangeMemberSettingsReceived((TeamChangeMemberSettingsMessage)message);
                    break;
                case 14355:
                    TeamSetMemberReadyReceived((TeamSetMemberReadyMessage)message);
                    break;
                case 14358:
                    TeamSpectateMessageReceived((TeamSpectateMessage)message);
                    break;
                case 14359:
                    TeamChatReceived((TeamChatMessage)message);
                    break;
                case 14361:
                    TeamMemberStatusReceived((TeamMemberStatusMessage)message);
                    ShowLobbyInfo();
                    break;
                case 14362:
                    TeamSetEventReceived((TeamSetEventMessage)message);
                    break;
                case 14363:
                    TeamSetLocationReceived((TeamSetLocationMessage)message);
                    break;
                case 14365:
                    TeamInviteReceived((TeamInviteMessage)message);
                    break;
                case 14366:
                    PlayerStatusReceived((PlayerStatusMessage)message);
                    ShowLobbyInfo();
                    break;
                case 14367:
                    TeamClearInviteMessageReceived((TeamClearInviteMessage)message);
                    break;
                case 14369:
                    TeamPremadeChatReceived((TeamPremadeChatMessage)message);
                    break;
                case 14370:
                    TeamInviteReceived((TeamInviteMessage)message);
                    break;
                case 14403:
                    GetLeaderboardReceived((GetLeaderboardMessage)message);
                    break;
                case 14479:
                    TeamInvitationResponseReceived((TeamInvitationResponseMessage)message);
                    break;
                case 14600:
                    AvatarNameCheckRequestReceived((AvatarNameCheckRequestMessage)message);
                    break;
                case 14777:
                    DoNotDisturbReceived((DoNotDisturbMessage)message);
                    break;
                case 14778:
                    SetTeamChatMutedReceived((SetTeamChatMutedMessage)message);
                    break;
                case 18686:
                    SetSupportedCreator((SetSupportedCreatorMessage)message);
                    break;
                case 19001:
                    LatencyTestResultReceived((LatencyTestResultMessage)message);
                    break;               
                    //default:
                    //    Logger.Print($"MessageManager::ReceiveMessage - no case for {message.GetType().Name} ({message.GetMessageType()})");
                    //    break;
            }
        }

        private void TeamSpectateMessageReceived(TeamSpectateMessage message)
        {
            TeamEntry team = Teams.Get(message.TeamId);
            if (team == null) return;
            HomeMode.Avatar.TeamId = team.Id;
            TeamMember member = new TeamMember();
            member.AccountId = HomeMode.Avatar.AccountId;
            member.CharacterId = HomeMode.Home.CharacterId;
            member.DisplayData = new PlayerDisplayData(HomeMode.Home.HasPremiumPass, HomeMode.Home.ThumbnailId, HomeMode.Home.NameColorId, HomeMode.Avatar.Name);

            Hero hero = HomeMode.Avatar.GetHero(HomeMode.Home.CharacterId);
            member.HeroLevel = hero.PowerLevel;
            if (hero.HasStarpower)
            {
                CardData card = null;
                CharacterData cd = DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(hero.CharacterId);
                card = DataTables.Get(DataType.Card).GetData<CardData>(cd.Name + "_unique");
                CardData card2 = DataTables.Get(DataType.Card).GetData<CardData>(cd.Name + "_unique_2");
                if (HomeMode.Avatar.SelectedStarpowers.Contains(card.GetGlobalId()))
                {
                    member.HeroLevel = 9;
                    member.Starpower = card.GetGlobalId();
                }
                else if (HomeMode.Avatar.SelectedStarpowers.Contains(card2.GetGlobalId()))
                {
                    member.HeroLevel = 9;
                    member.Starpower = card2.GetGlobalId();
                }
                else if (HomeMode.Avatar.Starpowers.Contains(card.GetGlobalId()))
                {
                    member.HeroLevel = 9;
                    member.Starpower = card.GetGlobalId();
                }
                else if (HomeMode.Avatar.Starpowers.Contains(card2.GetGlobalId()))
                {
                    member.HeroLevel = 9;
                    member.Starpower = card2.GetGlobalId();
                }
            }
            else
            {
                member.Starpower = 0;
            }
            if (hero.PowerLevel > 5)
            {
                string[] cards = { "GrowBush", "Shield", "Heal", "Jump", "ShootAround", "DestroyPet", "PetSlam", "Slow", "Push", "Dash", "SpeedBoost", "BurstHeal", "Spin", "Teleport", "Immunity", "Trail", "Totem", "Grab", "Swing", "Vision", "Regen", "HandGun", "Promote", "Sleep", "Slow", "Reload", "Fake", "Trampoline", "Explode", "Blink", "PoisonTrigger", "Barrage", "Focus", "MineTrigger", "Reload", "Seeker", "Meteor", "HealPotion", "Stun", "TurretBuff", "StaticDamage" };
                CharacterData cd = DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(hero.CharacterId);
                CardData WildCard = null;
                foreach (string cardname in cards)
                {
                    string n = char.ToUpper(cd.Name[0]) + cd.Name.Substring(1);
                    WildCard = DataTables.Get(DataType.Card).GetData<CardData>(n + "_" + cardname);
                    if (WildCard != null)
                    {
                        if (HomeMode.Avatar.Starpowers.Contains(WildCard.GetGlobalId()))
                        {
                            member.Gadget = WildCard.GetGlobalId();
                            break;
                        }

                    }
                }
            }
            else
            {
                member.Gadget = 0;
            }
            member.HeroTrophies = hero.Trophies;
            member.HeroHighestTrophies = hero.HighestTrophies;

            member.IsOwner = false;
            member.State = 2;
            team.Members.Add(member);
            team.TeamUpdated();
        }

        private void SetBlockFriendRequestsReceived(SetBlockFriendRequestsMessage message)
        {
            //HomeMode.Home.BlockFriendRequests = message.State;
        }

        private void ReportAllianceStreamReceived(ReportAllianceStreamMessage message)
        {
            if (HomeMode.Avatar.AllianceId < 0) return;
            Alliance myAlliance = Alliances.Load(HomeMode.Avatar.AllianceId);
            if (myAlliance == null) return;
            if (HomeMode.Home.ReportsIds.Count > 5)
            {
                Connection.Send(new ReportUserStatusMessage()
                {
                    Status = 2
                });
                return;
            }
            long index = 0;
            foreach (AllianceStreamEntry e in myAlliance.Stream.GetEntries())
            {
                index++;
                if (e.Id == message.MessageIndex)
                {
                    if (HomeMode.Home.ReportsIds.Contains(e.AuthorId))
                    {
                        Connection.Send(new ReportUserStatusMessage()
                        {
                            Status = 3
                        });
                        return;
                    }
                    string reporterTag = LogicLongCodeGenerator.ToCode(HomeMode.Avatar.AccountId);
                    string reporterName = HomeMode.Avatar.Name;
                    string susTag = LogicLongCodeGenerator.ToCode(e.AuthorId);
                    string susName = e.AuthorName;
                    HomeMode.Home.ReportsIds.Add(e.AuthorId);
                    string text = "";
                    try { text += myAlliance.Stream.GetEntries()[index - 5].SendTime + " " + LogicLongCodeGenerator.ToCode(myAlliance.Stream.GetEntries()[index - 5].AuthorId) + " " + myAlliance.Stream.GetEntries()[index - 5].AuthorName + ">>> " + myAlliance.Stream.GetEntries()[index - 5].Message + "\n"; } catch { }
                    try { text += myAlliance.Stream.GetEntries()[index - 4].SendTime + " " + LogicLongCodeGenerator.ToCode(myAlliance.Stream.GetEntries()[index - 4].AuthorId) + " " + myAlliance.Stream.GetEntries()[index - 4].AuthorName + ">>> " + myAlliance.Stream.GetEntries()[index - 4].Message + "\n"; } catch { }
                    try { text += myAlliance.Stream.GetEntries()[index - 3].SendTime + " " + LogicLongCodeGenerator.ToCode(myAlliance.Stream.GetEntries()[index - 3].AuthorId) + " " + myAlliance.Stream.GetEntries()[index - 3].AuthorName + ">>> " + myAlliance.Stream.GetEntries()[index - 3].Message + "\n"; } catch { }
                    try { text += myAlliance.Stream.GetEntries()[index - 2].SendTime + " " + LogicLongCodeGenerator.ToCode(myAlliance.Stream.GetEntries()[index - 2].AuthorId) + " " + myAlliance.Stream.GetEntries()[index - 2].AuthorName + ">>> " + myAlliance.Stream.GetEntries()[index - 2].Message + "\n"; } catch { }
                    try { text += myAlliance.Stream.GetEntries()[index - 1].SendTime + " " + LogicLongCodeGenerator.ToCode(myAlliance.Stream.GetEntries()[index - 1].AuthorId) + " " + myAlliance.Stream.GetEntries()[index - 1].AuthorName + ">>> " + myAlliance.Stream.GetEntries()[index - 1].Message + "\n"; } catch { }
                    try { text += myAlliance.Stream.GetEntries()[index - 0].SendTime + " " + LogicLongCodeGenerator.ToCode(myAlliance.Stream.GetEntries()[index - 0].AuthorId) + " " + myAlliance.Stream.GetEntries()[index - 0].AuthorName + ">>> " + myAlliance.Stream.GetEntries()[index - 0].Message + "\n"; } catch { }
                    try { text += myAlliance.Stream.GetEntries()[index + 1].SendTime + " " + LogicLongCodeGenerator.ToCode(myAlliance.Stream.GetEntries()[index + 1].AuthorId) + " " + myAlliance.Stream.GetEntries()[index + 1].AuthorName + ">>> " + myAlliance.Stream.GetEntries()[index + 1].Message + "\n"; } catch { }
                    try { text += myAlliance.Stream.GetEntries()[index + 2].SendTime + " " + LogicLongCodeGenerator.ToCode(myAlliance.Stream.GetEntries()[index + 2].AuthorId) + " " + myAlliance.Stream.GetEntries()[index + 2].AuthorName + ">>> " + myAlliance.Stream.GetEntries()[index + 2].Message + "\n"; } catch { }
                    try { text += myAlliance.Stream.GetEntries()[index + 3].SendTime + " " + LogicLongCodeGenerator.ToCode(myAlliance.Stream.GetEntries()[index + 3].AuthorId) + " " + myAlliance.Stream.GetEntries()[index + 3].AuthorName + ">>> " + myAlliance.Stream.GetEntries()[index + 3].Message + "\n"; } catch { }
                    try { text += myAlliance.Stream.GetEntries()[index + 4].SendTime + " " + LogicLongCodeGenerator.ToCode(myAlliance.Stream.GetEntries()[index + 4].AuthorId) + " " + myAlliance.Stream.GetEntries()[index + 4].AuthorName + ">>> " + myAlliance.Stream.GetEntries()[index + 4].Message + "\n"; } catch { }
                    try { text += myAlliance.Stream.GetEntries()[index + 5].SendTime + " " + LogicLongCodeGenerator.ToCode(myAlliance.Stream.GetEntries()[index + 5].AuthorId) + " " + myAlliance.Stream.GetEntries()[index + 5].AuthorName + ">>> " + myAlliance.Stream.GetEntries()[index + 5].Message + "\n"; } catch { }
                    Logger.HandleReport($"Player {reporterName}, {reporterTag} reported player {susName}, {susTag}, in this msgs:\n`\n{text}`");
                    Connection.Send(new ReportUserStatusMessage()
                    {
                        Status = 1
                    });
                    break;
                }
            }
        }
        
        private void PlayAgainReceived(PlayAgainMessage message)
        {
            int slot = 1;
            Connection.Send(new PlayAgainStatusMessage());
            Matchmaking.RequestMatchmake(Connection, slot);
        }
        private void GetSeasonRewardsReceived(GetSeasonRewardsMessage message)
        {
            Connection.Send(new SeasonRewardsMessage());
        }
        private void addNotifToAllAccounts(string message, long club)
        {
            var allAccounts = Accounts.GetRankingList();
            foreach (var account in allAccounts)
            {
                string accountId = LogicLongCodeGenerator.ToCode(account.AccountId);
                addNotif(accountId, message, club);
            }
        }

        private void addNotif(string id, string message, long club)
        {
            long player = LogicLongCodeGenerator.ToId(id);
            Account targetAccount = Accounts.Load(player);
            if (targetAccount.Avatar.AllianceId == club)
            {
                if (targetAccount == null)
                {
                    Logger.Error($"Fail: account not found for ID {id}!");
                    return;
                }

                Account acc = new Account();

                Notification nGems = new Notification
                {                   
                    Id = 82,
                    Sender = Accounts.Load(HomeMode.Avatar.AccountId).Avatar.Name,                    
                    NameColorSender = Accounts.Load(HomeMode.Avatar.AccountId).Home.NameColorId,
                    MessageEntry = $"{message}"
                };
                targetAccount.Home.NotificationFactory.Add(nGems);
                LogicAddNotificationCommand acmGems = new()
                {
                    Notification = nGems
                };
                AvailableServerCommandMessage asmGems = new AvailableServerCommandMessage();
                asmGems.Command = acmGems;
                if (Sessions.IsSessionActive(player))
                {
                    var sessionGems = Sessions.GetSession(player);
                    sessionGems.GameListener.SendTCPMessage(asmGems);
                }
            }
        }
        private void DoNotDisturbReceived(DoNotDisturbMessage message)
        {
            HomeMode.Avatar.DoNotDisturb = message.State;
            LogicInviteBlockingChangedCommand command = new LogicInviteBlockingChangedCommand();
            command.State = message.State;
            AvailableServerCommandMessage serverCommandMessage = new AvailableServerCommandMessage();
            serverCommandMessage.Command = command;
            Connection.Send(serverCommandMessage);
            if (HomeMode.Avatar.AllianceId > 0)
            {
                Alliance a = Alliances.Load(HomeMode.Avatar.AllianceId);
                AllianceMember m = a.GetMemberById(HomeMode.Avatar.AccountId);
                m.DoNotDisturb = message.State;
                AllianceDataMessage dataMessage = new AllianceDataMessage()
                {
                    Alliance = a,
                    IsMyAlliance = true,
                };
                a.SendToAlliance(dataMessage);
            }
        }
        
        private void SetTeamChatMutedReceived(SetTeamChatMutedMessage message)
        {
            HomeMode.Avatar.DisabledTeamChat = message.State;
            LogicTeamChatMuteStateChangedCommand command = new LogicTeamChatMuteStateChangedCommand();            
            command.State = message.State;
            AvailableServerCommandMessage serverCommandMessage = new AvailableServerCommandMessage();
            serverCommandMessage.Command = command;
            Connection.Send(serverCommandMessage);
        }

        private static readonly object _creatorFileLock = new object();

        private void SetSupportedCreator(SetSupportedCreatorMessage message)
        {
            string creatorCode = message.Creator;

            if (string.IsNullOrWhiteSpace(creatorCode))
            {
                HomeMode.Avatar.SupportedCreator = null;

                LogicSetSupportedCreatorCommand successResponse = new()
                {
                    Name = string.Empty
                };

                AvailableServerCommandMessage msg = new()
                {
                    Command = successResponse
                };

                Connection.Send(msg);
                return;
            }
            string filePath = "/root/PabloBrawl/prod31/Supercell.Laser.Server/JSON/supp.json";
            long currentPlayerLowId = HomeMode.Avatar.AccountId;
            bool codeFound = false;
            JObject matchingCreator = null;

            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    JObject data = JObject.Parse(json);
                    JArray creators = data["Creators"] as JArray;

                    if (creators != null)
                    {
                        matchingCreator = creators
                            .OfType<JObject>()
                            .FirstOrDefault(c => c["Code"]?.ToString() == creatorCode);

                        if (matchingCreator != null)
                            codeFound = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка чтения JSON: {ex.Message}");
                }
            }

            if (!codeFound)
            {
                SetSupportedCreatorResponse failresponse = new();
                Connection.Send(failresponse);
                return;
            }
            
            HomeMode.Avatar.SupportedCreator = creatorCode;

            try
            {
                lock (_creatorFileLock)
                {
                    string json = File.ReadAllText(filePath);
                    JObject data = JObject.Parse(json);
                    JArray creators = data["Creators"] as JArray;

                    if (creators == null) throw new Exception("Creators array is null");

                    var targetCreator = creators
                        .OfType<JObject>()
                        .FirstOrDefault(c => c["Code"]?.ToString() == creatorCode);

                    if (targetCreator == null) throw new Exception("Creator not found after reload");
                    JArray activationHistory = targetCreator["ActivationHistory"] as JArray ?? new JArray();
                    List<long> activationHistoryList = activationHistory.ToObject<List<long>>() ?? new List<long>();

                    if (activationHistoryList.Contains(HomeMode.Avatar.AccountId))
                    {
                        Console.WriteLine($"Игрок {HomeMode.Avatar.AccountId} уже активировал код '{creatorCode}'.");
                    }
                    else
                    {
                        activationHistoryList.Add(HomeMode.Avatar.AccountId);
                        targetCreator["ActivationHistory"] = JArray.FromObject(activationHistoryList);
                        File.WriteAllText(filePath, data.ToString());
                    }

                    string tag = targetCreator["Tag"]?.ToString();
                    int rewardCount = targetCreator["RewardCount"]?.ToObject<int>() ?? 0;

                    if (!string.IsNullOrEmpty(tag) && rewardCount > 0 && !activationHistoryList.Contains(HomeMode.Avatar.AccountId))
                    {
                        long tagId = LogicLongCodeGenerator.ToId(tag);
                        Account codeAcc = Accounts.Load(tagId);
                        codeAcc.Avatar.Diamonds += rewardCount;
                        Console.WriteLine($"✅ Награда {rewardCount} выдана владельцу {tag} за код {creatorCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке награды: {ex.Message}");
            }
        
            LogicSetSupportedCreatorCommand response = new()
            {
                Name = creatorCode
            };

            AvailableServerCommandMessage msgFinal = new()
            {
                Command = response
            };

            Connection.Send(msgFinal);
        }

        private bool IsPromoCodeValid(string code, string filePath, out JObject promo)
        {
            promo = null;
            try
            {
                if (!File.Exists(filePath)) return false;

                string json = File.ReadAllText(filePath);
                JObject data = JObject.Parse(json);
                var promoObj = data["Promocodes"]?.FirstOrDefault(p => p["Code"]?.ToString() == code);
                if (promoObj == null) return false;

                DateTime nowUtc = DateTime.UtcNow;
                TimeZoneInfo moscowZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");
                DateTime nowMsk = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, moscowZone);

                string startTimeStr = promoObj["StartTime"]?.ToString();
                string endTimeStr = promoObj["EndTime"]?.ToString();

                if (!string.IsNullOrEmpty(startTimeStr) && DateTime.TryParse(startTimeStr, out DateTime startTimeMsk) && nowMsk < startTimeMsk)
                    return false;

                if (!string.IsNullOrEmpty(endTimeStr) && DateTime.TryParse(endTimeStr, out DateTime endTimeMsk) && nowMsk > endTimeMsk)
                    return false;

                promo = promoObj as JObject;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Reward(string code, JObject promo, long lowID, string filePath, List<long> history, ref int maxActivations)
        {
            Account account = Accounts.Load(lowID);
            string promocode = promo["Code"]?.ToString();
            JArray items = promo["Items"] as JArray;

            if (items == null || !items.HasValues)
                return;
            
            
                        
            var command = new LogicGiveDeliveryItemsCommand();
            var unit = new DeliveryUnit(100);
            bool isBox = false;

            foreach (JObject item in items)
            {
                string type = item["Type"]?.ToString().ToLower() ?? "";
                int count = item.ContainsKey("Count") ? (int)item["Count"] : 1;
                int brawlerId = item.ContainsKey("BrawlerID") ? (int)item["BrawlerID"] : 0;
                int skinId = item.ContainsKey("SkinID") ? (int)item["SkinID"] : 0;
                int months = item.ContainsKey("Months") ? (int)item["Months"] : 1;
                int extra = item.ContainsKey("Extra") ? (int)item["Extra"] : 0;

                switch (type)
                {                    
                    case "coin":
                        unit.AddDrop(new GatchaDrop(7) { Count = count });
                        break;
                    case "gems":
                        unit.AddDrop(new GatchaDrop(8) { Count = count });
                        break;
                    case "starpoints":
                        unit.AddDrop(new GatchaDrop(12) { Count = count });
                        break;
                    case "powerpoints":
                        unit.AddDrop(new GatchaDrop(6) { Count = count });
                        break;
                    case "thumbnail":
                        unit.AddDrop(new GatchaDrop(11)
                        {
                            PinGlobalId = GlobalId.CreateGlobalId(28, extra)
                        });
                        break;
                    case "emote":
                        unit.AddDrop(new GatchaDrop(11)
                        {
                            PinGlobalId = extra
                        });
                        break;
                    case "brawler":
                        unit.AddDrop(new GatchaDrop(1)
                        {
                            DataGlobalId = GlobalId.CreateGlobalId(16, brawlerId)
                        });
                        break;
                    case "skin":
                        unit.AddDrop(new GatchaDrop(9)
                        {
                            SkinGlobalId = GlobalId.CreateGlobalId(29, skinId)
                        });
                        break;    
                    case "omegabox":
                        isBox = true;
                        for (int x = 0; x < count; x++)
                        {
                            var boxUnit = new DeliveryUnit(11);
                            boxUnit.TypeBox = 2;
                            HomeMode.SimulateGatcha(boxUnit);
                            if (x + 1 != count)
                            {
                                command.Execute(HomeMode);
                            }
                            command.DeliveryUnits.Add(boxUnit);
                        }
                        break;                
                    case "megabox":
                        isBox = true;
                        for (int x = 0; x < count; x++)
                        {
                            var boxUnit = new DeliveryUnit(11);
                            HomeMode.SimulateGatcha(boxUnit);
                            if (x + 1 != count)
                            {
                                command.Execute(HomeMode);
                            }
                            command.DeliveryUnits.Add(boxUnit);
                        }
                        break;
                    case "bigbox":
                        isBox = true;
                        for (int x = 0; x < count; x++)
                        {
                            var boxUnit = new DeliveryUnit(12);
                            HomeMode.SimulateGatcha(boxUnit);
                            if (x + 1 != count)
                            {
                                command.Execute(HomeMode);
                            }
                            command.DeliveryUnits.Add(boxUnit);
                        }
                        break;
                    case "brawlbox":
                        isBox = true;
                        for (int x = 0; x < count; x++)
                        {
                            var boxUnit = new DeliveryUnit(10);
                            HomeMode.SimulateGatcha(boxUnit);
                            if (x + 1 != count)
                            {
                                command.Execute(HomeMode);
                            }
                            command.DeliveryUnits.Add(boxUnit);
                        }
                        break;
                    case "pinpack":                        
                    {
                        isBox = true;
                        var pinsUnit = new DeliveryUnit(13);
                        var pinsCommand = new LogicGiveDeliveryItemsCommand();

                        List<int> Emotes_All = new List<int>();
                        for (int i = 0; i <= 300; i++)
                        {
                            Emotes_All.Add(i);
                        }

                        List<int> Emotes_Locked = Emotes_All.Except(HomeMode.Home.UnlockedEmotes).OrderBy(x => Guid.NewGuid()).Take(3).ToList();

                        foreach (int x in Emotes_Locked)
                        {
                            var drop = new GatchaDrop(11)
                            {
                                PinGlobalId = GlobalId.CreateGlobalId(52, x),
                                Count = 1
                            };

                            pinsUnit.AddDrop(drop);
                            HomeMode.Home.UnlockedEmotes.Add(x);
                        }

                        pinsCommand.DeliveryUnits.Add(pinsUnit);
                        pinsCommand.Execute(HomeMode);

                        var pinMessage = new AvailableServerCommandMessage
                        {
                            Command = pinsCommand
                        };
                        HomeMode.GameListener.SendMessage(pinMessage);
                        break;
                    }
                    case "premium":
                        unit.AddDrop(new GatchaDrop(8) { Count = 170 });
                        /*unit.AddDrop(new GatchaDrop(9)
                        {
                            SkinGlobalId = GlobalId.CreateGlobalId(29, 217)
                        });*/

                        if (account.Home.PremiumEndTime < DateTime.UtcNow)
                            account.Home.PremiumEndTime = DateTime.UtcNow.AddMonths(months);
                        else
                            account.Home.PremiumEndTime = account.Home.PremiumEndTime.AddMonths(months);

                        account.Avatar.PremiumLevel = 1;

                        string formattedDate = account.Home.PremiumEndTime.ToString("dd'th of' MMMM yyyy");
                        Notification n2 = new()
                        {
                            Id = 81,
                            MessageEntry = $"За активацию промокода <c6>{promocode}</c>\nВы получили Pablo Premium! (до {account.Home.PremiumEndTime})."
                        };
                        account.Home.NotificationFactory.Add(n2);
                        LogicAddNotificationCommand acm2 = new() { Notification = n2 };
                        AvailableServerCommandMessage asm2 = new() { Command = acm2 };

                        if (Sessions.IsSessionActive(lowID))
                        {
                            Session session = Sessions.GetSession(lowID);
                            session.GameListener.SendTCPMessage(asm2);
                        }
                        break;                    
                }
            }
            
            if (!isBox)
            {
                command.DeliveryUnits.Add(unit);     
                command.Execute(HomeMode);            
            }
    
            var message = new AvailableServerCommandMessage
            {
                Command = command
            };
            HomeMode.GameListener.SendMessage(message);    

            // Обновляем статистику
            history.Add(lowID);
            promo["ActivationHistory"] = JArray.FromObject(history);
            promo["MaxActivations"] = maxActivations - 1;

            JObject root = JObject.Parse(File.ReadAllText(filePath));
            var promocodes = root["Promocodes"] as JArray;
            var target = promocodes.FirstOrDefault(p => p["Code"]?.ToString() == code);
            if (target != null)
            {
                int index = promocodes.IndexOf(target);
                promocodes[index] = promo;
                File.WriteAllText(filePath, root.ToString());
            }
        }
                
        private void SendEmptyCreatorResponse()
        {
            LogicSetSupportedCreatorCommand response = new()
            {
                Name = string.Empty
            };
            AvailableServerCommandMessage msg = new AvailableServerCommandMessage
            {
                Command = response
            };
            Connection.Send(msg);
        }

        private void LatencyTestResultReceived(LatencyTestResultMessage message)
        {
            LatencyTestStatusMessage l = new()
            {
                Ping = Connection.Ping
            };
            Connection.Send(l);
        }
        private void TeamPremadeChatReceived(TeamPremadeChatMessage message)
        {
            if (HomeMode.Avatar.TeamId <= 0) return;

            TeamEntry team = GetTeam();
            if (team == null) return;

            QuickChatStreamEntry entry = new QuickChatStreamEntry();
            entry.AccountId = HomeMode.Avatar.AccountId;
            entry.TargetId = message.TargetId;
            entry.Name = HomeMode.Avatar.Name;

            if (message.TargetId > 0)
            {
                TeamMember member = team.GetMember(message.TargetId);
                if (member != null)
                {
                    entry.TargetPlayerName = member.DisplayData.Name;
                }
            }

            entry.MessageDataId = message.MessageDataId;
            entry.Unknown1 = message.Unknown1;
            entry.Unknown2 = message.Unknown2;

            team.AddStreamEntry(entry);
        }

        private void TeamChatReceived(TeamChatMessage message)
        {
            
            if (HomeMode.Avatar.TeamId <= 0) return;
            if (HomeMode.Avatar.IsCommunityBanned) return;

            TeamEntry team = GetTeam();
            if (team == null) return;

            ChatStreamEntry entry = new ChatStreamEntry();
            entry.AccountId = HomeMode.Avatar.AccountId;
            entry.Name = HomeMode.Avatar.Name;
            entry.Message = message.Message;
            
            if (message.Message.StartsWith("/"))
            {
                string[] cmd = message.Message.Substring(1).Split(' ');
                if (cmd.Length == 0) return;
                
                DebugOpenMessage debugopen = new()
                {

                };                

                long accountId = HomeMode.Avatar.AccountId;

                switch (cmd[0])
                {
                    case "status":
                        long megabytesUsed = Process.GetCurrentProcess().PrivateMemorySize64 / (1024 * 1024);
                        DateTime now = Process.GetCurrentProcess().StartTime;
                        DateTime futureDate = DateTime.Now;

                        TimeSpan timeDifference = futureDate - now;

                        string formattedTime = string.Format("{0}{1}{2}{3}",
                        timeDifference.Days > 0 ? $"{timeDifference.Days} дней, " : string.Empty,
                        timeDifference.Hours > 0 || timeDifference.Days > 0 ? $"{timeDifference.Hours} часов, " : string.Empty,
                        timeDifference.Minutes > 0 || timeDifference.Hours > 0 ? $"{timeDifference.Minutes} минут, " : string.Empty,
                        timeDifference.Seconds > 0 ? $"{timeDifference.Seconds} секунд" : string.Empty);

                        entry.Message = $"Статус сервера:\n" +
                            $"Серверная версия игры: v30.242\n" +
                            $"Билд сервера: v1.0 с 22.04.2025\n" +
                            $"SHA Ресурсы: {Fingerprint.Sha}\n" +
                            $"Среда: Production\n" +
                            $"Серверное время: {DateTime.Now} EEST\n" +
                            $"Онлайн: {Sessions.Count}\n" +
                            $"Используется ОЗУ: {megabytesUsed} МБ\n" + 
                            $"Аптайм: {formattedTime}\n";
                        team.AddStreamEntry(entry);
                        break;
                    case "help":
                        entry.Message = $"Доступные команды:\n/help - показать все доступные команды\n/status - посмотреть статус сервера\n/promo - активировать промокод\n/givecoins - передать монеты\n/givegems - передать гемы\n/givesp - передать старпоинты\n/lobbyinfo [on/off] - включить/выключить информацию в лобби\n\nPablo Connect\n/register [пароль] [сновапароль] - зарегистрировать аккаунт\n/login [тэг] [пароль] - загрузить аккаунт\n/changepass [старый пароль] [новый пароль] - поменять пароль";
                        team.AddStreamEntry(entry);
                        break;
                    default:
                        entry.Message = $"Неизвестная команда \"{cmd[0]}\" - введи \"/help\" чтобы получить список команд!";
                        team.AddStreamEntry(entry);
                        break;
                    case "debug":
                        if (HomeMode.Avatar.AccountId != 1)
                        {
                            entry.Message = $"You don\'t have right to use this command"; // /usecode [code] - use bonus code
                            team.AddStreamEntry(entry);
                            return;
                        }
                        entry.Message = "Debug open!";
                        team.AddStreamEntry(entry);
                        Connection.Send(debugopen);
                        break;
                    case "promo":
                        if (cmd.Length < 2)
                        {
                            entry.Message = "Использование: /promo [код]";
                            team.AddStreamEntry(entry);
                            break;
                        }

                        string promoCode = cmd[1];
                        string promoFilePath = "/root/PabloBrawl/prod31/Supercell.Laser.Server/JSON/promo.json";

                        if (IsPromoCodeValid(promoCode, promoFilePath, out JObject promoData))
                        {
                            var history = promoData["ActivationHistory"]?.ToObject<List<long>>() ?? new List<long>();
                            int maxActivations = promoData["MaxActivations"]?.ToObject<int>() ?? -1;
                            
                            if (maxActivations == 0)
                            {
                                entry.Message = "Промокод исчерпал лимит активаций.";
                                team.AddStreamEntry(entry);
                                break;
                            }

                            if (history.Contains(HomeMode.Avatar.AccountId))
                            {
                                entry.Message = "Вы уже активировали этот промокод.";
                                team.AddStreamEntry(entry);
                                break;
                            }
                            
                            Reward(promoCode, promoData, HomeMode.Avatar.AccountId, promoFilePath, history, ref maxActivations);

                            entry.Message = $"Промокод <c6>{promoCode}</c> успешно активирован!";
                        }
                        else
                        {
                            entry.Message = "Неверный или просроченный промокод.";
                        }

                        team.AddStreamEntry(entry);
                        break;                                                            
                    case "lobbyinfo":
                    {
                        if (cmd.Length == 1)
                        {
                            bool isLobbyInfoEnabled = HomeMode.Home.LobbyInfo;
                            string currentText = string.IsNullOrEmpty(HomeMode.Home.CustomLobbyInfo)
                                ? "Стандартный текст"
                                : HomeMode.Home.CustomLobbyInfo;

                            entry.Message = $"Текущее состояние информации в лобби: {(isLobbyInfoEnabled ? "включено" : "выключено")}\n\n" +
                                $"Текущий кастомный текст:\n{currentText}\n\n" +
                                "Используйте:\n" +
                                "/lobbyinfo on - включить отображение\n" +
                                "/lobbyinfo off - выключить отображение\n" +
                                "/lobbyinfo change [текст] - установить кастомный текст (можно использовать {pingInfo}, {onlineCount}, {currentTime}, {hasPremium})\n" +
                                "/lobbyinfo addcustom [текст] - добавить текст к существующему лоббиинфо\n" +
                                "/lobbyinfo default - сбросить кастомный текст\n" +
                                "/lobbyinfo info - показать текущий текст лобби";
                        }
                        else
                        {
                            string mode = cmd[1].ToLower();

                            bool requiresPremium = mode == "change" || mode == "addcustom";
                            if (requiresPremium && HomeMode.Home.PremiumEndTime < DateTime.UtcNow)
                            {
                                entry.Message = "Только для Pablo Premium!";
                                team.AddStreamEntry(entry);
                                return;
                            }

                            switch (mode)
                            {
                                case "on":
                                    HomeMode.Home.LobbyInfo = true;
                                    entry.Message = "Информация в лобби включена.";
                                    break;

                                case "off":
                                    HomeMode.Home.LobbyInfo = false;
                                    entry.Message = "Информация в лобби выключена.";
                                    break;

                                case "default":
                                    HomeMode.Home.CustomLobbyInfo = null;
                                    entry.Message = "Кастомный текст лобби был сброшен.";
                                    break;

                                case "info":
                                    entry.Message = string.IsNullOrEmpty(HomeMode.Home.CustomLobbyInfo)
                                        ? "Кастомный текст лобби не установлен."
                                        : $"Текущий кастомный текст:\n{HomeMode.Home.CustomLobbyInfo}";
                                    break;

                                case "change":
                                    if (cmd.Length < 3)
                                    {
                                        entry.Message = "Введите текст для лоббиинфо после команды. Пример: /lobbyinfo change Привет! Онлайн: {onlineCount}";
                                    }
                                    else
                                    {
                                        string customText = string.Join(" ", cmd.Skip(2)).Trim();
                                        customText = customText.Replace("\\n", "\n");

                                        if (string.IsNullOrEmpty(customText))
                                        {
                                            entry.Message = "Текст для лоббиинфо не может быть пустым.";
                                        }
                                        else
                                        {
                                            HomeMode.Home.CustomLobbyInfo = customText;
                                            entry.Message = "Кастомный лоббиинфо установлен.";
                                        }
                                    }
                                    break;

                                case "addcustom":
                                    if (cmd.Length < 3)
                                    {
                                        entry.Message = "Введите текст, который нужно добавить. Пример: /lobbyinfo addcustom \\nСпециальное сообщение!";
                                    }
                                    else
                                    {
                                        string newText = string.Join(" ", cmd.Skip(2)).Trim();
                                        newText = newText.Replace("\\n", "\n");

                                        if (string.IsNullOrEmpty(newText))
                                        {
                                            entry.Message = "Нельзя добавить пустой текст.";
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(HomeMode.Home.CustomLobbyInfo))
                                            {
                                                HomeMode.Home.CustomLobbyInfo = newText;
                                                entry.Message = "Текст добавлен. Теперь лоббиинфо:\n" + newText;
                                            }
                                            else
                                            {
                                                HomeMode.Home.CustomLobbyInfo += newText;
                                                entry.Message = "Текст успешно добавлен к лоббиинфо.";
                                            }
                                        }
                                    }
                                    break;

                                default:
                                    entry.Message = "Неверный параметр. Используйте /lobbyinfo help для справки.";
                                    break;
                            }
                        }

                        team.AddStreamEntry(entry);
                        break;
                    }
                    case "givegems":
                        if (cmd.Length != 3)
                        {
                            entry.Message = "Использование: /givegems #тэг кол-во";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        if (!cmd[1].StartsWith('#'))
                        {
                            entry.Message = "Невалидный тэг.";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        long receiverId = LogicLongCodeGenerator.ToId(cmd[1]);
                        Account receiverAccount = Accounts.Load(receiverId);
                        long lowIDgiver = HomeMode.Avatar.AccountId;
                        Account accountgiver = Accounts.Load(lowIDgiver);
                        if (receiverAccount == null)
                        {
                            entry.Message = $"Невалидный тэг получателя.";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        int amount;
                        if (!int.TryParse(cmd[2], out amount) || amount <= 0)
                        {
                            entry.Message = "Невалидное кол-во.";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        int leftgems = amount - accountgiver.Avatar.Diamonds;
                        if (accountgiver.Avatar.Diamonds < amount)
                        {
                            entry.Message = $"Недостаточно гемов! Тебе нужно {leftgems} гемов.";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        accountgiver.Avatar.Diamonds -= amount;
                        int receivedAmount = (int)(amount * 0.8);
                        Accounts.Save(accountgiver);
                        string receiverTag = LogicLongCodeGenerator.ToCode(receiverId);
                        string accountsender = LogicLongCodeGenerator.ToCode(lowIDgiver);
                        entry.Message = $"Ты отправил {amount} гемов пользователю {receiverTag}!";
                        team.AddStreamEntry(entry);
                        Notification nGems = new()
                        {
                            Id = 89,
                            DonationCount = receivedAmount,
                            MessageEntry = $"<c6>Пользователь {accountsender} передал тебе {receivedAmount} гемов!</c>"
                        };
                        receiverAccount.Home.NotificationFactory.Add(nGems);
                        LogicAddNotificationCommand acmGems = new() { Notification = nGems };
                        AvailableServerCommandMessage asmGems = new();
                        asmGems.Command = acmGems;
                        if (Sessions.IsSessionActive(receiverId))
                        {
                            Session sessionGems = Sessions.GetSession(receiverId);
                            sessionGems.GameListener.SendTCPMessage(asmGems);
                        }
                        // Логирование передачи гемов
                        Logger.VLog($"Пользователь {accountsender} передал {amount} ГЕМОВ (получено: {receivedAmount}) пользователю {receiverTag}. Команда: /givegems {cmd[1]} {cmd[2]}");
                        break;
                    case "givecoins":
                        if (cmd.Length != 3)
                        {
                            entry.Message = "Использование: /givecoins #тэг кол-во"; // Исправлена опечатка в сообщении
                            team.AddStreamEntry(entry);
                            return;
                        }
                        if (!cmd[1].StartsWith('#'))
                        {
                            entry.Message = "Невалидный тэг.";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        long creceiverId = LogicLongCodeGenerator.ToId(cmd[1]);
                        Account creceiverAccount = Accounts.Load(creceiverId);
                        long clowIDgiver = HomeMode.Avatar.AccountId;
                        Account caccountgiver = Accounts.Load(clowIDgiver);
                        if (creceiverAccount == null)
                        {
                            entry.Message = $"Невалидный тэг получателя.";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        int camount;
                        if (!int.TryParse(cmd[2], out camount) || camount <= 0)
                        {
                            entry.Message = "Невалидное кол-во.";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        // Исправлена ошибка: проверка должна быть на Gold, а не Diamonds
                        int leftcoins = camount - caccountgiver.Avatar.Gold;
                        if (caccountgiver.Avatar.Gold < camount)
                        {
                            entry.Message = $"Недостаточно монет! Тебе нужно {leftcoins} монет."; // Исправлено сообщение
                            team.AddStreamEntry(entry);
                            return;
                        }
                        caccountgiver.Avatar.Gold -= camount;
                        int creceivedAmount = (int)(camount * 0.8);
                        Accounts.Save(caccountgiver);
                        string creceiverTag = LogicLongCodeGenerator.ToCode(creceiverId);
                        string caccountsender = LogicLongCodeGenerator.ToCode(clowIDgiver);
                        entry.Message = $"Ты отправил {camount} монет пользователю {creceiverTag}!";
                        team.AddStreamEntry(entry);
                        Notification nCoins = new()
                        {
                            Id = 90,
                            DonationCount = creceivedAmount,
                            MessageEntry = $"<c6>Пользователь {caccountsender} передал тебе {creceivedAmount} монет!</c>"
                        };
                        creceiverAccount.Home.NotificationFactory.Add(nCoins);
                        LogicAddNotificationCommand acmCoins = new() { Notification = nCoins };
                        AvailableServerCommandMessage asmCoins = new();
                        asmCoins.Command = acmCoins;
                        if (Sessions.IsSessionActive(creceiverId))
                        {
                            Session sessionCoins = Sessions.GetSession(creceiverId);
                            sessionCoins.GameListener.SendTCPMessage(asmCoins);
                        }
                        // Логирование передачи монет
                        Logger.VLog($"Пользователь {caccountsender} передал {camount} МОНЕТ (получено: {creceivedAmount}) пользователю {creceiverTag}. Команда: /givecoins {cmd[1]} {cmd[2]}");
                        break;
                    case "givesp":
                        if (cmd.Length != 3)
                        {
                            entry.Message = "Использование: /givesp #тэг кол-во";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        if (!cmd[1].StartsWith('#'))
                        {
                            entry.Message = "Невалидный тэг.";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        long spreceiverId = LogicLongCodeGenerator.ToId(cmd[1]);
                        Account spreceiverAccount = Accounts.Load(spreceiverId);
                        long splowIDgiver = HomeMode.Avatar.AccountId;
                        Account spaccountgiver = Accounts.Load(splowIDgiver);
                        if (spreceiverAccount == null)
                        {
                            entry.Message = $"Невалидный тэг получателя.";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        int spamount;
                        if (!int.TryParse(cmd[2], out spamount) || spamount <= 0)
                        {
                            entry.Message = "Невалидное кол-во.";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        // Исправлена ошибка: проверка должна быть на StarPoints, а не Diamonds
                        int leftsp = spamount - spaccountgiver.Avatar.StarPoints;
                        if (spaccountgiver.Avatar.StarPoints < spamount)
                        {
                            entry.Message = $"Недостаточно старпоинтов! Тебе нужно {leftsp} старпоинтов."; // Исправлено сообщение
                            team.AddStreamEntry(entry);
                            return;
                        }
                        spaccountgiver.Avatar.StarPoints -= spamount;
                        int spreceivedAmount = (int)(spamount * 0.8);
                        Accounts.Save(spaccountgiver);
                        string spreceiverTag = LogicLongCodeGenerator.ToCode(spreceiverId);
                        string spaccountsender = LogicLongCodeGenerator.ToCode(splowIDgiver);
                        entry.Message = $"Ты отправил {spamount} старпоинтов пользователю {spreceiverTag}!";
                        team.AddStreamEntry(entry);
                        Notification nSp = new()
                        {
                            Id = 91,
                            DonationCount = spreceivedAmount,
                            MessageEntry = $"<c6>Пользователь {spaccountsender} передал тебе {spreceivedAmount} старпоинтов!</c>"
                        };
                        spreceiverAccount.Home.NotificationFactory.Add(nSp);
                        LogicAddNotificationCommand acmSp = new() { Notification = nSp };
                        AvailableServerCommandMessage asmSp = new();
                        asmSp.Command = acmSp;
                        if (Sessions.IsSessionActive(spreceiverId))
                        {
                            Session sessionSp = Sessions.GetSession(spreceiverId);
                            sessionSp.GameListener.SendTCPMessage(asmSp);
                        }
                        // Логирование передачи старпоинтов
                        Logger.VLog($"Пользователь {spaccountsender} передал {spamount} СТАРПОИНТОВ (получено: {spreceivedAmount}) пользователю {spreceiverTag}. Команда: /givesp {cmd[1]} {cmd[2]}");
                        break;
                    case "load":
                        if (cmd.Length != 3)
                        {
                            entry.Message = $"Использование: /load [тэг] [код]";
                            team.AddStreamEntry(entry);
                            return;
                        }

                        string loadTag = cmd[1];
                        string loadCode = cmd[2];
                        
                        if (!loadTag.StartsWith("#"))
                        {
                            entry.Message = "Тэг должен начинаться с #.";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        
                        long loadid = LogicLongCodeGenerator.ToId(cmd[1]);
                                                                        
                        Account loadaccount = Accounts.Load(loadid);

                        if (loadaccount == null)
                        {
                            entry.Message = $"Аккаунт не найден.";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        if (!int.TryParse(loadCode, out int parsedLoadCode))
                        {
                            entry.Message = "Неверный формат кода.";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        if (parsedLoadCode != loadaccount.Home.LoadCode)
                        {
                            entry.Message = "Код для входа неверный.";
                            team.AddStreamEntry(entry);
                            return;
                        }

                        Connection.Send(new CreateAccountOkMessage
                        {
                            AccountId = loadaccount.AccountId,
                            PassToken = loadaccount.PassToken
                        });

                        Connection.Send(new AuthenticationFailedMessage
                        {                            
                            ErrorCode = 22,
                            Message = "Вы успешно вошли в аккаунт!"
                        });
                        loadaccount.Home.LoadCode = null;
                        break;                    
                    // ACCOUNT SYSTEM HERE
                    case "register":
                        if (cmd.Length != 3)
                        {
                            entry.Message = $"Использование: /register [пароль] [повтор пароля]";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        
                        string password = cmd[1];
                        string againpassword = cmd[2];                        

                            if (password != againpassword)
                            {
                                entry.Message = "Пароль не совпадает!";
                                team.AddStreamEntry(entry);
                                return;
                            }
                        bool registrationSuccess = RegisterUserToDatabase(password, accountId);

                        if (!registrationSuccess)
                        {
                            entry.Message = $"Аккаунт уже привязан!";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        Account plreaccount = Accounts.Load(accountId);
                        if (!plreaccount.Home.UnlockedSkins.Contains(29000059))
                        {
                            Notification brlyn = new()
                            {
                                Id = 94,
                                skin = 59,
                                MessageEntry = "<c6>Спасибо за привязку!</c>"
                            };
                            plreaccount.Home.NotificationFactory.Add(brlyn);
                            LogicAddNotificationCommand acm = new() { Notification = brlyn };

                            AvailableServerCommandMessage asm = new() { Command = acm };
                            if (Sessions.IsSessionActive(accountId))
                            {
                                Session session = Sessions.GetSession(accountId);
                                session.GameListener.SendTCPMessage(asm);
                            }
                        }
                        entry.Message = $"Регистрация успешна! Вы получили награду за привязку.";    
                        team.AddStreamEntry(entry);
                        break;
                    case "login":
                        if (cmd.Length != 3)
                        {
                            entry.Message = $"Использование: /login [тэг] [пароль]";
                            team.AddStreamEntry(entry);
                            return;
                        }

                        string loginTag = cmd[1];
                        string loginPassword = cmd[2];
                        
                        if (!loginTag.StartsWith("#"))
                        {
                            entry.Message = "Тэг должен начинаться с #.";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        
                        long id = LogicLongCodeGenerator.ToId(loginTag);

                        string accountIdS = LoginUserFromDatabase(id, loginPassword);

                        if (string.IsNullOrEmpty(accountIdS))
                        {
                            entry.Message = $"Никнейм или пароль неверны.";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        
                        Account account = Accounts.Load(id);

                        if (account == null)
                        {
                            entry.Message = $"Аккаунт не найден.";
                            team.AddStreamEntry(entry);
                            return;
                        }

                        Connection.Send(new CreateAccountOkMessage
                        {
                            AccountId = account.AccountId,
                            PassToken = account.PassToken
                        });

                        Connection.Send(new AuthenticationFailedMessage
                        {
                            ErrorCode = 8,
                            Message = "Вы успешно вошли в аккаунт!"
                        });
                        break;
                    case "changepass":
                        if (cmd.Length != 3)
                        {
                            entry.Message = $"Использование: /changepass [старый пароль] [новый пароль]";
                            team.AddStreamEntry(entry);
                            return;
                        }
                        
                        string oldPassword = cmd[1];
                        string newPassword = cmd[2]; 
                        long idlog = HomeMode.Avatar.AccountId;
                        bool passwordChanged = ChangeUserPassword(idlog, oldPassword, newPassword);

                        if (!passwordChanged)
                        {
                            entry.Message = $"Никнейм или старый пароль неверны.";
                            team.AddStreamEntry(entry);
                            return;
                        }

                        entry.Message = $"Пароль успешно изменён.";
                        team.AddStreamEntry(entry);
                        break;
                    /*case "theme":
                        if (HomeMode.Home.PremiumEndTime < DateTime.UtcNow)
                        {
                            entry.Message = $"Только для Pablo Premium!"; // /usecode [code] - use bonus code
                            team.AddStreamEntry(entry);
                            return;
                        }
                        if (cmd.Length != 2)
                        {
                            entry.Message = $"Использование: /theme [айди]\nЕсли не знаешь айди тем:\n0. Default\n1. Winter\n2. LNY\n3. CR\n4. GoldenWeek\n5. Retropolis\n6. Mecha\n7. Halloween\n8. Brawlidays\n9. LNY20\n10. PSG\n11. SC10\n12. Bazaar\n13. Monsters\n14. MoonFestival20\n15. Brawlidays 2023\n16. LNY 2024\n17. China Emz\n18. China Colt\n19. China Bo\n20. China Spike\n21. China Mortis\n22. China Edgar\n23. China Fang\n24. China Piper\n25. China Stu\n26. China Bull\n27. Stuntshow\n28. Melodie\n29. Hindu\n30. Ghosttrain\n31. Candyland\n32. Babyshark\n33. Loveswamp\n34. Spongebob";
                            team.AddStreamEntry(entry);
                            return;
                        }                      
                        Account account1 = Accounts.LoadNoCache(accountId);
                        if (account1 == null) return;
                        account1.Home.Theme = int.Parse(cmd[1]);
                        Accounts.Save(account1);
                            
                        // todo
                        entry.Message = $"Успешно! Перезайдите для полного применения.";
                        team.AddStreamEntry(entry);
                        
                        break;*/
                }
                return;
            }
            
            string cleanedMessage = Profanity(entry.Message);
            if (cleanedMessage != entry.Message)
            {
                var chatEntry = new ChatStreamEntry
                {
                    Id = entry.Id,
                    AccountId = entry.AccountId,
                    Name = entry.Name,
                    Message = cleanedMessage
                };

                team.AddStreamEntry(chatEntry);
            }
            else
            {
                team.AddStreamEntry(entry);
            }
                
        }
                        
        private bool ContainsForbiddenWords(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            var profanity = new HashSet<string>(
                File.ReadAllLines("Assets/profanityads.txt")
                    .Select(line => line.Trim())
                    .Where(line => !string.IsNullOrEmpty(line)),
                StringComparer.OrdinalIgnoreCase);
                            
            Regex wordRegex = new Regex(@"\b[\p{L}']+\b", RegexOptions.IgnoreCase);

            foreach (Match match in wordRegex.Matches(input))
            {
                string word = match.Value;
        
                foreach (string badWord in profanity)
                {
                    if (word.StartsWith(badWord, StringComparison.OrdinalIgnoreCase) &&
                        word.Length <= badWord.Length + 3)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void AvatarNameCheckRequestReceived(AvatarNameCheckRequestMessage message)
        {
            if (message.Name.Length < 3)
            {
                AvatarNameChangeFailedMessage lowLength = new AvatarNameChangeFailedMessage();
                lowLength.SetReason(message.Name.Length < 3 ? 2 : message.Name.Length >= 15 ? 1 : 0);
                Connection.Send(lowLength);
                Console.WriteLine($"Account {HomeMode.Avatar.AccountId} крч поставил ник с малым кол-во букв!");
                return;
            }
            if (message.Name.Length >= 15)
            {
                AvatarNameChangeFailedMessage maxLength = new AvatarNameChangeFailedMessage();
                maxLength.SetReason(message.Name.Length < 3 ? 2 : message.Name.Length >= 15 ? 1 : 0);
                Connection.Send(maxLength);
                Console.WriteLine($"Account {HomeMode.Avatar.AccountId} exceeded length limit!");
                return;
            }
            if (ContainsForbiddenWords(message.Name))
            {
                AvatarNameChangeFailedMessage forbiddenName = new AvatarNameChangeFailedMessage();
                forbiddenName.SetReason(message.Name.Length < 3 ? 2 : message.Name.Length >= 15 ? 1 : 0);
                Connection.Send(forbiddenName);
                Console.WriteLine($"Account {HomeMode.Avatar.AccountId} try to set name with forbidden word!");
                return;
            }            
            if (HomeMode.Home.CooldownChangeName > DateTime.Now)
            {
                AvatarNameChangeFailedMessage cooldown = new AvatarNameChangeFailedMessage();
                cooldown.SetReason(message.Name.Length < 3 ? 2 : message.Name.Length >= 15 ? 1 : 0);
                Connection.Send(cooldown);
                Console.WriteLine($"Account {HomeMode.Avatar.AccountId} try to set name when he have a cooldown!");
                return;
            }
                
            if (HomeMode.Avatar.UseDiamonds(HomeMode.Home.CostChangeName))
            {
                TimeZoneInfo moscowZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");
                DateTime nowMoscow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, moscowZone);
                if (HomeMode.Avatar.AllianceId >= 0)
                {
                    Alliance alliance = Alliances.Load(HomeMode.Avatar.AllianceId);
                    if (alliance != null)
                    {
                        AllianceMember member = alliance.GetMemberById(HomeMode.Avatar.AccountId);
                        if (member != null)
                        {
                            member.DisplayData.Name = message.Name;
                        }
                    }
                }                
                var command = new LogicChangeAvatarNameCommand
                {
                    Name = message.Name,
                    ChangeNameCost = HomeMode.Home.CostChangeName
                };

                command.Execute(HomeMode);
                Connection.Send(new AvailableServerCommandMessage { Command = command });            
                HomeMode.Home.CooldownChangeName = nowMoscow.AddDays(1).AddHours(12);
                HomeMode.Home.CostChangeName += 30;
            }
            else
            {
                AvatarNameChangeFailedMessage fewDiamonds = new AvatarNameChangeFailedMessage();
               fewDiamonds.SetReason(message.Name.Length < 3 ? 2 : message.Name.Length >= 15 ? 1 : 0);
               Connection.Send(fewDiamonds);
               Console.WriteLine($"Account {HomeMode.Avatar.AccountId} don't have enough diamonds!");
               return;
            }
        }

        private void TeamSetEventReceived(TeamSetEventMessage message)
        {
            if (HomeMode.Avatar.TeamId <= 0) return;

            TeamEntry team = GetTeam();
            if (team == null) return;
            if (message.EventSlot == 2) return;

            EventData data = Events.GetEvent(message.EventSlot);
            if (data == null) return;

            team.EventSlot = message.EventSlot;
            team.LocationId = data.LocationId;
            team.TeamUpdated();
        }

        private BattleMode SpectatedBattle;
        private void StopSpectateReceived(StopSpectateMessage message)
        {
            if (SpectatedBattle != null)
            {
                SpectatedBattle.RemoveSpectator(Connection.UdpSessionId);
                SpectatedBattle = null;
            }

            if (Connection.Home != null && Connection.Avatar != null)
            {
                OwnHomeDataMessage ohd = new OwnHomeDataMessage();
                ohd.Home = Connection.Home;
                ohd.Avatar = Connection.Avatar;
                Connection.Send(ohd);
            }
        }

        private void StartSpectateReceived(StartSpectateMessage message)
        {
            Account data = Accounts.Load(message.AccountId);
            if (data == null) return;

            ClientAvatar avatar = data.Avatar;
            long battleId = avatar.BattleId;

            BattleMode battle = Battles.Get(battleId);
            if (battle == null) return;

            SpectatedBattle = battle;
            UDPSocket socket = UDPGateway.CreateSocket();
            socket.Battle = battle;
            socket.IsSpectator = true;
            socket.TCPConnection = Connection;
            Connection.UdpSessionId = socket.SessionId;
            battle.AddSpectator(socket.SessionId, new UDPGameListener(socket, Connection));

            StartLoadingMessage startLoading = new StartLoadingMessage();
            startLoading.LocationId = battle.Location.GetGlobalId();
            startLoading.TeamIndex = 0;
            startLoading.OwnIndex = 0;
            startLoading.GameMode = battle.GetGameModeVariation() == 6 ? 6 : 1;
            startLoading.Players.AddRange(battle.GetPlayers());
            startLoading.SpectateMode = 1;

            Connection.Send(startLoading);

            UdpConnectionInfoMessage info = new UdpConnectionInfoMessage();
            info.SessionId = Connection.UdpSessionId;
            info.ServerPort = Configuration.Instance.Port;
            Connection.Send(info);
        }

        private void GoHomeFromOfflinePractiseReceived(GoHomeFromOfflinePractiseMessage message)
        {
            if (Connection.Home != null && Connection.Avatar != null)
            {
                if (Connection.Avatar.IsTutorialState())
                {
                    Connection.Avatar.SkipTutorial();
                }
                Connection.Home.Events = Events.GetEventsById(HomeMode.Home.PowerPlayGamesPlayed, Connection.Avatar.AccountId);

                OwnHomeDataMessage ohd = new OwnHomeDataMessage();
                ohd.Home = Connection.Home;
                ohd.Avatar = Connection.Avatar;
                ShowLobbyInfo();
                Connection.Send(ohd);
            }
        }

        private void TeamSetLocationReceived(TeamSetLocationMessage message)
        {
            if (HomeMode.Avatar.TeamId <= 0) return;

            TeamEntry team = GetTeam();
            if (team == null) return;

            team.Type = 1;
            team.TeamUpdated();
        }

        private void ChangeAllianceSettingsReceived(ChangeAllianceSettingsMessage message)
        {
            if (HomeMode.Avatar.AllianceId <= 0) return;

            if (HomeMode.Avatar.AllianceRole != AllianceRole.Leader) return;

            Alliance alliance = Alliances.Load(HomeMode.Avatar.AllianceId);
            if (alliance == null) return;

            if (message.BadgeId >= 8000000 && message.BadgeId < 8000000 + DataTables.Get(DataType.AllianceBadge).Count)
            {
                alliance.AllianceBadgeId = message.BadgeId;
            }
            else
            {
                alliance.AllianceBadgeId = 8000000;
            }
            if (ContainsForbiddenWords(message.Description))
            {
                Connection.Send(new AllianceResponseMessage()
                {
                    ResponseType = 23
                });
                return;
            }         
           
            alliance.Description = message.Description;
            alliance.RequiredTrophies = message.RequiredTrophies;
            alliance.Type = message.Type == 1 ? 1 : message.Type == 2 ? 2 : 3;

            Connection.Send(new AllianceResponseMessage()
            {
                ResponseType = 10
            });

            MyAllianceMessage myAlliance = new MyAllianceMessage();
            myAlliance.Role = HomeMode.Avatar.AllianceRole;
            myAlliance.OnlineMembers = alliance.OnlinePlayers;
            myAlliance.AllianceHeader = alliance.Header;
            Console.WriteLine($"Type Message: {message.Type}");
            Connection.Send(myAlliance);
        }
        private void SendAllianceMailMessage(SendAllianceMailMessage message)
        {
            SendAllianceMailMessage sendAllianceMailMessage = message;

            if (HomeMode.Avatar.AllianceRole != AllianceRole.Leader && HomeMode.Avatar.AllianceRole != AllianceRole.CoLeader) return;
            if (!string.IsNullOrWhiteSpace(message.Text))
            {
                if (HomeMode.Avatar.AllianceRole != AllianceRole.Leader && HomeMode.Avatar.AllianceRole != AllianceRole.CoLeader) return;
                if (message.Text.Length > 450) return;
                addNotifToAllAccounts(message.Text, HomeMode.Avatar.AllianceId);

                AllianceResponseMessage responseMessages = new AllianceResponseMessage();
                responseMessages.ResponseType = 113;
                Connection.Send(responseMessages);
            }

            AllianceResponseMessage responseMessage = new AllianceResponseMessage();
            responseMessage.ResponseType = 114;
            Connection.Send(responseMessage);

            Connection.Send(sendAllianceMailMessage);
        }
        private void KickAllianceMemberReceived(KickAllianceMemberMessage message)
        {
            if (HomeMode.Avatar.AllianceId <= 0) return;

            Alliance alliance = Alliances.Load(HomeMode.Avatar.AllianceId);
            if (alliance == null) return;

            AllianceMember member = alliance.GetMemberById(message.AccountId);
            if (member == null) return;

            ClientAvatar avatar = Accounts.Load(message.AccountId).Avatar;
            ClientHome home = Accounts.Load(message.AccountId).Home;

            if (HomeMode.Avatar.AllianceRole <= avatar.AllianceRole) return;

            alliance.Members.Remove(member);
            avatar.AllianceId = -1;
            avatar.KickedAllianceId = alliance.Id;
            home.CooldownKickedClan = DateTime.Now.AddDays(2);

            AllianceStreamEntry entry = new AllianceStreamEntry();
            entry.AuthorId = HomeMode.Avatar.AccountId;
            entry.AuthorName = HomeMode.Avatar.Name;
            entry.Id = ++alliance.Stream.EntryIdCounter;
            entry.PlayerId = avatar.AccountId;
            entry.PlayerName = avatar.Name;
            entry.Type = 4;
            entry.Event = 1; // kicked
            entry.AuthorRole = HomeMode.Avatar.AllianceRole;
            alliance.AddStreamEntry(entry);

            AllianceResponseMessage response = new AllianceResponseMessage();
            response.ResponseType = 70;
            Connection.Send(response);

            if (LogicServerListener.Instance.IsPlayerOnline(avatar.AccountId))
            {
                LogicServerListener.Instance.GetGameListener(avatar.AccountId).SendTCPMessage(new AllianceResponseMessage()
                {
                    ResponseType = 100
                });
                LogicServerListener.Instance.GetGameListener(avatar.AccountId).SendTCPMessage(new MyAllianceMessage());
            }
        }

        private void TeamSetMemberReadyReceived(TeamSetMemberReadyMessage message)
        {
            TeamEntry team = Teams.Get(HomeMode.Avatar.TeamId);
            if (team == null) return;

            TeamMember member = team.GetMember(HomeMode.Avatar.AccountId);
            if (member == null) return;

            member.IsReady = message.IsReady;

            team.TeamUpdated();

            //if (team.IsEveryoneReady())
            // {
            //Teams.StartGame(team);
            //}
        }

        private void TeamChangeMemberSettingsReceived(TeamChangeMemberSettingsMessage message)
        {
            ;
        }

        private void TeamMemberStatusReceived(TeamMemberStatusMessage message)
        {
            if (HomeMode == null) return;
            if (message.Status < 0) return;
            TeamEntry team = Teams.Get(HomeMode.Avatar.TeamId);
            if (team == null) return;

            TeamMember member = team.GetMember(HomeMode.Avatar.AccountId);
            if (member == null) return;

            member.State = message.Status;
            team.TeamUpdated();
        }

        private void TeamInvitationResponseReceived(TeamInvitationResponseMessage message)
        {
            bool isAccept = message.Response == 1;

            TeamEntry team = Teams.Get(message.TeamId);
            if (team == null) return;

            TeamInviteEntry invite = team.GetInviteById(HomeMode.Avatar.AccountId);
            if (invite == null) return;

            team.Invites.Remove(invite);

            if (isAccept)
            {
                TeamMember member = new TeamMember();
                member.AccountId = HomeMode.Avatar.AccountId;
                member.CharacterId = HomeMode.Home.CharacterId;
                Hero hero = HomeMode.Avatar.GetHero(HomeMode.Home.CharacterId);
                member.SkinId = GlobalId.CreateGlobalId(29, HomeMode.Home.SelectedSkins[GlobalId.GetInstanceId(HomeMode.Home.CharacterId)]);
                member.DisplayData = new PlayerDisplayData(HomeMode.Home.HasPremiumPass, HomeMode.Home.ThumbnailId, HomeMode.Home.NameColorId, HomeMode.Avatar.Name);
                member.HeroTrophies = hero.Trophies;
                member.HeroHighestTrophies = hero.HighestTrophies;
                member.HeroLevel = hero.PowerLevel;
                member.IsOwner = false;
                member.State = 0;
                team.Members.Add(member);

                HomeMode.Avatar.TeamId = team.Id;
            }

            team.TeamUpdated();
        }

        private TeamEntry GetTeam()
        {
            return Teams.Get(HomeMode.Avatar.TeamId);
        }

        private void TeamInviteReceived(TeamInviteMessage message)
        {
            TeamEntry team = GetTeam();
            if (team == null) return;

            Account data = Accounts.Load(message.AvatarId);
            if (data == null) return;

            TeamInviteEntry entry = new TeamInviteEntry();
            entry.Slot = message.Team;
            entry.Name = data.Avatar.Name;
            entry.Id = message.AvatarId;
            entry.InviterId = HomeMode.Avatar.AccountId;

            team.Invites.Add(entry);

            team.TeamUpdated();

            LogicGameListener gameListener = LogicServerListener.Instance.GetGameListener(message.AvatarId);
            if (gameListener != null)
            {
                TeamInvitationMessage teamInvitationMessage = new TeamInvitationMessage();
                teamInvitationMessage.TeamId = team.Id;

                Friend friendEntry = new Friend();
                friendEntry.AccountId = HomeMode.Avatar.AccountId;
                friendEntry.DisplayData = new PlayerDisplayData(HomeMode.Home.HasPremiumPass, HomeMode.Home.ThumbnailId, HomeMode.Home.NameColorId, HomeMode.Avatar.Name);
                friendEntry.Trophies = HomeMode.Avatar.Trophies;
                teamInvitationMessage.Unknown = 1;
                teamInvitationMessage.FriendEntry = friendEntry;

                gameListener.SendTCPMessage(teamInvitationMessage);
            }
        }

        private void TeamClearInviteMessageReceived(TeamClearInviteMessage message)
        {
            TeamEntry team = GetTeam();
            if (team == null) return;

            TeamInviteEntry inviteToRemove = team.Invites.FirstOrDefault(invite => invite.Slot == message.Slot);
            if (inviteToRemove != null)
            {
                team.Invites.Remove(inviteToRemove);
                team.TeamUpdated();
            }
        }

        private void TeamLeaveReceived(TeamLeaveMessage message)
        {
            if (HomeMode.Avatar.TeamId <= 0) return;

            TeamEntry team = Teams.Get(HomeMode.Avatar.TeamId);

            if (team == null)
            {
                Logger.Print("TeamLeave - Team is NULL!");
                HomeMode.Avatar.TeamId = -1;
                Connection.Send(new TeamLeftMessage());
                return;
            }

            TeamMember entry = team.GetMember(HomeMode.Avatar.AccountId);

            if (entry == null) return;
            HomeMode.Avatar.TeamId = -1;

            team.Members.Remove(entry);

            Connection.Send(new TeamLeftMessage());
            team.TeamUpdated();

            if (team.Members.Count == 0)
            {
                Teams.Remove(team.Id);
            }
        }

        private void TeamCreateReceived(TeamCreateMessage message)
        {
            TeamEntry team = Teams.Create();

            team.Type = message.TeamType;
            team.LocationId = Events.GetEvents()[0].LocationId;

            TeamMember member = new TeamMember();
            member.AccountId = HomeMode.Avatar.AccountId;
            member.CharacterId = HomeMode.Home.CharacterId;
            Hero hero = HomeMode.Avatar.GetHero(HomeMode.Home.CharacterId);
            member.SkinId = GlobalId.CreateGlobalId(29, HomeMode.Home.SelectedSkins[GlobalId.GetInstanceId(HomeMode.Home.CharacterId)]);
            member.DisplayData = new PlayerDisplayData(HomeMode.Home.HasPremiumPass, HomeMode.Home.ThumbnailId, HomeMode.Home.NameColorId, HomeMode.Avatar.Name);
            member.HeroLevel = hero.PowerLevel;
            if (hero.HasStarpower)
            {
                CardData card = null;
                CharacterData cd = DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(hero.CharacterId);
                card = DataTables.Get(DataType.Card).GetData<CardData>(cd.Name + "_unique");
                CardData card2 = DataTables.Get(DataType.Card).GetData<CardData>(cd.Name + "_unique_2");
                if (HomeMode.Avatar.SelectedStarpowers.Contains(card.GetGlobalId()))
                {
                    member.HeroLevel = 9;
                    member.Starpower = card.GetGlobalId();
                }
                else if (HomeMode.Avatar.SelectedStarpowers.Contains(card2.GetGlobalId()))
                {
                    member.HeroLevel = 9;
                    member.Starpower = card2.GetGlobalId();
                }
                else if (HomeMode.Avatar.Starpowers.Contains(card.GetGlobalId()))
                {
                    member.HeroLevel = 9;
                    member.Starpower = card.GetGlobalId();
                }
                else if (HomeMode.Avatar.Starpowers.Contains(card2.GetGlobalId()))
                {
                    member.HeroLevel = 9;
                    member.Starpower = card2.GetGlobalId();
                }
            }
            else
            {
                member.Starpower = 0;
            }
            if (hero.PowerLevel > 5)
            {
                string[] cards = { "GrowBush", "Shield", "Heal", "Jump", "ShootAround", "DestroyPet", "PetSlam", "Slow", "Push", "Dash", "SpeedBoost", "BurstHeal", "Spin", "Teleport", "Immunity", "Trail", "Totem", "Grab", "Swing", "Vision", "Regen", "HandGun", "Promote", "Sleep", "Slow", "Reload", "Reload", "Fake", "Trampoline", "Explode" };
                CharacterData cd = DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(hero.CharacterId);
                CardData WildCard = null;
                foreach (string cardname in cards)
                {
                    string n = char.ToUpper(cd.Name[0]) + cd.Name.Substring(1);
                    WildCard = DataTables.Get(DataType.Card).GetData<CardData>(n + "_" + cardname);
                    if (WildCard != null)
                    {
                        break;
                    }
                }
                if (HomeMode.Avatar.Starpowers.Contains(WildCard.GetGlobalId()))
                {
                    member.Gadget = WildCard.GetGlobalId();
                }
            }
            else
            {
                member.Gadget = 0;
            } 
            member.HeroTrophies = hero.Trophies;
            member.HeroHighestTrophies = hero.HighestTrophies;

            member.HeroLevel = hero.PowerLevel;
            member.IsOwner = true;
            member.State = 0;
            team.Members.Add(member);

            TeamMessage teamMessage = new TeamMessage();
            teamMessage.Team = team;
            HomeMode.Avatar.TeamId = team.Id;
            Connection.Send(teamMessage);
        }

        private void AcceptFriendReceived(AcceptFriendMessage message)
        {
            Account data = Accounts.Load(message.AvatarId);
            if (data == null) return;

            {
                Friend entry = HomeMode.Avatar.GetRequestFriendById(message.AvatarId);
                if (entry == null) return;

                Friend oldFriend = HomeMode.Avatar.GetAcceptedFriendById(message.AvatarId);
                if (oldFriend != null)
                {
                    HomeMode.Avatar.Friends.Remove(entry);
                    Connection.Send(new OutOfSyncMessage());
                    return;
                }

                entry.FriendReason = 0;
                entry.FriendState = 4;

                FriendListUpdateMessage update = new FriendListUpdateMessage();
                update.Entry = entry;
                Connection.Send(update);
            }

            {
                ClientAvatar avatar = data.Avatar;
                Friend entry = avatar.GetFriendById(HomeMode.Avatar.AccountId);
                if (entry == null) return;

                entry.FriendState = 4;
                entry.FriendReason = 0;

                if (LogicServerListener.Instance.IsPlayerOnline(avatar.AccountId))
                {
                    FriendListUpdateMessage update = new FriendListUpdateMessage();
                    update.Entry = entry;
                    LogicServerListener.Instance.GetGameListener(avatar.AccountId).SendTCPMessage(update);
                }
            }
        }

        private void RemoveFriendReceived(RemoveFriendMessage message)
        {
            Account data = Accounts.Load(message.AvatarId);
            if (data == null) return;

            ClientAvatar avatar = data.Avatar;

            Friend MyEntry = HomeMode.Avatar.GetFriendById(message.AvatarId);
            if (MyEntry == null) return;

            MyEntry.FriendState = 0;

            HomeMode.Avatar.Friends.Remove(MyEntry);

            FriendListUpdateMessage update = new FriendListUpdateMessage();
            update.Entry = MyEntry;
            Connection.Send(update);

            Friend OtherEntry = avatar.GetFriendById(HomeMode.Avatar.AccountId);

            if (OtherEntry == null) return;

            OtherEntry.FriendState = 0;

            avatar.Friends.Remove(OtherEntry);

            if (LogicServerListener.Instance.IsPlayerOnline(avatar.AccountId))
            {
                FriendListUpdateMessage update2 = new FriendListUpdateMessage();
                update2.Entry = OtherEntry;
                LogicServerListener.Instance.GetGameListener(avatar.AccountId).SendTCPMessage(update2);
            }
        }

        private void AddFriendReceived(AddFriendMessage message)
        {
            Account data = Accounts.Load(message.AvatarId);
            if (data == null)
            {
                Connection.Send(new AddFriendFailedMessage
                {
                    Reason = 5
                });
                return;
            }
            if (data.Avatar.AccountId == HomeMode.Avatar.AccountId)
            {
                // 2 - too many invites
                // 4 - invite urself
                // 5 doesnt exist
                // 7 - u have too many friends, rm
                // 8 - u have too many friends
                Connection.Send(new AddFriendFailedMessage
                {
                    Reason = 4
                });
                return;
            }
            if (data.Home.BlockFriendRequests)
            {
                Connection.Send(new AddFriendFailedMessage
                {
                    Reason = 0
                });
                return;
            }

            ClientAvatar avatar = data.Avatar;

            Friend requestEntry = HomeMode.Avatar.GetFriendById(message.AvatarId);
            if (requestEntry != null)
            {
                AcceptFriendReceived(new AcceptFriendMessage()
                {
                    AvatarId = message.AvatarId
                });
                return;
            }
            else
            {
                Friend friendEntry = new Friend();
                friendEntry.AccountId = HomeMode.Avatar.AccountId;
                friendEntry.DisplayData = new PlayerDisplayData(HomeMode.Home.HasPremiumPass, HomeMode.Home.ThumbnailId, HomeMode.Home.NameColorId, HomeMode.Avatar.Name);
                friendEntry.FriendReason = message.Reason;
                friendEntry.FriendState = 3;
                avatar.Friends.Add(friendEntry);

                Friend request = new Friend();
                request.AccountId = avatar.AccountId;
                request.DisplayData = new PlayerDisplayData(data.Home.HasPremiumPass, data.Home.ThumbnailId, data.Home.NameColorId, data.Avatar.Name);
                request.FriendReason = 0;
                request.FriendState = 2;
                HomeMode.Avatar.Friends.Add(request);

                if (LogicServerListener.Instance.IsPlayerOnline(message.AvatarId))
                {
                    var gameListener = LogicServerListener.Instance.GetGameListener(message.AvatarId);

                    FriendListUpdateMessage update = new FriendListUpdateMessage();
                    update.Entry = friendEntry;

                    gameListener.SendTCPMessage(update);
                }

                FriendListUpdateMessage update2 = new FriendListUpdateMessage();
                update2.Entry = request;
                Connection.Send(update2);
            }
        }

        private void AskForFriendListReceived(AskForFriendListMessage message)
        {
            FriendListMessage friendList = new FriendListMessage();
            friendList.Friends = HomeMode.Avatar.Friends.ToArray();
            Connection.Send(friendList);
        }

        private void PlayerStatusReceived(PlayerStatusMessage message)
        {
            if (HomeMode == null) return;
            if (message.Status < 0) return;
            int oldstatus = HomeMode.Avatar.PlayerStatus;
            int newstatus = message.Status;
            /*
             * practice:
             * 10
             * -1
             * 8
             *
             * battle:
             * 3
             * -1
             * 8
             */

            HomeMode.Avatar.PlayerStatus = message.Status;
            if (oldstatus == 3 && newstatus == 8)
            {
                HomeMode.Avatar.BattleStartTime = DateTime.UtcNow;
            }
            if (oldstatus == 8 && newstatus == 3)
            {
                Hero h = HomeMode.Avatar.GetHero(HomeMode.Home.CharacterId);
                int lose = 0;
                int brawlerTrophies = h.Trophies;
                if (brawlerTrophies <= 49)
                {
                    lose = 0;
                }
                else if (50 <= brawlerTrophies && brawlerTrophies <= 99)
                {
                    lose = -1;
                }
                else if (100 <= brawlerTrophies && brawlerTrophies <= 199)
                {
                    lose = -2;
                }
                else if (200 <= brawlerTrophies && brawlerTrophies <= 299)
                {
                    lose = -3;
                }
                else if (300 <= brawlerTrophies && brawlerTrophies <= 399)
                {
                    lose = -4;
                }
                else if (400 <= brawlerTrophies && brawlerTrophies <= 499)
                {
                    lose = -5;
                }
                else if (500 <= brawlerTrophies && brawlerTrophies <= 599)
                {
                    lose = -6;
                }
                else if (600 <= brawlerTrophies && brawlerTrophies <= 699)
                {
                    lose = -7;
                }
                else if (700 <= brawlerTrophies && brawlerTrophies <= 799)
                {
                    lose = -8;
                }
                else if (800 <= brawlerTrophies && brawlerTrophies <= 899)
                {
                    lose = -9;
                }
                else if (900 <= brawlerTrophies && brawlerTrophies <= 999)
                {
                    lose = -10;
                }
                else if (1000 <= brawlerTrophies && brawlerTrophies <= 1099)
                {
                    lose = -11;
                }
                else if (1100 <= brawlerTrophies && brawlerTrophies <= 1199)
                {
                    lose = -12;
                }
                else if (brawlerTrophies >= 1200)
                {
                    lose = -12;
                }
                h.AddTrophies(lose);
                HomeMode.Home.PowerPlayGamesPlayed = Math.Max(0, HomeMode.Home.PowerPlayGamesPlayed - 1);
                HomeMode.Avatar.BattleStartTime = new DateTime();
                HomeMode.Home.TrophiesReward = 0;
                Logger.BLog($"Игрок {LogicLongCodeGenerator.ToCode(HomeMode.Avatar.AccountId)} вышел с матча!");
                HomeMode.Avatar.BattleStartTime = DateTime.MinValue;
                if (HomeMode.Home.NotificationFactory.NotificationList.Count < 5)
                {  
                    long id = HomeMode.Avatar.AccountId;
                    Notification n = new()
                {
                    Id = 81,
                    MessageEntry = $"Поскольку ты вышел с матча досрочно, ты теряешь {lose} трофеев!"
                };

                HomeMode.Home.NotificationFactory.Add(n);

                LogicAddNotificationCommand acm = new() { Notification = n };

                AvailableServerCommandMessage asm = new() { Command = acm };

                if (Sessions.IsSessionActive(id))
            {
                Session session = Sessions.GetSession(id);
                session.GameListener.SendTCPMessage(asm);
            }
                }
            }

            FriendOnlineStatusEntryMessage entryMessage = new FriendOnlineStatusEntryMessage();
            entryMessage.AvatarId = HomeMode.Avatar.AccountId;
            entryMessage.PlayerStatus = HomeMode.Avatar.PlayerStatus;

            foreach (Friend friend in HomeMode.Avatar.Friends.ToArray())
            {
                if (LogicServerListener.Instance.IsPlayerOnline(friend.AccountId))
                {
                    LogicServerListener.Instance.GetGameListener(friend.AccountId).SendTCPMessage(entryMessage);
                }
            }
        }

        private void SendMyAllianceData(Alliance alliance)
        {
            MyAllianceMessage myAlliance = new MyAllianceMessage();
            myAlliance.Role = HomeMode.Avatar.AllianceRole;
            myAlliance.OnlineMembers = alliance.OnlinePlayers;
            myAlliance.AllianceHeader = alliance.Header;
            Connection.Send(myAlliance);

            AllianceStreamMessage stream = new AllianceStreamMessage();
            stream.Entries = alliance.Stream.GetEntries();
            Connection.Send(stream);
        }
        
        private string Profanity(string input)
        {
            var profanity = new HashSet<string>(
                File.ReadAllLines("Assets/profanity.txt")
                    .Select(line => line.Trim())
                    .Where(line => !string.IsNullOrEmpty(line)),
                StringComparer.OrdinalIgnoreCase);
            Regex wordRegex = new Regex(@"\b[\p{L}']+\b", RegexOptions.IgnoreCase);
            string result = wordRegex.Replace(input, match =>
            {
                string word = match.Value;
                foreach (string badWord in profanity)
                {
                    if (word.StartsWith(badWord, StringComparison.OrdinalIgnoreCase) &&
                        word.Length <= badWord.Length + 3)
                    {
                        return new string('*', word.Length);
                    }
                }
                return word;
            });

            return result;
        }
                
        private void ChatToAllianceStreamReceived(ChatToAllianceStreamMessage message)
        {
            Alliance alliance = Alliances.Load(HomeMode.Avatar.AllianceId);
            if (alliance == null) return;

            if (message.Message.StartsWith("/"))
            {
                string[] cmd = message.Message.Substring(1).Split(' ');
                if (cmd.Length == 0) return;
                
                DebugOpenMessage debugopen = new()
                {

                };

                AllianceStreamEntryMessage response = new()
                {
                    Entry = new AllianceStreamEntry
                    {
                        AuthorName = "Pablo Helper Bot",
                        AuthorId = HomeMode.Avatar.AccountId + 1,
                        Id = alliance.Stream.EntryIdCounter + 1,
                        AuthorRole = AllianceRole.Member,
                        Type = 2
                    }
                };

                long accountId = HomeMode.Avatar.AccountId;

                switch (cmd[0])
                {
                    case "status":
                        long megabytesUsed = Process.GetCurrentProcess().PrivateMemorySize64 / (1024 * 1024);
                        DateTime now = Process.GetCurrentProcess().StartTime;
                        DateTime futureDate = DateTime.Now;

                        TimeSpan timeDifference = futureDate - now;

                        string formattedTime = string.Format("{0}{1}{2}{3}",
                        timeDifference.Days > 0 ? $"{timeDifference.Days} дней, " : string.Empty,
                        timeDifference.Hours > 0 || timeDifference.Days > 0 ? $"{timeDifference.Hours} часов, " : string.Empty,
                        timeDifference.Minutes > 0 || timeDifference.Hours > 0 ? $"{timeDifference.Minutes} минут, " : string.Empty,
                        timeDifference.Seconds > 0 ? $"{timeDifference.Seconds} секунд" : string.Empty);

                        response.Entry.Message = $"Статус сервера:\n" +
                            $"Серверная версия игры: v30.242\n" +
                            $"Билд сервера: v1.0 с 22.04.2025\n" +
                            $"SHA Ресурсы: {Fingerprint.Sha}\n" +
                            $"Среда: Production\n" +
                            $"Серверное время: {DateTime.Now} EEST\n" +
                            $"Онлайн: {Sessions.Count}\n" +
                            $"Используется ОЗУ: {megabytesUsed} МБ\n" + 
                            $"Аптайм: {formattedTime}\n";
                        Connection.Send(response);
                        break;
                    case "help":
                        response.Entry.Message = $"Доступные команды:\n/help - показать все доступные команды\n/status - посмотреть статус сервера\n/promo - активировать промокод\n/givecoins - передать монеты\n/givegems - передать гемы\n/givesp - передать старпоинты\n/lobbyinfo [on/off] - включить/выключить информацию в лобби\n\nPablo Connect\n/register [пароль] [сновапароль] - зарегистрировать аккаунт\n/login [тэг] [пароль] - загрузить аккаунт\n/changepass [старый пароль] [новый пароль] - поменять пароль";
                        Connection.Send(response);
                        break;
                    case "promo":
                        if (cmd.Length < 2)
                        {
                            response.Entry.Message = "Использование: /promo [код]";
                            Connection.Send(response);
                            break;
                        }

                        string promoCode = cmd[1];
                        string promoFilePath = "/root/PabloBrawl/prod31/Supercell.Laser.Server/JSON/promo.json";

                        if (IsPromoCodeValid(promoCode, promoFilePath, out JObject promoData))
                        {
                            var history = promoData["ActivationHistory"]?.ToObject<List<long>>() ?? new List<long>();
                            int maxActivations = promoData["MaxActivations"]?.ToObject<int>() ?? -1;
                            
                            if (maxActivations == 0)
                            {
                                response.Entry.Message = "Промокод исчерпал лимит активаций.";
                                Connection.Send(response);
                                break;
                            }

                            if (history.Contains(HomeMode.Avatar.AccountId))
                            {
                                response.Entry.Message = "Вы уже активировали этот промокод.";
                                Connection.Send(response);
                                break;
                            }
                            
                            Reward(promoCode, promoData, HomeMode.Avatar.AccountId, promoFilePath, history, ref maxActivations);

                            response.Entry.Message = $"Промокод <c6>{promoCode}</c> успешно активирован!";
                        }
                        else
                        {
                            response.Entry.Message = "Неверный или просроченный промокод.";
                        }

                        Connection.Send(response);
                        break;
                    default:
                        response.Entry.Message = $"Неизвестная команда \"{cmd[0]}\" - введи \"/help\" чтобы получить список команд!";
                        Connection.Send(response);
                        break;
                    case "debug":
                        if (HomeMode.Avatar.AccountId != 1 || HomeMode.Avatar.AccountId != 1195)
                        {
                            response.Entry.Message = $"You don\'t have right to use this command"; // /usecode [code] - use bonus code
                            Connection.Send(response);
                            return;
                        }
                        response.Entry.Message = "Debug open!";
                        Connection.Send(response);
                        Connection.Send(debugopen);
                        break;
                    case "lobbyinfo":
                    {
                        if (cmd.Length == 1)
                        {
                            bool isLobbyInfoEnabled = HomeMode.Home.LobbyInfo;
                            string currentText = string.IsNullOrEmpty(HomeMode.Home.CustomLobbyInfo)
                                ? "Стандартный текст"
                                : HomeMode.Home.CustomLobbyInfo;

                            response.Entry.Message = $"Текущее состояние информации в лобби: {(isLobbyInfoEnabled ? "включено" : "выключено")}\n\n" +
                                $"Текущий кастомный текст:\n{currentText}\n\n" +
                                "Используйте:\n" +
                                "/lobbyinfo on - включить отображение\n" +
                                "/lobbyinfo off - выключить отображение\n" +
                                "/lobbyinfo change [текст] - установить кастомный текст (можно использовать {pingInfo}, {onlineCount}, {currentTime}, {hasPremium})\n" +
                                "/lobbyinfo addcustom [текст] - добавить текст к существующему лоббиинфо\n" +
                                "/lobbyinfo default - сбросить кастомный текст\n" +
                                "/lobbyinfo info - показать текущий текст лобби";
                        }
                        else
                        {
                            string mode = cmd[1].ToLower();

                            bool requiresPremium = mode == "change" || mode == "addcustom";
                            if (requiresPremium && HomeMode.Home.PremiumEndTime < DateTime.UtcNow)
                            {
                                response.Entry.Message = "Только для Pablo Premium!";
                                Connection.Send(response);
                                return;
                            }

                            switch (mode)
                            {
                                case "on":
                                    HomeMode.Home.LobbyInfo = true;
                                    response.Entry.Message = "Информация в лобби включена.";
                                    break;

                                case "off":
                                    HomeMode.Home.LobbyInfo = false;
                                    response.Entry.Message = "Информация в лобби выключена.";
                                    break;

                                case "default":
                                    HomeMode.Home.CustomLobbyInfo = null;
                                    response.Entry.Message = "Кастомный текст лобби был сброшен.";
                                    break;

                                case "info":
                                    response.Entry.Message = string.IsNullOrEmpty(HomeMode.Home.CustomLobbyInfo)
                                        ? "Кастомный текст лобби не установлен."
                                        : $"Текущий кастомный текст:\n{HomeMode.Home.CustomLobbyInfo}";
                                    break;

                                case "change":
                                    if (cmd.Length < 3)
                                    {
                                        response.Entry.Message = "Введите текст для лоббиинфо после команды. Пример: /lobbyinfo change Привет! Онлайн: {onlineCount}";
                                    }
                                    else
                                    {
                                        string customText = string.Join(" ", cmd.Skip(2)).Trim();
                                        customText = customText.Replace("\\n", "\n");

                                        if (string.IsNullOrEmpty(customText))
                                        {
                                            response.Entry.Message = "Текст для лоббиинфо не может быть пустым.";
                                        }
                                        else
                                        {
                                            HomeMode.Home.CustomLobbyInfo = customText;
                                            response.Entry.Message = "Кастомный лоббиинфо установлен.";
                                        }
                                    }
                                    break;

                                case "addcustom":
                                    if (cmd.Length < 3)
                                    {
                                        response.Entry.Message = "Введите текст, который нужно добавить. Пример: /lobbyinfo addcustom \\nСпециальное сообщение!";
                                    }
                                    else
                                    {
                                        string newText = string.Join(" ", cmd.Skip(2)).Trim();
                                        newText = newText.Replace("\\n", "\n");

                                        if (string.IsNullOrEmpty(newText))
                                        {
                                            response.Entry.Message = "Нельзя добавить пустой текст.";
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(HomeMode.Home.CustomLobbyInfo))
                                            {
                                                HomeMode.Home.CustomLobbyInfo = newText;
                                                response.Entry.Message = "Текст добавлен. Теперь лоббиинфо:\n" + newText;
                                            }
                                            else
                                            {
                                                HomeMode.Home.CustomLobbyInfo += newText;
                                                response.Entry.Message = "Текст успешно добавлен к лоббиинфо.";
                                            }
                                        }
                                    }
                                    break;

                                default:
                                    response.Entry.Message = "Неверный параметр. Используйте /lobbyinfo help для справки.";
                                    break;
                            }
                        }

                        Connection.Send(response);
                        break;
                    }
                    case "givegems":
                        if (cmd.Length != 3)
                        {
                            response.Entry.Message = "Использование: /givegems #тэг кол-во";
                            Connection.Send(response);
                            return;
                        }

                        if (!cmd[1].StartsWith('#'))
                        {
                            response.Entry.Message = "Невалидный тэг.";
                            Connection.Send(response);
                            return;
                        }

                        long receiverId = LogicLongCodeGenerator.ToId(cmd[1]);
                        Account receiverAccount = Accounts.Load(receiverId);
                        long lowIDgiver = HomeMode.Avatar.AccountId;
                        Account accountgiver = Accounts.Load(lowIDgiver);

                        if (receiverAccount == null)
                        {
                            response.Entry.Message = $"Невалидный тэг получателя.";
                            Connection.Send(response);
                            return;
                        }

                        int amount;
                        if (!int.TryParse(cmd[2], out amount) || amount <= 0)
                        {
                            response.Entry.Message = "Невалидное кол-во.";
                            Connection.Send(response);
                            return;
                        }

                        int leftgems = amount - accountgiver.Avatar.Diamonds;
                        if (accountgiver.Avatar.Diamonds < amount)
                        {
                            response.Entry.Message = $"Недостаточно гемов! Тебе нужно {leftgems} гемов.";
                            Connection.Send(response);
                            return;
                        }

                        accountgiver.Avatar.Diamonds -= amount;
                        int receivedAmount = (int)(amount * 0.8);
                        Accounts.Save(accountgiver);

                        string receiverTag = LogicLongCodeGenerator.ToCode(receiverId);
                        string accountsender = LogicLongCodeGenerator.ToCode(lowIDgiver);

                        response.Entry.Message = $"Ты отправил {amount} гемов пользователю {receiverTag}!";
                        Connection.Send(response);
    
                        Notification nGems = new()
                        {
                            Id = 89,
                            DonationCount = receivedAmount,
                            MessageEntry = $"<c6>Пользователь {accountsender} передал тебе {receivedAmount} гемов!</c>"
                        };
                        receiverAccount.Home.NotificationFactory.Add(nGems);
                        LogicAddNotificationCommand acmGems = new() { Notification = nGems };
                        AvailableServerCommandMessage asmGems = new();
                        asmGems.Command = acmGems;

                        if (Sessions.IsSessionActive(receiverId))
                        {
                            Session sessionGems = Sessions.GetSession(receiverId);
                            sessionGems.GameListener.SendTCPMessage(asmGems);
                        }
                        Logger.VLog($"Пользователь {accountsender} передал {amount} ГЕМОВ (получено: {receivedAmount}) пользователю {receiverTag}. Команда: /givegems {cmd[1]} {cmd[2]}");
                        break;
                    case "givecoins":
                        if (cmd.Length != 3)
                        {
                            response.Entry.Message = "Использование: /givegems #тэг кол-во";
                            Connection.Send(response);
                            return;
                        }

                        if (!cmd[1].StartsWith('#'))
                        {
                            response.Entry.Message = "Невалидный тэг.";
                            Connection.Send(response);
                            return;
                        }

                        long creceiverId = LogicLongCodeGenerator.ToId(cmd[1]);
                        Account creceiverAccount = Accounts.Load(creceiverId);
                        long clowIDgiver = HomeMode.Avatar.AccountId;
                        Account caccountgiver = Accounts.Load(clowIDgiver);

                        if (creceiverAccount == null)
                        {
                            response.Entry.Message = $"Невалидный тэг получателя.";
                            Connection.Send(response);
                            return;
                        }

                        int camount;
                        if (!int.TryParse(cmd[2], out camount) || camount <= 0)
                        {
                            response.Entry.Message = "Невалидное кол-во.";
                            Connection.Send(response);
                            return;
                        }

                        int leftcoins = camount - caccountgiver.Avatar.Gold;
                        if (caccountgiver.Avatar.Gold < camount)
                        {
                            response.Entry.Message = $"Недостаточно гемов! Тебе нужно {leftcoins} монет.";
                            Connection.Send(response);
                            return;
                        }

                        caccountgiver.Avatar.Gold -= camount;
                        int creceivedAmount = (int)(camount * 0.8);
                        Accounts.Save(caccountgiver);

                        string creceiverTag = LogicLongCodeGenerator.ToCode(creceiverId);
                        string caccountsender = LogicLongCodeGenerator.ToCode(clowIDgiver);

                        response.Entry.Message = $"Ты отправил {camount} монет пользователю {creceiverTag}!";
                        Connection.Send(response);
    
                        Notification nCoins = new()
                        {
                            Id = 90,
                            DonationCount = creceivedAmount,
                            MessageEntry = $"<c6>Пользователь {caccountsender} передал тебе {creceivedAmount} монет!</c>"
                        };
                        creceiverAccount.Home.NotificationFactory.Add(nCoins);
                        LogicAddNotificationCommand acmCoins = new() { Notification = nCoins };
                        AvailableServerCommandMessage asmCoins = new();
                        asmCoins.Command = acmCoins;

                        if (Sessions.IsSessionActive(creceiverId))
                        {
                            Session sessionCoins = Sessions.GetSession(creceiverId);
                            sessionCoins.GameListener.SendTCPMessage(asmCoins);
                        }
                        Logger.VLog($"Пользователь {caccountsender} передал {camount} МОНЕТ (получено: {creceivedAmount}) пользователю {creceiverTag}. Команда: /givecoins {cmd[1]} {cmd[2]}");
                        break;
                    case "givesp":
                        if (cmd.Length != 3)
                        {
                            response.Entry.Message = "Использование: /givesp #тэг кол-во";
                            Connection.Send(response);
                            return;
                        }

                        if (!cmd[1].StartsWith('#'))
                        {
                            response.Entry.Message = "Невалидный тэг.";
                            Connection.Send(response);
                            return;
                        }

                        long spreceiverId = LogicLongCodeGenerator.ToId(cmd[1]);
                        Account spreceiverAccount = Accounts.Load(spreceiverId);
                        long splowIDgiver = HomeMode.Avatar.AccountId;
                        Account spaccountgiver = Accounts.Load(splowIDgiver);

                        if (spreceiverAccount == null)
                        {
                            response.Entry.Message = $"Невалидный тэг получателя.";
                            Connection.Send(response);
                            return;
                        }

                        int spamount;
                        if (!int.TryParse(cmd[2], out spamount) || spamount <= 0)
                        {
                            response.Entry.Message = "Невалидное кол-во.";
                            Connection.Send(response);
                            return;
                        }

                        int leftsp = spamount - spaccountgiver.Avatar.StarPoints;
                        if (spaccountgiver.Avatar.StarPoints < spamount)
                        {
                            response.Entry.Message = $"Недостаточно гемов! Тебе нужно {leftsp} старпоинтов.";
                            Connection.Send(response);
                            return;
                        }

                        spaccountgiver.Avatar.StarPoints -= spamount;
                        int spreceivedAmount = (int)(spamount * 0.8);
                        Accounts.Save(spaccountgiver);

                        string spreceiverTag = LogicLongCodeGenerator.ToCode(spreceiverId);
                        string spaccountsender = LogicLongCodeGenerator.ToCode(splowIDgiver);

                        response.Entry.Message = $"Ты отправил {spamount} старпоинтов пользователю {spreceiverTag}!";
                        Connection.Send(response);
    
                        Notification nSp = new()
                        {
                            Id = 91,
                            DonationCount = spreceivedAmount,
                            MessageEntry = $"<c6>Пользователь {spaccountsender} передал тебе {spreceivedAmount} старпоинтов!</c>"
                        };
                        spreceiverAccount.Home.NotificationFactory.Add(nSp);
                        LogicAddNotificationCommand acmSp = new() { Notification = nSp };
                        AvailableServerCommandMessage asmSp = new();
                        asmSp.Command = acmSp;

                        if (Sessions.IsSessionActive(spreceiverId))
                        {
                            Session sessionSp = Sessions.GetSession(spreceiverId);
                            sessionSp.GameListener.SendTCPMessage(asmSp);
                        }
                        Logger.VLog($"Пользователь {spaccountsender} передал {spamount} СТАРПОИНТОВ (получено: {spreceivedAmount}) пользователю {spreceiverTag}. Команда: /givesp {cmd[1]} {cmd[2]}");
                        break;
                    case "load":
                        if (cmd.Length != 3)
                        {
                            response.Entry.Message = $"Использование: /load [тэг] [код]";
                            Connection.Send(response);
                            return;
                        }

                        string loadTag = cmd[1];
                        string loadCode = cmd[2];
                        
                        if (!loadTag.StartsWith("#"))
                        {
                            response.Entry.Message = "Тэг должен начинаться с #.";
                            Connection.Send(response);
                            return;
                        }
                        
                        long loadid = LogicLongCodeGenerator.ToId(cmd[1]);
                                                                        
                        Account loadaccount = Accounts.Load(loadid);

                        if (loadaccount == null)
                        {
                            response.Entry.Message = $"Аккаунт не найден.";
                            Connection.Send(response);
                            return;
                        }
                        if (!int.TryParse(loadCode, out int parsedLoadCode))
                        {
                            response.Entry.Message = "Неверный формат кода.";
                            Connection.Send(response);
                            return;
                        }
                        if (parsedLoadCode != loadaccount.Home.LoadCode)
                        {
                            response.Entry.Message = "Код для входа неверный.";
                            Connection.Send(response);
                            return;
                        }

                        Connection.Send(new CreateAccountOkMessage
                        {
                            AccountId = loadaccount.AccountId,
                            PassToken = loadaccount.PassToken
                        });

                        Connection.Send(new AuthenticationFailedMessage
                        {                            
                            ErrorCode = 22,
                            Message = "Вы успешно вошли в аккаунт!"
                        });
                        loadaccount.Home.LoadCode = null;
                        break;
                    // ACCOUNT SYSTEM HERE
                    case "register":
                        if (cmd.Length != 3)
                        {
                            response.Entry.Message = $"Использование: /register [пароль] [повтор пароля]";
                            Connection.Send(response);
                            return;
                        }
                        
                        string password = cmd[1];
                        string againpassword = cmd[2];                        

                            if (password != againpassword)
                            {
                                response.Entry.Message = "Пароль не совпадает!";
                                Connection.Send(response);
                                return;
                            }
                        bool registrationSuccess = RegisterUserToDatabase(password, accountId);

                        if (!registrationSuccess)
                        {
                            response.Entry.Message = $"Аккаунт уже привязан!";
                            Connection.Send(response);
                            return;
                        }
                        Account plreaccount = Accounts.Load(accountId);
                        Notification brlyn = new()
                        {
                            Id = 94,
                            skin = 59,
                            MessageEntry = "<c6>Спасибо за привязку!</c>"
                        };
                        plreaccount.Home.NotificationFactory.Add(brlyn);
                        LogicAddNotificationCommand acm = new() { Notification = brlyn };

                        AvailableServerCommandMessage asm = new() { Command = acm };
                        if (Sessions.IsSessionActive(accountId))
                    {
                        Session session = Sessions.GetSession(accountId);
                        session.GameListener.SendTCPMessage(asm);
                    }                        
                        response.Entry.Message = $"Регистрация успешна! Вы получили награду за привязку.";    
                        Connection.Send(response);
                        break;
                    case "login":
                        if (cmd.Length != 3)
                        {
                            response.Entry.Message = $"Использование: /login [тэг] [пароль]";
                            Connection.Send(response);
                            return;
                        }

                        string loginTag = cmd[1];
                        string loginPassword = cmd[2];
                        
                        if (!loginTag.StartsWith("#"))
                        {
                            response.Entry.Message = "Тэг должен начинаться с #.";
                            Connection.Send(response);
                            return;
                        }
                        
                        long id = LogicLongCodeGenerator.ToId(loginTag);

                        string accountIdS = LoginUserFromDatabase(id, loginPassword);

                        if (string.IsNullOrEmpty(accountIdS))
                        {
                            response.Entry.Message = $"Никнейм или пароль неверны.";
                            Connection.Send(response);
                            return;
                        }
                        
                        Account account = Accounts.Load(id);

                        if (account == null)
                        {
                            response.Entry.Message = $"Аккаунт не найден.";
                            Connection.Send(response);
                            return;
                        }

                        Connection.Send(new CreateAccountOkMessage
                        {
                            AccountId = account.AccountId,
                            PassToken = account.PassToken
                        });

                        Connection.Send(new AuthenticationFailedMessage
                        {
                            ErrorCode = 8,
                            Message = "Вы успешно вошли в аккаунт!"
                        });
                        break;
                    case "changepass":
                        if (cmd.Length != 3)
                        {
                            response.Entry.Message = $"Использование: /changepass [старый пароль] [новый пароль]";
                            Connection.Send(response);
                            return;
                        }
                        
                        string oldPassword = cmd[1];
                        string newPassword = cmd[2]; 
                        long idlog = HomeMode.Avatar.AccountId;
                        bool passwordChanged = ChangeUserPassword(idlog, oldPassword, newPassword);

                        if (!passwordChanged)
                        {
                            response.Entry.Message = $"Никнейм или старый пароль неверны.";
                            Connection.Send(response);
                            return;
                        }

                        response.Entry.Message = $"Пароль успешно изменён.";
                        Connection.Send(response);
                        break;
                    /*case "theme":
                        if (HomeMode.Home.PremiumEndTime < DateTime.UtcNow)
                        {
                            response.Entry.Message = $"Только для Pablo Premium!"; // /usecode [code] - use bonus code
                            Connection.Send(response);
                            return;
                        }
                        if (cmd.Length != 2)
                        {
                            response.Entry.Message = $"Использование: /theme [айди]\nЕсли не знаешь айди тем:\n0. Default\n1. Winter\n2. LNY\n3. CR\n4. GoldenWeek\n5. Retropolis\n6. Mecha\n7. Halloween\n8. Brawlidays\n9. LNY20\n10. PSG\n11. SC10\n12. Bazaar\n13. Monsters\n14. MoonFestival20\n15. Brawlidays 2023\n16. LNY 2024\n17. China Emz\n18. China Colt\n19. China Bo\n20. China Spike\n21. China Mortis\n22. China Edgar\n23. China Fang\n24. China Piper\n25. China Stu\n26. China Bull\n27. Stuntshow\n28. Melodie\n29. Hindu\n30. Ghosttrain\n31. Candyland\n32. Babyshark\n33. Loveswamp\n34. Spongebob";
                            Connection.Send(response);
                            return;
                        }                      
                        Account account1 = Accounts.LoadNoCache(accountId);
                        if (account1 == null) return;
                        account1.Home.Theme = int.Parse(cmd[1]);
                        Accounts.Save(account1);
                            
                        // todo
                        response.Entry.Message = $"Успешно! Перезайдите для полного применения.";
                        Connection.Send(response);
                        
                        break;*/
                }
                return;
            }
            if (!HomeMode.Avatar.IsCommunityBanned && message.Message.Length < 100)
            {
                alliance.SendChatMessage(HomeMode.Avatar.AccountId, Profanity(message.Message));
            }
            else if (HomeMode.Avatar.IsCommunityBanned)
            {
                AllianceStreamEntryMessage response = new()
                {
                    Entry = new AllianceStreamEntry
                    {
                        AuthorName = "Console",
                        AuthorId = 0,
                        Id = alliance.Stream.EntryIdCounter + 1,
                        AuthorRole = AllianceRole.Member,
                        Message = "Это сообщение не будет видно, пока не закончится срок мута!",
                        Type = 2
                    }
                };
                Connection.Send(response);
            }
            else
            {
                AllianceStreamEntryMessage response = new()
                {
                    Entry = new AllianceStreamEntry
                    {
                        AuthorName = "Console",
                        AuthorId = 0,
                        Id = alliance.Stream.EntryIdCounter + 1,
                        AuthorRole = AllianceRole.Member,
                        Message = "Произошла неизвестная ошибка.",
                        Type = 2
                    }
                };
                Connection.Send(response);
            }
        }

        private void JoinAllianceReceived(JoinAllianceMessage message)
        {
            Alliance alliance = Alliances.Load(message.AllianceId);
            if (HomeMode.Avatar.AllianceId > 0) return;
            if (message.AllianceId == HomeMode.Avatar.KickedAllianceId && HomeMode.Home.CooldownKickedClan > DateTime.Now)
            {
                AllianceResponseMessage failresponse = new AllianceResponseMessage();
                failresponse.ResponseType = 54;
                Connection.Send(failresponse);
                return;
            }
            if (alliance == null) return;
            if (alliance.Members.Count >= 100) return;

            AllianceStreamEntry entry = new AllianceStreamEntry();
            entry.AuthorId = HomeMode.Avatar.AccountId;
            entry.AuthorName = HomeMode.Avatar.Name;
            entry.Id = ++alliance.Stream.EntryIdCounter;
            entry.PlayerId = HomeMode.Avatar.AccountId;
            entry.PlayerName = HomeMode.Avatar.Name;
            entry.Type = 4;
            entry.Event = 3;
            entry.AuthorRole = HomeMode.Avatar.AllianceRole;
            alliance.AddStreamEntry(entry);

            HomeMode.Avatar.AllianceRole = AllianceRole.Member;
            HomeMode.Avatar.AllianceId = alliance.Id;
            alliance.Members.Add(new AllianceMember(HomeMode.Avatar));

            AllianceResponseMessage response = new AllianceResponseMessage();
            response.ResponseType = 40;
            Connection.Send(response);

            SendMyAllianceData(alliance);
        }

        private void LeaveAllianceReceived(LeaveAllianceMessage message)
        {
            if (HomeMode.Avatar.AllianceId < 0 || HomeMode.Avatar.AllianceRole == AllianceRole.None) return;

            Alliance alliance = Alliances.Load(HomeMode.Avatar.AllianceId);
            if (alliance == null) return;
            if (HomeMode.Avatar.AllianceRole == AllianceRole.Leader)
            {
                AllianceMember nextLeader = alliance.GetNextRoleMember();
                if (nextLeader == null)
                {
                    alliance.RemoveMemberById(HomeMode.Avatar.AccountId);
                    if (alliance.Members.Count < 1)
                    {
                        Alliances.Delete(HomeMode.Avatar.AllianceId);
                    }
                    HomeMode.Avatar.AllianceId = -1;
                    HomeMode.Avatar.AllianceRole = AllianceRole.None;

                    Connection.Send(new AllianceResponseMessage
                    {
                        ResponseType = 80
                    });

                    Connection.Send(new MyAllianceMessage());

                    return;
                };
                Account target = Accounts.Load(nextLeader.AccountId);
                if (target == null) return;
                target.Avatar.AllianceRole = AllianceRole.Leader;
                nextLeader.Role = AllianceRole.Leader;
                if (LogicServerListener.Instance.IsPlayerOnline(target.AccountId))
                {
                    LogicServerListener.Instance.GetGameListener(target.AccountId).SendTCPMessage(new AllianceResponseMessage()
                    {
                        ResponseType = 101
                    });
                    MyAllianceMessage targetAlliance = new()
                    {
                        AllianceHeader = alliance.Header,
                        Role = HomeMode.Avatar.AllianceRole
                    };
                    LogicServerListener.Instance.GetGameListener(target.AccountId).SendTCPMessage(targetAlliance);
                }
            }
            alliance.RemoveMemberById(HomeMode.Avatar.AccountId);
            if (alliance.Members.Count < 1)
            {
                Alliances.Delete(HomeMode.Avatar.AllianceId);
            }
            
            HomeMode.Avatar.AllianceId = -1;
            HomeMode.Avatar.AllianceRole = AllianceRole.None;

            AllianceStreamEntry entry = new AllianceStreamEntry();
            entry.AuthorId = HomeMode.Avatar.AccountId;
            entry.AuthorName = HomeMode.Avatar.Name;
            entry.Id = ++alliance.Stream.EntryIdCounter;
            entry.PlayerId = HomeMode.Avatar.AccountId;
            entry.PlayerName = HomeMode.Avatar.Name;
            entry.Type = 4;
            entry.Event = 4;
            entry.AuthorRole = HomeMode.Avatar.AllianceRole;
            alliance.AddStreamEntry(entry);

            AllianceResponseMessage response = new AllianceResponseMessage();
            response.ResponseType = 80;
            Connection.Send(response);

            MyAllianceMessage myAlliance = new MyAllianceMessage();
            Connection.Send(myAlliance);
        }

        private void CreateAllianceReceived(CreateAllianceMessage message)                
        {
            if (HomeMode.Avatar.AllianceId >= 0) return;
            if (message.Name.Contains("</c"))
            {
                Connection.Send(new AllianceResponseMessage()
                {
                    ResponseType = 21
                });
                return;
            }
            if (message.Description.Contains("</c"))
            {
                Connection.Send(new AllianceResponseMessage()
                {
                    ResponseType = 21
                });
                return;
            }
            if (message.Name.Length > 20)
            {
                Connection.Send(new AllianceResponseMessage()
                {
                    ResponseType = 21
                });
                return;
            }
            if (message.Description.Length > 250)
            {
                Connection.Send(new AllianceResponseMessage()
                {
                    ResponseType = 21
                });
                return;
            }
            
            if (ContainsForbiddenWords(message.Name))
            {
                Connection.Send(new AllianceResponseMessage()
                {
                    ResponseType = 22
                });
                return;
            }
            
            if (ContainsForbiddenWords(message.Description))
            {
                Connection.Send(new AllianceResponseMessage()
                {
                    ResponseType = 23
                });
                return;
            }
            Alliance alliance = new Alliance();
            alliance.Name = message.Name;
            alliance.Description = message.Description;
            alliance.RequiredTrophies = message.RequiredTrophies;
            alliance.Type = message.Type == 1 ? 1 : message.Type == 2 ? 2 : 3;

            if (message.BadgeId >= 8000000 && message.BadgeId < 8000000 + DataTables.Get(DataType.AllianceBadge).Count)
            {
                alliance.AllianceBadgeId = message.BadgeId;
            }
            else
            {
                alliance.AllianceBadgeId = 8000000;
            }

            HomeMode.Avatar.AllianceRole = AllianceRole.Leader;
            alliance.Members.Add(new AllianceMember(HomeMode.Avatar));

            Alliances.Create(alliance);

            HomeMode.Avatar.AllianceId = alliance.Id;

            AllianceResponseMessage response = new AllianceResponseMessage();
            response.ResponseType = 20;
            Connection.Send(response);

            SendMyAllianceData(alliance);
        }

        private void AskForAllianceDataReceived(AskForAllianceDataMessage message)
        {
            Alliance alliance = Alliances.Load(message.AllianceId);
            if (alliance == null) return;

            AllianceDataMessage data = new AllianceDataMessage();
            data.Alliance = alliance;
            data.IsMyAlliance = message.AllianceId == HomeMode.Avatar.AllianceId;
            Connection.Send(data);
        }
        private void ChangeAllianceMemberRoleReceived(ChangeAllianceMemberRoleMessage message)
        {
            if (HomeMode.Avatar.AllianceId <= 0) return;
            if (HomeMode.Avatar.AllianceRole == AllianceRole.Member || HomeMode.Avatar.AllianceRole == AllianceRole.None) return;

            Alliance alliance = Alliances.Load(HomeMode.Avatar.AllianceId);
            if (alliance == null) return;

            AllianceMember member = alliance.GetMemberById(message.AccountId);
            if (member == null) return;

            ClientAvatar avatar = Accounts.Load(message.AccountId).Avatar;

            AllianceRole
                    Member = (AllianceRole)1,
                    Leader = (AllianceRole)2,
                    Elder = (AllianceRole)3,
                    CoLeader = (AllianceRole)4;
            if (HomeMode.Avatar.AllianceRole == (AllianceRole)Member) return;
            //if (member.Role == Leader) return;
            if (alliance.getRoleVector(member.Role, (AllianceRole)message.Role))
            {
                if (avatar.AllianceRole == (AllianceRole)Member)
                {
                    avatar.AllianceRole = (AllianceRole)Elder;
                }
                else if (avatar.AllianceRole == (AllianceRole)Elder)
                {
                    avatar.AllianceRole = (AllianceRole)CoLeader;
                }
                else if (avatar.AllianceRole == (AllianceRole)CoLeader)
                {
                    HomeMode.Avatar.AllianceRole = (AllianceRole)CoLeader;
                    avatar.AllianceRole = (AllianceRole)Leader;
                    AllianceStreamEntry entry2 = new()
                    {
                        AuthorId = HomeMode.Avatar.AccountId,
                        AuthorName = HomeMode.Avatar.Name,
                        Id = ++alliance.Stream.EntryIdCounter,
                        PlayerId = HomeMode.Avatar.AccountId,
                        PlayerName = HomeMode.Avatar.Name,
                        Type = 4,
                        Event = 6,
                        AuthorRole = HomeMode.Avatar.AllianceRole
                    };
                    alliance.AddStreamEntry(entry2);

                    AllianceMember me = alliance.GetMemberById(HomeMode.Avatar.AccountId);
                    me.Role = HomeMode.Avatar.AllianceRole;

                }
                member.Role = avatar.AllianceRole;

                AllianceStreamEntry entry = new()
                {
                    AuthorId = HomeMode.Avatar.AccountId,
                    AuthorName = HomeMode.Avatar.Name,
                    Id = ++alliance.Stream.EntryIdCounter,
                    PlayerId = avatar.AccountId,
                    PlayerName = avatar.Name,
                    Type = 4,
                    Event = 5,
                    AuthorRole = HomeMode.Avatar.AllianceRole
                };
                alliance.AddStreamEntry(entry);

                AllianceResponseMessage response = new()
                {
                    ResponseType = 81
                };
                Connection.Send(response);
                MyAllianceMessage myAlliance = new()
                {
                    AllianceHeader = alliance.Header,
                    Role = HomeMode.Avatar.AllianceRole
                };
                Connection.Send(myAlliance);
                if (LogicServerListener.Instance.IsPlayerOnline(avatar.AccountId))
                {
                    LogicServerListener.Instance.GetGameListener(avatar.AccountId).SendTCPMessage(new AllianceResponseMessage()
                    {
                        ResponseType = 101
                    });
                    MyAllianceMessage targetAlliance = new()
                    {
                        AllianceHeader = alliance.Header,
                        Role = avatar.AllianceRole
                    };
                    LogicServerListener.Instance.GetGameListener(avatar.AccountId).SendTCPMessage(targetAlliance);
                }
            }
            else
            {
                if (avatar.AllianceRole == (AllianceRole)Elder)
                {
                    avatar.AllianceRole = (AllianceRole)Member;
                }
                else if (avatar.AllianceRole == (AllianceRole)CoLeader)
                {
                    avatar.AllianceRole = (AllianceRole)Elder;
                }
                member.Role = avatar.AllianceRole;

                AllianceStreamEntry entry = new()
                {
                    AuthorId = HomeMode.Avatar.AccountId,
                    AuthorName = HomeMode.Avatar.Name,
                    Id = ++alliance.Stream.EntryIdCounter,
                    PlayerId = avatar.AccountId,
                    PlayerName = avatar.Name,
                    Type = 4,
                    Event = 6,
                    AuthorRole = HomeMode.Avatar.AllianceRole
                };
                alliance.AddStreamEntry(entry);

                AllianceResponseMessage response = new()
                {
                    ResponseType = 82
                };
                Connection.Send(response);
                MyAllianceMessage myAlliance = new()
                {
                    AllianceHeader = alliance.Header,
                    Role = HomeMode.Avatar.AllianceRole
                };
                Connection.Send(myAlliance);
                if (LogicServerListener.Instance.IsPlayerOnline(avatar.AccountId))
                {
                    LogicServerListener.Instance.GetGameListener(avatar.AccountId).SendTCPMessage(new AllianceResponseMessage()
                    {
                        ResponseType = 102
                    });
                    MyAllianceMessage targetAlliance = new()
                    {
                        AllianceHeader = alliance.Header,
                        Role = avatar.AllianceRole
                    };
                    LogicServerListener.Instance.GetGameListener(avatar.AccountId).SendTCPMessage(targetAlliance);
                }
            }
        }

        private void AskForJoinableAllianceListReceived(AskForJoinableAllianceListMessage message)
        {
            JoinableAllianceListMessage list = new JoinableAllianceListMessage();
    
            List<Alliance> alliances = Alliances.GetRandomAlliances(25)
                                       .Distinct()
                                       .ToList();
            if (alliances.Count < 25)
            {
                long maxId = Alliances.GetMaxAllianceId();
                for (long id = 1; id <= maxId && alliances.Count < 25; id++)
                {
                    Alliance alliance = Alliances.LoadNoCache(id);
                    if (alliance != null && !alliances.Contains(alliance))
                    {
                        alliances.Add(alliance);
                    }
                }
            }
            alliances = alliances.Take(25).ToList();

            if (alliances.Count == 0)
            {
                return;
            }
            foreach (Alliance alliance in alliances)
            {
                list.JoinableAlliances.Add(alliance.Header);
            }

            Connection.Send(list);
        }

        private void ClientCapabilitesReceived(ClientCapabilitiesMessage message)
        {
            Connection.PingUpdated(message.Ping);
            ShowLobbyInfo();
        }

        private bool RegisterUserToDatabase(string password, long id)
        {
            bool success = false;

            try
            {
                string connectionString = $"server={Configuration.Instance.MysqlHost};" +
                                          $"user={Configuration.Instance.MysqlUsername};" +
                                          $"database={Configuration.Instance.MysqlDatabase};" +
                                          $"port={Configuration.Instance.MysqlPort};" +
                                          $"password={Configuration.Instance.MysqlPassword}";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO users (password, id) VALUES (@password, @id)";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@id", id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return success;
        }

        private string LoginUserFromDatabase(long id, string password)
        {
            string accountId = null;

            try
            {
                string connectionString = $"server={Configuration.Instance.MysqlHost};" +
                                          $"user={Configuration.Instance.MysqlUsername};" +
                                          $"database={Configuration.Instance.MysqlDatabase};" +
                                          $"port={Configuration.Instance.MysqlPort};" +
                                          $"password={Configuration.Instance.MysqlPassword}";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT id FROM users WHERE id = @id AND password = @password";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@password", password);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        accountId = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return accountId;
        }
        
        private bool ChangeUserPassword(long id, string oldPassword, string newPassword)
        {
            try
            {
                string connectionString = $"server={Configuration.Instance.MysqlHost};" +
                                          $"user={Configuration.Instance.MysqlUsername};" +
                                          $"database={Configuration.Instance.MysqlDatabase};" +
                                          $"port={Configuration.Instance.MysqlPort};" +
                                          $"password={Configuration.Instance.MysqlPassword}";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE id = @id AND password = @oldPassword";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@id", id);
                    checkCmd.Parameters.AddWithValue("@oldPassword", oldPassword);

                    int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (userExists == 0)
            {
                return false;
            }

            string updateQuery = "UPDATE users SET password = @newPassword WHERE id = @id";
            MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
            updateCmd.Parameters.AddWithValue("@id", id);
            updateCmd.Parameters.AddWithValue("@newPassword", newPassword);

            int rowsAffected = updateCmd.ExecuteNonQuery();
            return rowsAffected > 0;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
        return false;
    }
}        

        private void AskForBattleEndReceived(AskForBattleEndMessage message)
        {
            try 
            {
            bool isPvP;
            BattlePlayer OwnPlayer = null;

            LocationData location = DataTables.Get(DataType.Location).GetDataWithId<LocationData>(message.LocationId);
            if (location == null || location.Disabled)
            {
                return;
            }
            if (DateTime.UtcNow > HomeMode.Home.PremiumEndTime && HomeMode.Avatar.PremiumLevel > 1)
            {                
                HomeMode.Avatar.PremiumLevel = 0;
                HomeMode.Home.NotificationFactory.Add(new Notification
                {
                    Id = 81,
                    //TimePassed =
                    MessageEntry = $"Срок твоего PREMIUM Статуса окончился."
                });
            }            

            isPvP = true;//Events.HasLocation(message.LocationId);

            for (int x = 0; x < message.BattlePlayersCount; x++)
            {
                BattlePlayer battlePlayer = message.BattlePlayers[x];
                if (battlePlayer.DisplayData.Name == HomeMode.Avatar.Name)
                {
                    battlePlayer.AccountId = HomeMode.Avatar.AccountId;
                    OwnPlayer = battlePlayer;

                    Hero hero = HomeMode.Avatar.GetHero(OwnPlayer.CharacterId);
                    if (hero == null)
                    {
                        return;
                    }
                    message.BattlePlayers[x].HeroPowerLevel = hero.PowerLevel + (hero.HasStarpower ? 1 : 0);
                    OwnPlayer.HeroPowerLevel = hero.PowerLevel + (hero.HasStarpower ? 1 : 0);
                    OwnPlayer.Trophies = hero.Trophies;
                    OwnPlayer.HighestTrophies = hero.HighestTrophies;
                    OwnPlayer.PowerPlayScore = HomeMode.Home.PowerPlayScore;

                    battlePlayer.DisplayData = new PlayerDisplayData(HomeMode.Home.HasPremiumPass, HomeMode.Home.ThumbnailId, HomeMode.Home.NameColorId, HomeMode.Avatar.Name);
                }
                else
                {
                    battlePlayer.DisplayData = new PlayerDisplayData(false, 28000000, 43000000, "Bot " + battlePlayer.DisplayData.Name);
                }
            }

            if (OwnPlayer == null)
            {
                return;
            }

            int StartExperience = HomeMode.Home.Experience;
            int[] ExperienceRewards = new[] { 8, 4, 6 };
            int[] TokensRewards = new[] { 32, 20, 8, 4, 0 };

            bool starToken = false;
            int gameMode = 0;
            int[] Trophies = new int[10];
            int trophiesResult = 0;
            int underdogTrophiesResult = 0;
            int experienceResult = 0;
            int totalTokensResult = 0;
            int totalCoinsResult = 0;
            int totalTokenEventResult = 0;
            int tokensResult = 0;
            int doubledTokensResult = 0;
            int MilestoneReward = 0;
            int starExperienceResult = 0;
            List<int> MilestoneRewards = new List<int>();
            int powerPlayScoreGained = 0;
            int powerPlayEpicScoreGained = 0;
            bool isPowerPlay = false;
            bool HasNoTokens = false;
            List<Quest> q = new();

            if (isPvP)
            {
                Hero hero = HomeMode.Avatar.GetHero(OwnPlayer.CharacterId);
                if (hero == null)
                {
                    return;
                }
                int slot = Events.SlotsLocations[message.LocationId];

                int brawlerTrophies = hero.Trophies;
                if (slot == 9)
                {
                    if (HomeMode.Home.PowerPlayGamesPlayed < 3)
                    {
                        isPvP = false;
                        isPowerPlay = true;
                        int[] powerPlayAwards = { 30, 5, 15 };
                        powerPlayScoreGained = powerPlayAwards[message.BattleResult];
                        HomeMode.Home.PowerPlayTrophiesReward = powerPlayScoreGained;
                        HomeMode.Home.PowerPlayGamesPlayed++;
                        HomeMode.Home.PowerPlayScore += powerPlayScoreGained;

                        HomeMode.Avatar.TrioWins++;
                        if (location.GameMode == "CoinRush")
                        {
                            if (DateTime.UtcNow.Subtract(HomeMode.Avatar.BattleStartTime).TotalSeconds <= 90)
                            {
                                HomeMode.Home.PowerPlayTrophiesReward += 3;
                                HomeMode.Home.PowerPlayScore += 3;
                                powerPlayEpicScoreGained = 3;
                            }
                        }
                        else if (location.GameMode == "LaserBall")
                        {
                            if (DateTime.UtcNow.Subtract(HomeMode.Avatar.BattleStartTime).TotalSeconds <= 30)
                            {
                                HomeMode.Home.PowerPlayTrophiesReward += 3;
                                HomeMode.Home.PowerPlayScore += 3;
                                powerPlayEpicScoreGained = 3;
                            }
                        }
                        else if (location.GameMode == "AttackDefend")
                        {
                            if (DateTime.UtcNow.Subtract(HomeMode.Avatar.BattleStartTime).TotalSeconds <= 45)
                            {
                                HomeMode.Home.PowerPlayTrophiesReward += 3;
                                HomeMode.Home.PowerPlayScore += 3;
                                powerPlayEpicScoreGained = 3;
                            }
                        }
                        else if (location.GameMode == "RoboWars")
                        {
                            if (DateTime.UtcNow.Subtract(HomeMode.Avatar.BattleStartTime).TotalSeconds <= 45)
                            {
                                HomeMode.Home.PowerPlayTrophiesReward += 3;
                                HomeMode.Home.PowerPlayScore += 3;
                                powerPlayEpicScoreGained = 3;
                            }
                        }
                        if (HomeMode.Home.PowerPlayScore >= HomeMode.Home.PowerPlayHighestScore)
                        {
                            HomeMode.Home.PowerPlayHighestScore = HomeMode.Home.PowerPlayScore;
                        }
                        if (HomeMode.Home.Quests != null)
                        {
                            if (location.GameMode == "BountyHunter")
                            {
                                q = HomeMode.Home.Quests.UpdateQuestsProgress(3, OwnPlayer.CharacterId, 0, 0, 0, HomeMode.Home);
                            }
                            else if (location.GameMode == "CoinRush")
                            {
                                q = HomeMode.Home.Quests.UpdateQuestsProgress(0, OwnPlayer.CharacterId, 0, 0, 0, HomeMode.Home);
                            }
                            else if (location.GameMode == "AttackDefend")
                            {
                                q = HomeMode.Home.Quests.UpdateQuestsProgress(2, OwnPlayer.CharacterId, 0, 0, 0, HomeMode.Home);
                            }
                            else if (location.GameMode == "LaserBall")
                            {
                                q = HomeMode.Home.Quests.UpdateQuestsProgress(5, OwnPlayer.CharacterId, 0, 0, 0, HomeMode.Home);
                            }
                            else if (location.GameMode == "RoboWars")
                            {
                                q = HomeMode.Home.Quests.UpdateQuestsProgress(11, OwnPlayer.CharacterId, 0, 0, 0, HomeMode.Home);
                            }
                        }
                    }


                }
                else if (location.GameMode == "BountyHunter" || location.GameMode == "CoinRush" || location.GameMode == "AttackDefend" || location.GameMode == "LaserBall" || location.GameMode == "RoboWars" || location.GameMode == "KingOfHill")
                {
                    if (message.BattleResult == 0)
                    {
                        // star player
                        OwnPlayer.isStarplayer = true;
                        starExperienceResult = 4;
                        HomeMode.Home.Experience += starExperienceResult;

                        // Commented out code for adding star tokens
                        /*
                        if (Events.PlaySlot(HomeMode.Avatar.AccountId, slot))
                        {
                            starToken = true;
                            HomeMode.Avatar.AddStarTokens(1);
                            HomeMode.Home.StarTokensReward = 1;
                        }
                        */
                    }
                    else
                    {
                        Random r = new();
                        message.BattlePlayers[r.Next(1, 5)].isStarplayer = true;
                    }

                    if (brawlerTrophies <= 49)
                    {
                        Trophies[0] = 16;
                        Trophies[1] = 0;
                    }
                    else if (brawlerTrophies <= 99)
                    {
                        Trophies[0] = 16;
                        Trophies[1] = -1;
                    }
                    else if (brawlerTrophies <= 199)
                    {
                        Trophies[0] = 16;
                        Trophies[1] = -2;
                    }
                    else if (brawlerTrophies <= 299)
                    {
                        Trophies[0] = 16;
                        Trophies[1] = -3;
                    }
                    else if (brawlerTrophies <= 399)
                    {
                        Trophies[0] = 16;
                        Trophies[1] = -4;
                    }
                    else if (brawlerTrophies <= 499)
                    {
                        Trophies[0] = 16;
                        Trophies[1] = -5;
                    }
                    else if (brawlerTrophies <= 599)
                    {
                        Trophies[0] = 16;
                        Trophies[1] = -6;
                    }
                    else if (brawlerTrophies <= 699)
                    {
                        Trophies[0] = 16;
                        Trophies[1] = -7;
                    }
                    else if (brawlerTrophies <= 799)
                    {
                        Trophies[0] = 16;
                        Trophies[1] = -8;
                    }
                    else if (brawlerTrophies <= 899)
                    {
                        Trophies[0] = 14;
                        Trophies[1] = -9;
                    }
                    else if (brawlerTrophies <= 999)
                    {
                        Trophies[0] = 12;
                        Trophies[1] = -10;
                    }
                    else if (brawlerTrophies <= 1099)
                    {
                        Trophies[0] = 10;
                        Trophies[1] = -11;
                    }
                    else if (brawlerTrophies <= 1199)
                    {
                        Trophies[0] = 8;
                        Trophies[1] = -12;
                    }
                    else if (brawlerTrophies >= 1200)
                    {
                        Trophies[0] = 6;
                        Trophies[1] = -12;
                    }

                    gameMode = 1;

                    trophiesResult = Trophies[message.BattleResult];
                    HomeMode.Home.TrophiesReward = Math.Max(trophiesResult, 0);

                    if (message.BattleResult == 0) // Win
                    {
                        HomeMode.Avatar.TrioWins++;

                        if (location.GameMode == "CoinRush")
                        {
                            if (DateTime.UtcNow.Subtract(HomeMode.Avatar.BattleStartTime).TotalSeconds <= 90)
                            {
                                if (HomeMode.Avatar.PremiumLevel > 0)
                                {
                                    //underdogTrophiesResult += (int)Math.Round((double)Trophies[message.BattleResult] / 4);
                                    trophiesResult += underdogTrophiesResult;
                                    HomeMode.Home.TrophiesReward = Math.Max(trophiesResult, 0);
                                }
                            }
                        }
                        else if (location.GameMode == "LaserBall")
                        {
                            if (DateTime.UtcNow.Subtract(HomeMode.Avatar.BattleStartTime).TotalSeconds <= 30)
                            {
                                if (HomeMode.Avatar.PremiumLevel > 0)
                                {
                                    //underdogTrophiesResult += (int)Math.Round((double)Trophies[message.BattleResult] / 4);
                                    trophiesResult += underdogTrophiesResult;
                                    HomeMode.Home.TrophiesReward = Math.Max(trophiesResult, 0);
                                }
                            }
                        }

                        // quests
                        if (HomeMode.Home.Quests != null)
                        {
                            switch (location.GameMode)
                            {
                                case "BountyHunter":
                                    q = HomeMode.Home.Quests.UpdateQuestsProgress(3, OwnPlayer.CharacterId, 0, 0, 0, HomeMode.Home);
                                    break;
                                case "CoinRush":
                                    q = HomeMode.Home.Quests.UpdateQuestsProgress(0, OwnPlayer.CharacterId, 0, 0, 0, HomeMode.Home);
                                    break;
                                case "AttackDefend":
                                    q = HomeMode.Home.Quests.UpdateQuestsProgress(2, OwnPlayer.CharacterId, 0, 0, 0, HomeMode.Home);
                                    break;
                                case "LaserBall":
                                    q = HomeMode.Home.Quests.UpdateQuestsProgress(5, OwnPlayer.CharacterId, 0, 0, 0, HomeMode.Home);
                                    break;
                                case "RoboWars":
                                    q = HomeMode.Home.Quests.UpdateQuestsProgress(11, OwnPlayer.CharacterId, 0, 0, 0, HomeMode.Home);
                                    break;
                            }
                        }
                    }
                    tokensResult = TokensRewards[message.BattleResult];
                    totalTokensResult = tokensResult;

                    experienceResult = ExperienceRewards[message.BattleResult];
                    HomeMode.Home.Experience += experienceResult;
                }
                else if (location.GameMode == "BattleRoyale")
                {
                    // star token logic
                    if (message.Rank < 5)
                    {
                        /*
                        if (Events.PlaySlot(HomeMode.Avatar.AccountId, slot))
                        {
                            starToken = true;
                            HomeMode.Avatar.AddStarTokens(1);
                            HomeMode.Home.StarTokensReward = 1;
                        }
                        */
                    }

                    if (brawlerTrophies >= 0 && brawlerTrophies <= 49)
                    {
                        Trophies = new[] { 20, 16, 14, 12, 8, 4, 4, 2, 0, 0 };
                    }
                    else if (brawlerTrophies <= 99)
                    {
                        Trophies = new[] { 20, 16, 14, 12, 8, 4, 4, 0, -1, -2 };
                    }
                    else if (brawlerTrophies <= 199)
                    {
                        Trophies = new[] { 20, 16, 14, 12, 10, 6, 0, -1, -2, -2 };
                    }
                    else if (brawlerTrophies <= 299)
                    {
                        Trophies = new[] { 20, 16, 12, 10, 6, 2, 0, -2, -3, -3 };
                    }
                    else if (brawlerTrophies <= 399)
                    {
                        Trophies = new[] { 20, 16, 12, 10, 4, 0, 0, -3, -4, -4 };
                    }
                    else if (brawlerTrophies <= 499)
                    {
                        Trophies = new[] { 20, 16, 12, 10, 4, -1, -2, -3, -5, -5 };
                    }
                    else if (brawlerTrophies <= 599)
                    {
                        Trophies = new[] { 20, 16, 12, 8, 4, -1, -2, -5, -6, -6 };
                    }
                    else if (brawlerTrophies <= 699)
                    {
                        Trophies = new[] { 20, 16, 12, 8, 2, -2, -2, -5, -7, -8 };
                    }
                    else if (brawlerTrophies <= 799)
                    {
                        Trophies = new[] { 20, 16, 12, 8, 2, -3, -4, -5, -8, -9 };
                    }
                    else if (brawlerTrophies <= 899)
                    {
                        Trophies = new[] { 18, 14, 10, 4, 0, -3, -4, -7, -9, -10 };
                    }
                    else if (brawlerTrophies <= 999)
                    {
                        Trophies = new[] { 16, 12, 8, 2, -1, -3, -6, -8, -10, -11 };
                    }
                    else if (brawlerTrophies <= 1099)
                    {
                        Trophies = new[] { 12, 10, 6, 2, -2, -5, -6, -9, -11, -12 };
                    }
                    else if (brawlerTrophies <= 1199)
                    {
                        Trophies = new[] { 10, 8, 2, 0, -2, -6, -7, -10, -12, -13 };
                    }
                    else if (brawlerTrophies >= 1200)
                    {
                        Trophies = new[] { 10, 6, 0, -1, -2, -6, -8, -11, -12, -13 };
                    }

                    gameMode = 2;
                    trophiesResult = Trophies[message.Rank - 1];

                    ExperienceRewards = new[] { 15, 12, 9, 6, 5, 4, 3, 2, 1, 0 };
                    TokensRewards = new[] { 30, 24, 21, 15, 12, 8, 6, 4, 2, 0 };

                    HomeMode.Home.TrophiesReward = Math.Max(trophiesResult, 0);

                    if (message.Rank == 1)
                    {
                        HomeMode.Avatar.SoloWins++;
                    }

                    if (message.Rank < 5 && HomeMode.Home.Quests != null)
                    {
                        q = HomeMode.Home.Quests.UpdateQuestsProgress(6, OwnPlayer.CharacterId, 0, 0, 0, HomeMode.Home);
                    }

                    tokensResult = TokensRewards[message.Rank - 1];
                    totalTokensResult = tokensResult;

                    experienceResult = ExperienceRewards[message.Rank - 1];
                    HomeMode.Home.Experience += experienceResult;
                }
                else if (location.GameMode == "BattleRoyaleTeam")
                {
                    // star token logic
                    if (message.Rank < 3)
                    {
                        /*
                        if (Events.PlaySlot(HomeMode.Avatar.AccountId, slot))
                        {
                            starToken = true;
                            HomeMode.Avatar.AddStarTokens(1);
                            HomeMode.Home.StarTokensReward = 1;
                        }
                        */
                    }

                    if (brawlerTrophies >= 0 && brawlerTrophies <= 49)
                    {
                        Trophies[0] = 18;
                        Trophies[1] = 14;
                        Trophies[2] = 8;
                        Trophies[3] = 0;
                        Trophies[4] = 0;
                    }
                    else if (brawlerTrophies <= 999)
                    {
                        Trophies[0] = 18;
                        Trophies[1] = 14;
                        int rankDiff = (brawlerTrophies - 100) / 100;
                        Trophies[2] = Math.Max(3 - rankDiff, 0);
                        Trophies[3] = Math.Max(-1 - rankDiff, -3);
                        Trophies[4] = Math.Max(-2 - rankDiff, -4);
                    }
                    else if (brawlerTrophies <= 1099)
                    {
                        Trophies[0] = 10;
                        Trophies[1] = 8;
                        int rankDiff = (brawlerTrophies - 1000) / 100;
                        Trophies[2] = Math.Max(-4 - rankDiff, -6);
                        Trophies[3] = Math.Max(-9 - rankDiff, -10);
                        Trophies[4] = Math.Max(-11 - rankDiff, -12);
                    }
                    else
                    {
                        Trophies[0] = 8;
                        Trophies[1] = 4;
                        Trophies[2] = -2;
                        Trophies[3] = -4;
                        Trophies[4] = -4;
                    }

                    gameMode = 5;
                    trophiesResult = Trophies[message.Rank - 1];

                    ExperienceRewards = new[] { 14, 8, 4, 2, 0 };
                    TokensRewards = new[] { 32, 20, 8, 4, 0 };

                    HomeMode.Home.TrophiesReward = Math.Max(trophiesResult, 0);

                    if (message.Rank < 3)
                    {
                        HomeMode.Avatar.DuoWins++;
                    }

                    if (message.Rank < 3 && HomeMode.Home.Quests != null)
                    {
                        q = HomeMode.Home.Quests.UpdateQuestsProgress(9, OwnPlayer.CharacterId, 0, 0, 0, HomeMode.Home);
                    }

                    tokensResult = TokensRewards[message.Rank - 1];
                    totalTokensResult = tokensResult;

                    experienceResult = ExperienceRewards[message.Rank - 1];
                    HomeMode.Home.Experience += experienceResult;
                }
                else if (location.GameMode == "BossFight")
                {                    
                    Random rnd = new Random();
    
                    int[][] bossFightRewards = new int[][]
                {
                    new int[] { 300, 400, 500, 600, 700 },
                    new int[] { 300, 400, 500, 600, 700 },
                    new int[] { 300, 400, 500, 600, 700 }
                }; 
                    int index = Math.Min(message.BattleResult, bossFightRewards.Length - 1);
                    int bossFightTokens = bossFightRewards[index][rnd.Next(bossFightRewards[index].Length)];
                    gameMode = 4;
                }
                else if (location.GameMode == "Raid_TownCrush")
                {
                    Random rnd = new Random();
    
                    int[][] raidTownRewards = new int[][]
                {
                    new int[] { 800 },
                    new int[] { 250 },
                    new int[] { 300, 400, 500, 600, 700, 800 }
                }; 
                    int index = Math.Min(message.BattleResult, raidTownRewards.Length - 1);
                    int raidTownTokens = raidTownRewards[index][rnd.Next(raidTownRewards[index].Length)];
                    totalTokensResult = raidTownTokens;
                    isPvP = false;
                    Console.WriteLine(message.BattleResult);
                    message.BattleResult = 0;
                    gameMode = 6;
                }
                else if (location.GameMode == "Raid")
                {
                    Random rnd = new Random();
    
                    int[][] raidRewards = new int[][]
                {
                    new int[] { 800 },
                    new int[] { 250 },
                    new int[] { 300, 400, 500, 600, 700, 800 }
                }; 
                    int index = Math.Min(message.BattleResult, raidRewards.Length - 1);
                    int raidTokens = raidRewards[index][rnd.Next(raidRewards[index].Length)];
                    totalTokensResult = raidTokens;      
                    isPvP = false;
                    Console.WriteLine(message.BattleResult);
                    message.BattleResult = 0;
                    gameMode = 6;
                }                
                
            /*if(location.GameMode.StartsWith("BattleRoyale"))
            {
                if(message.BattleResult == 1 || message.BattleResult == 2 || message.BattleResult == 3 || message.BattleResult == 4)
            {
                HomeMode.Avatar.WinStreak += 1;
            }
            else
            {
                HomeMode.Avatar.WinStreak = 0;
            }

                if(HomeMode.Avatar.WinStreak == 2)
            {
                trophiesResult += 1;
                underdogTrophiesResult += 1;
            }
            else if(HomeMode.Avatar.WinStreak == 3)
            {
                trophiesResult += 2;
                underdogTrophiesResult += 2;
            }
                else if(HomeMode.Avatar.WinStreak == 4)
            {
                trophiesResult += 3;
                underdogTrophiesResult += 3;
            }
                else if(HomeMode.Avatar.WinStreak == 5)
            {
                trophiesResult += 4;
                underdogTrophiesResult += 4;
            }
                else if(HomeMode.Avatar.WinStreak >= 6)
            {
                trophiesResult += 5;
                underdogTrophiesResult += 5;
            }
        }
                if(location.GameMode.StartsWith("BattleRoyaleTeam"))
            {
                if(message.BattleResult == 1 || message.BattleResult == 2)
            {
                HomeMode.Avatar.WinStreak += 1;
            }
            else
            {
                HomeMode.Avatar.WinStreak = 0;
            }

                if(HomeMode.Avatar.WinStreak == 2)
            {
                trophiesResult += 1;
                underdogTrophiesResult += 1;
            }
            else if(HomeMode.Avatar.WinStreak == 3)
            {
                trophiesResult += 2;
                underdogTrophiesResult += 2;
            }
                else if(HomeMode.Avatar.WinStreak == 4)
            {
                trophiesResult += 3;
                underdogTrophiesResult += 3;
            }
                else if(HomeMode.Avatar.WinStreak == 5)
            {
                trophiesResult += 4;
                underdogTrophiesResult += 4;
            }
                else if(HomeMode.Avatar.WinStreak >= 6)
            {
                trophiesResult += 5;
                underdogTrophiesResult += 5;
            }
        }
                if(!location.GameMode.StartsWith("BattleRoyale") && !location.GameMode.StartsWith("BattleRoyaleTeam"))
        {
            if(message.BattleResult != 0)
            {
                HomeMode.Avatar.WinStreak = 0;
            }
            else
            {
                HomeMode.Avatar.WinStreak += 1;
            }

            if(HomeMode.Avatar.WinStreak == 2)
            {
                trophiesResult += 1;
                underdogTrophiesResult += 1;
            }
            else if(HomeMode.Avatar.WinStreak == 3)
            {
                trophiesResult += 2;
                underdogTrophiesResult += 2;
            }
            else if(HomeMode.Avatar.WinStreak == 4)
            {
                trophiesResult += 3;
                underdogTrophiesResult += 3;
            }
            else if(HomeMode.Avatar.WinStreak == 5)
            {
                trophiesResult += 4;
                underdogTrophiesResult += 4;
            }
            else if(HomeMode.Avatar.WinStreak >= 6)
            {
                trophiesResult += 5;
                underdogTrophiesResult += 5;
            }
        }*/                      
                                
                if (HomeMode.Avatar.PremiumLevel > 0)
                {
                    switch (HomeMode.Avatar.PremiumLevel)
                    {
                        case 1:
                            if (location.GameMode == "BountyHunter" || location.GameMode == "CoinRush" || location.GameMode == "AttackDefend" || location.GameMode == "LaserBall" || location.GameMode == "RoboWars" || location.GameMode == "KingOfHill")
                            {
                                //underdogTrophiesResult += (int)Math.Floor((double)Trophies[0] / 2);
                                trophiesResult += (int)Math.Floor((double)Trophies[0] / 2);
                            }
                            else if (location.GameMode == "BattleRoyale" || location.GameMode == "BattleRoyaleTeam")
                            {
                                //underdogTrophiesResult += (int)Math.Floor((double)Math.Abs(Trophies[message.Rank - 1]) / 2);
                                trophiesResult += (int)Math.Floor((double)Math.Abs(Trophies[message.Rank - 1]) / 2);
                            }
                            break;
                        case 3:
                            if (location.GameMode == "BountyHunter" || location.GameMode == "CoinRush" || location.GameMode == "AttackDefend" || location.GameMode == "LaserBall" || location.GameMode == "RoboWars" || location.GameMode == "KingOfHill")
                            {
                                //underdogTrophiesResult += (int)(Math.Floor((double)Trophies[0] * 2 * 1.5));
                                trophiesResult += (int)(Math.Floor((double)Trophies[0] * 2 * 1.5));
                            }
                            else if (location.GameMode == "BattleRoyale" || location.GameMode == "BattleRoyaleTeam")
                            {
                                //underdogTrophiesResult += (int)(Math.Floor((double)Math.Abs(Trophies[message.Rank - 1]) * 2 * 1.5));
                                trophiesResult += (int)(Math.Floor((double)Math.Abs(Trophies[message.Rank - 1]) * 2 * 1.5));
                            }
                            break;
                        case 2:
                            if (location.GameMode == "BountyHunter" || location.GameMode == "CoinRush" || location.GameMode == "AttackDefend" || location.GameMode == "LaserBall" || location.GameMode == "RoboWars" || location.GameMode == "KingOfHill")
                            {
                                //underdogTrophiesResult += (int)(Math.Floor((double)Trophies[0] / 2) + (Math.Round((double)Trophies[0] / 4)));
                                trophiesResult += (int)(Math.Floor((double)Trophies[0] / 2) + (Math.Round((double)Trophies[0] / 4)));
                            }
                            else if (location.GameMode == "BattleRoyale" || location.GameMode == "BattleRoyaleTeam")
                            {
                                //underdogTrophiesResult += (int)(Math.Floor((double)Math.Abs(Trophies[message.Rank - 1]) / 2) + (Math.Round((double)Math.Abs(Trophies[message.Rank - 1]) / 4)));
                                trophiesResult += (int)(Math.Floor((double)Math.Abs(Trophies[message.Rank - 1]) / 2) + (Math.Round((double)Math.Abs(Trophies[message.Rank - 1]) / 4)));
                            }
                            break;
                    }
                    HomeMode.Home.TrophiesReward = Math.Max(trophiesResult, 0);
                }
               

                if (HomeMode.Home.BattleTokens > 0)
                {
                    if (HomeMode.Home.BattleTokens - tokensResult < 0)
                    {
                        tokensResult = HomeMode.Home.BattleTokens;
                        HomeMode.Home.BattleTokens = 0;
                    }
                    else
                    {
                        HomeMode.Home.BattleTokens -= tokensResult;
                    }
                    totalTokensResult += tokensResult;

                    if (HomeMode.Home.BattleTokensRefreshStart == new DateTime())
                    {
                        HomeMode.Home.BattleTokensRefreshStart = DateTime.UtcNow;
                    }
                }
                else
                {
                    tokensResult = 0;
                    totalTokensResult = 0;
                    HasNoTokens = true;
                }

                int startExperience = HomeMode.Home.Experience;
                HomeMode.Home.Experience += experienceResult + starExperienceResult;
                int endExperience = HomeMode.Home.Experience;

                for (int i = 34; i < 500; i++)
                {
                    MilestoneData milestone = DataTables.Get(DataType.Milestone).GetDataByGlobalId<MilestoneData>(GlobalId.CreateGlobalId(39, i));
                    int milestoneThreshold = milestone.ProgressStart + milestone.Progress;

                    if (startExperience < milestoneThreshold && endExperience >= milestoneThreshold)
                    {
                        MilestoneReward = GlobalId.CreateGlobalId(39, i);
                        MilestoneRewards.Add(MilestoneReward);
                        HomeMode.Avatar.StarPoints += milestone.SecondaryLvlUpRewardCount;
                        HomeMode.Home.StarPointsGained += milestone.SecondaryLvlUpRewardCount;
                        totalTokensResult += milestone.PrimaryLvlUpRewardCount;
                        break;
                    }
                }

                int startTrophies = hero.HighestTrophies;
                HomeMode.Avatar.AddTrophies(trophiesResult);
                hero.AddTrophies(trophiesResult);
                int endTrophies = hero.HighestTrophies;

                for (int i = 0; i < 34; i++)
                {
                    MilestoneData milestone = DataTables.Get(DataType.Milestone).GetDataByGlobalId<MilestoneData>(GlobalId.CreateGlobalId(39, i));
                    int milestoneThreshold = milestone.ProgressStart + milestone.Progress;

                    if (startTrophies < milestoneThreshold && endTrophies >= milestoneThreshold)
                    {
                        MilestoneReward = GlobalId.CreateGlobalId(39, i);
                        MilestoneRewards.Add(MilestoneReward);
                        HomeMode.Avatar.StarPoints += milestone.SecondaryLvlUpRewardCount;
                        HomeMode.Home.StarPointsGained += milestone.SecondaryLvlUpRewardCount;
                        totalTokensResult += milestone.PrimaryLvlUpRewardCount;
                        break;
                    }
                }
                
                string eventsFilePath = "/root/PabloBrawl/prod31/Supercell.Laser.Server/JSON/events.json";
                string eventsJson = File.ReadAllText(eventsFilePath);
                JObject eventsData = JObject.Parse(eventsJson);

                int tokensEvent = eventsData["TokensEvent"]?.ToObject<int>() ?? 0;
                int coinsEvent = eventsData["CoinsEvent"]?.ToObject<int>() ?? 0;
                
                HomeMode.Home.BrawlPassTokens += totalTokensResult;
                HomeMode.Home.TokenReward += totalTokensResult;
                                
                if (location.GameMode == "BountyHunter" || location.GameMode == "CoinRush" || location.GameMode == "AttackDefend" ||
                    location.GameMode == "LaserBall" || location.GameMode == "RoboWars")
                {
                    gameMode = 1;
                }
                else if (location.GameMode == "BattleRoyale")
                {
                    gameMode = 2;
                }
                else if (location.GameMode == "BattleRoyaleTeam")
                {
                    gameMode = 5;
                }
                else if (location.GameMode == "BossFight")
                {
                    gameMode = 4;
                }

                // battle log
                string[] battleResults = { "Выиграл", "Проиграл", "Ничья" };
                string logMessage = location.GameMode.StartsWith("BattleRoyale")
                    ? $"Игрок {LogicLongCodeGenerator.ToCode(HomeMode.Avatar.AccountId)} завершил матч! Место: {message.BattleResult} за {DateTime.UtcNow.Subtract(HomeMode.Avatar.BattleStartTime).TotalSeconds}с. Режим: {location.GameMode}!"
                    : $"Игрок {LogicLongCodeGenerator.ToCode(HomeMode.Avatar.AccountId)} завершил матч! Итог: {battleResults[message.BattleResult]} за {DateTime.UtcNow.Subtract(HomeMode.Avatar.BattleStartTime).TotalSeconds}с. Режим: {location.GameMode}!";
                    
                if(location.GameMode.StartsWith("BattleRoyaleTeam") && !location.GameMode.Equals("BattleRoyale") && message.Rank < 2 && DateTime.UtcNow.Subtract(HomeMode.Avatar.BattleStartTime).TotalSeconds < 25)
            {
                long accid = HomeMode.Avatar.AccountId;
                Account account = Accounts.Load(accid);
                account.Avatar.Banned = true;
                DateTime banEndTime = DateTime.UtcNow.AddDays(30);
                TimeSpan remainingTime = banEndTime - DateTime.UtcNow;
                int remainingDays = (int)Math.Ceiling(remainingTime.TotalDays);
                account.Home.BanReason = $"Использование читов\nВаш тэг: {LogicLongCodeGenerator.ToCode(HomeMode.Avatar.AccountId)}";
                account.Home.BanEndTime = banEndTime;
                SendAuthenticationFailed(11, $"Твой аккаунт заблокирован на {remainingDays} дн. (до {banEndTime:yyyy-MM-dd HH:mm:ss} UTC).\nПричина: Использование читов\nЕсли не согласны с наказанием, обратитесь в поддержку: @pbs_manager.");
                string acbrteamlogMessage = $"Игрок {LogicLongCodeGenerator.ToCode(HomeMode.Avatar.AccountId)} был заблокирован античитом. Место: {message.BattleResult}. Режим: {location.GameMode}. Время игры: {DateTime.UtcNow.Subtract(HomeMode.Avatar.BattleStartTime).TotalSeconds}с.";
                Logger.ACLog(acbrteamlogMessage);
                return;
            }
            
                if(location.GameMode.StartsWith("BattleRoyale") && !location.GameMode.StartsWith("BattleRoyaleTeam") && message.Rank < 5  && DateTime.UtcNow.Subtract(HomeMode.Avatar.BattleStartTime).TotalSeconds < 25) 
            {
                long accid = HomeMode.Avatar.AccountId;
                Account account = Accounts.Load(accid);                
                account.Avatar.Banned = true;
                DateTime banEndTime = DateTime.UtcNow.AddDays(30);
                TimeSpan remainingTime = banEndTime - DateTime.UtcNow;
                int remainingDays = (int)Math.Ceiling(remainingTime.TotalDays);
                account.Home.BanReason = "Использование читов";
                account.Home.BanEndTime = banEndTime;
                SendAuthenticationFailed(11, $"Твой аккаунт заблокирован на {remainingDays} дн. (до {banEndTime:yyyy-MM-dd HH:mm:ss} UTC).\nПричина: Использование читов\nЕсли не согласны с наказанием, обратитесь в поддержку: @pbs_manager.");
                string acbrlogMessage = $"Игрок {LogicLongCodeGenerator.ToCode(HomeMode.Avatar.AccountId)} был заблокирован античитом. Место: {message.BattleResult}. Режим: {location.GameMode}. Время игры: {DateTime.UtcNow.Subtract(HomeMode.Avatar.BattleStartTime).TotalSeconds}с.";
                Logger.ACLog(acbrlogMessage);
                return;
            }
                            
                if(!location.GameMode.StartsWith("BattleRoyale") && !location.GameMode.StartsWith("BattleRoyaleTeam") && !location.GameMode.StartsWith("LaserBall") && DateTime.UtcNow.Subtract(HomeMode.Avatar.BattleStartTime).TotalSeconds < 25) 
            {
                long accid = HomeMode.Avatar.AccountId;
                Account account = Accounts.Load(accid);                
                account.Avatar.Banned = true;
                DateTime banEndTime = DateTime.UtcNow.AddDays(30);
                TimeSpan remainingTime = banEndTime - DateTime.UtcNow;
                int remainingDays = (int)Math.Ceiling(remainingTime.TotalDays);
                account.Home.BanReason = "Использование читов";
                account.Home.BanEndTime = banEndTime;
                SendAuthenticationFailed(11, $"Твой аккаунт заблокирован на {remainingDays} дн. (до {banEndTime:yyyy-MM-dd HH:mm:ss} UTC).\nПричина: Использование читов\nЕсли не согласны с наказанием, обратитесь в поддержку: @pbs_manager.");
                string acteamlogMessage = $"Игрок {LogicLongCodeGenerator.ToCode(HomeMode.Avatar.AccountId)} был заблокирован античитом. Режим: {location.GameMode}. Время игры: {DateTime.UtcNow.Subtract(HomeMode.Avatar.BattleStartTime).TotalSeconds}с.";
                Logger.ACLog(acteamlogMessage);
                return;
            }
                if(location.GameMode.StartsWith("LaserBall") && DateTime.UtcNow.Subtract(HomeMode.Avatar.BattleStartTime).TotalSeconds < 14) 
            {
                long accid = HomeMode.Avatar.AccountId;
                Account account = Accounts.Load(accid);                
                account.Avatar.Banned = true;
                DateTime banEndTime = DateTime.UtcNow.AddDays(30);
                TimeSpan remainingTime = banEndTime - DateTime.UtcNow;
                int remainingDays = (int)Math.Ceiling(remainingTime.TotalDays);
                account.Home.BanReason = "Использование читов";
                account.Home.BanEndTime = banEndTime;
                SendAuthenticationFailed(11, $"Твой аккаунт заблокирован на {remainingDays} дн. (до {banEndTime:yyyy-MM-dd HH:mm:ss} UTC).\nПричина: Использование читов\nЕсли не согласны с наказанием, обратитесь в поддержку: @pbs_manager.");
                string acballlogMessage = $"Игрок {LogicLongCodeGenerator.ToCode(HomeMode.Avatar.AccountId)} был заблокирован античитом. Режим: {location.GameMode}. Время игры: {DateTime.UtcNow.Subtract(HomeMode.Avatar.BattleStartTime).TotalSeconds}с.";
                Logger.ACLog(acballlogMessage);
                return;
            }

                Logger.BLog(logMessage);
                HomeMode.Avatar.BattleStartTime = new DateTime();

                BattleEndMessage battleend = new()
                {
                    GameMode = gameMode,
                    Result = message.BattleResult,
                    StarToken = starToken,
                    IsPowerPlay = isPowerPlay,
                    IsPvP = isPvP,
                    pp = message.BattlePlayers,
                    OwnPlayer = OwnPlayer,
                    TrophiesReward = trophiesResult,
                    ExperienceReward = experienceResult,
                    StarExperienceReward = starExperienceResult,
                    DoubledTokensReward = doubledTokensResult,
                    TokenDoublersLeft = HomeMode.Home.TokenDoublers,
                    TokensReward = tokensResult,
                    Experience = StartExperience,
                    MilestoneReward = MilestoneReward,
                    ProgressiveQuests = q,
                    UnderdogTrophies = underdogTrophiesResult,
                    PowerPlayScoreGained = powerPlayScoreGained,
                    PowerPlayEpicScoreGained = powerPlayEpicScoreGained,
                    HasNoTokens = HasNoTokens,
                    MilestoneRewards = MilestoneRewards,
                };
                if (tokensEvent == 1)
                {
                    battleend.DoubleTokenEvent += totalTokensResult;
                    HomeMode.Home.BrawlPassTokens += totalTokensResult;
                    HomeMode.Home.TokenReward += totalTokensResult;
                }
                if (coinsEvent == 1)
                {
                    battleend.CoinEventReward = totalTokensResult;
                    HomeMode.Home.CoinsGained += totalTokensResult;
                    HomeMode.Avatar.Gold += totalTokensResult;
                }

                Connection.Send(battleend);
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AskForBattleEndReceived: Error! {ex.Message}.");
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void GetLeaderboardReceived(GetLeaderboardMessage message)
        {
            LeaderboardMessage leaderboard = new()
            {
                LeaderboardType = message.LeaderboardType,
                Region = message.IsRegional ? HomeMode.Avatar.Region : null                
            };
            Console.WriteLine($"LeaderboardType: {message.LeaderboardType}.");

            switch (message.LeaderboardType)
            {
                case 1: // main leaderboard
                    ProcessAvatarLeaderboard(leaderboard, Leaderboards.GetAvatarRankingList());
                    break;

                case 2: // club leaderboard
                    ProcessAllianceLeaderboard(leaderboard, Leaderboards.GetAllianceRankingList());
                    break;

                case 0: // brawl leaderboard
                    ProcessBrawlersLeaderboard(leaderboard, Leaderboards.GetBrawlersRankingList(), message.CharachterId);
                    break;
                    
                case 3: // powerplay leaderboard
                    ProcessPowerPlayLeaderboard(leaderboard, Leaderboards.GetPowerPlayRankingList());
                    break;
            }

            leaderboard.OwnAvatarId = Connection.Avatar.AccountId;
            Connection.Send(leaderboard);
        }

        private void ProcessAvatarLeaderboard(LeaderboardMessage leaderboard, Account[] rankingList)
        {
            foreach (Account data in rankingList)
            {
                leaderboard.Avatars.Add(new KeyValuePair<ClientHome, ClientAvatar>(data.Home, data.Avatar));
            }
        }
        
        private void ProcessPowerPlayLeaderboard(LeaderboardMessage leaderboard, Account[] rankingList)
        {
            foreach (Account data in rankingList)
            {
                leaderboard.Avatars.Add(new KeyValuePair<ClientHome, ClientAvatar>(data.Home, data.Avatar));
            }
        }

        private void ProcessAllianceLeaderboard(LeaderboardMessage leaderboard, Alliance[] rankingList)
        {
            leaderboard.AllianceList.AddRange(rankingList);
        }

        private void ProcessBrawlersLeaderboard(LeaderboardMessage leaderboard, Dictionary<int, List<Account>> rankingList, int characterId)
        {
            var brawlersData = rankingList
                .Where(data => data.Key == characterId)
                .SelectMany(data => data.Value)
                .ToDictionary(account => account.Home, account => account.Avatar);

            var sortedBrawlers = brawlersData
                .OrderByDescending(x => x.Value.GetHero(characterId).Trophies)
                .Take(Math.Min(brawlersData.Count, 200))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            leaderboard.CharachterId = characterId;
            leaderboard.Brawlers = sortedBrawlers;
        }
        /*private void SetCountryReceived(SetCountryMessage message)
        {
            SetCountryResponseMessage response = new SetCountryResponseMessage();
            response.CountryID = message.CountryID;
            HomeMode.Avatar.Region = DataTables.Get(14).GetDataWithId<RegionData>(message.CountryID).Name;
            Console.WriteLine(HomeMode.Avatar.Region);
            Connection.Send(response);
        }*/
        private void GoHomeReceived(GoHomeMessage message)
        {
            if (Connection.Home != null && Connection.Avatar != null)
            {
                Connection.Home.Events = Events.GetEventsById(HomeMode.Home.PowerPlayGamesPlayed, Connection.Avatar.AccountId);
                OwnHomeDataMessage ohd = new OwnHomeDataMessage();
                ohd.Home = Connection.Home;
                ohd.Avatar = Connection.Avatar;
                Connection.Send(ohd);
                ShowLobbyInfo();
            }
        }

        private void ClientInfoReceived(ClientInfoMessage message)
        {
            UdpConnectionInfoMessage info = new UdpConnectionInfoMessage();
            info.SessionId = Connection.UdpSessionId;
            info.ServerPort = Configuration.Instance.Port;
            Connection.Send(info);
        }

        private void CancelMatchMaking(CancelMatchmakingMessage message)
        {
            Matchmaking.CancelMatchmake(Connection);
            Connection.Send(new MatchMakingCancelledMessage());
        }

        private void MatchmakeRequestReceived(MatchmakeRequestMessage message)
        {
            int slot = message.EventSlot;

            if (HomeMode.Home.Character.Disabled)
            {
                Connection.Send(new OutOfSyncMessage());
                return;
            }

            if (!Events.HasSlot(slot))
            {
                slot = 1;
            }

            Matchmaking.RequestMatchmake(Connection, slot);
        }

        private void EndClientTurnReceived(EndClientTurnMessage message)
        {
            foreach (Command command in message.Commands)
            {
                if (!CommandManager.ReceiveCommand(command))
                {
                    OutOfSyncMessage outOfSync = new();
                    Connection.Send(outOfSync);
                }
            }
            HomeMode.ClientTurnReceived(message.Tick, message.Checksum, message.Commands);
        }

        private void GetPlayerProfile(GetPlayerProfileMessage message)
        {
            if (message.AccountId == 0)
            {
                Profile p = Profile.CreateConsole();
                PlayerProfileMessage a = new PlayerProfileMessage();
                a.Profile = p;

                Connection.Send(a);
                return;
            }
            Account data = Accounts.Load(message.AccountId);
            if (data == null) return;

            Profile profile = Profile.Create(data.Home, data.Avatar);

            PlayerProfileMessage profileMessage = new PlayerProfileMessage();
            profileMessage.Profile = profile;
            if (data.Avatar.AllianceId >= 0)
            {
                Alliance alliance = Alliances.Load(data.Avatar.AllianceId);
                if (alliance != null)
                {
                    profileMessage.AllianceHeader = alliance.Header;
                    profileMessage.AllianceRole = data.Avatar.AllianceRole;
                }
            }
            Connection.Send(profileMessage);
        }
        
        private void BattleLogMessageReceived(BattleLogMessage message)
        {
            Connection.Send(message);
        }

        private void ChangeName(ChangeAvatarNameMessage message) // установка ника в начале
        {
            TimeZoneInfo moscowZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");
            DateTime nowMoscow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, moscowZone);
            if (message.Name.Length < 3)
            {
                AvatarNameChangeFailedMessage lowLength = new AvatarNameChangeFailedMessage();
                lowLength.SetReason(message.Name.Length < 3 ? 2 : message.Name.Length >= 15 ? 1 : 0);
                Connection.Send(lowLength);
                Console.WriteLine($"Account {HomeMode.Avatar.AccountId} крч поставил ник с малым кол-во букв!");
                return;
            }
            if (message.Name.Length >= 15)
            {
                AvatarNameChangeFailedMessage maxLength = new AvatarNameChangeFailedMessage();
                maxLength.SetReason(message.Name.Length < 3 ? 2 : message.Name.Length >= 15 ? 1 : 0);
                Connection.Send(maxLength);
                Console.WriteLine($"Account {HomeMode.Avatar.AccountId} exceeded length limit!");
                return;
            }
            if (ContainsForbiddenWords(message.Name))
            {
                AvatarNameChangeFailedMessage forbiddenName = new AvatarNameChangeFailedMessage();
                forbiddenName.SetReason(message.Name.Length < 3 ? 2 : message.Name.Length >= 15 ? 1 : 0);
                Connection.Send(forbiddenName);
                Console.WriteLine($"Account {HomeMode.Avatar.AccountId} try to set name with forbidden word!");
                return;
            }            
                        
            if (HomeMode.Avatar.AllianceId >= 0)
            {
                Alliance alliance = Alliances.Load(HomeMode.Avatar.AllianceId);
                if (alliance != null)
                {
                    AllianceMember member = alliance.GetMemberById(HomeMode.Avatar.AccountId);
                    if (member != null)
                    {
                        member.DisplayData.Name = message.Name;
                    }
                }
            }
            if (HomeMode.Home.CooldownChangeName > DateTime.Now)
            {
                HomeMode.Home.CooldownChangeName = DateTime.MinValue;
            }

            var command = new LogicChangeAvatarNameCommand
            {
                Name = message.Name
            };
            
            Console.WriteLine($"Account {HomeMode.Avatar.AccountId} set name {message.Name}!");
            command.Execute(HomeMode);
            Connection.Send(new AvailableServerCommandMessage { Command = command });
        }

        private void OnChangeCharacter(int characterId)
        {
            TeamEntry team = Teams.Get(HomeMode.Avatar.TeamId);
            if (team == null) return;

            TeamMember member = team.GetMember(HomeMode.Avatar.AccountId);
            if (member == null) return;

            Hero hero = HomeMode.Avatar.GetHero(characterId);
            if (hero == null) return;
            member.CharacterId = characterId;
            member.SkinId = GlobalId.CreateGlobalId(29, HomeMode.Home.SelectedSkins[GlobalId.GetInstanceId(HomeMode.Home.CharacterId)]);
            member.HeroTrophies = hero.Trophies;
            member.HeroHighestTrophies = hero.HighestTrophies;
            member.HeroLevel = hero.PowerLevel;

            team.TeamUpdated();
        }        

        private void LoginReceived(AuthenticationMessage message)
        {
            Account account = GetAccount(message);   

            string bannedDevicesPath = "banned_devices.txt";

            if (File.Exists(bannedDevicesPath))
            {
                string[] bannedDeviceIds = File.ReadAllLines(bannedDevicesPath);

                if (bannedDeviceIds.Contains(message.AndroidId))
                {
                    AuthenticationFailedMessage loginFailedDevice = new AuthenticationFailedMessage
                    {
                        ErrorCode = 11,
                        Message = "Твоё устройство заблокировано.\nЕсли считаешь, что это ошибка — напиши в поддержку: @pbs_manager."
                    };
                    Connection.Send(loginFailedDevice);
                    return;
                }
            }         

            string techFilePath = "/root/PabloBrawl/prod31/Supercell.Laser.Server/JSON/serverinfo.json";
            string techJson = File.ReadAllText(techFilePath);
            JObject techData = JObject.Parse(techJson);

            bool maintenance = techData["Maintenance"]?.ToObject<bool>() ?? false;
            string maintenanceTimeString = techData["MaintenanceTime"]?.ToString();

            int maintenanceTimeSeconds = 0;

            if (!string.IsNullOrEmpty(maintenanceTimeString))
            {
                try
                {
                    DateTime maintenanceTimeMoscow = DateTime.Parse(maintenanceTimeString);
                    TimeSpan mskOffset = new TimeSpan(3, 0, 0);
                    DateTimeOffset moscowTime = new DateTimeOffset(maintenanceTimeMoscow, mskOffset);
                    DateTimeOffset utcMaintenanceTime = moscowTime.ToUniversalTime();
                    DateTime nowUtc = DateTime.UtcNow;
                    TimeSpan difference = utcMaintenanceTime.DateTime - nowUtc;
                    
                    if (difference.TotalSeconds > 0)
                    {
                        maintenanceTimeSeconds = (int)difference.TotalSeconds;
                    }
                    else
                    {
                        maintenanceTimeSeconds = 0;
                    }
                    //Console.WriteLine($"[DEBUG] Осталось секунд до окончания техработ: {maintenanceTimeSeconds}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Не удалось обработать дату техработ: {ex.Message}");
                }
            }
            if (Sessions.Maintenance || maintenance && message.AccountId != 1)
            {
                Connection.Send(new AuthenticationFailedMessage()
                {
                    ErrorCode = 10,
                    MaintenanceTime = maintenanceTimeSeconds
                });                
                return;
            }

            if (account == null)
            {
                SendAuthenticationFailed(1, "Неизвестная ошибка при загрузке аккаунта.");
                return;
            }

            if (message.AccountId == 0)
            {
                account = Accounts.Create();
            }
            
            else
            {
                account = Accounts.Load(message.AccountId);
                if (account.PassToken != message.PassToken)
                {
                    account = null;
                }
            }
            
            //Console.WriteLine($"[DEBUG] Новое подключение от {LogicLongCodeGenerator.ToCode(HomeMode.Avatar.AccountId)}.");

            if (account == null)
            {

                AuthenticationFailedMessage loginFailed = new AuthenticationFailedMessage();
                loginFailed.ErrorCode = 1;
                loginFailed.Message = "Не найден аккаунт на устройстве! Очисти данные игры.";
                Connection.Send(loginFailed);

                return;
            }            

            string versionFilePath = "/root/PabloBrawl/prod31/Supercell.Laser.Server/JSON/serverinfo.json";
            string versionJson = File.ReadAllText(versionFilePath);
            JObject versionData = JObject.Parse(versionJson);

            string clientVersion = versionData["ClientVersion"]?.ToString();
            string rustoreVersion = versionData["RustoreVersion"]?.ToString();
            string clientUpdateUrl = versionData["ClientUpdateUrl"]?.ToString();
            string rustoreUpdateUrl = versionData["RustoreUpdateUrl"]?.ToString();

            bool isRustore = message.ClientVersion.EndsWith("_rustore");
            string strippedClientVersion = isRustore ? message.ClientVersion.Replace("_rustore", "") : message.ClientVersion;

            bool isVersionValid = isRustore 
    ? strippedClientVersion == rustoreVersion 
    : strippedClientVersion == clientVersion;

            if (!isVersionValid)
            {
                AuthenticationFailedMessage loginFailed = new AuthenticationFailedMessage();
                loginFailed.ErrorCode = 8;

                loginFailed.Message = "Твоя версия игры устарела. Установи новую версию игры и продолжай играть!";
                loginFailed.UpdateUrl = isRustore ? rustoreUpdateUrl : clientUpdateUrl;

                Connection.Send(loginFailed);
                return;
            }
            
            if (account.Avatar.Banned)
            {
                if (DateTime.Now > account.Home.BanEndTime)
                {
                    long accountId = HomeMode.Avatar.AccountId;
                    Account mutedacc = Accounts.Load(accountId);
                    account.Avatar.Banned = false;
                }
            
                if (account.Home.BanEndTime == DateTime.MaxValue)
                {
                    DateTime banEndTime = account.Home.BanEndTime;
                    TimeSpan remainingTime = banEndTime - DateTime.UtcNow;
                    int remainingDays = (int)Math.Ceiling(remainingTime.TotalDays);

                    string reason = account.Home.BanReason;
                    
                    if (string.IsNullOrWhiteSpace(reason))
                    {
                        reason = "Не указана";
                    }

                    SendAuthenticationFailed(13, $"Твой аккаунт заблокирован НАВСЕГДА!\nПричина: {reason}\nЕсли не согласны с наказанием, обратитесь в поддержку: @pbs_manager.");
                    return;
                }
                else
                {        
                    DateTime banEndTime = account.Home.BanEndTime;
                    TimeSpan remainingTime = banEndTime - DateTime.UtcNow;
                    int remainingDays = (int)Math.Ceiling(remainingTime.TotalDays);

                    string reason = account.Home.BanReason;
                    if (string.IsNullOrWhiteSpace(reason))
                    {
                        reason = "Не указана";
                    }

                    SendAuthenticationFailed(11, $"Твой аккаунт заблокирован на {remainingDays} дн. (до {banEndTime:yyyy-MM-dd HH:mm:ss} UTC).\nПричина: {reason}\nЕсли не согласны с наказанием, обратитесь в поддержку: @pbs_manager.");
                    return;
                }
            }
            
            if (account.Avatar.IsCommunityBanned)
            {
                if (DateTime.UtcNow >= account.Home.MuteEndTime)
                {
                    long accountId = HomeMode.Avatar.AccountId;
                    Account mutedacc = Accounts.Load(accountId);
                    account.Avatar.IsCommunityBanned = false;                    
                    account.Home.MuteEndTime = DateTime.MinValue;
                    account.Home.MuteReason = null;
                    Notification nmute = new()
                    {
                        Id = 81,
                        MessageEntry = "Срок мута закончился, теперь ты снова можешь общаться!"
                    };
                    mutedacc.Home.NotificationFactory.Add(nmute);
                    LogicAddNotificationCommand muteacm = new() { Notification = nmute };
                    AvailableServerCommandMessage muteasm = new() { Command = muteacm };
                    if (Sessions.IsSessionActive(accountId))
                    {
                        Session session = Sessions.GetSession(accountId);
                        session.GameListener.SendTCPMessage(muteasm);
                    }
                }
            }
            
        if (message.ClientVersion.Contains("_rustore") && !account.Avatar.RustoreRewardClaimed && account.Avatar.Trophies > 40)  
            {
            long lowID = account.Avatar.AccountId;
            Account loadAcc = Accounts.Load(lowID);
            account.Avatar.RustoreRewardClaimed = true;
            Notification nstore = new()
                        {
                            Id = 89,
                            DonationCount = 30,
                            MessageEntry = "<c6>Награда за установку с RuStore!</c>"
                        };
                        loadAcc.Home.NotificationFactory.Add(nstore);
                        LogicAddNotificationCommand acm = new() { Notification = nstore };

                        AvailableServerCommandMessage asm = new() { Command = acm };
                        if (Sessions.IsSessionActive(lowID))
                    {
                        Session session = Sessions.GetSession(lowID);
                        session.GameListener.SendTCPMessage(asm);
                    }
            }
            
            string[] androidVersionParts = message.Android.Split('.'); // check if android version is between 0 and 100 (if not then it's not an android device and login will be prevented)
            if (androidVersionParts.Length == 0 || !int.TryParse(androidVersionParts[0], out int androidMajorVersion) || androidMajorVersion < 0 || androidMajorVersion > 100)
            {
                AuthenticationFailedMessage loginFailed = new AuthenticationFailedMessage
                {
                    ErrorCode = 8,
                    Message = "Обнаружена невалидная версия андроида, сообщите администратору если считаете что это ошибка."
                };
                Connection.Send(loginFailed);
                return;
            }                        

            if (IsSessionActive((int)account.Avatar.AccountIdRedirect))
            {
                HandleActiveSession((int)account.Avatar.AccountIdRedirect);
            }

            if (IsSessionActive((int)message.AccountId))
            {
                HandleActiveSession((int)message.AccountId);
            }

            if (account.Avatar.AccountIdRedirect != 0)
            {
                account = Accounts.Load((int)account.Avatar.AccountIdRedirect);
            }                                    

            SendAuthenticationSuccess(account, message);

            InitializeHomeMode(account, message);

            HandleBattleState(account);

            UpdateLastOnline(account);

            InitializeSession(account);

            SendFriendAndAllianceData(account);
            
            CheckPremium(account);
        }

        private Account GetAccount(AuthenticationMessage message)
        {
            if (message.AccountId == 0)
            {
                return Accounts.Create();
            }

            Account account = Accounts.Load((int)message.AccountId);
            return account.PassToken == message.PassToken ? account : null;
        }

        private void CheckPremium(Account account)
        {            
            if (DateTime.UtcNow > account.Home.PremiumEndTime && account.Avatar.PremiumLevel > 1)
            {                
                account.Avatar.PremiumLevel = 0;
                account.Home.CustomLobbyInfo = null;
            }
        }
            

        private bool IsSessionActive(int accountId)
        {
            return Sessions.IsSessionActive(accountId);
        }

        private void HandleActiveSession(int accountId)
        {
            var session = Sessions.GetSession(accountId);
            session.GameListener.SendTCPMessage(new AuthenticationFailedMessage
            {
                Message = "Другое устройство подключилось к этой игре!"
            });
            Sessions.Remove(accountId);
        }


        private void SendAuthenticationFailed(int errorCode, string message)
        {
            Connection.Send(new AuthenticationFailedMessage
            {
                ErrorCode = errorCode,
                Message = message
            });
        }

        private void SendAuthenticationSuccess(Account account, AuthenticationMessage message)
        {
            var loginOk = new AuthenticationOkMessage
            {
                AccountId = account.AccountId,
                PassToken = account.PassToken,
                Major = message.Major,
                Minor = message.Minor,
                Build = message.Build,
                ServerEnvironment = "prod"
            };
            Connection.Send(loginOk);
        }

        private void InitializeHomeMode(Account account, AuthenticationMessage message)
        {
            HomeMode = HomeMode.LoadHomeState(new HomeGameListener(Connection), account.Home, account.Avatar, Events.GetEventsById(account.Home.PowerPlayGamesPlayed, account.Avatar.AccountId));
            HomeMode.CharacterChanged += OnChangeCharacter;
            if (HomeMode.Home.HasIP == false)
            {
                HomeMode.Home.HasIP = true;
                HomeMode.Home.FirstIpAddress = Connection.Socket.RemoteEndPoint.ToString().Split(" ")[0];
            }
            HomeMode.Home.IpAddress = Connection.Socket.RemoteEndPoint.ToString().Split(" ")[0];
            HomeMode.Home.Device = message.Device;
            HomeMode.Home.AndroidId = message.AndroidId;

            if (HomeMode.Avatar.HighestTrophies == 0 && HomeMode.Avatar.Trophies != 0)
            {
                HomeMode.Avatar.HighestTrophies = HomeMode.Avatar.Trophies;
            }

            CommandManager = new(HomeMode, Connection);
        }

        private void HandleBattleState(Account account)
        {
            if (HomeMode.Avatar.BattleStartTime != new DateTime())
            {
                Hero hero = HomeMode.Avatar.GetHero(HomeMode.Home.CharacterId);
                int trophiesLost = CalculateTrophiesLost(hero.Trophies);
                hero.AddTrophies(trophiesLost);
                HomeMode.Home.PowerPlayGamesPlayed = Math.Max(0, HomeMode.Home.PowerPlayGamesPlayed - 1);
                Connection.Home.Events = Events.GetEventsById(HomeMode.Home.PowerPlayGamesPlayed, Connection.Avatar.AccountId);
                HomeMode.Avatar.BattleStartTime = new DateTime();
            }

            BattleMode battle = HomeMode.Avatar.BattleId > 0 ? Battles.Get((int)HomeMode.Avatar.BattleId) : null;

            if (battle == null)
            {
                Connection.Send(new OwnHomeDataMessage { Home = HomeMode.Home, Avatar = HomeMode.Avatar });
            }
            else
            {
                StartLoadingMessage startLoading = new StartLoadingMessage
                {
                    LocationId = battle.Location.GetGlobalId(),
                    TeamIndex = HomeMode.Avatar.TeamIndex,
                    OwnIndex = HomeMode.Avatar.OwnIndex,
                    GameMode = battle.GetGameModeVariation() == 6 ? 6 : 1,
                    Players = new List<Supercell.Laser.Logic.Battle.Structures.BattlePlayer>(battle.GetPlayers())
                };

                UDPSocket socket = UDPGateway.CreateSocket();
                socket.TCPConnection = Connection;
                socket.Battle = battle;
                Connection.UdpSessionId = socket.SessionId;
                battle.ChangePlayerSessionId(HomeMode.Avatar.UdpSessionId, socket.SessionId);
                HomeMode.Avatar.UdpSessionId = socket.SessionId;
                Connection.Send(startLoading);
            }
        }

        private int CalculateTrophiesLost(int brawlerTrophies)
        {
            if (brawlerTrophies <= 49) return 0;
            if (brawlerTrophies <= 99) return -1;
            if (brawlerTrophies <= 199) return -2;
            if (brawlerTrophies <= 299) return -3;
            if (brawlerTrophies <= 399) return -4;
            if (brawlerTrophies <= 499) return -5;
            if (brawlerTrophies <= 599) return -6;
            if (brawlerTrophies <= 699) return -7;
            if (brawlerTrophies <= 799) return -8;
            if (brawlerTrophies <= 899) return -9;
            if (brawlerTrophies <= 999) return -10;
            if (brawlerTrophies <= 1099) return -11;
            return -12;
        }

        private void UpdateLastOnline(Account account)
        {
            Connection.Avatar.LastOnline = DateTime.UtcNow;
        }

        private void InitializeSession(Account account)
        {
            Sessions.Create(HomeMode, Connection);
        }

        private void SendFriendAndAllianceData(Account account)
        {
            Connection.Send(new FriendListMessage { Friends = HomeMode.Avatar.Friends.ToArray() });

            if (HomeMode.Avatar.AllianceRole != AllianceRole.None && HomeMode.Avatar.AllianceId > 0)
            {
                Alliance alliance = Alliances.Load((int)HomeMode.Avatar.AllianceId);
                if (alliance != null)
                {
                    SendMyAllianceData(alliance);
                    Connection.Send(new AllianceDataMessage { Alliance = alliance, IsMyAlliance = true });
                }
            }

            foreach (Friend entry in HomeMode.Avatar.Friends.ToArray())
            {
                if (LogicServerListener.Instance.IsPlayerOnline(entry.AccountId))
                {
                    Connection.Send(new FriendOnlineStatusEntryMessage
                    {
                        AvatarId = entry.AccountId,
                        PlayerStatus = entry.Avatar.PlayerStatus
                    });
                }
            }

            if (HomeMode.Avatar.TeamId > 0)
            {
                TeamMessage teamMessage = new TeamMessage { Team = Teams.Get((int)HomeMode.Avatar.TeamId) };
                if (teamMessage.Team != null)
                {
                    Connection.Send(teamMessage);
                    TeamMember member = teamMessage.Team.GetMember(HomeMode.Avatar.AccountId);
                    member.State = 0;
                    teamMessage.Team.TeamUpdated();
                }
            }
        }

        private void ClientHelloReceived(ClientHelloMessage message)
        {
            if (message.KeyVersion != PepperKey.VERSION)
            {
                //return;
            }

            Connection.Messaging.Seed = message.ClientSeed;
            Random r = new();

            Connection.Messaging.serversc = (byte)r.Next(1, 256);
            ServerHelloMessage hello = new ServerHelloMessage();
            hello.serversc = Connection.Messaging.serversc;
            hello.SetServerHelloToken(Connection.Messaging.SessionToken);
            Connection.Send(hello);
        }
        private void DebugAllMessage(DebugAllMessage message)
        {
            long id = HomeMode.Avatar.AccountId;
            Account account = Accounts.Load(id);
            
            if (account.Avatar.AccountId != 1)
            {
                AuthenticationFailedMessage loginFailed = new AuthenticationFailedMessage();
                loginFailed.ErrorCode = 1;
                loginFailed.Message = "иди нахуй, нельзя тебе дебаг.";
                Connection.Send(loginFailed);
                return;
            }
            try
            {                
                List<int> allBrawlers =
                    new()
                    {                        
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
            }
            catch (Exception ex)
            {
                //return $"An error occurred while unlocking content: {ex.Message}";
            }
        }          
        private void DebugGemsMessage(DebugGemsMessage message)
        {
            long lowID = HomeMode.Avatar.AccountId;
            Account account = Accounts.Load(lowID);
            if (account.Home.DevEndTime > DateTime.UtcNow)
            {
                account.Home.GemsGained += 500;
                account.Avatar.Diamonds += 500;
            }
            else
            {
                AuthenticationFailedMessage loginFailed = new AuthenticationFailedMessage();
                loginFailed.ErrorCode = 1;
                loginFailed.Message = "иди нахуй, нельзя тебе дебаг.";
                Connection.Send(loginFailed);
                return;
            }
        }
        private void DebugGoldMessage(DebugGoldMessage message)
        {
            long lowID = HomeMode.Avatar.AccountId;
            Account account = Accounts.Load(lowID);
            
            if (account.Home.DevEndTime > DateTime.UtcNow)
            {
                account.Home.CoinsGained += 500;
                account.Avatar.Gold += 500;
            }
            else
            {
                AuthenticationFailedMessage loginFailed = new AuthenticationFailedMessage();
                loginFailed.ErrorCode = 1;
                loginFailed.Message = "иди нахуй, нельзя тебе дебаг.";
                Connection.Send(loginFailed);
                return;
            }
        }
        private void DebugSPMessage(DebugSPMessage message)
        {
            long lowID = HomeMode.Avatar.AccountId;
            Account account = Accounts.Load(lowID);
            
            if (account.Home.DevEndTime > DateTime.UtcNow)
            {
                account.Home.StarPointsGained += 500;
                account.Avatar.StarPoints += 500;
            }
            else
            {
                AuthenticationFailedMessage loginFailed = new AuthenticationFailedMessage();
                loginFailed.ErrorCode = 1;
                loginFailed.Message = "иди нахуй, нельзя тебе дебаг.";
                Connection.Send(loginFailed);
                return;
            }
        }
        private void DebugNTMessage(DebugNTMessage message)
        {
            long lowID = HomeMode.Avatar.AccountId;
            Account account = Accounts.Load(lowID);
            
            if (account.Home.DevEndTime > DateTime.UtcNow)
            {                            
                if (account.Home.Theme == 34)
                {
                    account.Home.Theme = 0;
                }
                else
                {
                    account.Home.Theme += 1;
                }
            }
            else
            {
                AuthenticationFailedMessage loginFailed = new AuthenticationFailedMessage();
                loginFailed.ErrorCode = 1;
                loginFailed.Message = "иди нахуй, нельзя тебе дебаг.";
                Connection.Send(loginFailed);
                return;
            }
        }        
        private void DebugPremMessage(DebugPremMessage message)
        {
            long lowID = HomeMode.Avatar.AccountId;
            Account account = Accounts.Load(lowID);
            
            if (account.Home.DevEndTime > DateTime.UtcNow)
            {
                account.Avatar.PremiumLevel = 1;
                if (account.Home.PremiumEndTime < DateTime.UtcNow)
            {
                account.Home.PremiumEndTime = DateTime.UtcNow.AddMonths(3);
            }
            else
            {
                account.Home.PremiumEndTime = account.Home.PremiumEndTime.AddMonths(3);
            }
                return;
            }
            else
            {
                AuthenticationFailedMessage loginFailed = new AuthenticationFailedMessage();
                loginFailed.ErrorCode = 1;
                loginFailed.Message = "иди нахуй, нельзя тебе дебаг.";
                Connection.Send(loginFailed);
                return;
            }
        }
        private void DebugTropMessage(DebugTropMessage message)
        {
            long lowID = HomeMode.Avatar.AccountId;
            Account account = Accounts.Load(lowID);
            
            if (account.Home.DevEndTime > DateTime.UtcNow)
            {
                account.Avatar.GiveTrophies(100, 0);                
            }
            else
            {
                AuthenticationFailedMessage loginFailed = new AuthenticationFailedMessage();
                loginFailed.ErrorCode = 1;
                loginFailed.Message = "иди нахуй, нельзя тебе дебаг.";
                Connection.Send(loginFailed);
                return;
            }
        }
    }
}
