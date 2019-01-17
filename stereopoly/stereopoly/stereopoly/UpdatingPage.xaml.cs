using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using stereopoly.api;
using stereopoly.cache;
using stereopoly.resx;

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
          this.IndexAvailableLanguages();
          await ((App)App.Current).SetMainPage();
        });
        return false;
      });
    }

    private void IndexAvailableLanguages()
    {
      ResourceManager rm;
      CultureInfo[] cultures;
      ResourceSet rs;

      Storage.ClearAvailableLanguages(local: true);

      rm = new ResourceManager(typeof(AppResources));

      cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

      foreach (CultureInfo culture in cultures)
      {
        try
        {
          if (culture.Equals(CultureInfo.InvariantCulture)) continue; //do not use "==", won't work

          rs = rm.GetResourceSet(culture, true, false);
          if (rs != null)
            Storage.UpdateAvailableLanguage(new Language{
              Name = culture.EnglishName,
              Code = culture.TwoLetterISOLanguageName,
              Local = true
            }, local: true);
        }
        catch (CultureNotFoundException)
        {
          //NOP
        }
      }

      Storage.UpdateAvailableLanguage(new Language{
        Name = "English",
        Code = "en",
        Local = true
      }, local: true);

      if(Storage.GetCurrentLanguage().Local == false)
      {
        Storage.SetCurrentLanguage(new Language{
          Name = "English",
          Code = "en",
          Local = true
        });
        ((App)App.Current).UpdateAppLanguage();
      }
    }
  }
}