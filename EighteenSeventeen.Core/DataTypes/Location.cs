using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core.DataTypes
{
    public static class Locations
    {
        public static readonly Location Boston = new Location("Boston");
        public static readonly Location SouthNewYork = new Location("South New York");
        public static readonly Location NorthNewYork = new Location("North New York");


    }

    public class Location
    {
        public string Name { get; }
        public override string ToString() => Name;

        public Location(string name)
        {
            Name = name;
        }
    }
}
