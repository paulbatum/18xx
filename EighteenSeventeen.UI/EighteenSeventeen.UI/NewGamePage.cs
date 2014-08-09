using System;
using System.Linq;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using ReactiveUI;

namespace EighteenSeventeen.UI
{
	public class NewGameViewModel : ReactiveObject
	{
		public ReactiveList<string> Players { get; private set; }

		public ReactiveCommand<Object> AddPlayer { get; private set; }
		public ReactiveCommand<Object> StartGame { get; private set; }

		string newPlayerName;
		public string NewPlayerName {
			get { return newPlayerName; }
			set { this.RaiseAndSetIfChanged(ref newPlayerName, value); }
		}

		public NewGameViewModel()
		{
			Players = new ReactiveList<string> ();
					
			var playerCount = this.WhenAnyValue (x => x.Players.Count);

			AddPlayer = ReactiveCommand.Create (this.WhenAnyValue(x => x.Players.Count, x => x.NewPlayerName, 
				(count, newPlayerName) => count < 7 && !string.IsNullOrEmpty(newPlayerName)));
			StartGame = ReactiveCommand.Create (playerCount.Select(count => count >= 3));
		}
	}

	public class NewGamePage : ContentPage, IViewFor<NewGameViewModel>
	{
		public Button addPlayerButton;
		public Button startButton;
		public Entry playerNameEntry;

		public NewGameViewModel ViewModel { get; set; }

		object IViewFor.ViewModel {
			get { return ViewModel; }
			set { ViewModel = (NewGameViewModel)value; }
		}

		public NewGamePage ()
		{
			this.ViewModel = new NewGameViewModel ();

			Title = "New Game";


			var playerListView = new ListView ();
			playerListView.ItemsSource = this.ViewModel.Players;
			//layout.Children.Add (playerListView);

			startButton = new Button 
			{ 
				Text = "Start", 
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.Start
			};					

			playerNameEntry = new Entry
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Placeholder = "Enter player name"
			};

			addPlayerButton = new Button {
				Text = "Add Player",
				HorizontalOptions = LayoutOptions.End
			};					

			var layout = new StackLayout {
				Children = 
				{ 
					new StackLayout 
					{ 
						Children = { playerNameEntry, addPlayerButton },
						Orientation = StackOrientation.Horizontal,
						Padding = new Thickness(10)
					}, 
					playerListView, 
					startButton 
				}
			};
			Content = layout;

			this.ViewModel.AddPlayer.Subscribe (_ => {
				this.ViewModel.Players.Add(this.ViewModel.NewPlayerName);
				this.ViewModel.NewPlayerName = string.Empty;
			});



			this.Bind (this.ViewModel, vm => vm.NewPlayerName, view => view.playerNameEntry.Text);
			this.BindCommand (this.ViewModel, vm => vm.AddPlayer, view => view.addPlayerButton);
			this.BindCommand (this.ViewModel, vm => vm.AddPlayer, view => view.playerNameEntry, "Completed");
			this.BindCommand (this.ViewModel, vm => vm.StartGame, view => view.startButton);
		}


	}
}

