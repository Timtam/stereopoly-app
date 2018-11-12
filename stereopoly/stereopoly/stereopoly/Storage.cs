using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using stereopoly.api;

namespace stereopoly
{
  public static class Storage
  {
    readonly static string StorageFile;
    private static List<Board> CachedBoards;
    static Storage()
    {
      StorageFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "storage.json");
      ReloadCache();
    }

    public static void ReloadCache()
    {
      string data;
      if(!File.Exists(StorageFile))
      {
        CachedBoards = new List<Board>();
      }
      else
      {
        data = File.ReadAllText(StorageFile);
        CachedBoards = JsonConvert.DeserializeObject<List<Board>>(data);
      }
    }

    public static void SaveCache()
    {
      string data;
      data = JsonConvert.SerializeObject(CachedBoards);
      File.WriteAllText(StorageFile, data);
    }

    public static List<string> GetCachedBoardNames()
    {
      return CachedBoards.Select(b => b.Name).ToList();
    }

    public static void UpdateBoardList(List<Board> b)
    {
      int i;

      if(CachedBoards == null)
      {
        CachedBoards = b;
        SaveCache();
        return;
      }

      for(i=0; i < b.Count; i++)
      {
        if(BoardUpdateRequired(b[i]))
          UpdateBoard(b[i]);
      }
      SaveCache();
    }

    public static bool BoardUpdateRequired(Board b)
    {
      List<Board> search = CachedBoards.Where(o => o.ID == b.ID).ToList();
      if(search.Count == 0)
        return true;

      if(search[0].Version != b.Version)
        return true;

      if(search[0].News == null || search[0].News.Count == 0 || search[0].MoneyScheme == null)
        return true;
      return false;
    }

    public static void UpdateBoard(Board b)
    {
      int i;
      List<Board> search = CachedBoards.Where(o => o.ID == b.ID).ToList();

      if(search.Count == 0)
      {
        CachedBoards.Add(b);
        return;
      }
      
      i = CachedBoards.IndexOf(search[0]);

      CachedBoards[i] = b;
      SaveCache();
    }
  }
}
