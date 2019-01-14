using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using stereopoly;
using stereopoly.api;

namespace stereopoly.cache
{
  public static class Storage
  {
    private static Language BoardLanguage;
    private static List<Board> CachedBoards;
    private static List<GameState> GameStates;
    private static string StorageFile;
    public static bool BoardUpdateRequired = true;
    public static bool OlderConfiguration = true;

    static Storage()
    {
      StorageFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "storage.json");
      LoadCache();
    }

    public static void LoadCache()
    {
      CacheLayout c;
      string data;
      if(!File.Exists(StorageFile))
      {
        CachedBoards = new List<Board>();
        GameStates = new List<GameState>();
        InitializeDefaultBoardLanguage();
        return;
      }

      data = File.ReadAllText(StorageFile);
      c = JsonConvert.DeserializeObject<CacheLayout>(data);

      BoardLanguage = c.BoardLanguage;
      CachedBoards = c.Boards;
      GameStates = c.GameStates;
      if(CachedBoards == null)
        CachedBoards = new List<Board>();
      if(GameStates == null)
        GameStates = new List<GameState>();
      if(BoardLanguage == null)
        InitializeDefaultBoardLanguage();
      if(c.Version == Constants.APP_VERSION)
        OlderConfiguration = false;

    }

    public static void SaveCache()
    {
      CacheLayout c = new CacheLayout();
      string data;
      c.BoardLanguage = BoardLanguage;
      c.Boards = CachedBoards;
      c.GameStates = GameStates;
      c.Version = Constants.APP_VERSION;
      data = JsonConvert.SerializeObject(c);
      File.WriteAllText(StorageFile, data);
      OlderConfiguration = false;
    }

    public static void UpdateBoard(Board b)
    {
      List<Board> search = CachedBoards.Where(o => o.ID == b.ID).ToList();
      int idx;

      if(search.Count == 0)
      {
        CachedBoards.Add(b);
        SaveCache();
        return;
      }

      idx = CachedBoards.IndexOf(search[0]);

      CachedBoards[idx] = b;
      SaveCache();
      
    }

    public static bool UpdateRequiredForBoard(int id, int version = 0)
    {
      List<Board> search = CachedBoards.Where(o => o.ID == id).ToList();

      if(search.Count == 0)
        return true;

      if(version > 0 && search[0].Version != version)
        return true;
        
      if(search[0].Newsgroups == null || search[0].Newsgroups.Count == 0 || search[0].MoneyScheme == null)
        return true;

      return false;
    }

    public static void UpdateBoards(List<Board> b)
    {
      int i;
      
      for(i=0; i < b.Count; i++)
        if(UpdateRequiredForBoard(b[i].ID, b[i].Version))
          UpdateBoard(b[i]);
    }

    public static List<Board> GetAllBoards()
    {
      return new List<Board>(CachedBoards);
    }

    public static Board GetBoard(int id)
    {
      List<Board> search = CachedBoards.Where(o => o.ID == id).ToList();
      
      if(search.Count == 0)
        return null;
      return search[0];
    }

    public static void ClearCache()
    {
      CachedBoards.Clear();
      SaveCache();
      BoardUpdateRequired = true;
    }

    public static void ClearGameStates()
    {
      GameStates.Clear();
      SaveCache();
    }

    public static List<GameState> GetAllGameStates()
    {
      return GameStates;
    }

    public static void InsertGameState(GameState g)
    {
      GameStates.Add(g);
    }

    public static void RemoveGameState(GameState s)
    {
      GameStates.Remove(s);
    }

    private static void InitializeDefaultBoardLanguage()
    {
      BoardLanguage = new Language();
      BoardLanguage.Name = "English";
      BoardLanguage.Code = "en";
    }

    public static Language GetBoardLanguage()
    {
      return BoardLanguage;
    }

    public static void SetBoardLanguage(Language l)
    {
      BoardLanguage = l;
      ClearCache();
    }
  }
}
