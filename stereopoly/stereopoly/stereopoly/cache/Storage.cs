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
    private static Language CurrentLanguage;
    private static List<Language> AvailableLanguages;
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
        InitializeLanguages();
        return;
      }

      data = File.ReadAllText(StorageFile);
      c = JsonConvert.DeserializeObject<CacheLayout>(data);

      CurrentLanguage = c.CurrentLanguage;
      AvailableLanguages = c.AvailableLanguages;
      CachedBoards = c.Boards;
      GameStates = c.GameStates;
      if(CachedBoards == null)
        CachedBoards = new List<Board>();
      if(GameStates == null)
        GameStates = new List<GameState>();
      if(CurrentLanguage == null || AvailableLanguages == null || AvailableLanguages.Count == 0)
        InitializeLanguages();
      if(c.Version == Constants.APP_VERSION)
        OlderConfiguration = false;

    }

    public static void SaveCache()
    {
      CacheLayout c = new CacheLayout();
      string data;
      c.CurrentLanguage = CurrentLanguage;
      c.AvailableLanguages = AvailableLanguages;
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

    private static void InitializeLanguages()
    {
      AvailableLanguages = new List<Language>();
      CurrentLanguage = new Language{
        Name = "English",
        Code = "en"
      };
    }

    public static Language GetCurrentLanguage()
    {
      return CurrentLanguage;
    }

    public static void SetCurrentLanguage(Language l)
    {
      CurrentLanguage = l;
      ClearCache();
    }

    // clears all available languages with the given indicator
    // if local is true, all local languages, else all remotely available ones
    // if a language got both local and remote == false,
    // this language will be purged from the list
    public static void ClearAvailableLanguages(bool local = false)
    {
      int i = 0;

      if(AvailableLanguages == null || AvailableLanguages.Count == 0)
        return;

      do
      {
        if(local == true)
          AvailableLanguages[i].Local = false;
        else
          AvailableLanguages[i].Remote = false;
        if(AvailableLanguages[i].Remote == false && AvailableLanguages[i].Local == false)
        {
          AvailableLanguages.RemoveAt(i);
          continue;
        }
        i++;
      }
      while(i < AvailableLanguages.Count);
    }

    // "adds" the language
    // if a language with that code already exists, nothing happens
    // if it exists and the remote / local tags are not equal, copy them over
    // if it doesn't exist, add it in
    public static void UpdateAvailableLanguage(Language l, bool local = false)
    {
      List<Language> found;

      if(AvailableLanguages == null)
        AvailableLanguages = new List<Language>();

      found = AvailableLanguages.Where(o => o.Code == l.Code).ToList();

      if(found.Count == 0)
      {
        AvailableLanguages.Add(l);
      }
      else
      {

        if(local == true && found[0].Local != l.Local)
          found[0].Local = l.Local;
        else if(local == false && found[0].Remote != l.Remote)
          found[0].Remote = l.Remote;
      }

      if(CurrentLanguage.Code == l.Code)
      {
        if(local == true && CurrentLanguage.Local != l.Local)
          CurrentLanguage.Local = l.Local;
        else if(local == false && CurrentLanguage.Remote != l.Remote)
          CurrentLanguage.Remote = l.Remote;
      }

      SaveCache();
    }

    public static List<Language> GetAvailableLanguages()
    {
      return AvailableLanguages;
    }
  }
}
