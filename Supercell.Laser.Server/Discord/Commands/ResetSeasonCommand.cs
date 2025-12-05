namespace Supercell.Laser.Server.Discord.Commands
{
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class ResetSeason : CommandModule<CommandContext>
    {
        [Command("resetseason")]
        public static string ResetSeasonCommand()
        {
            long lastAccId = Accounts.GetMaxAvatarId();
            if (lastAccId <= 0)
                return "Нет аккаунтов для обработки.";

            long processedCount = 0;

            // Константы вне цикла
            int[] trophyRangesStart = {
                550, 600, 650, 700, 750, 800, 850, 900, 950, 1000, 1050, 1100, 1150, 1200, 1250, 1300, 1350, 1400, 1450, 1500, 1550, 1600, 1650, 1700, 1750, 1800, 1850, 1900, 1950, 2000, 2050, 2100, 2150, 2200, 2250, 2300, 2350, 2400, 2450, 2500, 2550, 2600, 2650, 2700, 2750, 2800, 2850, 2900, 2950, 3000, 3050, 3100, 3150, 3200, 3250, 3300, 3350, 3400, 3450, 3500, 3550, 3600, 3650, 3700, 3750, 3800, 3850, 3900, 3950, 4000, 4050, 4100, 4150, 4200, 4250, 4300, 4350, 4400, 4450, 4500
            };

            int[] trophyRangesEnd = {
                599, 649, 699, 749, 799, 849, 899, 949, 999, 1049, 1099, 1149, 1199, 1249, 1299, 1349, 1399, 1449, 1499, 1549, 1599, 1649, 1699, 1749, 1799, 1849, 1899, 1949, 1999, 2049, 2099, 2149, 2199, 2249, 2299, 2349, 2399, 2449, 2499, 2549, 2599, 2649, 2699, 2749, 2799, 2849, 2899, 2949, 2999, 3049, 3099, 3149, 3199, 3249, 3299, 3349, 3399, 3449, 3499, 3549, 3599, 3649, 3699, 3749, 3799, 3849, 3899, 3949, 3999, 4049, 4099, 4149, 4199, 4249, 4299, 4349, 4399, 4449, 4499, 1000000
            };

            int[] seasonRewardAmounts = {
                70, 120, 160, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500, 520, 540, 560, 580, 600, 620, 640, 660, 680, 700, 720, 740, 760, 780, 800, 820, 840, 860, 880, 900, 920, 940, 960, 980, 1000, 1020, 1040, 1060, 1080, 1100, 1120, 1140, 1160, 1180, 1200, 1220, 1240, 1260, 1280, 1300, 1320, 1340, 1360, 1380, 1400, 1420, 1440, 1460, 1480, 1500, 1520, 1540, 1560, 1580, 1600, 1620, 1640, 1660, 1680, 1700, 1720
            };

            int[] trophyResetValues = {
                525, 550, 600, 650, 700, 725, 750, 775, 800, 825, 850, 875, 900, 925, 950, 975, 1000, 1025, 1050, 1075, 1100, 1125, 1150, 1175, 1200, 1225, 1250, 1275, 1300, 1325, 1350, 1375, 1400, 1425, 1450, 1475, 1500, 1525, 1550, 1575, 1600, 1625, 1650, 1675, 1700, 1725, 1750, 1775, 1800, 1825, 1850, 1875, 1900, 1925, 1950, 1975, 2000, 2025, 2050, 2075, 2100, 2125, 2150, 2175, 2200, 2225, 2250, 2275, 2300, 2325, 2350, 2375, 2400, 2425, 2450, 2475, 2500, 2525, 2550, 2575, 2600, 2625, 2650, 2675, 2700, 2725, 2750, 2775, 2800, 2825, 2850, 2875, 2900, 2925, 2950, 2975, 3000, 3025, 3050, 3075, 3100, 3125, 3150, 3175, 3200, 3225, 3250, 3275, 3300, 3325, 3350, 3375, 3400, 3425, 3450, 3475, 3500, 3525, 3550, 3575, 3600, 3625, 3650, 3675, 3700, 3725, 3750, 3775, 3800, 3825, 3850, 3875, 3900, 3925, 3950, 3975, 4000
            };

            for (long accId = 1; accId <= lastAccId; accId++)
            {
                Account thisacc = Accounts.LoadNoCache(accId);
                if (thisacc == null)
                    continue;

                if (thisacc.Avatar.Trophies >= 550)
                {
                    List<int> heroIds = new();
                    List<int> heroTrophies = new();
                    List<int> resetTrophies = new();
                    List<int> starPointsAwarded = new();

                    foreach (Hero hero in thisacc.Avatar.Heroes)
                    {
                        if (hero.Trophies >= trophyRangesStart[0])
                        {
                            heroIds.Add(hero.CharacterId);
                            heroTrophies.Add(hero.Trophies);

                            int index = 0;
                            while (index < trophyRangesStart.Length)
                            {
                                if (hero.Trophies >= trophyRangesStart[index] && hero.Trophies <= trophyRangesEnd[index])
                                {
                                    if (trophyRangesStart[index] != 4500)
                                    {
                                        int trophiesReset = hero.Trophies - trophyResetValues[index];
                                        hero.Trophies = trophyResetValues[index];
                                        resetTrophies.Add(trophiesReset);
                                        starPointsAwarded.Add(seasonRewardAmounts[index]);
                                    }
                                    else
                                    {
                                        int extraTrophies = hero.Trophies - 1440;
                                        extraTrophies /= 2;
                                        int trophiesReset = hero.Trophies - trophyResetValues[index] - extraTrophies;
                                        hero.Trophies = trophyResetValues[index] + extraTrophies;
                                        starPointsAwarded.Add(seasonRewardAmounts[index] + (extraTrophies / 2));
                                        resetTrophies.Add(trophiesReset);
                                    }
                                    break;
                                }
                                index++;
                            }
                        }
                    }

                    if (heroIds.Count > 0)
                    {
                        thisacc.Home.NotificationFactory.Add(
                            new Notification
                            {
                                Id = 79,
                                HeroesIds = heroIds,
                                HeroesTrophies = heroTrophies,
                                HeroesTrophiesReseted = resetTrophies,
                                StarpointsAwarded = starPointsAwarded,
                            }
                        );
                    }
                }

                Accounts.Save(thisacc);

                // Обновляем счётчик обработанных аккаунтов
                Interlocked.Increment(ref processedCount);

                // Выводим прогресс каждые 100 аккаунтов
                if (processedCount % 100 == 0 || processedCount == lastAccId)
                {
                    double percent = (double)processedCount / lastAccId * 100;
                    Console.WriteLine($"ResetSeason: Обработано {processedCount} / {lastAccId} аккаунтов ({percent:F2}% завершено)");
                }
            }

            return $"Трофейный сезон был сброшен для всех пользователей. Обработано: {processedCount} аккаунтов.";
        }
    }
}