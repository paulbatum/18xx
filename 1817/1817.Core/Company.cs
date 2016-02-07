using System.Collections.Generic;

namespace _1817.Core
{
    public class Company
    {
        public string Name { get; private set; }
        public int LoanCount { get; private set; }
        public int TokenCount { get; private set; }
        public Wallet Wallet { get; private set; }
        public CompanySize Size { get; private set; }
        public List<Train> Trains { get; private set; }
        public List<Share> Shares { get; private set; }
        public List<PrivateCompany> Privates { get; private set; }
    }

    public enum CompanySize
    {
        TwoShare = 2,
        FiveShare = 5,
        TenShare = 10
    }
}