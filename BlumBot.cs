using System.Text;
using System.Text.Json;

namespace BlumBot
{
    public class BlumBots
    {

        private readonly BlumQuery PubQuery;
        private readonly string AccessToken;
        public readonly bool HasError;
        public readonly string ErrorMessage;

        public BlumBots(BlumQuery Query)
        {
            PubQuery = Query;
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

        private async Task<BlumLoginResponse?> BlumLogin()
        {
            var BAPI = new BlumApi(0, PubQuery.Auth, PubQuery.Index);
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
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index);
            var httpResponse = await BAPI.BAPIGet($"https://game-domain.blum.codes/api/v1/time/now");
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BlumTimeNowResponse>(responseStream);
                    return (responseJson ?? new()).Now;
                }
            }

            return 0;
        }

        public async Task<bool> BlumDailyReward()
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index);
            var httpResponse = await BAPI.BAPIPost($"https://game-domain.blum.codes/api/v1/daily-reward?offset=-210", null);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                    return true;
            }

            return false;
        }

        public async Task<BlumUserBalanceResponse?> BlumUserBalance()
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index);
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
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index);
            var httpResponse = await BAPI.BAPIPost($"https://game-domain.blum.codes/api/v1/farming/start", null);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                    return true;
            }

            return false;
        }

        public async Task<bool> BlumClaimFarming()
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index);
            var httpResponse = await BAPI.BAPIPost($"https://game-domain.blum.codes/api/v1/farming/claim", null);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                    return true;
            }

            return false;
        }

        public async Task<BlumFriendsBalanceResponse?> BlumFriendsBalance()
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index);
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
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index);
            var httpResponse = await BAPI.BAPIPost($"https://user-domain.blum.codes/api/v1/friends/claim", null);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                    return true;
            }
            
            return false;
        }

        public async Task<List<BlumAnswersResponse>?> BlumAnswers()
        {
            var client = new HttpClient() { Timeout = new TimeSpan(0, 0, 30) };
            var httpResponse = await client.GetAsync($"https://raw.githubusercontent.com/glad-tidings/BlumBot/refs/heads/main/tasks.json");
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
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index);
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
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index);
            var httpResponse = await BAPI.BAPIPost($"https://earn-domain.blum.codes/api/v1/tasks/{taskId}/start", null);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                    return true;
            }

            return false;
        }

        public async Task<bool> BlumValidateTask(string taskId, string keyword)
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index);
            var request = new BlumTasksValidateRequest() { Keyword = keyword };
            string serializedRequest = JsonSerializer.Serialize(request);
            var serializedRequestContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
            var httpResponse = await BAPI.BAPIPost($"https://earn-domain.blum.codes/api/v1/tasks/{taskId}/validate", serializedRequestContent);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                    return true;
            }

            return false;
        }

        public async Task<bool> BlumClaimTask(string taskId)
        {
            var BAPI = new BlumApi(1, AccessToken, PubQuery.Index);
            var httpResponse = await BAPI.BAPIPost($"https://earn-domain.blum.codes/api/v1/tasks/{taskId}/claim", null);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                    return true;
            }

            return false;
        }

    }
}
