using System;
using System.Globalization;
using System.Resources;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using stereopoly.api;
using stereopoly.cache;
using stereopoly.resx;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace stereopoly
{
  public partial class App : Application
  {
    public App()
    {
      this.UpdateAppLanguage();
      InitializeComponent();
    }

    protected async override void OnStart()
    {
      bool conf = Storage.OlderConfiguration;
      // Handle when your app starts

      if(conf == true)
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

    public void UpdateAppLanguage()
    {
      CultureInfo ci;
      Language l;

      if(Storage.GetCurrentLanguage().Local == true)
      {
        ci = new CultureInfo(Storage.GetCurrentLanguage().Code);
        AppResources.Culture = ci;
        CultureInfo.CurrentCulture = ci;
        CultureInfo.CurrentUICulture = ci;
        CultureInfo.DefaultThreadCurrentCulture = ci;
        CultureInfo.DefaultThreadCurrentUICulture = ci;
        if(Device.RuntimePlatform != Device.UWP)
          DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
      }
      else
      {
        if (Device.RuntimePlatform != Device.UWP)
          ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
        else
          ci = CultureInfo.CurrentCulture;

        l = new Language{
          Name = ci.EnglishName,
          Code = ci.TwoLetterISOLanguageName
        };

        Storage.SetCurrentLanguage(l);
        CultureInfo.CurrentCulture = ci;
        CultureInfo.CurrentUICulture = ci;
        CultureInfo.DefaultThreadCurrentCulture = ci;
        CultureInfo.DefaultThreadCurrentUICulture = ci;
        AppResources.Culture = ci; // set the RESX for resource localization
        if(Device.RuntimePlatform != Device.UWP)
          DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
      }
    }
  }
}
