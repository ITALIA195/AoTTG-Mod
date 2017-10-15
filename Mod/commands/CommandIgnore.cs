using Mod.exceptions;

namespace Mod.commands
{
    [Command("ignore")]
    public class CommandIgnore 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("/ignore [list/add/rem] [id]");
            PhotonPlayer player = PhotonPlayer.Find(args[1].ToInt());
            if (player == null)
                throw new PlayerNotFoundException();
            switch (args[0].ToLower())
            {
                case "list":
                {
                    Core.SendMessage("Lista player ignorati:");
                    foreach (int id in FengGameManagerMKII.ignoreList)
                        Core.SendMessage(id);
                    break;
                }

                case "add":
                {
                    if (args.Length < 2)
                        throw new ArgumentException("/ignore [list/add/rem] [id]");
                    if (!FengGameManagerMKII.ignoreList.Contains(player.ID))
                        FengGameManagerMKII.ignoreList.Add(player.ID);
                    Core.SendMessage($"Hai ignorato {player.HexName}.");
                    break;
                }

                case "remove":
                case "rem":
                {
                    if (args.Length < 2)
                        throw new ArgumentException("/ignore [list/add/rem] [id]");
                    if (FengGameManagerMKII.ignoreList.Contains(player.ID))
                        FengGameManagerMKII.ignoreList.Remove(player.ID);
                    Core.SendMessage($"Hai un-ignorato {player.HexName}.");
                    break;
                }

                default:
                    throw new ArgumentException("/ignore [list/add/remove] [id]");
            }
        }
    }
}
