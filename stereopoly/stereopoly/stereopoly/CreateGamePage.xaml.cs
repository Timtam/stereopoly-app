using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using stereopoly.api;
using stereopoly.cache;

namespace stereopoly
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class CreateGamePage : ContentPage
  {
    private bool Initialized = false;
    private Dictionary<Button, int> ButtonBoardMapping;

    public CreateGamePage ()
    {
      InitializeComponent ();
      this.Initialized = true;
      this.ButtonBoardMapping = new Dictionary<Button, int>();
    }

    protected override async void OnAppearing()
    {
      if(!this.Initialized)
        return;
      await this.UpdateBoards();
      Storage.BoardUpdateRequired = false;
      this.Initialized = false;
    }

    async void OnRefresh(object sender, EventArgs e)
    {
      this.BoardLayout.Children.Clear();
      await this.UpdateBoards(true);
    }

    private async Task UpdateBoards(bool forced = false)
    {
      Button b;
      int i;
      Board a;
      List<Board> boards;
      if(Storage.BoardUpdateRequired || forced)
      {
        this.DownloadingIndicator.IsRunning = true;
        this.RefreshButton.IsEnabled = false;
        try
        {
          boards = await Caller.RequestBoards();
          Storage.UpdateBoards(boards);
          Storage.BoardUpdateRequired = false;
        }
        catch (WebException ex)
        {
          boards = null;
          await DisplayAlert("Error", "An error occurred while retrieving the list of known boards: " + ex.Message, "OK");
        }
        finally
        {
          this.DownloadingIndicator.IsRunning=false;
          this.RefreshButton.IsEnabled = true;
        }
      }

      this.ButtonBoardMapping.Clear();
      boards = Storage.GetAllBoards();

      for(i=0; i < boards.Count; i++)
      {
        a = boards[i];
        b = new Button {
          Text = a.Name,
        };
        b.Clicked += async (s,e) => {
          await this.OnBoardSelect(this.ButtonBoardMapping[(Button)s]);
        };
        this.BoardLayout.Children.Add(b);
        this.ButtonBoardMapping.Add(b, a.ID);
      }
    }

    public async Task OnBoardSelect(int id)
    {
      Board b;

      if(Storage.UpdateRequiredForBoard(id))
      {
        this.DownloadingIndicator.IsRunning = true;
        this.RefreshButton.IsEnabled = false;
        try
        {
          b = await Caller.RequestBoard(id);
          Storage.UpdateBoard(b);
        }
        catch (WebException ex)
        {
          b = null;
          await DisplayAlert("Error", "An error occurred while retrieving board information: " + ex.Message, "OK");
        }
        finally
        {
          this.DownloadingIndicator.IsRunning=false;
          this.RefreshButton.IsEnabled = true;
        }
        if(b == null)
          return;
      }

      b = Storage.GetBoard(id);
      await Navigation.PushAsync(new GameBoardPage(b));
    }
  }
}