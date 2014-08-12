using System;
using System.Collections.Generic;
using Xamarin.Forms;
using ReactiveUI;
using System.Linq;
using System.Reactive.Linq;

namespace EighteenSeventeen.UI
{	
	public partial class NewGamePage : ContentPage, IViewFor<NewGameViewModel>
	{	
		public NewGameViewModel ViewModel { get; set; }

		object IViewFor.ViewModel {
			get { return ViewModel; }
			set { ViewModel = (NewGameViewModel)value; }
		}

		public NewGamePage ()
		{
			InitializeComponent ();
            
			this.ViewModel = new NewGameViewModel ();
            this.BindingContext = this.ViewModel;

            var unfocusOnTap = new TapGestureRecognizer { Command = new Command(this.playerNameEntry.Unfocus) };            
            this.Content.GestureRecognizers.Add(unfocusOnTap);            

            
            this.WhenAnyValue(x => x.playerListView.SelectedItem).Subscribe(obj =>
            {
                this.playerNameEntry.Unfocus();                
            });
          
            this.WhenAnyObservable(x => x.ViewModel.AddPlayer).Subscribe (_ => {
				this.ViewModel.Players.Add(this.ViewModel.NewPlayerName.Trim());
				this.ViewModel.NewPlayerName = string.Empty;
                this.playerNameEntry.Focus();
			});

            this.WhenAnyObservable(x => x.ViewModel.RandomizeOrder).Subscribe (_ => {
                RandomizeSeating();
            });

            this.WhenAnyObservable(x => x.ViewModel.RemovePlayer).Subscribe(player =>
            {
                this.ViewModel.Players.Remove((string)player);
            });            

			this.Bind (this.ViewModel, vm => vm.NewPlayerName, view => view.playerNameEntry.Text);
            
			this.BindCommand (this.ViewModel, vm => vm.AddPlayer, view => view.addPlayerButton);
			this.BindCommand (this.ViewModel, vm => vm.AddPlayer, view => view.playerNameEntry, "Completed");
			this.BindCommand (this.ViewModel, vm => vm.StartGame, view => view.startButton);
			this.BindCommand (this.ViewModel, vm => vm.RandomizeOrder, view => view.randomizeButton);
		}

        private void onDeleteClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;            
            this.ViewModel.RemovePlayer.Execute(button.BindingContext);                
        }

        private void RandomizeSeating()
        {
            using (this.ViewModel.Players.SuppressChangeNotifications())
            {
                var r = new Random();
                var newOrder = this.ViewModel.Players.OrderBy(x => r.NextDouble()).ToList();
                this.ViewModel.Players.Clear();
                this.ViewModel.Players.AddRange(newOrder);
            }
        }
	}
}

