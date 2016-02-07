using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core
{
    public interface ICompanyAction
    {
        CompanyState Apply(CompanyState current);        
    }

    public class LayTrackAction : ICompanyAction
    {
        public bool IsDoubleLay { get; }

        public LayTrackAction(bool isDoubleLay)
        {
            IsDoubleLay = isDoubleLay;
        }

        public CompanyState Apply(CompanyState current)
        {
            if (IsDoubleLay)                
                return current.SubtractCash(20);
            else
                return current;
        }
    }

    public class BuyTrainAction : ICompanyAction
    {
        public Train Train { get; }        

        public BuyTrainAction(Train train)
        {
            Train = train;
        }

        public CompanyState Apply(CompanyState current)
        {
            return current
                .SubtractCash(Train.Cost)
                .AddTrain(Train);
        }
    }
}
