using System.Collections.Generic;
using ExitGames.Client.Photon;
using Mod.exceptions;

namespace Mod.commands
{
    [Command("roast")]
    public class CommandRoast 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            if (args.Length < 1) return;
            PhotonPlayer target = PhotonPlayer.Find(args[0].ToInt());
            if (target == null) throw new PlayerNotFoundException(); 
            PhotonNetwork.RaiseEvent(228, new Hashtable { { 223, 0 }}, true, new RaiseEventOptions {TargetActors = new[] {args[0].ToInt()}});
            Core.SendMessage($"{target.HexName} e' stato roastato.");
        }
    }
}
