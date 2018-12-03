using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using stereopoly;

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
      UriBuilder u = new UriBuilder(Constants.API_URL);
      
      // adding boards
      u.Path += "boards";

      // app version
      u.Query += "api=" + Constants.APP_VERSION;

      resp = await Client.GetAsync(u.Uri);

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
      UriBuilder u = new UriBuilder(Constants.API_URL);
      
      // adding boards
      u.Path += "boards";
      u.Path += "/" + id;

      // app version
      u.Query += "api=" + Constants.APP_VERSION;

      resp = await Client.GetAsync(u.Uri);

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
  }
}
