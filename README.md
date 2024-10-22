# BlumBot
BlumBot Auto Farm

## ⚠️Warning
I am not responsible for your account. Please consider the potential risks before using this bot.

## Features
| Feature                   | Supported |
| :------------------------ | :-------- |
| Multithreading            | ✅        |
| Auto Claim Tasks          | ✅        |
| Auto Claim Daily Reward   | ✅        |
| Auto Claim Weekly Tasks   | ✅        |
| Auto Claim Farming        | ✅        |
| Auto Start Farming        | ✅        |

## Settings
open project in visual studio and in program.cs find
```c#
BlumQueries.Add(new BlumQuery() { Index = 0, Name = "Account 1", Auth = "query_id of account 1" });
```
for each account(max 17) you need to add an Add in a new line, for example for 3 accounts:
```c#
BlumQueries.Add(new BlumQuery() { Index = 0, Name = "Account 1", Auth = "query_id of account 1" });
BlumQueries.Add(new BlumQuery() { Index = 1, Name = "Account 2", Auth = "query_id of account 2" });
BlumQueries.Add(new BlumQuery() { Index = 2, Name = "Account 3", Auth = "query_id of account 3" });
```
and build or start project ([Get Telegram MiniGame Query ID using Chrome](https://youtu.be/r0Ulqev-9M4))

![](http://visit.parselecom.com/Api/Visit/27/CF3476)
