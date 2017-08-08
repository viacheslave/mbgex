using Newtonsoft.Json;

namespace SpringOff.MGBEx
{
  public sealed class FlatInfoRequest
  {
    [JsonProperty("idf")]
    public int FlatId { get; set; }
  }
}