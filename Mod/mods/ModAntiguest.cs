using System;
using UnityEngine;

namespace Mod.mods
{
    [Module("antiguest")]
    public class ModAntiguest
    {
        public void OnEnable()
        {
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
                if (player.Name.StartsWith("GUEST") || player.Name.ContainsIgnoreCase("vivid-assassin") || player.Name.ContainsIgnoreCase("hyper-megacannon") || player.Name.ContainsIgnoreCase("tokyo ghoul"))
                    FengGameManagerMKII.instance.photonView.RPC("showResult", player, "", "", "", "", "", "[FF0000]Kicked by antiguest");
        }
    }
}
