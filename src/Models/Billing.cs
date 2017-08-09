using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpringOff.MGBEx
{
  public sealed class Billing
  {
    [JsonProperty("orderlist")]
    public List<BillingOrder> Orders { get; set; } = new List<BillingOrder>();
  }
}