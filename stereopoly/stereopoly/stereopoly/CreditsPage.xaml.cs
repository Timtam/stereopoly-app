﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    void OnContact(object sender, EventArgs e)
    {
      Device.OpenUri(new Uri("Mailto:software@satoprogs.de"));
    }
  }
}