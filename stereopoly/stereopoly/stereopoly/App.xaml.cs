using System;
using System.Globalization;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using stereopoly.resx;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace stereopoly
{
  public partial class App : Application
  {
    public App()
    {
      InitializeComponent();

      MainPage = new NavigationPage(new MainPage());
    }

    protected override void OnStart()
    {
      // Handle when your app starts
      if (Device.RuntimePlatform != Device.UWP)
      {
        var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
        AppResources.Culture = ci; // set the RESX for resource localization
        DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
      }
    }

    protected override void OnSleep()
    {
      // Handle when your app sleeps
    }

    protected override void OnResume()
    {
      // Handle when your app resumes
    }
  }
}
