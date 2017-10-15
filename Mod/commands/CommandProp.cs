using System.Collections.Generic;
using System.Linq;
using Mod.exceptions;

namespace Mod.commands
{
    [Command("prop")]
    public class CommandProp 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("/prop [id]");
            PhotonPlayer player = PhotonPlayer.Find(args[0].ToInt());
            if (player == null)
                throw new PlayerNotFoundException();
            var list = player.customProperties.Keys.Where(prop => !PhotonNetwork.player.customProperties.Keys.Contains(prop)).Select(prop => prop.ToString()).ToList();
            foreach (var str in list)
                if (str != "sender")
                    Core.SendMessage(str);
        }
    }
}
