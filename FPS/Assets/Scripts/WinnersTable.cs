namespace Fps.UI
{
    public class WinnersTable
    {
        public const int TotalAmountPlayers = 5;

        public static WinnersTablePlayerIdentifier[] Winners { get; set; } = new WinnersTablePlayerIdentifier[TotalAmountPlayers];

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