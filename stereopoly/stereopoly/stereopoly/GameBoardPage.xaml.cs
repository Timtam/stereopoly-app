using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using stereopoly.api;
using stereopoly.cache;

namespace stereopoly
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class GameBoardPage : ContentPage
  {

    private GameState state;
    public GameBoardPage (GameState s)
    {
      InitializeComponent ();
      this.state = s;
    }

    public async void OnWatchNews(object sender, EventArgs e)
    {
      News n = this.state.GetNextNews();
      await Navigation.PushAsync(new WatchNewsPage(n));
    }

    public async void OnSettings(object sender, EventArgs e)
    {
      await Navigation.PushAsync(new SettingsPage());
    }

    public async void OnEndGame(object sender, EventArgs e)
    {
      Storage.RemoveGameState(this.state);
      Storage.SaveCache();
      await Navigation.PopToRootAsync();
    }
  }
}