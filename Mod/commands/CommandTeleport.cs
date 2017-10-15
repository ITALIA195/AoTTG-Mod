using Mod.exceptions;
using UnityEngine;

namespace Mod.commands
{
    [Command("tp")]
    public class CommandTeleport 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            if (args.Length < 1)
                throw new exceptions.ArgumentException("/tp [id]");
            PhotonPlayer player = PhotonPlayer.Find(args[0].ToInt());
            if (player == null)
                throw new PlayerNotFoundException();
            if (player.isLocal)
                throw new TargetCantBeLocalException("Non puoi teletrasportarti da te stesso.");

            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (Equals(go.GetPhotonView().owner, player))
                {
                    IN_GAME_MAIN_CAMERA.instance.main_object.transform.position = go.transform.position;
                    IN_GAME_MAIN_CAMERA.instance.main_object.transform.rotation = go.transform.rotation;
                }
            }
            
        }
    }
}
