﻿using Newtonsoft.Json;

namespace stereopoly.api
{
  public class Language
  {
    [JsonProperty("name")]
    public string Name;
    [JsonProperty("code")]
    public string Code;
  }
}
