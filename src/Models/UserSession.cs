using Newtonsoft.Json;

namespace SpringOff.MGBEx
{
  public sealed class UserProfile
  {
    [JsonProperty("sessid")]
    public string SessionId { get; set; }

    [JsonProperty("session_name")]
    public string SessionName { get; set; }

    [JsonProperty("token")]
    public string Token { get; set; }
  }
}