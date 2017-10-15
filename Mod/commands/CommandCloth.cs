namespace Mod.commands
{
    [Command("cloth")]
    public class CommandCloth 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            Core.SendMessage($"Active clothes: {ClothFactory.GetDebugInfo()}");
        }
    }
}
