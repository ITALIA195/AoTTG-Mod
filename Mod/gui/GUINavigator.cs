using System;
using System.Collections;
using Mod.manager;
using UnityEngine;

namespace Mod.gui
{
    public class GUINavigator : Gui
    {
        private Action<Rect> _currentGui;
        private readonly GUIStyle _buttonTextStyle = new GUIStyle { normal = { textColor = Color.grey }, fontSize = 14 };
        private readonly GUIStyle _button2TextStyle = new GUIStyle { normal = { textColor = InterfaceManager.FloatColor("0095FF") }, fontSize = 18 };
        private readonly GUIStyle _textStyle = new GUIStyle
        {
            normal = { textColor = InterfaceManager.FloatColor("FFFFFF") },
            fontSize = 30
        };

        private readonly GUIStyle _buttonStyleDisabled = new GUIStyle
        {
            normal = { background = InterfaceManager.CreateTexture(0, 20, 0, 80), textColor = Color.grey },
            alignment = TextAnchor.MiddleLeft,
            padding = { left = 10 },
            fontSize = 15
        };
        private readonly GUIStyle _buttonStyleEnabled = new GUIStyle
        {
            normal = { background = InterfaceManager.CreateTexture("64DCFF"), textColor = Color.grey },
            alignment = TextAnchor.MiddleLeft,
            padding = { left = 10 },
            fontSize = 15
        };
        private bool _showNavigator;
        private string _searchQuery = string.Empty;

        public IEnumerator Start()
        {
            while (true)
            {
                if (_showNavigator)
                {
                    if (Input.GetKeyDown(KeyCode.Backspace))
                    {
                        if (_searchQuery.Length > 0)
                            _searchQuery = _searchQuery.Substring(0, _searchQuery.Length - 1);
                    }
                    else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Insert) || Input.GetKeyDown(KeyCode.Return)) { }
                    else if (Input.anyKeyDown)
                        _searchQuery += Input.inputString;
                }
                if (Input.GetKeyDown(KeyCode.RightControl))
                {
                    _showNavigator = !_showNavigator;
                    if (!_showNavigator)
                    {
                        _searchQuery = string.Empty;
                        _currentGui = null;
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnGUI()
        {
            if (!_showNavigator) return;

            GUI.DrawTexture(GUIHelper.screenRect, InterfaceManager.CreateTexture(0, 0, 0, 85));
            Rect rect = GUIHelper.AlignRect(Screen.width/100*58, Screen.height/100*90, GUIHelper.Alignment.CENTER);
            _currentGui?.Invoke(rect);
            if (_currentGui != null) return;
            GUI.Label(rect.Move(0, 10), "Search:", _textStyle);
            GUI.Label(rect.Move(120, 10), _searchQuery, _textStyle);

            CreateButtons(rect.x, rect.y + 50, rect.width - 50, rect.height);
        }

        private void CreateButtons(float x, float y, float width, float height)
        {
            int times = Mathf.FloorToInt(width/200);
            Rect rect = new Rect(x, y, width, height);
            foreach (Module mod in ModManager.Mods)
            {
                if (!string.IsNullOrEmpty(_searchQuery) && !mod.Name.ContainsIgnoreCase(_searchQuery))
                    continue;

                if (rect.y >= height)
                    continue;

                rect = new Rect(rect.x, rect.y, 200, 40);
                GUI.DrawTexture(rect, mod.Enabled ? Textures.EnabledTexture : Textures.DisabledTexture);
                GUI.Label(new Rect(rect.x + 10, rect.y + rect.height / 2 - 7, rect.width - 10 - 20, rect.height - 10), mod.Name, _buttonTextStyle);
                if (Core.ModManager.HasGui(mod))
                {
                    GUI.DrawTexture(new Rect(rect.x + rect.width - 30, rect.y + 5, 1, rect.height - 10), Textures.GrayTexture);
                    GUI.Label(new Rect(rect.x + rect.width - 25, rect.y + rect.height / 2 - 9, 50, rect.height), "▼", _button2TextStyle);
                    if (GUI.Button(new Rect(rect.x + rect.width - 30, rect.y, 30, rect.height), string.Empty, GUIStyle.none))
                        _currentGui = Core.ModManager.GetGui(mod); //TODO: Make all mods implements abstract class Module (rip attribute) and add GetGui() method;
                }
                if (GUI.Button(new Rect(rect.x, rect.y, rect.width - 30, rect.height), string.Empty, GUIStyle.none))
                    mod.Toggle();


                rect = new Rect(rect.x + (width - 200 * times) / times + 200, rect.y, 0, 0);

                if (rect.x - 200 > width)
                    rect = new Rect(x, rect.y + 50, 0, 0);
            }
        }

    }
}
