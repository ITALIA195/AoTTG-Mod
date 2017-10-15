using Mod.manager;
using UnityEngine;

namespace Mod.mods
{
    [Module("advancedlog")]
    public class ModAdvancedLog
    {
        public void Start()
        {
            if (ModManager.Find("module.advancedlog").Enabled)
                OnEnable();
        }

        public void OnEnable() 
        {
            PhotonNetwork.logLevel = PhotonLogLevel.Full;
        }

        public void OnDisable()
        {
            PhotonNetwork.logLevel = PhotonLogLevel.ErrorsOnly;
        }
    }
}
