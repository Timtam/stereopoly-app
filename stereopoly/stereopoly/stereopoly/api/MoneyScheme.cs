using Newtonsoft.Json;
using System.Collections.Generic;

namespace stereopoly.api
{
  public class MoneyScheme
  {
    [JsonProperty("name")]
    public string Name;
    [JsonProperty("money")]
    public List<int> Money;
  }
}
