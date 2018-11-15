using Newtonsoft.Json;
using System.Collections.Generic;

using stereopoly.api;

namespace stereopoly.cache
{
  class CacheLayout
  {
    [JsonProperty("boards")]
    public List<Board> Boards;
  }
}