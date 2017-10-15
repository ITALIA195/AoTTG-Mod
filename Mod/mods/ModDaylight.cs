using System;
using System.Globalization;
using Mod.animation;
using UnityEngine;
using Animation = Mod.animation.Animation;

namespace Mod.mods
{
    [Module("daylight")]
    public class ModDaylight
    {
        private DayLight _daylight;

        public void OnEnable()
        {
            IN_GAME_MAIN_CAMERA.dayLight = _daylight;
        }

        public void OnDisable()
        {
            string serverDaylight = PhotonNetwork.room.name.Split('`')[4];
            IN_GAME_MAIN_CAMERA.dayLight = serverDaylight.EqualsIgnoreCase("day") ? DayLight.Day : (serverDaylight.EqualsIgnoreCase("dawn") ? DayLight.Dawn : DayLight.Night);
        }

        public Action<Rect> GetGui()
        {
            return window =>
            {
                GUI.DrawTexture(window, Textures.WhiteTexture);
                GUILayout.BeginArea(window);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Day")) _daylight = DayLight.Day;
                if (GUILayout.Button("Dawn")) _daylight = DayLight.Dawn;
                if (GUILayout.Button("Night")) _daylight = DayLight.Night;
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            };
        }
    }
}
