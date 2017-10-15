using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Mod.exceptions;
using Mod.gui;

namespace Mod.commands
{
    [Command("pm;msg")]
    public class CommandPrivateMessage 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            if (args.Length < 2)
                throw new ArgumentException("/pm [id] [msg]");
            PhotonPlayer player = PhotonPlayer.Find(args[0].ToInt());
            if (player == null)
                throw new PlayerNotFoundException();
            player.SendPrivateMessage(Regex.Match(GUIChat.Message, @"[\\\/][a-zA-Z]*[' '][0-9]*[' '](.*)").Groups[1].Value);
            FengGameManagerMKII.instance.reply = player;
        }
    }
}
