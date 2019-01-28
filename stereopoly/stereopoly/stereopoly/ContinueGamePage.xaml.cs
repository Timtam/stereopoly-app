using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using stereopoly.cache;
using stereopoly.resx;

namespace stereopoly
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ContinueGamePage : ContentPage
  {

    private Dictionary<Button, GameState> ButtonGameStateMapping;

    public ContinueGamePage ()
    {
      InitializeComponent ();
      this.ButtonGameStateMapping = new Dictionary<Button, GameState>();
    }

    protected override void OnAppearing()
    {
      Button b;
      int i;
      List<GameState> states = Storage.GetAllGameStates();

      this.ButtonGameStateMapping.Clear();

      for(i=0; i < states.Count; i++)
      {
        b = new Button{
          Text = String.Format(AppResources.GameStateButtonText, states[i].Board.Name, states[i].Date),
        };
        b.Clicked += async (s,e) => {
          await this.OnGameStateSelect(this.ButtonGameStateMapping[(Button)s]);
        };
        this.GameStateLayout.Children.Add(b);
        this.ButtonGameStateMapping.Add(b, states[i]);
      }

      if(this.ButtonGameStateMapping.Count == 0)
        this.DeleteAllButton.IsEnabled = false;
    }

    public async Task OnGameStateSelect(GameState s)
    {
      await Navigation.PushAsync(new GameBoardPage(s));
      Navigation.RemovePage(this);
    }

    public void OnDeleteAll(object sender, EventArgs e)
    {
      this.ButtonGameStateMapping.Clear();
      Storage.ClearGameStates();
      this.GameStateLayout.Children.Clear();
    }
  }
}