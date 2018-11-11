using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using stereopoly.iOS;
using System.ComponentModel;
using System.IO;
using System.Net;

[assembly: Dependency(typeof(iOSDownloader))]
namespace stereopoly.iOS
{
  public class iOSDownloader : IDownloader
  {
    public void DownloadString(Uri url, Action<string> action)
    {
      try
      {
        WebClient webClient = new WebClient();
        webClient.DownloadStringCompleted += (s,e) => action(e.Result);
        webClient.DownloadStringAsync(url);
      }
      catch (Exception)
      {
        action("");
      }
    }
  }
}