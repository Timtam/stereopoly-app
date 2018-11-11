using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

using stereopoly;

namespace stereopoly.api
{
  public static class Caller
  {
    public static void RequestBoards(Action<List<Board>, string> handler)
    {
      UriBuilder u = new UriBuilder(Constants.API_URL);
      
      // adding boards
      u.Path += "boards";

      // app version
      u.Query += "api=" + AppInfo.VersionString;

      DependencyService.Get<IDownloader>().DownloadString(u.Uri, 
        (r) => BoardListResponder(r, handler)
      );
    }

    private static void BoardListResponder(string r, Action<List<Board>, string> handler)
    {
      try
      {
        List<Board> boards = JsonConvert.DeserializeObject<List<Board>>(r);
        handler(boards, "");
      }
      catch (Exception)
      {
        Error err = JsonConvert.DeserializeObject<Error>(r);
        handler(null, err.Text);
      }
    }
  }
}
