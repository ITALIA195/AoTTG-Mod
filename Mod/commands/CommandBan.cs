using Mod.exceptions;

namespace Mod.commands
{
    [Command("ban")]
    public class CommandBan 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("/ban [list/id]");
            if (args[0].EqualsIgnoreCase("list"))
            {
                Core.SendMessage("Banned players:");
                foreach (int id in FengGameManagerMKII.banHash.Keys)
                    Core.SendMessage($"[{id}] {FengGameManagerMKII.banHash[id]}");
            }
            else
            {
                if (args[0].ToInt().Equals(sender.ID))
                    Core.SendMessage("Cannot ban urself.");
                else
                {
                    PhotonPlayer player = PhotonPlayer.Find(args[0].ToInt());
                    if (player == null)
                        throw new PlayerNotFoundException();
                    FengGameManagerMKII.instance.kickPlayerRC(player, true, string.Empty);
                    Core.SendMessage("Il player è stato bannato");
                }
            }
        }
    }
}
