namespace Fps.UI
{
    public struct WinnersTablePlayerIdentifier
    {
        public int Pontuation { get; set; }
        public string Name { get; set; }

        public WinnersTablePlayerIdentifier(int pontuation, string name)
        {
            Pontuation = pontuation;
            Name = name;
        }
    }
}