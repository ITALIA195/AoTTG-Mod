using Mod.exceptions;

namespace Mod.commands
{
    [Command("close")]
    public class CommandCloseConnection 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException();
            PhotonPlayer player = PhotonPlayer.Find(args[0].ToInt());
            if (player == null)
                throw new PlayerNotFoundException();
            if (PhotonNetwork.isMasterClient)
                PhotonNetwork.RaiseEvent(203, null, true, new RaiseEventOptions { TargetActors = new[] { player.ID } });
            PhotonNetwork.DestroyPlayerObjects(player);
            FengGameManagerMKII.instance.photonView.RPC("showResult", player, "", "", "", "", "", "Kicked by " + RefStrings.PlayerName);
            Core.SendMessage("Player has been kicked!");
        }
    }
}
