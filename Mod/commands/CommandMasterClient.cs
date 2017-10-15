namespace Mod.commands
{
    [Command("mc;getmc;setmc")]
    public class CommandMasterClient 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            PhotonPlayer player = PhotonPlayer.Find(args[0].ToInt()) ?? sender;
            PhotonNetwork.SetMasterClient(player);
            Core.SendMessage($"{player.HexName} e' diventato MasterClient.");
        }
    }
}
