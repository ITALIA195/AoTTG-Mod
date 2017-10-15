using ExitGames.Client.Photon;
using Mod.exceptions;

namespace Mod.commands
{
    [Command("resetkd;kd;rkd")]
    public class CommandResetkd 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            Hashtable hashtable = new Hashtable
            {
                {PhotonPlayerProperty.kills, 0},
                {PhotonPlayerProperty.deaths, 0},
                {PhotonPlayerProperty.max_dmg, 0},
                {PhotonPlayerProperty.total_dmg, 0}
            };

            if (args.Length > 0 && args[0].EqualsIgnoreCase("all"))
            {
                foreach (PhotonPlayer p in PhotonNetwork.playerList)
                    p.SetCustomProperties(hashtable);
                Core.SendMessage("Hai resettato gli stats di tutti.");
            }
            else
            {
                if (args.Length < 1)
                {
                    sender.SetCustomProperties(hashtable);
                    Core.SendMessage("Ti sei resettato gli stats.");
                }
                else
                {
                    PhotonPlayer player = PhotonPlayer.Find(args[0].ToInt());
                    if (player == null)
                        throw new PlayerNotFoundException();

                    player.SetCustomProperties(hashtable);
                    Core.SendMessage($"Hai resettato gli stats di {player.HexName}");
                }
            }
        }
    }
}
