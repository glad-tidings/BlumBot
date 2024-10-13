namespace BlumBot
{
    class Program
    {
        private readonly static List<BlumQuery> BlumQueries = [];

        static void Main()
        {
            BlumQueries.Add(new BlumQuery() { Index = 0, Name = "Account 1", Auth = "query_id of account 1" });

            foreach (var Query in BlumQueries)
            {
                var BotThread = new Thread(() => BlumThread(Query));
                BotThread.Start();
                Thread.Sleep(120000);
            }
        }

        public async static void BlumThread(BlumQuery Query)
        {
            while (true)
            {
                var RND = new Random();

                var Bot = new BlumBots(Query);
                if (!Bot.HasError)
                {
                    Log.Show("Blum", Query.Name, $"login successfully.", ConsoleColor.Green);
                    var Sync = await Bot.BlumUserBalance();
                    if (Sync is not null)
                    {
                        Log.Show("Blum", Query.Name, $"synced successfully. B<{Sync.AvailableBalance}> G<{Sync.PlayPasses}>", ConsoleColor.Blue);

                        Thread.Sleep(3000);

                        bool claimReward = await Bot.BlumDailyReward();
                        if (claimReward)
                            Log.Show("Blum", Query.Name, $"daily reward claimed", ConsoleColor.Green);

                        Thread.Sleep(3000);

                        long timeNow = await Bot.BlumTimeNow();
                        if (Sync.Farming is not null)
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

                        var friends = await Bot.BlumFriendsBalance();
                        if (friends is not null)
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
                        }

                        var tasks = await Bot.BlumTasks();
                        if (tasks is not null)
                        {
                            foreach (var task in tasks.Where(x => x.SectionType == "WEEKLY_ROUTINE").ElementAtOrDefault(0)?.Tasks?[0].SubTasks?.Where(x => x.Status == "NOT_STARTED" & x.IsDisclaimerRequired == false) ?? [])
                            {
                                var startTask = Bot.BlumStartTask(task.Id);
                                if (startTask is not null)
                                    Log.Show("Blum", Query.Name, $"task '{task.Title}' started", ConsoleColor.Green);
                                else
                                    Log.Show("Blum", Query.Name, $"start task '{task.Title}' failed", ConsoleColor.Red);

                                int eachtaskRND = RND.Next(7, 20);
                                Thread.Sleep(eachtaskRND * 1000);
                            }

                            foreach (var task in tasks.Where(x => x.SectionType == "WEEKLY_ROUTINE").ElementAtOrDefault(0)?.Tasks?[0].SubTasks?.Where(x => x.Status == "READY_FOR_CLAIM" & x.IsDisclaimerRequired == false) ?? [])
                            {
                                var claimTask = Bot.BlumClaimTask(task.Id);
                                if (claimTask is not null)
                                    Log.Show("Blum", Query.Name, $"task '{task.Title}' claimed", ConsoleColor.Green);
                                else
                                    Log.Show("Blum", Query.Name, $"claim task '{task.Title}' failed", ConsoleColor.Red);

                                int eachtaskRND = RND.Next(7, 20);
                                Thread.Sleep(eachtaskRND * 1000);
                            }

                            var taskAnswers = await Bot.BlumAnswers();
                            if (taskAnswers is not null)
                            {
                                foreach (var task in tasks.Where(x => x.SectionType == "DEFAULT").ElementAtOrDefault(0)?.SubSections?.Where(x => x.title == "Academy").ElementAtOrDefault(0)?.Tasks?.Where(x => x.ValidationType == "KEYWORD" & x.Status == "NOT_STARTED" & x.IsHidden == false & x.IsDisclaimerRequired == false) ?? [])
                                {
                                    var startTask = Bot.BlumStartTask(task.Id);
                                    if (startTask is not null)
                                        Log.Show("Blum", Query.Name, $"task '{task.Title}' started", ConsoleColor.Green);
                                    else
                                        Log.Show("Blum", Query.Name, $"start task '{task.Title}' failed", ConsoleColor.Red);

                                    int eachtaskRND = RND.Next(7, 20);
                                    Thread.Sleep(eachtaskRND * 1000);
                                }

                                foreach (var task in tasks.Where(x => x.SectionType == "DEFAULT").ElementAtOrDefault(0)?.SubSections?.Where(x => x.title == "Academy").ElementAtOrDefault(0)?.Tasks?.Where(x => x.ValidationType == "KEYWORD" & x.Status == "READY_FOR_VERIFY" & x.IsHidden == false & x.IsDisclaimerRequired == false) ?? [])
                                {
                                    var answer = taskAnswers.Where(x => (x.Id ?? "") == (task.Id ?? ""));
                                    if (answer.Count() != 0)
                                    {
                                        var verifyTask = Bot.BlumValidateTask(task.Id, answer.ElementAtOrDefault(0)?.Keyword ?? string.Empty);
                                        if (verifyTask is not null)
                                            Log.Show("Blum", Query.Name, $"task '{task.Title}' verified", ConsoleColor.Green);
                                        else
                                            Log.Show("Blum", Query.Name, $"verify task '{task.Title}' failed", ConsoleColor.Red);

                                        int eachtaskRND = RND.Next(7, 20);
                                        Thread.Sleep(eachtaskRND * 1000);
                                    }
                                }

                                foreach (var task in tasks.Where(x => x.SectionType == "DEFAULT").ElementAtOrDefault(0).SubSections.Where(x => x.title == "Academy").ElementAtOrDefault(0)?.Tasks?.Where(x => x.ValidationType == "KEYWORD" & x.Status == "READY_FOR_CLAIM" & x.IsHidden == false & x.IsDisclaimerRequired == false) ?? [])
                                {
                                    var claimTask = Bot.BlumClaimTask(task.Id);
                                    if (claimTask is not null)
                                        Log.Show("Blum", Query.Name, $"task '{task.Title}' claimed", ConsoleColor.Green);
                                    else
                                        Log.Show("Blum", Query.Name, $"claim task '{task.Title}' failed", ConsoleColor.Red);

                                    int eachtaskRND = RND.Next(7, 20);
                                    Thread.Sleep(eachtaskRND * 1000);
                                }
                            }
                        }
                    }
                    else
                        Log.Show("Blum", Query.Name, $"synced failed", ConsoleColor.Red);
                }
                else
                    Log.Show("Blum", Query.Name, $"{Bot.ErrorMessage}", ConsoleColor.Red);

                int syncRND = RND.Next(2700, 4500);
                Log.Show("Blum", Query.Name, $"sync sleep '{Convert.ToInt32(syncRND / 3600d)}h {Convert.ToInt32(syncRND % 3600 / 60d)}m {syncRND % 60}s'", ConsoleColor.Yellow);
                Thread.Sleep(syncRND * 1000);
            }
        }
    }
}