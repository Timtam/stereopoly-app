using Medallion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using stereopoly.api;

namespace stereopoly
{
  public class GameState
  {
    [JsonProperty("board")]
    public Board Board;
    [JsonProperty("date")]
    public DateTime Date;
    [JsonProperty("remaining_newsgroups")]
    public List<int> RemainingNewsgroups;
    [JsonProperty("current_newsgroup")]
    public int CurrentNewsgroup;
    [JsonProperty("current_news")]
    public int CurrentNews;
    [JsonProperty("remaining_chance_cards")]
    public List<int> RemainingChanceCards;
    
    [JsonProperty("remaining_community_chest_cards")]
    public List<int> RemainingCommunityChestCards;

    public GameState()
    {
      this.Date = DateTime.Now;
    }

    public GameState(Board b) : this()
    {
      int i;

      this.Board = b;

      this.RemainingNewsgroups = new List<int>(this.Board.Newsgroups.Count);

      for(i = 0; i < this.Board.Newsgroups.Count; i++)
        this.RemainingNewsgroups.Add(i);

      this.CurrentNewsgroup = -1;
      this.CurrentNews = 0;
    }

    // returns the next news and updates its date
    public News GetNextNews()
    {
      int i;
      int ng;
      News n;
      if(this.CurrentNewsgroup != -1 && this.CurrentNews == this.Board.Newsgroups[this.CurrentNewsgroup].News.Count)
      {
        this.CurrentNewsgroup = -1;
      }
      if(this.CurrentNewsgroup == -1)
      {
        // if there are no newsgroups left
        if(this.RemainingNewsgroups.Count == 0)
        {
          for(i=0; i < this.Board.Newsgroups.Count; i++)
            this.RemainingNewsgroups.Add(i);
        }
        // we will randomly choose a new newsgroup
        i = Rand.Next(0, this.RemainingNewsgroups.Count);
        ng = this.RemainingNewsgroups[i];
        this.RemainingNewsgroups.Remove(ng);
        this.CurrentNewsgroup = ng;
        this.CurrentNews = 0;
      }
      n = this.Board.Newsgroups[this.CurrentNewsgroup].News[this.CurrentNews];
      this.CurrentNews++;
      this.Date = DateTime.Now;
      return n;
    }

    public ChanceCard GetNextChanceCard()
    {
      ChanceCard c;
      if(this.RemainingChanceCards == null || this.RemainingChanceCards.Count == 0)
      {
        this.RemainingChanceCards = Enumerable.Range(0, this.Board.ChanceCards.Count).ToList();
        this.RemainingChanceCards.Shuffle();
      }
      c = this.Board.ChanceCards[this.RemainingChanceCards[0]];
      this.RemainingChanceCards.RemoveAt(0);
      this.Date = DateTime.Now;
      return c;
    }

    public CommunityChestCard GetNextCommunityChestCard()
    {
      CommunityChestCard c;
      if(this.RemainingCommunityChestCards == null || this.RemainingCommunityChestCards.Count == 0)
      {
        this.RemainingCommunityChestCards = Enumerable.Range(0, this.Board.CommunityChestCards.Count).ToList();
        this.RemainingCommunityChestCards.Shuffle();
      }
      c = this.Board.CommunityChestCards[this.RemainingCommunityChestCards[0]];
      this.RemainingCommunityChestCards.RemoveAt(0);
      this.Date = DateTime.Now;
      return c;
    }
  }
}
