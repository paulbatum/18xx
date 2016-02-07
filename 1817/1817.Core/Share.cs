namespace _1817.Core
{
    public class Share
    {
        public Company Company { get; private set; }
        public ShareType Type { get; private set; }
    }

    public enum ShareType
    {
        Normal,
        Presidents,
        Short
    }
}