using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UriExtend;

using stereopoly;
using stereopoly.cache;

namespace stereopoly.api
{
  public static class Caller
  {
    static HttpClient Client;

    static Caller()
    {
      Client = new HttpClient();
    }

    public static async Task<List<Board>> RequestBoards()
    {
      Error e;
      HttpResponseMessage resp;
      string data;
      resp = await Client.GetAsync(BuildUri("/boards"));

      try
      {
        data = await resp.Content.ReadAsStringAsync();
        if(resp.StatusCode == HttpStatusCode.OK)
          return JsonConvert.DeserializeObject<List<Board>>(data);
        else if(resp.StatusCode == HttpStatusCode.BadRequest)
        {
          e = JsonConvert.DeserializeObject<Error>(data);
          throw(new WebException(e.Text));
        }
        else
        {
          throw(new WebException("Invalid status code: " + resp.StatusCode));
        }
      }
      catch (HttpRequestException ex)
      {
        throw(ex.InnerException);
      }
    }

    public static async Task<Board> RequestBoard(int id)
    {
      Error e;
      HttpResponseMessage resp;
      string data;

      resp = await Client.GetAsync(BuildUri("/boards/" + id));

      try
      {
        data = await resp.Content.ReadAsStringAsync();
        if(resp.StatusCode == HttpStatusCode.OK)
          return JsonConvert.DeserializeObject<Board>(data);
        else if(resp.StatusCode == HttpStatusCode.BadRequest)
        {
          e = JsonConvert.DeserializeObject<Error>(data);
          throw(new WebException(e.Text));
        }
        else
        {
          throw(new WebException("Invalid status code: " + resp.StatusCode));
        }
      }
      catch (HttpRequestException ex)
      {
        throw(ex.InnerException);
      }
    }

    public static async Task<List<Language>> RequestBoardLanguages()
    {
      Error e;
      HttpResponseMessage resp;
      string data;

      resp = await Client.GetAsync(BuildUri("/languages", language: false));

      try
      {
        data = await resp.Content.ReadAsStringAsync();
        if(resp.StatusCode == HttpStatusCode.OK)
          return JsonConvert.DeserializeObject<List<Language>>(data);
        else if(resp.StatusCode == HttpStatusCode.BadRequest)
        {
          e = JsonConvert.DeserializeObject<Error>(data);
          throw(new WebException(e.Text));
        }
        else
        {
          throw(new WebException("Invalid status code: " + resp.StatusCode));
        }
      }
      catch (HttpRequestException ex)
      {
        throw(ex.InnerException);
      }
    }

    private static Uri BuildUri(string path, bool language = true)
    {
      Uri u;
      string p = "/api";
      if(!path.StartsWith("/"))
        p += "/";
      p += path;
      u = new UriBuilder(Constants.API_URL)
      {
        Path = p
      }.Uri
      .AddQuery(new { api = Constants.APP_VERSION });

      if(language)
        u = u.AddQuery(new { language = Storage.GetBoardLanguage().Code });
      return u;
    }

    public static void Cancel()
    {
      Client.CancelPendingRequests();
    }
  }
}
