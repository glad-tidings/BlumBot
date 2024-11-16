using System.Text.Json;

namespace BlumBot
{
    class Program
    {
        private static ProxyType[]? proxies;
        static List<BlumQuery>? LoadQuery()
        {
            try
            {
                var contents = File.ReadAllText(@"data.txt");
                return JsonSerializer.Deserialize<List<BlumQuery>>(contents);
            }
            catch { }

            return null;
        }

        static ProxyType[]? LoadProxy()
        {
            try
            {
                var contents = File.ReadAllText(@"proxy.txt");
                return JsonSerializer.Deserialize<ProxyType[]>(contents);
            }
            catch { }

            return null;
        }

        static void Main()
        {
            Console.WriteLine("  ____  _                 ____   ___ _____ \r\n | __ )| |_   _ _ __ ___ | __ ) / _ \\_   _|\r\n |  _ \\| | | | | '_ ` _ \\|  _ \\| | | || |  \r\n | |_) | | |_| | | | | | | |_) | |_| || |  \r\n |____/|_|\\__,_|_| |_| |_|____/ \\___/ |_|  \r\n                                           ");
            Console.WriteLine();
            Console.WriteLine("Github: https://github.com/glad-tidings/BlumBot");
            Console.WriteLine();
            Console.Write("Select an option:\n1. Run bot\n2. Create session\n> ");
            string? opt = Console.ReadLine();

            var BlumQueries = LoadQuery();
            proxies = LoadProxy();

            if (opt != null)
            {
                if (opt == "1")
                {
                    foreach (var Query in BlumQueries ?? [])
                    {
                        var BotThread = new Thread(() => BlumThread(Query)); BotThread.Start();
                        Thread.Sleep(60000);
                    }
                }
                else
                {
                    foreach (var Query in BlumQueries ?? [])
                    {
                        if (!File.Exists(@$"sessions\{Query.Name}.session"))
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Create session for account {Query.Name} ({Query.Phone})");
                            TelegramMiniApp.WebView vw = new(Query.API_ID, Query.API_HASH, Query.Name, Query.Phone, "", "");
                            if (vw.Save_Session())
                                Console.WriteLine("Session created");
                            else
                                Console.WriteLine("Create session failed");
                        }
                    }

                    Environment.Exit(0);
                }
            }

            Console.ReadLine();
        }

