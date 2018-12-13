using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using stereopoly.api;
using stereopoly.cache;

namespace stereopoly
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class WatchPage : ContentPage
  {

    public WatchPage(Watchable w)
    {
      InitializeComponent();
      this.TextLabel.Text = w.Text;
    }

    protected override void OnAppearing()
    {
      NavigationPage.SetHasBackButton(this, false);
    }

    public async void OnFinish(object sender, EventArgs e)
    {
      Storage.SaveCache();
      await Navigation.PopAsync();
    }
  }
}