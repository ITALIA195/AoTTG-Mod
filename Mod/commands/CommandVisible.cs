using System;
using ExitGames.Client.Photon;

namespace Mod.commands
{
    [Command("visible")]
    public class CommandVisible 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            try
            {
                PhotonNetwork.RaiseEvent(230,
                    new Hashtable {{222, new Hashtable {{PhotonNetwork.room.name, new Hashtable {{254, false}}}}}}, true,
                    new RaiseEventOptions {TargetActors = new[] {args[0].ToInt()}});
            }
            catch (Exception e)
            {
                Core.SendPublicMessage(e);
            }
            Core.SendMessage(PhotonNetwork.room.visible);
        }
    }
}
