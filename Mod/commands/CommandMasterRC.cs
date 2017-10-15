namespace Mod.commands
{
    [Command("isrc")]
    public class CommandMasterRC 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            Core.SendMessage($"{PhotonNetwork.masterClient.HexName} {(FengGameManagerMKII.masterRC ? "ha l'rc." : "non ha l'rc")}");
        }
    }
}
