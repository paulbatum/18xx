using EighteenSeventeen.Core;
using EighteenSeventeen.Core.Actions;
using EighteenSeventeen.Core.DataTypes;
using EighteenSeventeen.Core.Rounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Cmd
{
    class Program
    {
        static Game Game;
        static IGameAction LastAction;

        static void Main(string[] args)
        {
            Game = new Game("Paul", "Stephen", "Jacky", "Chris");

            try
            {
                GameLoop();
            }
            catch(Exception e)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;                
                Console.WriteLine("----------------KABOOM!!----------------");
                Console.WriteLine();
                Console.WriteLine("Paul has failed you, here are the gory details:");
                Console.WriteLine();
                Console.WriteLine(e.ToString());

                Console.WriteLine();
                Console.WriteLine("Press any key to get out of here.");
                Console.ReadKey();
            }
        }

        private static void GameLoop()
        {
            while (true)
            {
                var validator = new GameActionValidator();
                var state = Game.GetLastValidState(LastAction, validator);

                if (validator.IsValid)
                {
                    var pendingAction = Game.GetPendingAction(state);
                    PrintState(state, pendingAction);
                    LastAction = PromptUser(pendingAction);
                }
                else
                {
                    PrintError(validator);
                    LastAction = LastAction.Parent;
                }
            }
        }

        private static IGameAction PromptUser(PendingAction pendingAction)
        {
            int optionIndex = -1;

            while (true)
            {
                Console.WriteLine();
                Console.Write($"{pendingAction.ActivePlayer.Name}>");

                var input = Console.ReadLine();

                if(pendingAction.Choices.OfType<PassChoice>().Any() && input.ToLower() == "pass")
                    return new PlayerPassAction(LastAction, pendingAction.ActivePlayer);

                if (input.ToLower() == "undo" && LastAction != null)
                    return LastAction.Parent;

                if (int.TryParse(input, out optionIndex) && optionIndex >= 0 && optionIndex < pendingAction.Choices.Count)
                {
                    var choice = pendingAction.Choices[optionIndex];
                    Console.WriteLine("You picked: " + choice.Description);
                    return GetActionForChoice(pendingAction.ActivePlayer, choice);                    
                }
                else
                {
                    Console.WriteLine("Unexpected input!");
                }
            }
        }

        private static IGameAction GetActionForChoice(Player activePlayer, IChoice choice)
        {
            if (choice is PassChoice)
                return new PlayerPassAction(LastAction, activePlayer);

            if (choice is BidChoice<PrivateCompany>)
                return GetActionForChoice(activePlayer, choice as BidChoice<PrivateCompany>);

            throw new Exception($"Choice of type '{choice.GetType()}' not recognized.");
        }

        private static PlayerPrivateBidAction GetActionForChoice(Player activePlayer, BidChoice<PrivateCompany> choice)
        {
            int price = -1;

            while (true)
            {
                Console.Write("Enter price:");
                string priceInput = Console.ReadLine();

                if (int.TryParse(priceInput, out price))
                {
                    return new PlayerPrivateBidAction(LastAction, activePlayer, choice.Target, price);
                }
                else
                {
                    Console.WriteLine("Unexpected input!");
                }
            }
        }

        private static void PrintError(GameActionValidator validator)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(validator.GetSummary());
            Console.ResetColor();
            Console.ReadLine();
        }

        private static void PrintState(GameState state, PendingAction pendingAction)
        {
            Console.Clear();

            Console.WriteLine($"Current Round: {state.Round.Description}");

            Console.WriteLine();
            Console.WriteLine("Players:");
            foreach (var p in state.Game.Players)
            {
                if (p == pendingAction.ActivePlayer)
                    Console.ForegroundColor = ConsoleColor.Yellow;

                var playerState = state.GetPlayerState(p);
                var privates = string.Join(", ", playerState.PrivateCompanies.Select(x => x.Name));
                Console.WriteLine($"{p.Name}\t- Cash: {playerState.Money}\t Privates: {privates} ");

                Console.ResetColor();
            }

            var privateAuctionRound = state.Round as PrivateAuctionRound;

            if(privateAuctionRound != null)
            {
                Console.WriteLine();
                Console.WriteLine($"Seed money: {privateAuctionRound.SeedMoney}");

                var currentAuction = (state.Round as PrivateAuctionRound)?.CurrentAuction;
                if (currentAuction != null)
                {                    
                    Console.WriteLine($"Current Auction: {currentAuction.Selection.Name}");
                    Console.WriteLine($"High bid: {currentAuction.HighBid} ({currentAuction.HighBidder.Name})");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Select an action - pass, undo, or pick a number:");            
            foreach (var c in pendingAction.Choices)
            {
                Console.WriteLine($"{pendingAction.Choices.IndexOf(c)}. {c.Description}");
            }

        }
    }
}
