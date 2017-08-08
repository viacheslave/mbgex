using Newtonsoft.Json;

namespace SpringOff.MGBEx
{
  public sealed class UserAccount
  {
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("cdflat")]
    public string AccountId { get; set; }
  }
}