using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core
{
    public static class PrivateCompanies
    {
        public static CoalPrivate MajorCoalMine { get; } = new CoalPrivate("Major Coal Mine", value: 90, mines: 3);
        public static CoalPrivate CoalMine { get; } = new CoalPrivate("Coal Mine", value: 60, mines: 2);
        public static CoalPrivate MinorCoalMine { get; } = new CoalPrivate("Minor Coal Mine", value: 30, mines: 1);

        public static MailContractPrivate MajorMailContract { get; } = new MailContractPrivate("Major Mail Contract", value: 120, payout: 30);
        public static MailContractPrivate MailContract { get; } = new MailContractPrivate("Mail Contract", value: 90, payout: 20);
        public static MailContractPrivate MinorMailContract { get; } = new MailContractPrivate("Minor Mail Contract", value: 60, payout: 10);

        public static BridgePrivate UnionBridge { get; } = new BridgePrivate("Union Bridge Co.", value: 80, bridges: 2);
        public static BridgePrivate OhioBridge { get; } = new BridgePrivate("Ohio Bridge Co.", value: 40, bridges: 1);

        public static MountainEngineersPrivate MountainEngineers { get; } = new MountainEngineersPrivate();
        public static PittsburghSteelMillPrivate PittsburghSteelMill { get; } = new PittsburghSteelMillPrivate();
        public static TrainStationPrivate TrainStation { get; } = new TrainStationPrivate();

        public static ImmutableList<PrivateCompany> All = new List<PrivateCompany>
        {
            MajorCoalMine, CoalMine, MinorCoalMine,
            MajorMailContract, MailContract, MinorMailContract,
            UnionBridge, OhioBridge,
            MountainEngineers, PittsburghSteelMill, TrainStation
        }.ToImmutableList();

    }

    public abstract class PrivateCompany
    {
        public string Name { get; }
        public int Value { get; }

        protected PrivateCompany(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }

    public class CoalPrivate : PrivateCompany
    {
        public int Mines { get; }

        public CoalPrivate(string name, int value, int mines) : base(name, value)
        {
            Mines = mines;
        }
    }

    public class BridgePrivate : PrivateCompany
    {
        public int Bridges { get; }

        public BridgePrivate(string name, int value, int bridges) : base(name, value)
        {
            Bridges = bridges;
        }
    }

    public class MailContractPrivate : PrivateCompany
    {
        public int Payout { get; }

        public MailContractPrivate(string name, int value, int payout) : base(name, value)
        {
            Payout = payout;
        }
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
