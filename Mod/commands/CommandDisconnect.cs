using System.Threading;
using Mod.exceptions;

namespace Mod.commands
{
    [Command("dc")]
    public class CommandDisconnect 
    {
#if DEBUG
        private DCInstance _instance;
        
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            if (_instance != null && args.Length < 1)
            {
                _instance.Abort();
                _instance = null;
                Core.SendMessage("Il dc e' stato disattivato.");
            }
            else if (_instance != null)
            {
                Core.SendMessage("Un dc e' già attivo!");
                Core.SendMessage("Usa /dc per annullarlo.");
            }
            else if (_instance == null && args.Length < 1)
                throw new ArgumentException("/dc [id] {dcType = 1}");
            else if (_instance == null)
            {
                int dcType = args.Length >= 2 ? args[1].ToInt() : 1;
                PhotonPlayer player = PhotonPlayer.Find(args[0].ToInt());
                if (player == null)
                    throw new PlayerNotFoundException();

                _instance = new DCInstance(player, dcType);
                _instance.Start();
                if (_instance.State != ThreadState.Running && _instance.State != ThreadState.WaitSleepJoin)
                {
                    Core.SendMessage($"Errore nel Thread, ThreadState: {_instance.State}");
                    _instance.Abort();
                    _instance = null;
                    return;
                }
                Core.SendMessage($"Il dc {dcType} e' stato attivato su {_instance.Target.HexName}.");
            }
            else
            {
                Core.SendMessage("Unexpected error @ " + typeof(CommandDamage).FullName);
            }
        }
#else
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            Core.SendMessage("Non sei autorizzato ad usare questo comando.");
        }
#endif
    }
}
