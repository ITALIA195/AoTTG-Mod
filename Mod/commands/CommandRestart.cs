namespace Mod.commands
{
    [Command("restart")]
    public class CommandRestart 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            FengGameManagerMKII.instance.RestartRC();
            Core.SendMessage("Hai riavviato la stanza.");
        }
    }
}
