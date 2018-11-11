using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stereopoly
{
  public interface IDownloader
  {
    void DownloadString(Uri uri, Action<string> action);
  }
}
