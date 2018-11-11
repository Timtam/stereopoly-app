using Newtonsoft.Json;
using System.Collections.Generic;

namespace stereopoly.api
{
  public class MoneyScheme
  {
    [JsonProperty("name")]
    public string Name;
    [JsonProperty("id")]
    public int ID;
    [JsonProperty("money")]
    public List<int> Money;
  }
}
