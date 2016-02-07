using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace EighteenSeventeen.Core
{
    public class Company
    {
        public CompanyIdentity Identity { get; }

        public Company(CompanyIdentity id)
        {
            Identity = id;
        }

    }

    public enum CompanyIdentity
    {
        Arcade,
        Belt,
        Shawmut
    }

    public class CompanyState
    {
        public int Money { get; }
        public int Loans { get; }
        public ImmutableList<Train> Trains { get; }

        public CompanyState(int money, int loans, ImmutableList<Train> trains)
        {
            Money = money;
            Loans = loans;
            Trains = trains;
        }

        public CompanyState AddCash(int amount) => new CompanyState(Money + amount, Loans, Trains);
        public CompanyState SubtractCash(int amount) => new CompanyState(Money - amount, Loans, Trains);

        public CompanyState AddTrain(Train train) => new CompanyState(Money, Loans, Trains.Add(train));
    }
}
