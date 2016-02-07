using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _1817.Core
{
    public class Game
    {
        public const int TotalStartingMoney = 1260;

        public Game(List<string> playerNames)
        {
            var startingMoney = TotalStartingMoney / playerNames.Count;
            Players = playerNames.Select(name => new Player(name, startingMoney)).ToList();
            
            OpeningAuction = new OpeningAuction();
        }

        public OpeningAuction OpeningAuction { get; private set; }
        public Market Market { get; private set; }
        public StockChart StockChart { get; private set; }
        public List<Company> Companies { get; private set; }
        public List<Player> Players { get; private set; }
    }


    public class Market
    {
        public int LoanCount { get; private set; }
        public Wallet Bank { get; private set; }
        public List<Train> Trains { get; private set; }
    }

    public class StockChart
    {

    }

    public class OpeningAuction
    {
        public const int StartingSeedMoney = 200;
        public List<PrivateCompany> Privates { get; private set; }
        public Wallet SeedMoney { get; private set; }

        public OpeningAuction()
        {
            SeedMoney = new Wallet(StartingSeedMoney);
            Privates = new List<PrivateCompany>();

            Privates.Add(new MailContractPrivate("Minor Mail Contract", value: 60, payout: 10));
            Privates.Add(new MailContractPrivate("Mail Contract", value: 90, payout: 20));
            Privates.Add(new MailContractPrivate("Major Mail Contract", value: 120, payout: 30));

            Privates.Add(new CoalPrivate("Minor Coal Mine", value: 30, mines: 1));
            Privates.Add(new CoalPrivate("Coal Mine", value: 60, mines: 2));
            Privates.Add(new CoalPrivate("Major Coal Mine", value: 90, mines: 3));

            Privates.Add(new BridgePrivate("Ohio Bridge Co.", value: 40, bridges: 1));
            Privates.Add(new BridgePrivate("Union Bridge Co.", value: 80, bridges: 2));

            Privates.Add(new MountainEngineersPrivate());
            Privates.Add(new PittsburghSteelMillPrivate());
            Privates.Add(new TrainStationPrivate());
        }
    }

}
