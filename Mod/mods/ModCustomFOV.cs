using System;
using Mod.manager;
using UnityEngine;

namespace Mod.mods
{
    [Module("customfov")]
    public class ModCustomFOV
    {
        private string _fov = string.Empty;

        public void OnEnable()
        {
            _fov = PlayerPrefs.GetInt("M0D|module.customfov.fov", 50).ToString();
        }

        public void OnDisable()
        {
            Camera.main.fieldOfView = 50;
        }

        public Action<Rect> GetGui()
        {
            Texture2D buttonActive = InterfaceManager.CreateTexture(25, 25, 25);
            Texture2D buttonHover = InterfaceManager.CreateTexture(40, 40, 40);
            GUIStyle style = new GUIStyle
            {
                fontSize = 25,
                normal = {textColor = InterfaceManager.FloatColor(9868950)},
                active = {background = buttonActive},
                hover = {background = buttonHover},
                alignment = TextAnchor.MiddleCenter

            };
            GUIStyle[] styles =
            {
                style,
                new GUIStyle(GUI.skin.textArea)
                {
                    fontSize = 25,
                    normal = {textColor = InterfaceManager.FloatColor(9868950), background = style.normal.background},
                    active = {background = style.active.background},
                    hover = {background = style.hover.background},
                    border = new RectOffset(0, 0, 0, 0),
                    alignment = TextAnchor.MiddleCenter
                },
                new GUIStyle(style)
                {
                    fontSize = 30,
                    normal = {textColor = InterfaceManager.FloatColor(200, 200, 200)}
                },
                new GUIStyle(style)
                {
                    fontSize = 20,
                    normal = {background = InterfaceManager.CreateTexture(0, 0, 0, 1)},
                    hover = {textColor = Color.white}
                },
                new GUIStyle(style)
                {
                    alignment = TextAnchor.MiddleCenter
                },
            };
            Rect rect;
            Screen.showCursor = true;
            return window =>
            {
                GUI.Box(new Rect(Screen.width / 100f * 10, window.y + 20, Screen.width - Screen.width / 100f * 20, 800), string.Empty);
                GUI.Label(new Rect(0, window.y + 30, Screen.width, 50), "Custom FOV", styles[2]);
                GUI.Label(rect = new Rect(Screen.width / 100f * 10 + 30, window.y + 100, Screen.width / 2f - Screen.width / 100f * 10 - 30, 30), "FOV:", styles[4]);
                //GUI.Label(rect = new Rect(Screen.width / 2f, window.y + 100, Screen.width / 2f - (Screen.width / 100f * 10) - 30, 35), "", styles[0]);
                _fov = GUI.TextField(rect = new Rect(Screen.width / 2f, window.y + 100, Screen.width / 2f - (Screen.width / 100f * 10) - 30, 35), _fov, styles[2]);
                if (GUI.Button(new Rect(100, rect.y + 50, window.width - 200, 100), "Apply", styles[0]))
                {
                    Camera.main.fieldOfView = _fov.ToInt();
                    PlayerPrefs.SetInt("MOD|module.customfov.fov", _fov.ToInt());
                }
            };
        }
    }
}
