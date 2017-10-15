using System.Text.RegularExpressions;
using Mod.exceptions;
using Mod.gui;
using Mod.mods;

namespace Mod.commands
{
    [Command("name")]
    public class CommandName 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            Match match = Regex.Match(GUIChat.Message, @"\S+\s(.*)");
            if (!match.Success)
                throw new ArgumentException("/name [name]");
            Core.Profile.PlayerName = match.Groups[1].Value;
            ModNameAnimation._animation.SetName(RefStrings.PlayerName);
            PhotonNetwork.player.SetName(RefStrings.PlayerName);
        }
    }
}
