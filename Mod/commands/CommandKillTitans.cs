using System.Text.RegularExpressions;
using Mod.gui;
using UnityEngine;
using ArgumentException = Mod.exceptions.ArgumentException;

namespace Mod.commands
{
    [Command("ktitans;ktitan")]
    public class CommandKillTitans
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("titan"))
            {
                if (obj.GetComponent<TITAN>() != null)
                {
                    obj.GetComponent<TITAN>().photonView.RPC("netDie", PhotonTargets.All);
                }
            }
        }
    }
}
