using System.Text.Json.Serialization;

namespace BlumBot
{

    public class BlumQuery
    {
        [JsonPropertyName("Index")]
        public int Index { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("API_ID")]
        public string API_ID { get; set; } = string.Empty;
        [JsonPropertyName("API_HASH")]
        public string API_HASH { get; set; } = string.Empty;
        [JsonPropertyName("Phone")]
        public string Phone { get; set; } = string.Empty;
        public string Auth { get; set; } = string.Empty;
        [JsonPropertyName("Active")]
        public bool Active { get; set; }
        [JsonPropertyName("DailyReward")]
        public bool DailyReward { get; set; }
        [JsonPropertyName("Farming")]
        public bool Farming { get; set; }
        [JsonPropertyName("FriendBonus")]
        public bool FriendBonus { get; set; }
        [JsonPropertyName("Game")]
        public bool Game { get; set; }
        [JsonPropertyName("GameCount")]
        public int[] GameCount { get; set; } = [];
        [JsonPropertyName("GamePoint")]
        public int[] GamePoint { get; set; } = [];
        [JsonPropertyName("GameSleep")]
        public int[] GameSleep { get; set; } = [];
        [JsonPropertyName("Task")]
        public bool Task { get; set; }
        [JsonPropertyName("TaskSleep")]
        public int[] TaskSleep { get; set; } = [];
        [JsonPropertyName("DaySleep")]
        public int[] DaySleep { get; set; } = [];
        [JsonPropertyName("NightSleep")]
        public int[] NightSleep { get; set; } = [];
    }

    public class BlumLoginRequest
    {
        [JsonPropertyName("query")]
        public string Query { get; set; } = string.Empty;
    }

    public class BlumLoginResponse
    {
        [JsonPropertyName("token")]
        public BlumLoginToken Token { get; set; } = new();
    }

    public class BlumLoginToken
    {
        [JsonPropertyName("access")]
        public string Access { get; set; } = string.Empty;
        [JsonPropertyName("refresh")]
        public string Refresh { get; set; } = string.Empty;
    }

    public class BlumFriendsBalanceResponse
    {
        [JsonPropertyName("amountForClaim")]
        public string AmountForClaim { get; set; } = string.Empty;
        [JsonPropertyName("canClaim")]
        public bool CanClaim { get; set; }
        [JsonPropertyName("canClaimAt")]
        public long? CanClaimAt { get; set; }
    }

