using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using stereopoly.api;
using stereopoly.cache;
using stereopoly.resx;

namespace stereopoly
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class LanguageSelectionPage : ContentPage
  {
    private Dictionary<Button, Language> ButtonLanguageMapping;

    public LanguageSelectionPage ()
    {
      InitializeComponent ();
      this.ButtonLanguageMapping = new Dictionary<Button, Language>();
    }

    protected override async void OnAppearing()
    {
      NavigationPage.SetHasBackButton(this, false);
      await this.UpdateLanguages();
    }

    async void OnRefresh(object sender, EventArgs e)
    {
      this.BoardLanguageLayout.Children.Clear();
      await this.UpdateLanguages();
    }

    private async Task UpdateLanguages()
    {
      Button b;
      int i;
      Language l;
      List<Language> languages;

      String text;
      this.DownloadingIndicator.IsRunning = true;
      this.RefreshButton.IsEnabled = false;
      try
      {
        languages = await Caller.RequestBoardLanguages();
      }
      catch (Exception ex) when (
        ex is WebException ||
        ex is HttpRequestException
      )
      {
        languages = null;
        await DisplayAlert("Error", "An error occurred while retrieving the list of known board languages: " + ex.Message, "OK");
      }
      finally
      {
        this.DownloadingIndicator.IsRunning=false;
        this.RefreshButton.IsEnabled = true;
      }

      if(languages == null)
        return;

      this.ButtonLanguageMapping.Clear();

      for(i=0; i < languages.Count; i++)
      {
        l = languages[i];
        l.Remote = true;
        Storage.UpdateAvailableLanguage(l, local: false);
      }

      languages = Storage.GetAvailableLanguages();

      for(i=0; i < languages.Count; i++)
      {
        l = languages[i];
        text = l.Name;
        
        if(l.Remote == false)
          text += " (" + AppResources.OnlyLocalLanguageText + ")";
        else if(l.Local == false)
          text += " (" + AppResources.OnlyRemoteLanguageText + ")";

        b = new Button {
          Text = text,
        };
        b.Clicked += async (s,e) => {
          await this.OnLanguageSelect(this.ButtonLanguageMapping[(Button)s]);
        };
        this.BoardLanguageLayout.Children.Add(b);
        this.ButtonLanguageMapping.Add(b, l);
      }
    }

    public async Task OnLanguageSelect(Language l)
    {
      Storage.SetCurrentLanguage(l);
      ((App)App.Current).UpdateAppLanguage();
      await Navigation.PopAsync();
    }

    public async void OnCancel(object sender, EventArgs e)
    {
      Caller.Cancel();
      await Navigation.PopAsync();
    }
  }
}