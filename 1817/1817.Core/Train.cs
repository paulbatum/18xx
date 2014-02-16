namespace _1817.Core
{
    public class Train
    {
        public TrainType Type { get; private set; }
    }

    public enum TrainType
    {
        Two,
        TwoPlus,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight
    }
}