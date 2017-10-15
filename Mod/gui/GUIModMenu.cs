using System.Collections;
using System.Runtime.InteropServices;
using Mod.manager;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Mod.gui
{
    public class GUIModMenu : Gui
    {
        private bool _visible;
        private Rect _rect;
        public static GameObject Panel;
        private readonly GUIStyle _windowTitle = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.cyan },
            fontSize = 30
        };
        private readonly GUIStyle _label = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 50,
            normal = { textColor = Color.Lerp(Color.blue, Color.cyan, 0.5f) },
            fontStyle = FontStyle.Bold
        };
        private readonly GUIStyle _label2 = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 20,
            normal = { textColor = Color.Lerp(Color.blue, Color.cyan, 0.5f) },
            fontStyle = FontStyle.Italic
        };
        private readonly string[] _windowsName = {"Info", "Chat Message Fade"};
        private int _currentSubGUI;
        private string _startColor = "FFFF00";
        private string _endColor = "FF0000";
        public static bool UseFade2;
        public static bool EnableFade;
        

        public void OnGUI()
        {
            if (!_visible) return;
            GUI.DrawTexture(_rect = GUIHelper.AlignRect(700, 500, GUIHelper.Alignment.CENTER), InterfaceManager.CreateTexture(32, 32, 32));
            _rect = _rect.Shrink(10);
            if (GUI.Button(new Rect(_rect.xMin, _rect.yMin, 50, 30), "◄", new GUIStyle(_label) { fontSize = 25 }))
            {
                _currentSubGUI--;
                if (_currentSubGUI < 0)
                    _currentSubGUI = _windowsName.Length - 1;
            }
            GUILayout.BeginArea(new Rect(_rect.xMin + 60, _rect.yMin, _rect.width - 120, 30));
            GUILayout.Label(_windowsName[_currentSubGUI], _windowTitle, GUILayout.MaxWidth(_rect.width - 120));
            GUILayout.EndArea();
            if (GUI.Button(new Rect(_rect.xMin + _rect.width - 50, _rect.yMin, 50, 30), "►", new GUIStyle(_label) {fontSize = 25}))
            {
                _currentSubGUI++;
                if (_currentSubGUI > _windowsName.Length - 1)
                    _currentSubGUI = 0;
            }
            GUI.DrawTexture(GUIHelper.AlignRect(700, 3, GUIHelper.Alignment.CENTER).MoveY(-(500/2-3)), InterfaceManager.CreateTexture(255, 140, 0));
            _rect = GUIHelper.AlignRect(700, 500, GUIHelper.Alignment.CENTER).Shrink(10, 30).MoveY(15);

            switch (_currentSubGUI)
            {
                case 0:
                    GUI.Label(_rect = new Rect(_rect.xMin, _rect.yMin - 100, _rect.width, _rect.height), "AoTTG Mod v3", _label);
                    GUI.Label(_rect = new Rect(_rect.xMin + 130, _rect.yMin + 34, _rect.width, _rect.height), "by Hawk.", _label2);
                    break;

                case 1:
                    GUILayout.BeginArea(_rect);
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginHorizontal();
                    EnableFade = GUILayout.Toggle(EnableFade, "Enable fade?");
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Chat fade start color: ");
                    _startColor = GUILayout.TextField(_startColor, 6, GUILayout.Width(60));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Chat fade end color: ");
                    _endColor = GUILayout.TextField(_endColor, 6, GUILayout.Width(60));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    UseFade2 = GUILayout.Toggle(UseFade2, "Use Fade2");
                    GUILayout.EndHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Apply"))
                    {
                        Chat._FadeStartColor = InterfaceManager.FloatColor(_startColor);
                        Chat._FadeEndColor = InterfaceManager.FloatColor(_endColor);
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndArea();
                    break;

            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                _visible = !_visible;
                if (PhotonNetwork.inRoom)
                {
                    if (_visible)
                    {
                        IN_GAME_MAIN_CAMERA.isPausing = true;
                        Screen.showCursor = true;
                        Screen.lockCursor = false;
                        if (Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled) return;
                        Camera.main.GetComponent<SpectatorMovement>().disable = true;
                        Camera.main.GetComponent<MouseLook>().disable = true;
                    }
                    else
                    {
                        if (!Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled)
                        {
                            Screen.showCursor = true;
                            Screen.lockCursor = true;
                            GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
                            Camera.main.GetComponent<SpectatorMovement>().disable = false;
                            Camera.main.GetComponent<MouseLook>().disable = false;
                        }
                        else
                        {
                            IN_GAME_MAIN_CAMERA.isPausing = false;
                            if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
                            {
                                Screen.showCursor = false;
                                Screen.lockCursor = true;
                            }
                            else
                            {
                                Screen.showCursor = false;
                                Screen.lockCursor = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
