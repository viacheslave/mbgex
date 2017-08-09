using Newtonsoft.Json;

namespace SpringOff.MGBEx
{
  public sealed class BillingOrder
  {
    [JsonProperty("idorder")]
    public string Id { get; set; }
  }
}