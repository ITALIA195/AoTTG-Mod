using Mod.gui;

namespace Mod.commands 
{
    [Command("cc;clear;clearchat")]
    public class CommandClear 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            for (int i = 0; i < 30; i++)
                Core.SendPublicMessage(string.Empty);
            if (args[0].EqualsIgnoreCase("true") || args[0].EqualsIgnoreCase("1"))
                GUIChat.Messages.Clear();
            Core.SendPublicMessage($"<color=#{RefStrings.MessageColor}>Chat has been cleaned up by {sender.HexName}</color>");
        }
    }
}
