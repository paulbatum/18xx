using System;
using ReactiveUI;
using System.Reactive.Linq;

namespace EighteenSeventeen.UI
{
	public class NewGameViewModel : ReactiveObject
	{
		public ReactiveList<string> Players { get; private set; }
		public ReactiveCommand<Object> AddPlayer { get; private set; }
		public ReactiveCommand<Object> StartGame { get; private set; }
		public ReactiveCommand<Object> RandomizeOrder { get; private set; }

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
			RandomizeOrder = ReactiveCommand.Create (playerCount.Select(count => count >= 2));
		}
	}
}

