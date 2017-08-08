using Newtonsoft.Json;

namespace SpringOff.MGBEx
{
  public sealed class LoginRequest
  {
    [JsonProperty("username")]
    public string UserName { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }
  }
}