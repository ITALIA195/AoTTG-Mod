using System.Text.RegularExpressions;
using Mod.exceptions;
using Mod.gui;

namespace Mod.commands
{
    [Command("reply;r")]
    public class CommandReply 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("/reply [message]");
            if (FengGameManagerMKII.instance.reply == null)
                throw new PlayerNotFoundException();
            FengGameManagerMKII.instance.reply.SendPrivateMessage(Regex.Match(GUIChat.Message, @"[\\\/]\w*\s(.*)").Groups[1].Value);
        }
    }
}
