namespace Fps.UI
{
    public class WinnersTable
    {
        public const int TotalAmountPlayers = 5;

        private static WinnersTablePlayerIdentifier[] winners = new WinnersTablePlayerIdentifier[TotalAmountPlayers];

        public static WinnersTablePlayerIdentifier[] Winners
        {
            get
            {
                return winners;
            }
            set
            {
                winners = value;
            }
        }

        public static WinnersTablePlayerIdentifier CurrentPlayer { get; set; }

        public static void RegisterCurrentPlayer(int pontuation, string name)
        {
            CurrentPlayer = new WinnersTablePlayerIdentifier(pontuation, name);
        }

        public static void UpdateCurrentPlayerPontuation(int pontuation)
        {
            CurrentPlayer = new WinnersTablePlayerIdentifier(pontuation, CurrentPlayer.Name);
        }
    }
}