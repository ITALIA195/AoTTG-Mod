using System.Runtime.Remoting.Messaging;
using Mod.exceptions;

namespace Mod.commands
{
    [Command("pause")]
    public class CommandPause 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("/pause [true/false]");
            FengGameManagerMKII.instance.photonView.RPC("pauseRPC", PhotonTargets.All, args[0].ToBool());
            Core.SendMessage("Hai cambiato lo stato del gioco.");
        }
    }
}
