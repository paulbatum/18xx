using System;
using Xamarin.Forms;

namespace EighteenSeventeen.UI
{
	public class TitlePage : ContentPage
	{
		public TitlePage () 
		{		
			NavigationPage.SetHasNavigationBar (this, false);
			Content = new StackLayout {
				Children = {
					new Label {
						Text = "1817",
						VerticalOptions = LayoutOptions.CenterAndExpand,
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						Font = Font.SystemFontOfSize (20, FontAttributes.Bold),
					},
					new Button {
						BorderWidth = 1,
						BorderRadius = 20,
						WidthRequest = 200,
						Text = "New Game",
						VerticalOptions = LayoutOptions.CenterAndExpand,
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						Command = new Command(async () => { await this.Navigation.PushAsync(new NewGamePage()); })
					}
				}
			};				
		}
	}
}

