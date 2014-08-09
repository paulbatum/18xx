using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace EighteenSeventeen.UI
{
	public class App
	{
		public static Page GetMainPage ()
		{	
			var root = new NavigationPage (new TitlePage ());
			return root;
		}
	}
}

