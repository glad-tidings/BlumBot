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
| Multi Proxy               | ✅        |

## Settings
open project in visual studio ([Download Visual Studio Express](https://visualstudio.microsoft.com/vs/express/)) and in program.cs find
```c#
BlumQueries.Add(new BlumQuery() { Index = 0, Name = "Account 1", Auth = "query_id of account 1", Proxy = "" });
```
for each account(max 17) you need to add an Add in a new line, for example for 3 accounts:
```c#
BlumQueries.Add(new BlumQuery() { Index = 0, Name = "Account 1", Auth = "query_id of account 1", Proxy = "" });
BlumQueries.Add(new BlumQuery() { Index = 1, Name = "Account 2", Auth = "query_id of account 2", Proxy = "socks5://10.10.10.10:1080" });
BlumQueries.Add(new BlumQuery() { Index = 2, Name = "Account 3", Auth = "query_id of account 3", Proxy = "socks4://10.10.10.10:1080" });
```
and build or start project ([Get Telegram MiniGame Query ID using Chrome](https://youtu.be/r0Ulqev-9M4))

## QueryID Script
Script to request query_id of all your Telegram accounts for any mini Telegram Bot App ==> [TelegramMiniAppQueryID](https://github.com/glad-tidings/TelegramMiniAppQueryID)

## ☕Buy me a coffee
```
USDT(TRC20): TRjdnBWpS1xT4a2oKQdFsM3AAc6TxVmqGZ
TON: UQBmpnet6lYCLXObDwJLitDuMcPIocJIKVxg6pLvaFv5fRmO
```

![](http://visit.parselecom.com/Api/Visit/27/CF3476)
