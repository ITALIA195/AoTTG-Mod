using Mod.manager;
using UnityEngine;

namespace Mod.mods
{
    [Module("passthrough", true)]
    public class ModPassThrough
    {
        public void OnPlayerRespawn()
        {
            if (ModManager.Find("module.passthrough").Enabled)
                OnEnable();
        }

        public void OnEnable()
        {
            Core.GetHero(PhotonNetwork.player.ID).GetComponent<Rigidbody>().collider.enabled = false;
        }

        public void OnDisable()
        {
            Core.GetHero(PhotonNetwork.player.ID).GetComponent<Rigidbody>().collider.enabled = true;
        }
    }
}
