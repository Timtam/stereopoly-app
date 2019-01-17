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

namespace stereopoly
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class BoardLanguagePage : ContentPage
  {
    private Dictionary<Button, Language> ButtonLanguageMapping;

    public BoardLanguagePage ()
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
        b = new Button {
          Text = l.Name,
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
      await Navigation.PopAsync();
    }

    public async void OnCancel(object sender, EventArgs e)
    {
      Caller.Cancel();
      await Navigation.PopAsync();
    }
  }
}