    public class BlumWalletMyResponse
    {
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("isAvailable")]
        public bool IsAvailable { get; set; }
        [JsonPropertyName("balanceMigrated")]
        public bool BalanceMigrated { get; set; }
    }

    public class BlumTimeNowResponse
    {
        [JsonPropertyName("now")]
        public long Now { get; set; }
    }

    public class BlumUserBalanceResponse
    {
        [JsonPropertyName("availableBalance")]
        public string AvailableBalance { get; set; } = string.Empty;
        [JsonPropertyName("playPasses")]
        public int PlayPasses { get; set; }
        [JsonPropertyName("isFastFarmingEnabled")]
        public bool IsFastFarmingEnabled { get; set; }
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
        [JsonPropertyName("farming")]
        public BlumUserBalanceFarming Farming { get; set; } = new();
    }

    public class BlumUserBalanceFarming
    {
        [JsonPropertyName("startTime")]
        public long StartTime { get; set; }
        [JsonPropertyName("endTime")]
        public long EndTime { get; set; }
        [JsonPropertyName("earningsRate")]
        public string EarningsRate { get; set; } = string.Empty;
        [JsonPropertyName("balance")]
        public string Balance { get; set; } = string.Empty;
    }

    public class BlumEligibilityResponse
    {
        [JsonPropertyName("eligible")]
        public bool Eligible { get; set; }
    }

    public class BlumAnswersResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("keyword")]
        public string Keyword { get; set; } = string.Empty;
    }

    public class BlumTasksResponse
    {
        [JsonPropertyName("sectionType")]
        public string SectionType { get; set; } = string.Empty;
        [JsonPropertyName("tasks")]
        public List<BlumTasksTasks> Tasks { get; set; } = [];
        [JsonPropertyName("subSections")]
        public List<BlumTasksSubSections> SubSections { get; set; } = [];
    }

    public class BlumTasksTasks
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("validationType")]
        public string ValidationType { get; set; } = string.Empty;
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("isHidden")]
        public bool IsHidden { get; set; }
        [JsonPropertyName("isDisclaimerRequired")]
        public bool IsDisclaimerRequired { get; set; }
        [JsonPropertyName("subTasks")]
        public List<BlumTasksTasksSubTasks> SubTasks { get; set; } = [];
    }

    public class BlumTasksTasksSubTasks
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("validationType")]
        public string ValidationType { get; set; } = string.Empty;
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("isDisclaimerRequired")]
        public bool IsDisclaimerRequired { get; set; }
    }

    public class BlumTasksSubSections
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("tasks")]
        public List<BlumTasksTasks> Tasks { get; set; } = [];
    }

    public class BlumTasksValidateRequest
    {
        [JsonPropertyName("keyword")]
        public string Keyword { get; set; } = string.Empty;
    }

    public class BlumGamePlayResponse
    {
        [JsonPropertyName("gameId")]
        public string GameId { get; set; } = string.Empty;
        [JsonPropertyName("assets")]
        public BlumGamePlayAssets Assets { get; set; } = new();
    }

    public class BlumGamePlayAssets
    {
        [JsonPropertyName("BOMB")]
        public BlumGamePlayAssetsItem BOMB { get; set; } = new();
        [JsonPropertyName("CLOVER")]
        public BlumGamePlayAssetsItem CLOVER { get; set; } = new();
        [JsonPropertyName("FREEZE")]
        public BlumGamePlayAssetsItem FREEZE { get; set; } = new();
    }

    public class BlumGamePlayAssetsItem
    {
        [JsonPropertyName("probability")]
        public string Probability { get; set; } = string.Empty;
        [JsonPropertyName("perClick")]
        public string PerClick { get; set; } = string.Empty;
    }

    public class BlumGameClaimRequest
    {
        [JsonPropertyName("payload")]
        public string Payload { get; set; } = string.Empty;
    }

    public class BlumPayloadRequest
    {
        [JsonPropertyName("gameId")]
        public string GameId { get; set; } = string.Empty;
        [JsonPropertyName("challenge")]
        public BlumPayloadChallenge Challenge { get; set; } = new();
        [JsonPropertyName("earnedPoints")]
        public BlumPayloadEarnedPoints EarnedPoints { get; set; } = new();
        [JsonPropertyName("assetClicks")]
        public Dictionary<string, BlumPayloadAssetClick> AssetClicks { get; set; } = [];
    }

    public class BlumPayloadChallenge
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("nonce")]
        public int Nonce { get; set; }
        [JsonPropertyName("hash")]
        public string Hash { get; set; } = string.Empty;
    }

    public class BlumPayloadEarnedPoints
    {
        [JsonPropertyName("BP")]
        public BlumPayloadEarnedPointsPoint BP { get; set; } = new();
    }

    public class BlumPayloadEarnedPointsPoint
    {
        [JsonPropertyName("amount")]
        public int Amount { get; set; }
    }

    public class BlumPayloadAssetClick
    {
        [JsonPropertyName("clicks")]
        public int Clicks { get; set; }
    }

    public class BlumTribeResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("chatname")]
        public string Chatname { get; set; } = string.Empty;
        [JsonPropertyName("countMembers")]
        public int CountMembers { get; set; }
        [JsonPropertyName("earnBalance")]
        public string EarnBalance { get; set; } = string.Empty;
        [JsonPropertyName("rank")]
        public int Rank { get; set; }
    }

    public class ProxyType
    {
        [JsonPropertyName("Index")]
        public int Index { get; set; }
        [JsonPropertyName("Proxy")]
        public string Proxy { get; set; } = string.Empty;
    }

    public class Httpbin
    {
        [JsonPropertyName("origin")]
        public string Origin { get; set; } = string.Empty;
    }
}