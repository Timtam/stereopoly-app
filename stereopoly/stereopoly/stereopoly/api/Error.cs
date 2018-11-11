using Newtonsoft.Json;

namespace stereopoly.api
{
  public class Error
  {
    [JsonProperty("error")]
    public string Text;
  }
}