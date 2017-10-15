using System;
using System.Globalization;
using Mod.animation;
using Mod.manager;
using UnityEngine;
using Animation = Mod.animation.Animation;

namespace Mod.mods
{
    [Module("nameanimation")]
    public class ModNameAnimation
    {
        private static float _timer;
        private static string _name = RefStrings.PlayerName;
        private static float _refreshRate = 0.03f;
        public static Animation _animation = new AnimationRainbow(RefStrings.PlayerName, AnimationType.LeftToRight, 5);

        public void Update()
        {
            _timer += Time.deltaTime;
            if (_timer < _refreshRate) return;
            _timer = 0f;
            _name = _animation.NextFrame();
            if (!PhotonNetwork.inRoom) return;
            PhotonNetwork.player.SetName(_name);
        }

        public void OnDisable()
        {
            PhotonNetwork.player.SetName(RefStrings.PlayerName);
        }

        public Action<Rect> GetGui()
        {
            Texture2D buttonActive = InterfaceManager.CreateTexture(25, 25, 25);
            Texture2D buttonHover = InterfaceManager.CreateTexture(40, 40, 40);
            GUIStyle style = new GUIStyle
            {
                fontSize = 25,
                normal = { textColor = InterfaceManager.FloatColor(9868950) },
                active = { background = buttonActive },
                hover = { background = buttonHover },
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

            var fadenumber = 5f;
            var animationType = 0;
            Screen.showCursor = true; //TODO: Save with Preferences
            return window =>
            {
                GUI.Box(new Rect(Screen.width / 100f * 10, window.y + 20, Screen.width - Screen.width / 100f * 20, 800), string.Empty);
                GUI.Label(new Rect(0, window.y + 30, Screen.width, 50), "Name Animation settings", styles[2]);
                GUI.Label(rect = new Rect(Screen.width / 100f * 10 + 30, window.y + 100, Screen.width / 2f - Screen.width / 100f * 10 - 30, 30), "Preview:", styles[4]);
                GUI.Label(rect = rect.MoveY(40), "Delay inbetween update:", styles[4]);
                GUI.Label(rect = rect.MoveY(40), "Shades each color:", styles[4]);
                GUI.Label(rect = rect.MoveY(40), "Animation type:", styles[4]);
                GUI.Label(rect.MoveY(40), "Fade type:", styles[4]);
                GUI.Label(rect = new Rect(Screen.width / 2f, window.y + 100, Screen.width / 2f - (Screen.width / 100f * 10) - 30, 35), _name.HexColor(), styles[0]);
                _refreshRate = GUI.HorizontalSlider(rect = rect.MoveY(40), _refreshRate, 0f, .5f);
                fadenumber = GUI.HorizontalSlider(rect = rect.MoveY(40), fadenumber, 1, 100);
                rect = new Rect(rect.x - (Screen.width / 2f - Screen.width / 100f * 10 - 60) / 5 - 10, rect.y + 40, (Screen.width / 2f - Screen.width / 100f * 10 - 30) / 5, 35);
                for (int i = 0; i < 5; i++)
                    if (GUI.Button(rect = rect.MoveX((Screen.width / 2f - Screen.width / 100f * 10 - 60) / 5 + 10), ((AnimationType) i).ToString(), styles[3]))
                        animationType = i;
                rect = new Rect(Screen.width/2f - (Screen.width / 2f - Screen.width / 100f * 10 - 30) / 3 - 10, rect.y + 40, (Screen.width / 2f - Screen.width / 100f * 10 - 30) / 3, 35);
                if (GUI.Button(rect = rect.MoveX((Screen.width / 2f - Screen.width / 100f * 10 - 30) / 4 + 10), "Rainbow", styles[3]))
                    _animation = new AnimationRainbow(RefStrings.PlayerName, (AnimationType)animationType, fadenumber.ToInt());
                if (GUI.Button(rect = rect.MoveX((Screen.width / 2f - Screen.width / 100f * 10 - 30) / 4 + 10), "Sea", styles[3]))
                    _animation = new AnimationSea(RefStrings.PlayerName, (AnimationType)animationType, fadenumber.ToInt());
                if (GUI.Button(rect = rect.MoveX((Screen.width / 2f - Screen.width / 100f * 10 - 30) / 4 + 10), "Shelter", styles[3]))
                    _animation = new AnimationShelter(RefStrings.PlayerName, (AnimationType)animationType, fadenumber.ToInt());
                if (GUI.Button(rect.MoveX((Screen.width / 2f - Screen.width / 100f * 10 - 30) / 4 + 10), "Shit", styles[3]))
                    _animation = new AnimationNoGameNoLife(RefStrings.PlayerName, (AnimationType)animationType, fadenumber.ToInt());
            };
        }
    }
}
