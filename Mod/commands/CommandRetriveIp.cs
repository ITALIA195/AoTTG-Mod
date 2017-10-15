using Mod.exceptions;

namespace Mod.commands
{
    [Command("getip;ip")]
    public class CommandRetriveIp  
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            PhotonPlayer player = PhotonPlayer.Find(args[0].ToInt());
            if (player == null)
                throw new PlayerNotFoundException();

            //Core.SendMessage($"{player.HexName} ha l'ip. {Core.PlayersIp[player.ID]}");
            Core.SendMessage("[IPSTEALER] Al momento non e' possibile eseguire la sua richiesta.");
        }
    }
}
