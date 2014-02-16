using System.Collections.Generic;

namespace _1817.Core
{
    public class Player
    {
        public Player(string name, int startingMoney)
        {
            Name = name;
            Wallet = new Wallet(startingMoney);
        }

        public string Name { get; private set; }
        public bool HasPriority { get; private set; }
        public Wallet Wallet { get; private set; }
        public List<Share> Shares { get; private set; }
        public List<PrivateCompany> Privates { get; private set; }

        public int Money
        {
            get { return Wallet.Money; }
        }
    }
}