namespace _1817.Core
{
    public class Wallet
    {
        public Wallet(int startingMoney)
        {
            Money = startingMoney;
        }

        public int Money { get; private set; }
    }
}