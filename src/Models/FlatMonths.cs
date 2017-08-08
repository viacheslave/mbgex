using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpringOff.MGBEx
{
  public sealed class FlatMonths
  {
    [JsonProperty("monthlist")]
    public List<FlatMonth> Months { get; set; }
  }
}