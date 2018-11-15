using Newtonsoft.Json;

namespace stereopoly.api
{
  public class News
  {
    [JsonProperty("text")]
    public string Text;
    [JsonProperty("id")]
    public int ID;
  }
}
