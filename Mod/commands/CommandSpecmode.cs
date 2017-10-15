using ExitGames.Client.Photon;

namespace Mod.commands
{
    [Command("specmode;s")]
    public class CommandSpecmode 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            var condition = (int)FengGameManagerMKII.settings[245] == 0;
            FengGameManagerMKII.settings[245] = condition ? 1 : 0;
            FengGameManagerMKII.instance.EnterSpecMode(condition);
            Core.SendMessage($"Sei {(condition ? "entrato" : "uscito")} dalla spectate mode.");
        }
    }
}
