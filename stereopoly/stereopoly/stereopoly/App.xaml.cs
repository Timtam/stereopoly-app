using System;
using System.Globalization;
using System.Resources;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using stereopoly.cache;
using stereopoly.resx;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace stereopoly
{
  public partial class App : Application
  {
    public App()
    {
      InitializeComponent();
    }

    protected async override void OnStart()
    {
      // Handle when your app starts
      if (Device.RuntimePlatform != Device.UWP)
      {
        var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
        AppResources.Culture = ci; // set the RESX for resource localization
        DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
      }

      if(Storage.OlderConfiguration == true)
        this.SetUpdatingPage();
      else
        await this.SetMainPage();
    }

    protected override void OnSleep()
    {
      // Handle when your app sleeps
    }

    protected override void OnResume()
    {
      // Handle when your app resumes
    }

    public void SetUpdatingPage()
    {
      this.MainPage = new UpdatingPage();
    }

    public async Task SetMainPage()
    {
      this.MainPage = new NavigationPage(new MainPage());
      await this.MainPage.Navigation.PopToRootAsync();
    }
  }
}
