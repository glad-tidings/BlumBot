using System.Text.Json.Serialization;

namespace BlumBot
{

    public class BlumQuery
    {
        public int Index { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Auth { get; set; } = string.Empty;
        public string Proxy { get; set; } = string.Empty;
    }

    public class Httpbin
    {
        [JsonPropertyName("origin")]
        public string Origin { get; set; } = string.Empty;
    }

    public class BlumLoginRequest
    {
        [JsonPropertyName("query")]
        public string Query { get; set; } = string.Empty;
    }

    public class BlumLoginResponse
    {
        [JsonPropertyName("token")]
        public BlumLoginToken? Token { get; set; }
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

    public class BlumTribeMyResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("rank")]
        public int Rank { get; set; }
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
        public BlumUserBalanceFarming? Farming { get; set; }
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
        public string Id { get; set; }
        [JsonPropertyName("keyword")]
        public string Keyword { get; set; } = string.Empty;
    }

    public class BlumTasksResponse
    {
        [JsonPropertyName("sectionType")]
        public string SectionType { get; set; } = string.Empty;
        [JsonPropertyName("tasks")]
        public List<BlumTasksTasks>? Tasks { get; set; }
        [JsonPropertyName("subSections")]
        public List<BlumTasksSubSections>? SubSections { get; set; }
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
        public List<BlumTasksTasksSubTasks>? SubTasks { get; set; }
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
        public string title { get; set; } = string.Empty;
        [JsonPropertyName("tasks")]
        public List<BlumTasksTasks>? Tasks { get; set; }
    }

    public class BlumTasksValidateRequest
    {
        [JsonPropertyName("keyword")]
        public string Keyword { get; set; } = string.Empty;
    }
}
