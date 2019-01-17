using Newtonsoft.Json;
using System.Collections.Generic;

using stereopoly.api;

namespace stereopoly.cache
{
  class CacheLayout
  {
    [JsonProperty("boards")]
    public List<Board> Boards;
    [JsonProperty("game_states")]
    public List<GameState> GameStates;
    [JsonProperty("version")]
    public string Version;
    [JsonProperty("available_languages")]
    public List<Language> AvailableLanguages;
    [JsonProperty("current_language")]
    public Language CurrentLanguage;
  }
}