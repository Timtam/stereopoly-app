using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using stereopoly.api;

namespace stereopoly
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class GameBoardPage : ContentPage
  {

    private Board board;
    public GameBoardPage (Board b)
    {
      InitializeComponent ();
      this.board = b;
    }

    public async void OnWatchNews(object sender, EventArgs e)
    {
    }

    public async void OnSettings(object sender, EventArgs e)
    {
      await Navigation.PushAsync(new SettingsPage());
    }
  }
}