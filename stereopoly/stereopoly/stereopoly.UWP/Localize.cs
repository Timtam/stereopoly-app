using System.Diagnostics;
using System.Globalization;
using System.Resources;

using Xamarin.Forms;

[assembly:Dependency(typeof(stereopoly.UWP.Localize))]

namespace stereopoly.UWP
{
  public class Localize : stereopoly.ILocalize
  {
    public void SetLocale (CultureInfo ci)
    {
      //Windows.Globalization.ApplicationSettings.PrimaryLanguageOverride = ci.Name;
      Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "EN-US";
      Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
      Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();
      Debug.WriteLine("uwp-specific dependency service finished");
    }

    public CultureInfo GetCurrentCultureInfo ()
    {
      return null;
    }
  }
}