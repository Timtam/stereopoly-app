using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace stereopoly
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class CreditsPage : ContentPage
  {
    public CreditsPage ()
    {
      InitializeComponent ();
    }

    protected override void OnAppearing()
    {
      AppVersionLabel.Text = "App Version " + Constants.APP_VERSION;
      BuildVersionLabel.Text = "Build Version " + AppInfo.VersionString;
    }

    void OnContact(object sender, EventArgs e)
    {
      Device.OpenUri(new Uri("Mailto:software@satoprogs.de"));
    }
  }
}