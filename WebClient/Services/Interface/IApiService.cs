namespace WebClient.Services.Interface
{
    public interface IApiService
    {
        Task<HttpResponseMessage> GetAsync(string endpoint, bool isSkip = true);
        Task<HttpResponseMessage> PostAsync(
            string endpoint,
            MultipartFormDataContent formData,
            bool isSkip = false
        );

        Task<HttpResponseMessage> PostAsync<T>(string endpoint, T content, bool isSkip = true);
        Task<HttpResponseMessage> PutAsync(
            string endpoint,
            MultipartFormDataContent formData,
            bool isSkip = false
        );
        Task<HttpResponseMessage> PutAsync<T>(string endpoint, T content, bool isSkip = true);
        Task<HttpResponseMessage> DeleteAsync(string endpoint, bool isSkip = true);
    }
}
