namespace WebClient.Services.Interface
{
    public interface IApiService
    {
        Task<HttpResponseMessage> GetAsync(string endpoint, bool isSkip = false);
        Task<HttpResponseMessage> PostAsync<T>(string endpoint, T content, bool isSkip = false);
        Task<HttpResponseMessage> PutAsync<T>(string endpoint, T content, bool isSkip = false);
        Task<HttpResponseMessage> DeleteAsync(string endpoint, bool isSkip = false);
    }
}
