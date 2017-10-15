using System;
using Mod.animation;
using Mod.manager;
using UnityEngine;
using Animation = Mod.animation.Animation;

namespace Mod.mods
{
    [Module("chatspammer")]
    public class ModChatSpammer
    {
        //TODO: I really dont like this shit. The classes are re-instantiated everytime it's re-enabled. DEOPTIMIZED AS HELL
        //FIXME: THIS IS BULLSHIT
        private static string _preview = string.Empty;
        private static string _message = "This is a test message.";
        private static Animation _animation = new AnimationRainbow(string.Empty, AnimationType.Cycle, 10);

        public void Update()
        {
            _preview = _animation.NextFrame().Replace("[", "<color=#").Replace("]", ">") + _message + "</color>";
            if (!PhotonNetwork.inRoom) return;
            Core.SendPublicMessage(_preview);
        }

        public Action<Rect> GetGui()
        {
            GUIStyle style = new GUIStyle
            {
                fontSize = 25,
                normal = { textColor = InterfaceManager.FloatColor(9868950) },
                active = { background = InterfaceManager.CreateTexture(25, 25, 25) },
                hover = { background = InterfaceManager.CreateTexture(40, 40, 40) },
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
                    normal = {background = InterfaceManager.CreateTexture(0,0,0,1)},
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
                GUI.Label(new Rect(0, window.y + 30, Screen.width, 50), "Chat Spammer", styles[2]);
                GUI.Label(rect = new Rect(Screen.width / 100f * 10 + 30, window.y + 100, Screen.width / 2f - Screen.width / 100f * 10 - 30, 30), "Preview:", styles[4]);
                GUI.Label(rect = rect.MoveY(40), "Text:", styles[4]);
                GUI.Label(rect.MoveY(40), "Fade type:", styles[4]);
                GUI.Label(rect = new Rect(Screen.width / 2f, window.y + 100, Screen.width / 2f - (Screen.width / 100f * 10) - 30, 35), _preview != string.Empty ? _preview.HexColor() : "Module is disabled!", styles[0]);
                _message = GUI.TextField(rect = rect.MoveY(40), _message, styles[1]);
                rect = new Rect(Screen.width / 2f - (Screen.width / 2f - Screen.width / 100f * 10 - 30) / 3 - 10, rect.y + 40, (Screen.width / 2f - Screen.width / 100f * 10 - 30) / 3, 35);
                if (GUI.Button(rect = rect.MoveX((Screen.width / 2f - Screen.width / 100f * 10 - 30) / 4 + 10), "Rainbow", styles[3]))
                    _animation = new AnimationRainbow(string.Empty, AnimationType.Cycle, 10);
                if (GUI.Button(rect = rect.MoveX((Screen.width / 2f - Screen.width / 100f * 10 - 30) / 4 + 10), "Sea", styles[3]))
                    _animation = new AnimationSea(string.Empty, AnimationType.Cycle, 10);
                if (GUI.Button(rect = rect.MoveX((Screen.width / 2f - Screen.width / 100f * 10 - 30) / 4 + 10), "Shelter", styles[3]))
                    _animation = new AnimationShelter(string.Empty, AnimationType.Cycle, 10);
                if (GUI.Button(rect.MoveX((Screen.width / 2f - Screen.width / 100f * 10 - 30) / 4 + 10), "Shit", styles[3]))
                    _animation = new AnimationNoGameNoLife(string.Empty, AnimationType.Cycle, 10);
            };
        }
    }
}
