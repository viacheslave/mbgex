using Newtonsoft.Json;

namespace SpringOff.MGBEx
{
  public sealed class FlatMonth
  {
    [JsonProperty("idmonth")]
    public int Id { get; set; }

    [JsonProperty("month")]
    public string Caption { get; set; }
  }
}