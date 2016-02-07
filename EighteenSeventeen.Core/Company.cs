using System;
using System.Collections.Generic;
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
        public List<Train> Trains { get; }
    }
}