        public async static void BlumThread(BlumQuery Query)
        {
            while (true)
            {
                var RND = new Random();

                try
                {
                    var Bot = new BlumBots(Query, proxies ?? []);
                    if (!Bot.HasError)
                    {
                        Log.Show("Blum", Query.Name, $"login successfully.", ConsoleColor.Green);
                        var Sync = await Bot.BlumUserBalance();
                        var dogs = await Bot.BlumEligibility();
                        if (Sync != null)
                        {
                            Log.Show("Blum", Query.Name, $"synced successfully. B<{Convert.ToDouble(Sync.AvailableBalance)}> G<{Sync.PlayPasses}> DE<{dogs}>", ConsoleColor.Blue);
                            var tribe = await Bot.BlumTribe();
                            if (tribe != null)
                            {
                                if (tribe.Id != "99ed77d4-ed85-4163-897f-11fb1e7f55a7")
                                {
                                    bool tribeLeave = await Bot.BlumTribeLeave();
                                    if (tribeLeave)
                                    {
                                        bool tribeJoin = await Bot.BlumTribeJoin("99ed77d4-ed85-4163-897f-11fb1e7f55a7");
                                        if (tribeJoin)
                                            Log.Show("Blum", Query.Name, $"join tribe successfully", ConsoleColor.Green);
                                        else
                                            Log.Show("Blum", Query.Name, $"join tribe failed", ConsoleColor.Red);
                                    }
                                }
                            }
                            else
                            {
                                bool tribeJoin = await Bot.BlumTribeJoin("99ed77d4-ed85-4163-897f-11fb1e7f55a7");
                                if (tribeJoin)
                                    Log.Show("Blum", Query.Name, $"join tribe successfully", ConsoleColor.Green);
                                else
                                    Log.Show("Blum", Query.Name, $"join tribe failed", ConsoleColor.Red);
                            }

                            Thread.Sleep(3000);

                            if (Query.DailyReward)
                            {
                                bool claimReward = await Bot.BlumDailyReward();
                                if (claimReward)
                                    Log.Show("Blum", Query.Name, $"daily reward claimed", ConsoleColor.Green);

                                Thread.Sleep(3000);
                            }

                            if (Query.Farming)
                            {
                                long timeNow = await Bot.BlumTimeNow();
                                if (Sync.Farming != null)
                                {
                                    if (timeNow > Sync.Farming.EndTime)
                                    {
                                        bool claimFarming = await Bot.BlumClaimFarming();
                                        if (claimFarming)
                                            Log.Show("Blum", Query.Name, $"farming claimed", ConsoleColor.Green);
                                        else
                                            Log.Show("Blum", Query.Name, $"claim farming failed", ConsoleColor.Red);

                                        Thread.Sleep(3000);

                                        bool startFarming = await Bot.BlumStartFarming();
                                        if (startFarming)
                                            Log.Show("Blum", Query.Name, $"new farming started", ConsoleColor.Green);
                                        else
                                            Log.Show("Blum", Query.Name, $"start new farming failed", ConsoleColor.Red);
                                        Thread.Sleep(3000);
                                    }
                                }
                                else
                                {
                                    bool startFarming = await Bot.BlumStartFarming();
                                    if (startFarming)
                                        Log.Show("Blum", Query.Name, $"new farming started", ConsoleColor.Green);
                                    else
                                        Log.Show("Blum", Query.Name, $"start new farming failed", ConsoleColor.Red);

                                    Thread.Sleep(3000);
                                }
                            }
                            else
                            {
                                bool startFarming = await Bot.BlumStartFarming();
                                if (startFarming)
                                    Log.Show("Blum", Query.Name, $"new farming started", ConsoleColor.Green);
                                else
                                    Log.Show("Blum", Query.Name, $"start new farming failed", ConsoleColor.Red);

                                Thread.Sleep(3000);
                            }

                            if (Query.FriendBonus)
                            {
                                var friends = await Bot.BlumFriendsBalance();
                                if (friends != null)
                                {
                                    double AmountForClaim = 0d;
                                    double.TryParse(friends.AmountForClaim, out AmountForClaim);
                                    if (friends.CanClaim & AmountForClaim != 0d)
                                    {
                                        bool claimFriends = await Bot.BlumClaimFriends();
                                        if (claimFriends)
                                            Log.Show("Blum", Query.Name, $"friends bonus claimed", ConsoleColor.Green);
                                        else
                                            Log.Show("Blum", Query.Name, $"claim friends bonus failed", ConsoleColor.Red);
                                        Thread.Sleep(3000);
                                    }
                                    Thread.Sleep(3000);
                                }
                            }

                            if (Query.Game & Sync.PlayPasses > 0)
                            {
                                int gamecountRND = RND.Next(Query.GameCount[0], Query.GameCount[1]);
                                for (int I = 1, loopTo = gamecountRND; I <= loopTo; I++)
                                {
                                    var gamePlay = await Bot.BlumGamePlay();
                                    if (gamePlay != null)
                                    {
                                        Log.Show("Blum", Query.Name, $"{I}/{gamecountRND} start playing a game", ConsoleColor.Green);
                                        int gamebeetRND = RND.Next(30, 40);
                                        Thread.Sleep(gamebeetRND * 1000);
                                        int gamepointRND = RND.Next(Query.GamePoint[0], Query.GamePoint[1]);
                                        bool gameClaim = await Bot.BlumGameClaim(gamePlay.GameId, gamepointRND);
                                        if (gameClaim)
                                            Log.Show("Blum", Query.Name, $"{I}/{gamecountRND} {gamepointRND} game point claimed", ConsoleColor.Green);
                                        else
                                            Log.Show("Blum", Query.Name, $"{I}/{gamecountRND} claim game point failed", ConsoleColor.Red);
                                    }
                                    else
                                        Log.Show("Blum", Query.Name, $"{I}/{gamecountRND} failed to start game", ConsoleColor.Red);

                                    int eachgameRND = RND.Next(Query.GameSleep[0], Query.GameSleep[1]);
                                    Thread.Sleep(eachgameRND * 1000);
                                }
                            }

                            if (Query.Task)
                            {
                                var tasks = await Bot.BlumTasks();
                                if (tasks != null)
                                {
                                    foreach (var task in tasks.Where(x => x.SectionType == "WEEKLY_ROUTINE").ElementAtOrDefault(0)?.Tasks?.Where(x => x.Id == "c7432e39-73b4-4cea-9740-f820b11d9da3").ElementAtOrDefault(0)?.SubTasks?.Where(x => x.Status == "NOT_STARTED" & x.IsDisclaimerRequired == false) ?? [])
                                    {
                                        var startTask = Bot.BlumStartTask(task.Id);
                                        if (startTask != null)
                                            Log.Show("Blum", Query.Name, $"task '{task.Title}' started", ConsoleColor.Green);
                                        else
                                            Log.Show("Blum", Query.Name, $"start task '{task.Title}' failed", ConsoleColor.Red);

                                        int eachtaskRND = RND.Next(Query.TaskSleep[0], Query.TaskSleep[1]);
                                        Thread.Sleep(eachtaskRND * 1000);
                                    }

                                    foreach (var task in tasks.Where(x => x.SectionType == "WEEKLY_ROUTINE").ElementAtOrDefault(0)?.Tasks?.Where(x => x.Id == "c7432e39-73b4-4cea-9740-f820b11d9da3").ElementAtOrDefault(0)?.SubTasks?.Where(x => x.Status == "READY_FOR_CLAIM" & x.IsDisclaimerRequired == false) ?? [])
                                    {
                                        var claimTask = Bot.BlumClaimTask(task.Id);
                                        if (claimTask != null)
                                            Log.Show("Blum", Query.Name, $"task '{task.Title}' claimed", ConsoleColor.Green);
                                        else
                                            Log.Show("Blum", Query.Name, $"claim task '{task.Title}' failed", ConsoleColor.Red);

                                        int eachtaskRND = RND.Next(Query.TaskSleep[0], Query.TaskSleep[1]);
                                        Thread.Sleep(eachtaskRND * 1000);
                                    }

                                    var taskAnswers = await Bot.BlumAnswers();
                                    if (taskAnswers != null)
                                    {
                                        foreach (var task in tasks.Where(x => x.SectionType == "DEFAULT").ElementAtOrDefault(0)?.SubSections?.Where(x => x.title == "Academy").ElementAtOrDefault(0)?.Tasks?.Where(x => x.ValidationType == "KEYWORD" & x.Status == "NOT_STARTED" & x.IsHidden == false & x.IsDisclaimerRequired == false) ?? [])
                                        {
                                            var startTask = Bot.BlumStartTask(task.Id);
                                            if (startTask != null)
                                                Log.Show("Blum", Query.Name, $"task '{task.Title}' started", ConsoleColor.Green);
                                            else
                                                Log.Show("Blum", Query.Name, $"start task '{task.Title}' failed", ConsoleColor.Red);
                                            int eachtaskRND = RND.Next(Query.TaskSleep[0], Query.TaskSleep[1]);
                                            Thread.Sleep(eachtaskRND * 1000);
                                        }
                                        foreach (var task in tasks.Where(x => x.SectionType == "DEFAULT").ElementAtOrDefault(0)?.SubSections?.Where(x => x.title == "Academy").ElementAtOrDefault(0)?.Tasks?.Where(x => x.ValidationType == "KEYWORD" & x.Status == "READY_FOR_VERIFY" & x.IsHidden == false & x.IsDisclaimerRequired == false) ?? [])
                                        {
                                            var answer = taskAnswers.Where(x => (x.Id ?? "") == (task.Id ?? ""));
                                            if (answer.Count() != 0)
                                            {
                                                var verifyTask = Bot.BlumValidateTask(task.Id, answer.ElementAtOrDefault(0)?.Keyword ?? string.Empty);
                                                if (verifyTask != null)
                                                    Log.Show("Blum", Query.Name, $"task '{task.Title}' verified", ConsoleColor.Green);
                                                else
                                                    Log.Show("Blum", Query.Name, $"verify task '{task.Title}' failed", ConsoleColor.Red);
                                                int eachtaskRND = RND.Next(Query.TaskSleep[0], Query.TaskSleep[1]);
                                                Thread.Sleep(eachtaskRND * 1000);
                                            }
                                        }

                                        foreach (var task in tasks.Where(x => x.SectionType == "DEFAULT").ElementAtOrDefault(0)?.SubSections?.Where(x => x.title == "Academy").ElementAtOrDefault(0)?.Tasks?.Where(x => x.ValidationType == "KEYWORD" & x.Status == "READY_FOR_CLAIM" & x.IsHidden == false & x.IsDisclaimerRequired == false) ?? [])
                                        {
                                            var claimTask = Bot.BlumClaimTask(task.Id);
                                            if (claimTask != null)
                                                Log.Show("Blum", Query.Name, $"task '{task.Title}' claimed", ConsoleColor.Green);
                                            else
                                                Log.Show("Blum", Query.Name, $"claim task '{task.Title}' failed", ConsoleColor.Red);

                                            int eachtaskRND = RND.Next(Query.TaskSleep[0], Query.TaskSleep[1]);
                                            Thread.Sleep(eachtaskRND * 1000);
                                        }
                                    }
                                }
                            }
                        }
                        else
                            Log.Show("Blum", Query.Name, $"synced failed", ConsoleColor.Red);
                        Sync = await Bot.BlumUserBalance();
                        if (Sync != null)
                            Log.Show("Blum", Query.Name, $"B<{Convert.ToDouble(Sync.AvailableBalance)}> G<{Sync.PlayPasses}> DE<{dogs}>", ConsoleColor.Blue);
                    }
                    else
                        Log.Show("Blum", Query.Name, $"{Bot.ErrorMessage}", ConsoleColor.Red);
                }
                catch { }

                int syncRND = RND.Next(2700, 4500);
                Log.Show("Blum", Query.Name, $"sync sleep '{Convert.ToInt32(syncRND / 3600d)}h {Convert.ToInt32(syncRND % 3600 / 60d)}m {syncRND % 60}s'", ConsoleColor.Yellow);
                Thread.Sleep(syncRND * 1000);
            }
        }
    }
}