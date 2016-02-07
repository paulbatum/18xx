namespace _1817.Core
{
    public abstract class PrivateCompany
    {
        protected PrivateCompany(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public int Value { get; private set; }
    }

    public class CoalPrivate : PrivateCompany
    {
        public CoalPrivate(string name, int value, int mines) : base(name, value)
        {
            Mines = mines;
        }

        public int Mines { get; private set; }
    }

    public class BridgePrivate : PrivateCompany
    {
        public BridgePrivate(string name, int value, int bridges) : base(name, value)
        {
            Bridges = bridges;
        }

        public int Bridges { get; private set; }
    }

    public class MailContractPrivate : PrivateCompany
    {
        public MailContractPrivate(string name, int value, int payout) : base(name, value)
        {
            Payout = payout;
        }

        public int Payout { get; private set; }
    }

    public class PittsburghSteelMillPrivate : PrivateCompany
    {
        public PittsburghSteelMillPrivate()
            : base("Pittsburgh Steel Mill", 40)
        {
        }
    }

    public class MountainEngineersPrivate : PrivateCompany
    {
        public MountainEngineersPrivate() : base("Mountain Engineers", 40)
        {
        }
    }

    public class TrainStationPrivate : PrivateCompany
    {
        public TrainStationPrivate() : base("Train Station", 80)
        {
        }
    }
}