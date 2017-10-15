using System;
using System.Linq;
using UnityEngine;

namespace Mod.gui
{
    public class GUIScoreboard : Gui
    {
        private const string EntryLayout = "<color=#8BAFBCFF>[<b><color=#00FFFFAA>{0}</color></b>] {1}{2} <b><color=#0000FF>{3}</color></b>{9} <color=#00FF00BB>{4}</color>/<color=#00FF00BB>{5}</color>/<color=#00FF00BB>{6}</color>/<color=#00FF00BB>{6}</color>/<color=#00FF00BB>{7}</color></color>";
        private Rect _mRect;

        public void OnGUI()
        {
            if (!PhotonNetwork.inRoom) return;
            _mRect = new Rect(0, -12, Screen.width, 20);
            foreach (PhotonPlayer p in PhotonNetwork.playerList.ToList())
                GUI.Label(_mRect = _mRect.MoveY(15), Entry(p));
        }

        private static string Entry(PhotonPlayer player)
        {
            object temp;
            string playerName = player.HexName.Trim() == string.Empty ? "Unknown" : player.HexName ?? "Unknown", humanType;
            var type = !FengGameManagerMKII.ignoreList.Contains(player.ID) ? ((temp = player.customProperties[PhotonPlayerProperty.dead]) != null ? ((bool)temp ? 4 : (temp = player.customProperties[PhotonPlayerProperty.team]) != null ? ((int)temp == 2 ? 2 : ((int)temp == 1 ? 1 : 3)) : 0) : 0) : 5;
            var kills = (temp = player.customProperties[PhotonPlayerProperty.kills]) != null && temp is int ? ((int)temp) : 0;
            var deaths = (temp = player.customProperties[PhotonPlayerProperty.deaths]) != null && temp is int ? ((int)temp) : 0;
            var maxDmg = (temp = player.customProperties[PhotonPlayerProperty.max_dmg]) != null && temp is int ? ((int)temp) : 0;
            var totDmg = (temp = player.customProperties[PhotonPlayerProperty.total_dmg]) != null && temp is int ? ((int)temp) : 0;
            var averangeDmg = totDmg > 0 && kills > 0 ? Convert.ToInt32(Math.Floor((decimal)totDmg / kills)) : 0;

            switch (type)
            {
                case 1:
                    humanType = string.Empty;
                    break;
                case 2:
                    humanType = " [<b><color=#CCFF00CC>A</color></b>]";
                    break;
                case 3:
                    humanType = " [<b><color=#FCACBFFF>T</color></b>]";
                    break;
                case 4:
                    humanType = " [<b><color=#FF0000FF>DEAD</color></b>]";
                    break;
                case 5:
                    humanType = " [<b><color=#FF0000FF>IGNORED</color></b>]";
                    break;
                default:
                    humanType = " [<i><color=#FCACBFFF>NULL</color></i>]";
                    break;
            }
            var mod = string.Empty;
            if (player.Has("AoTTG_Mod"))
                mod = $"|<b><color=#00FFFF>Hawk</color></b>|";
            else if (player.Has("HawkUser"))
                mod = "|<b><color=#4281FF>HawkUser</color></b>|";
            else if (player.Has("AlphaX"))
                mod = "|<b><color=#00D5FF>AlphaX</color></b>|";
            else if (player.Has("Alpha"))
                mod = "|<b><color=#00FFA2>Alpha</color></b>|";
            else if (player.Has("coin") || player.Has("UPublica2") || player.Has("Hats"))
                mod = "|<b><color=#0061FC>Universe</color></b>|";
            else if (player.Has("PBCheater"))
                mod = "|<b><color=#91FF00>PedoBear</color></b>|";
            else if (player.Has("SRC"))
                mod = "|<b><color=#FF6200>SRC</color></b>|";
            else if (player.Has("RCteam"))
                mod = "|<b><color=#00FF00AA>RC</color></b>|";
                
            return string.Format(EntryLayout, player.ID, mod, humanType, playerName, kills, deaths, maxDmg, totDmg, averangeDmg, player.name != string.Empty ? " | " + player.name : "");
        }
    }
}
