using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpringOff.MGBEx
{
  public sealed class BillingCollection
  {
    [JsonProperty("billlist")]
    public List<Billing> Billings { get; set; } = new List<Billing>();
  }
}