using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;

namespace SpringOff.MGBEx
{
  internal interface IApiService
  {
    Task<string> Login(LoginRequest request);
    Task<string> GetAccounts(UserProfile profile);

    Task<string> GetFlatInfo(int flatId, UserProfile profile);

    Task<string> GetFlatMonthList(int flatId, UserProfile profile);

    Task<string> GetFlatInfoByMonth(int flatId, UserProfile profile, int month);

    Task<string> GetBillings(int status, UserProfile profile);

    Task<string> GetBillingOrder(string id, UserProfile profile);
  }
  
  internal sealed class ApiService : IApiService
  {
    private const string BaseUrl = "https://erc.megabank.net/ru/infoapi/v1.0";
    private readonly Microsoft.Extensions.Logging.ILogger _logger;

    public ApiService(ILoggerFactory loggerFactory)
    {
      _logger = loggerFactory.CreateLogger<ApiService>();
    }

    public async Task<string> Login(LoginRequest request)
    {
      if (request == null)
        throw new ArgumentNullException(nameof(request));
      
      var client = new HttpClient();
      
      var httpContent = new StringContent(JsonConvert.SerializeObject(request));
      httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json"); 
      
      return await GetResponse(client, $"{BaseUrl}/user/login.json", httpContent);
    }
    
    public async Task<string> GetAccounts(UserProfile profile)
    {
      if (profile == null)
        throw new ArgumentNullException(nameof(profile));
      
      var client = GetClient(profile);
      
      var httpContent = new StringContent(string.Empty);
      httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json"); 
            
      return await GetResponse(client, $"{BaseUrl}/flatlist/listinfo.json", httpContent);
    }

    public async Task<string> GetFlatInfo(int flatId, UserProfile profile)
    {
      if (profile == null)
        throw new ArgumentNullException(nameof(profile));
      
      var client = GetClient(profile);
      
      var httpContent = new StringContent(JsonConvert.SerializeObject(new { idf = flatId  }));
      httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json"); 
            
      return await GetResponse(client, $"{BaseUrl}/flatlist/flatinfo.json", httpContent);
    }

    public async Task<string> GetFlatMonthList(int flatId, UserProfile profile)
    {
      if (profile == null)
        throw new ArgumentNullException(nameof(profile));
      
      var client = GetClient(profile);
      
      var httpContent = new StringContent(JsonConvert.SerializeObject(new { idf = flatId  }));
      httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json"); 
            
      return await GetResponse(client, $"{BaseUrl}/flatlist/monthlist.json", httpContent);
    }

    public async Task<string> GetFlatInfoByMonth(int flatId, UserProfile profile, int month)
    {
      if (profile == null)
        throw new ArgumentNullException(nameof(profile));
      
      var client = GetClient(profile);
      
      var httpContent = new StringContent(JsonConvert.SerializeObject(new { idf = flatId, month }));
      httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json"); 
            
      return await GetResponse(client, $"{BaseUrl}/flatlist/flatnac.json", httpContent);
    }

    public async Task<string> GetBillings(int status, UserProfile profile)
    {
      if (profile == null)
        throw new ArgumentNullException(nameof(profile));
      
      var client = GetClient(profile);
      
      var httpContent = new StringContent(JsonConvert.SerializeObject(new { status = status }));
      httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json"); 
            
      return await GetResponse(client, $"{BaseUrl}/flatpay/billlist.json", httpContent);
    }

    public async Task<string> GetBillingOrder(string id, UserProfile profile)
    {
      if (profile == null)
        throw new ArgumentNullException(nameof(profile));
      
      var client = GetClient(profile);
      
      var httpContent = new StringContent(JsonConvert.SerializeObject(new { idorder = id }));
      httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json"); 
            
      return await GetResponse(client, $"{BaseUrl}/flatpay/orderinfo.json", httpContent);
    }

    private static HttpClient GetClient(UserProfile profile)
    {
      var client = new HttpClient();
      client.DefaultRequestHeaders.Add("x-csrf-token", profile.Token);
      client.DefaultRequestHeaders.Add("Cookie", $"{profile.SessionName}={profile.SessionId}");

      return client;
    }

    private async static Task<string> GetResponse(HttpClient client, string url, HttpContent content)
    {
      HttpResponseMessage responseMessage = await client.PostAsync(url, content);
      if (responseMessage.IsSuccessStatusCode)
        return await responseMessage.Content.ReadAsStringAsync();
      
      return null;
    }
  }
}