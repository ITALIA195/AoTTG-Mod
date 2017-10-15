using System.Text.RegularExpressions;
using Mod.exceptions;
using Mod.gui;
using UnityEngine;

namespace Mod.commands
{
    [Command("kill")]
    public class CommandKill 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("/kill [id]");
            string msg = Regex.Match(GUIChat.Message, @"[\\\/]\w+\s(?:\d+|\w+)\s(.*)").Groups[1].Value;
            if (string.IsNullOrEmpty(msg))
                msg = RefStrings.PlayerName;

            if (args[0].EqualsIgnoreCase("all"))
            {
                foreach (var obj in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (obj.GetComponent<HERO>() != null)
                    {
                        obj.GetComponent<HERO>().markDie();
                        obj.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, -1, "[FF0000]" + msg + "[-]  ");
                    }
                }
                return;
            }

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (obj.GetComponent<HERO>() != null && obj.GetComponent<HERO>().photonView.owner.ID == args[0].ToInt())
                {
                    obj.GetComponent<HERO>().markDie();
                    obj.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, -1, "[FF0000]" + msg + "[-]  ");
                }
            }
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("titan"))
            {
                if (obj.GetComponent<TITAN>() != null && obj.GetComponent<TITAN>().photonView.owner.ID == args[0].ToInt())
                {
                    obj.GetComponent<TITAN>().photonView.RPC("netDie", PhotonTargets.All);
                }
            }

        }
    }
}
