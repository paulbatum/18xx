using System.Collections.Generic;
using System.Collections.Immutable;

namespace EighteenSeventeen.Core.DataTypes
{
    public static class Companies
    {
        public static readonly Company Alton = new Company("Alton and Southern Railway", "ALT");
        public static readonly Company Arcade = new Company("Arcade and Attica Railroad", "AA");
        public static readonly Company Belt = new Company("Belt Railway of Chicago ", "BELT");

        public static readonly ImmutableList<Company> All = new List<Company>
        {
            Alton,
            Arcade,
            Belt
        }.ToImmutableList();
    }

    public class Company
    {
        public string Name { get; }
        public string Abbreviation { get; }

        public Company(string name, string abbreviation)
        {
            Name = name;
            Abbreviation = abbreviation;
        }
    }

    public class CompanyState
    {
        public Company Company { get; }
        public int Money { get; }
        public int Loans { get; }
        public ImmutableList<Train> Trains { get; }

        public CompanyState(Company company, int money, int loans, ImmutableList<Train> trains)
        {
            Company = company;
            Money = money;
            Loans = loans;
            Trains = trains;
        }

        //public CompanyState AddCash(int amount) => new CompanyState(Company, Money + amount, Loans, Trains);
        //public CompanyState SubtractCash(int amount) => new CompanyState(Company, Money - amount, Loans, Trains);
        //public CompanyState AddTrain(Train train) => new CompanyState(Company, Money, Loans, Trains.Add(train));
    }
}
