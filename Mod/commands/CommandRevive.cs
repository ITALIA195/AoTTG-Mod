using Mod.exceptions;

namespace Mod.commands
{
    [Command("revive")]
    public class CommandRevive 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            if (args.Length < 1)
            {
                FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", sender);
                Core.SendMessage("Ti sei re-spawnato.");
            }
            else if (args[0].EqualsIgnoreCase("all"))
            {
                foreach (PhotonPlayer player in PhotonNetwork.playerList)
                    FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", player);
                Core.SendMessage("I player sono stati resuscitati.");
            }
            else
            {
                FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", PhotonPlayer.Find(args[0].ToInt()));
                Core.SendMessage("Il player e' stato resuscitato.");
            }
        }
    }
}


