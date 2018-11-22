using Newtonsoft.Json;
using System.Collections.Generic;

namespace stereopoly.api
{
  public class Board
  {
    [JsonProperty("name")]
    public string Name;
    [JsonProperty("version")]
    public int Version;
    [JsonProperty("id")]
    public int ID;
    [JsonProperty("money_scheme")]
    public MoneyScheme MoneyScheme;
    [JsonProperty("newsgroups")]
    public List<Newsgroup> Newsgroups;
  }
}
