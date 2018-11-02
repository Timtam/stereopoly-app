using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace stereopoly
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GameBoard : ContentPage
	{
		public GameBoard ()
		{
			InitializeComponent ();
		}
	}
}