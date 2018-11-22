using Newtonsoft.Json;

using System.Collections.Generic;

namespace stereopoly.api
{
  public class Newsgroup
  {
    [JsonProperty("news")]
    public List<News> News;
  }
}
