using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace stereopoly
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class UpdatingPage : ContentPage
  {
    public UpdatingPage()
    {
      InitializeComponent();
    }

    protected override void OnAppearing()
    {
      Device.StartTimer(new TimeSpan(0, 0, 0, 0, 500), () =>
      {
        Device.BeginInvokeOnMainThread(async () =>
        {
          await ((App)App.Current).SetMainPage();
        });
        return false;
      });
    }
  }
}