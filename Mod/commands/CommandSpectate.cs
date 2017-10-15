using UnityEngine;

namespace Mod.commands
{
    [Command("spectate")]
    public class CommandSpectate
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (obj.GetPhotonView().owner.ID != args[0].ToInt()) continue;
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(obj);
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
            }
        }
    }
}
