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
}