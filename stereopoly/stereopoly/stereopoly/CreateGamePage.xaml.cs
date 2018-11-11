using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using stereopoly.api;

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
      Button b;
      int i;
      List<Board> boards;
      boards = await Caller.RequestBoards();
      this.DownloadingIndicator.IsRunning=false;
      for(i=0; i < boards.Count; i++)
      {
        b = new Button {
          Text = boards[i].Name
        };
        this.BoardLayout.Children.Add(b);
      }
    }
  }
}