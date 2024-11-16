using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BlumBot
{

    public class BlumBots
    {

        public readonly BlumQuery PubQuery;
        private readonly ProxyType[] PubProxy;
        private readonly string AccessToken;
        public readonly bool HasError;
        public readonly string ErrorMessage;
        public readonly string IPAddress;

        public BlumBots(BlumQuery Query, ProxyType[] Proxy)
        {
            PubQuery = Query;
            PubProxy = Proxy;
            IPAddress = GetIP().Result;
            PubQuery.Auth = getSession();
            var Login = BlumLogin().Result;
            if (Login is not null)
            {
                AccessToken = Login.Token?.Access ?? string.Empty;
                HasError = false;
                ErrorMessage = "";
            }
            else
            {
                AccessToken = string.Empty;
                HasError = true;
                ErrorMessage = "login failed";
            }
        }

        private async Task<string> GetIP()
        {
            HttpClient client;
            var FProxy = PubProxy.Where(x => x.Index == PubQuery.Index);
            if (FProxy.Count() != 0)
            {
                if (!string.IsNullOrEmpty(FProxy.ElementAtOrDefault(0)?.Proxy))
                {
                    var handler = new HttpClientHandler() { Proxy = new WebProxy() { Address = new Uri(FProxy.ElementAtOrDefault(0)?.Proxy ?? string.Empty) } };
                    client = new HttpClient(handler) { Timeout = new TimeSpan(0, 0, 30) };
                }
                else
                    client = new HttpClient() { Timeout = new TimeSpan(0, 0, 30) };
            }
            else
                client = new HttpClient() { Timeout = new TimeSpan(0, 0, 30) };
            HttpResponseMessage httpResponse = null;
            try
            {
                httpResponse = await client.GetAsync($"https://httpbin.org/ip");
            }
            catch { }
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<Httpbin>(responseStream);
                    return responseJson?.Origin ?? string.Empty;
                }
            }

            return "";
        }

        private string getSession()
        {
            string query = "";
            var vw = new TelegramMiniApp.WebView(PubQuery.API_ID, PubQuery.API_HASH, PubQuery.Name, PubQuery.Phone, "BlumCryptoBot", "https://telegram.blum.codes/");
            vw.Get_tgWebAppData(out query);

            return query;
        }

        private async Task<BlumLoginResponse?> BlumLogin()
        {
            var BAPI = new BlumApi(0, PubQuery.Auth, PubQuery.Index, PubProxy);
            var request = new BlumLoginRequest() { Query = PubQuery.Auth };
            string serializedRequest = JsonSerializer.Serialize(request);
            var serializedRequestContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
            var httpResponse = await BAPI.BAPIPost($"https://user-domain.blum.codes/api/v1/auth/provider/PROVIDER_TELEGRAM_MINI_APP", serializedRequestContent);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BlumLoginResponse>(responseStream);
                    return responseJson;
                }
            }

            return null;
        }

        public async Task<long> BlumTimeNow()
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIGet($"https://game-domain.blum.codes/api/v1/time/now");
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BlumTimeNowResponse>(responseStream);
                    return responseJson?.Now ?? 0;
                }
            }

            return 0;
        }

        public async Task<bool> BlumEligibility()
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIGet($"https://game-domain.blum.codes/api/v2/game/eligibility/dogs_drop");
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BlumEligibilityResponse>(responseStream);
                    return responseJson?.Eligible ?? false;
                }
            }

            return false;
        }

        public async Task<bool> BlumDailyReward()
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIPost($"https://game-domain.blum.codes/api/v1/daily-reward?offset=-210", (HttpContent)null);
            if (httpResponse is not null)
                return httpResponse.IsSuccessStatusCode;

            return false;
        }

        public async Task<BlumUserBalanceResponse?> BlumUserBalance()
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIGet($"https://game-domain.blum.codes/api/v1/user/balance");
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BlumUserBalanceResponse>(responseStream);
                    return responseJson;
                }
            }

            return null;
        }

        public async Task<bool> BlumStartFarming()
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIPost($"https://game-domain.blum.codes/api/v1/farming/start", (HttpContent)null);
            if (httpResponse is not null)
                return httpResponse.IsSuccessStatusCode;

            return false;
        }

        public async Task<bool> BlumClaimFarming()
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIPost($"https://game-domain.blum.codes/api/v1/farming/claim", (HttpContent)null);
            if (httpResponse is not null)
                return httpResponse.IsSuccessStatusCode;

            return false;
        }

        public async Task<BlumFriendsBalanceResponse?> BlumFriendsBalance()
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIGet($"https://user-domain.blum.codes/api/v1/friends/balance");
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BlumFriendsBalanceResponse>(responseStream);
                    return responseJson;
                }
            }

            return null;
        }

        public async Task<bool> BlumClaimFriends()
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIPost($"https://user-domain.blum.codes/api/v1/friends/claim", (HttpContent)null);
            if (httpResponse is not null)
                return httpResponse.IsSuccessStatusCode;

            return false;
        }

        public async Task<List<BlumAnswersResponse>?> BlumAnswers()
        {
            var client = new HttpClient() { Timeout = new TimeSpan(0, 0, 30) };
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true, NoStore = true, MaxAge = TimeSpan.FromSeconds(0d) };
            HttpResponseMessage httpResponse = null;
            try
            {
                httpResponse = await client.GetAsync($"https://raw.githubusercontent.com/glad-tidings/BlumBot/refs/heads/main/tasks.json");
            }
            catch { }
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<List<BlumAnswersResponse>>(responseStream);
                    return responseJson;
                }
            }

            return null;
        }

        public async Task<List<BlumTasksResponse>?> BlumTasks()
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIGet($"https://earn-domain.blum.codes/api/v1/tasks");
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<List<BlumTasksResponse>>(responseStream);
                    return responseJson;
                }
            }

            return null;
        }

        public async Task<bool> BlumStartTask(string taskId)
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIPost($"https://earn-domain.blum.codes/api/v1/tasks/{taskId}/start", (HttpContent)null);
            if (httpResponse is not null)
                return httpResponse.IsSuccessStatusCode;

            return false;
        }

        public async Task<bool> BlumValidateTask(string taskId, string keyword)
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index, PubProxy);
            var request = new BlumTasksValidateRequest() { Keyword = keyword };
            string serializedRequest = JsonSerializer.Serialize(request);
            var serializedRequestContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
            var httpResponse = await BAPI.BAPIPost($"https://earn-domain.blum.codes/api/v1/tasks/{taskId}/validate", serializedRequestContent);
            if (httpResponse is not null)
                return httpResponse.IsSuccessStatusCode;

            return false;
        }

        public async Task<bool> BlumClaimTask(string taskId)
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIPost($"https://earn-domain.blum.codes/api/v1/tasks/{taskId}/claim", (HttpContent)null);
            if (httpResponse is not null)
                return httpResponse.IsSuccessStatusCode;

            return false;
        }

        public async Task<BlumGamePlayResponse?> BlumGamePlay()
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIPost($"https://game-domain.blum.codes/api/v2/game/play", (HttpContent)null);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BlumGamePlayResponse>(responseStream);
                    return responseJson;
                }
            }

            return null;
        }

        public async Task<bool> BlumGameClaim(string gameID, int points)
        {
            var client = new HttpClient() { Timeout = new TimeSpan(0, 0, 30) };
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true, NoStore = true, MaxAge = TimeSpan.FromSeconds(0d) };
            HttpResponseMessage prehttpResponse = null;
            int freeze = (int)Math.Round(points / 50d + Random.Shared.NextDouble() * 2d);
            int bombs = Convert.ToInt32(points < 150 ? Random.Shared.NextDouble() * 2d : 0);
            var prerequest = new BlumPayloadRequest()
            {
                GameId = gameID,
                EarnedPoints = new BlumPayloadEarnedPoints() { BP = new BlumPayloadEarnedPointsPoint() { Amount = points } },
                AssetClicks = new Dictionary<string, BlumPayloadAssetClick>() { { "CLOVER", new BlumPayloadAssetClick() { Clicks = points } }, { "FREEZE", new BlumPayloadAssetClick() { Clicks = freeze } }, { "BOMB", new BlumPayloadAssetClick() { Clicks = bombs } } }
            };
            string preserializedRequest = JsonSerializer.Serialize(prerequest);
            var preserializedRequestContent = new StringContent(preserializedRequest, Encoding.UTF8, "application/json");
            try
            {
                prehttpResponse = await client.PostAsync($"https://blum-payload-generator.vercel.app/api/generate", preserializedRequestContent);
            }
            catch { }
            if (prehttpResponse is not null)
            {
                if (prehttpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await prehttpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BlumGameClaimRequest>(responseStream);
                    if (!string.IsNullOrEmpty(responseJson?.Payload))
                    {
                        var BAPI = new BlumApi(1, AccessToken, PubQuery.Index, PubProxy);
                        var request = new BlumGameClaimRequest() { Payload = responseJson.Payload };
                        string serializedRequest = JsonSerializer.Serialize(request);
                        var serializedRequestContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
                        var httpResponse = await BAPI.BAPIPost($"https://game-domain.blum.codes/api/v2/game/claim", serializedRequestContent);
                        if (httpResponse is not null)
                            return httpResponse.IsSuccessStatusCode;
                    }
                }
            }

            return false;
        }

        public async Task<BlumTribeResponse?> BlumTribe()
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIGet($"https://tribe-domain.blum.codes/api/v1/tribe/my");
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BlumTribeResponse>(responseStream);
                    return responseJson;
                }
            }

            return null;
        }

        public async Task<bool> BlumTribeLeave()
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIPost($"https://tribe-domain.blum.codes/api/v1/tribe/leave", (HttpContent)null);
            if (httpResponse is not null)
                return httpResponse.IsSuccessStatusCode;

            return false;
        }

        public async Task<bool> BlumTribeJoin(string tribeId)
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIPost($"https://tribe-domain.blum.codes/api/v1/tribe/{tribeId}/join", (HttpContent)null);
            if (httpResponse is not null)
                return httpResponse.IsSuccessStatusCode;

            return false;
        }

    }
}