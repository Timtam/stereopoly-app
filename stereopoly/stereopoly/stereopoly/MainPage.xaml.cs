using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace stereopoly
{
  public partial class MainPage : ContentPage
  {
    public MainPage()
    {
      InitializeComponent();
    }

    async void OnCreate(object sender, EventArgs e)
    {
      await Navigation.PushAsync(new CreateGamePage());
    }
  }
}
