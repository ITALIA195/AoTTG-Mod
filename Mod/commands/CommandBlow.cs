using System.Reflection;
using Mod.exceptions;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Mod.commands
{
    [Command("blow")]
    public class CommandBlow 
    {
        [Obfuscation]
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("/blow [id] {x} {y} {z}");
            Vector3 vector = new Vector3(args[1] != string.Empty ? args[1].ToInt() : 0, args[2] != string.Empty ? args[2].ToInt() : 100, args[3] != string.Empty ? args[3].ToInt() : 0);
            var player = PhotonPlayer.Find(args[0].ToInt());
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (obj != null && obj.GetComponent<HERO>() != null)
                    if (args[0].EqualsIgnoreCase("all"))
                    {
                        obj.GetComponent<HERO>().photonView.RPC("blowAway", PhotonTargets.All, vector);
                    }
                    else
                    {
                        if (player == null)
                            throw new PlayerNotFoundException();
                        if (Equals(obj.GetComponent<HERO>().photonView.owner.ID, args[0].ToInt()))
                            obj.GetComponent<HERO>().photonView.RPC("blowAway", PhotonTargets.All, vector);
                    }
            }
            Core.SendMessage($"{player.HexName} e' stato mandato nello spazio.");
        }
    }
}
