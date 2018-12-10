using Newtonsoft.Json;
using System;

namespace stereopoly.api
{
  public class ChanceCard
  {
    [JsonProperty("text")]
    public string Text;
  }
}
