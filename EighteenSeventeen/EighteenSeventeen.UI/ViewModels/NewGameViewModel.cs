using System;
using ReactiveUI;
using System.Reactive.Linq;

namespace EighteenSeventeen.UI
{
	public class NewGameViewModel : ReactiveObject
	{
		public ReactiveList<string> Players { get; private set; }
		public ReactiveCommand<Object> AddPlayer { get; private set; }
        public ReactiveCommand<Object> RemovePlayer { get; private set; }
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

            var canStart = this.Players.CountChanged.Select(count => count >= 3);
            StartGame = canStart.ToCommand();
            RandomizeOrder = canStart.ToCommand();			
            
			AddPlayer = this.WhenAnyValue(x => x.Players.Count, x => x.NewPlayerName, 
				(count, newPlayerName) => count < 7 && !string.IsNullOrWhiteSpace(newPlayerName) && !this.Players.Contains(newPlayerName))
                .ToCommand();

            RemovePlayer = ReactiveCommand.Create();            
		}
	}
}

