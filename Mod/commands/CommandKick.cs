using Mod.exceptions;

namespace Mod.commands
{
    [Command("kick")]
    public class CommandKick 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("/kick [list/id]");
            if (args[0].ToInt().Equals(PhotonNetwork.player.ID))
                Core.SendMessage("Cannot kick urself.");
            else
            {
                PhotonPlayer player = PhotonPlayer.Find(args[0].ToInt());
                if (player == null)
                    throw new PlayerNotFoundException();
                FengGameManagerMKII.instance.kickPlayerRC(player, false, string.Empty);
                Core.SendMessage("Il player è stato kickato");
            }
        }
    }
}
