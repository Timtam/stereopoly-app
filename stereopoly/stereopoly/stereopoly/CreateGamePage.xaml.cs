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
    public CreateGamePage ()
    {
      InitializeComponent ();
    }

    protected override async void OnAppearing()
    {
      await this.UpdateBoards();
      Storage.BoardUpdateRequired = false;
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

      boards = Storage.GetAllBoards();

      for(i=0; i < boards.Count; i++)
      {
        b = new Button {
          Text = boards[i].Name,
          Command = new Command(async () => {
            await this.OnBoardSelect(boards[i].ID);
          })
        };
        this.BoardLayout.Children.Add(b);
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