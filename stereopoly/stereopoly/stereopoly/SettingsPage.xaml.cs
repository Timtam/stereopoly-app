using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using stereopoly.cache;

namespace stereopoly
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class SettingsPage : ContentPage
  {
    public SettingsPage ()
    {
      InitializeComponent ();
    }

    void OnClearCache(object sender, EventArgs e)
    {
      Storage.ClearCache();
      this.UpdateClearCacheButton();
    }

    void UpdateClearCacheButton()
    {
      if(Storage.GetAllBoards().Count == 0)
        this.ClearCacheButton.IsEnabled = false;
      else
        this.ClearCacheButton.IsEnabled = true;
    }

    protected override void OnAppearing()
    {
      this.UpdateClearCacheButton();
      this.UpdateChangeBoardLanguageButton();
    }

    public void UpdateChangeBoardLanguageButton()
    {
      this.ChangeBoardLanguageButton.Text = "Board language: " + Storage.GetBoardLanguage().Name;
    }

    public async void OnChangeBoardLanguage(object sender, EventArgs e)
    {
      await Navigation.PushAsync(new BoardLanguagePage());
    }
  }
}