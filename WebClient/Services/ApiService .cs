using System.Net.Http.Headers;
using BusinessObject.DTOs.Authorize;
using WebClient.Services.Interface;

namespace WebClient.Services
{
    public class ApiService : IApiService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public ApiService(
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            IConfiguration config
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint, bool isSkip = true)
        {
            var client = _httpClientFactory.CreateClient();

            if (!isSkip)
            {
                var token = await GetValidAccessTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer",
                        token
                    );
                }
            }

            return await client.GetAsync(_config["API:BaseUrl"] + endpoint);
        }

        public async Task<HttpResponseMessage> PostAsync(
            string endpoint,
            MultipartFormDataContent formData,
            bool isSkip = false
        )
        {
            var client = _httpClientFactory.CreateClient();

            if (!isSkip)
            {
                var token = await GetValidAccessTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer",
                        token
                    );
                }
            }

            return await client.PostAsync(_config["API:BaseUrl"] + endpoint, formData);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(
            string endpoint,
            T content,
            bool isSkip = false
        )
        {
            var client = _httpClientFactory.CreateClient();

            if (!isSkip)
            {
                var token = await GetValidAccessTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer",
                        token
                    );
                }
            }

            return await client.PostAsJsonAsync(_config["API:BaseUrl"] + endpoint, content);
        }

        public async Task<HttpResponseMessage> PutAsync(
            string endpoint,
            MultipartFormDataContent formData,
            bool isSkip = true
        )
        {
            var client = _httpClientFactory.CreateClient();

            if (!isSkip)
            {
                var token = await GetValidAccessTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer",
                        token
                    );
                }
            }

            return await client.PutAsync(_config["API:BaseUrl"] + endpoint, formData);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(
            string endpoint,
            T content,
            bool isSkip = true
        )
        {
            var client = _httpClientFactory.CreateClient();

            if (!isSkip)
            {
                var token = await GetValidAccessTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer",
                        token
                    );
                }
            }

            return await client.PutAsJsonAsync(_config["API:BaseUrl"] + endpoint, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint, bool isSkip = true)
        {
            var client = _httpClientFactory.CreateClient();

            if (!isSkip)
            {
                var token = await GetValidAccessTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer",
                        token
                    );
                }
            }

            return await client.DeleteAsync(_config["API:BaseUrl"] + endpoint);
        }

        private async Task<string> GetValidAccessTokenAsync()
        {
            var context = _httpContextAccessor.HttpContext!;
            var accessToken = context.Session.GetString("AccessToken");
            var refreshToken = context.Session.GetString("RefreshToken");
            var expiresAtStr = context.Session.GetString("AccessTokenExpiresAt");

            // Nếu token còn hạn → dùng luôn
            if (DateTime.TryParse(expiresAtStr, out var expiresAt) && expiresAt > DateTime.UtcNow)
            {
                return accessToken!;
            }

            // Token đã hết hạn → refresh
            var client = _httpClientFactory.CreateClient();
            var refreshRequest = new RefreshTokenRequestDto { RefreshToken = refreshToken };

            var response = await client.PostAsJsonAsync(
                _config["API:BaseUrl"] + "/api/Authorize/refresh-token",
                refreshRequest
            );

            if (!response.IsSuccessStatusCode)
            {
                // RefreshToken hết hạn → logout
                context.Session.Clear();
                context.Response.Redirect("/Authorize/Login");
                return string.Empty;
            }

            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponseDto>();

            context.Session.SetString("AccessToken", tokenResponse.AccessToken);
            context.Session.SetString("RefreshToken", tokenResponse.RefreshToken);
            context.Session.SetString(
                "AccessTokenExpiresAt",
                tokenResponse.AccessTokenExpiresAt.ToString("O")
            );

            return tokenResponse.AccessToken;
        }
    }
}
