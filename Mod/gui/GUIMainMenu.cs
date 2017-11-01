using System;
using System.Linq;
using Mod.gui.components;
using Mod.manager;
using UnityEngine;

namespace Mod.gui
{
    public class GUIMainMenu : Gui
    {
        private readonly GUIStyle _buttonTextStyle = new GUIStyle { normal = { textColor = Color.grey }, fontSize = 14 };
        private readonly GUIStyle _button2TextStyle = new GUIStyle { normal = { textColor = InterfaceManager.FloatColor("0095FF") }, fontSize = 18 };
        private Action<Rect> _windowFrame;
        private readonly GUIStyle _menuButton;
        private readonly GUIStyle _loginButton;
        private readonly GUIStyle _style;
        private readonly GUIStyle _invisButton;
        private Rect rect;
        private string _searchQuery = string.Empty;
        private string _filter = string.Empty;
        private Vector2 _scrollPosition = Vector2.zero;
        private string _map = "The City";
        private string _character = "Levi";
        private string _difficulty = "0";
        private readonly Colorbar3 _bar = new Colorbar3(new Rect(Screen.width / 2f - 300, 1000, 600, 50));


        public GUIMainMenu()
        {
            Texture2D buttonActive = InterfaceManager.CreateTexture(25, 25, 25);
            Texture2D buttonHover = InterfaceManager.CreateTexture(40, 40, 40);

            _menuButton = new GUIStyle
            {
                normal = { textColor = InterfaceManager.FloatColor(148, 148, 148) },
                alignment = TextAnchor.MiddleCenter,
                fontSize = 26
            };
            _style = new GUIStyle
            {
                fontSize = 25,
                normal = { textColor = InterfaceManager.FloatColor(9868950) },
                active = { background = buttonActive },
                hover = { background = buttonHover },
                alignment = TextAnchor.MiddleCenter
            };
            _invisButton = new GUIStyle(GUIStyle.none)
            {
                active = { background = buttonActive },
                hover = { background = buttonHover },
                alignment = TextAnchor.MiddleCenter
            };
            _loginButton = new GUIStyle(_menuButton)
            {
                normal = { textColor = InterfaceManager.FloatColor(177, 162, 0) },
                active = { textColor = InterfaceManager.FloatColor(177, 162, 0) },
                hover = { textColor = InterfaceManager.FloatColor(177, 162, 0) }
            };
        }

        public void Start()
        {
            PhotonNetwork.ConnectToMaster("app-eu.exitgamescloud.com", 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
//            PhotonNetwork.ConnectToMaster("127.0.0.1", 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
        }

        public void OnGUI()
        {
            if (!Enabled) return;
            Rect menu = new Rect(0, 0, Screen.width, 50);

            Texture2D background = InterfaceManager.CreateTexture(35, 35, 35);
            GUI.DrawTexture(GUIHelper.screenRect, background);
            Destroy(background);

            Texture2D menubackground = InterfaceManager.CreateTexture(30, 30, 30);
            GUI.DrawTexture(menu, menubackground);
            Destroy(menubackground);

            if (GUI.Button(menu = new Rect(Screen.width - Screen.width / 7f, 0, Screen.width / 7f, menu.height), string.Empty, _invisButton))
                _windowFrame = WelcomeFrame();
            GUI.Label(menu, "Login", _loginButton);

            if (GUI.Button(menu = new Rect(menu.x - menu.width, menu.y, menu.width, menu.height), string.Empty, _invisButton))
                Application.Quit();
            GUI.Label(menu, "Quit", _menuButton);

            if (GUI.Button(menu = new Rect(menu.x - menu.width, menu.y, menu.width, menu.height), string.Empty, _invisButton))
                _windowFrame = WelcomeFrame();
            GUI.Label(menu, "Settings", _menuButton);

            if (GUI.Button(menu = new Rect(menu.x - menu.width, menu.y, menu.width, menu.height), string.Empty, _invisButton))
                _windowFrame = ModuleManagerFrame();
            GUI.Label(menu, "Modules", _menuButton);

            if (GUI.Button(menu = new Rect(menu.x - menu.width, menu.y, menu.width, menu.height), string.Empty, _invisButton))
                _windowFrame = ProfileSelectorFrame();
            GUI.Label(menu, "Profile Settings", _menuButton);

            if (GUI.Button(menu = new Rect(menu.x - menu.width, menu.y, menu.width, menu.height), string.Empty, _invisButton))
                _windowFrame = MultiplayerFrame();
            GUI.Label(menu, "Multiplayer", _menuButton);

            if (GUI.Button(menu = new Rect(menu.x - menu.width, menu.y, menu.width, menu.height), string.Empty, _invisButton))
                _windowFrame = SingleplayerFrame();
            GUI.Label(menu, "Singleplayer", _menuButton);


            Rect window = new Rect(0, menu.height, Screen.width, Screen.height-menu.height);
            if (_windowFrame == null) _windowFrame = WelcomeFrame();
            _windowFrame.Invoke(window);
        }

        private Action<Rect> WelcomeFrame()
        {
            GUIStyle[] styles =
            {
                _style,
                new GUIStyle(_style) {fontSize = 45, normal = {textColor = InterfaceManager.FloatColor("A10000") }},
            };
            return window =>
            {
                GUI.Label(window.MoveY(-100), "Welcome to the HawkMod", styles[1]);
                GUI.Label(new Rect(Screen.width / 2 + 157, window.y + window.height / 2 - 80, 100, 30), "Build 3.41", styles[0]);
            };
        }

        private Action<Rect> SingleplayerFrame()
        {
            GUIStyle[] styles =
            {
                _style,
                new GUIStyle(_style)
                {
                    fontSize = 22,
                    hover = {textColor = InterfaceManager.FloatColor(16777215)},
                    alignment = TextAnchor.UpperCenter
                }
            };

            return window =>
            {
                GUILayout.BeginArea(new Rect(10, window.y + 30, Screen.width - 20, Screen.height - window.y - 30));
                GUILayout.Label("Room settings", styles[0]);
                GUILayout.Space(60f);
                GUILayout.Label("Character:", styles[0]);
                _character = GUILayout.TextField(_character, styles[1]);
                GUILayout.Space(10f);
                GUILayout.Label("Map:", styles[0]);
                _map = GUILayout.TextField(_map, styles[1]);
                GUILayout.Space(10f);
                GUILayout.Label("Difficulty:", styles[0]);
                _difficulty = GUILayout.TextField(_difficulty, styles[1]);
                GUILayout.Space(60f);
                if (GUILayout.Button("Play", styles[0], GUILayout.MinHeight(100)))
                {
                    IN_GAME_MAIN_CAMERA.difficulty = _difficulty.ToInt();
                    IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.SINGLE;
                    IN_GAME_MAIN_CAMERA.singleCharacter = _character.ToUpper();
                    if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
                        Screen.lockCursor = true;
                    Screen.showCursor = false;
                    if (_map == "trainning_0")
                        IN_GAME_MAIN_CAMERA.difficulty = -1;
                    FengGameManagerMKII.level = _map;
                    Application.LoadLevel(LevelInfo.GetInfo(_map).mapName);
                    Toggle();
                }
                GUILayout.Space(10f);
                GUILayout.EndArea();
            };
        }

        private Action<Rect> MultiplayerFrame()
        {
            GUIStyle[] styles =
            {
                new GUIStyle(GUI.skin.textArea)
                {
                    fontSize = 22,
                    normal = {textColor = InterfaceManager.FloatColor(9868950), background = _style.normal.background},
                    active = {background = _style.active.background},
                    hover = {background = _style.hover.background, textColor = InterfaceManager.FloatColor(16777215)},
                    border = new RectOffset(0, 0, 0, 0),
                    alignment = TextAnchor.UpperCenter
                },
                new GUIStyle(_invisButton) {fontSize = 20},
                new GUIStyle(_style)
                {
                    alignment = TextAnchor.UpperCenter,
                    fontSize = 22,
                    normal = {textColor = Color.gray}
                }
            };

            if (PhotonNetwork.connectionStatesDetailed != PeerStates.JoinedLobby)
                PhotonNetwork.ConnectToMaster("app-eu.exitgamescloud.com", 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
//                PhotonNetwork.ConnectToMaster("127.0.0.1", 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
            return window =>
            {
                if (GUI.GetNameOfFocusedControl() != "Search" && string.IsNullOrEmpty(_filter))
                    GUI.Label(new Rect(0, window.y, Screen.width, 30), "Search", styles[2]);
                GUILayout.BeginArea(window);
                GUI.SetNextControlName("Search");
                _filter = GUILayout.TextField(_filter.Replace("\n", ""), styles[0]);
                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, false, GUIStyle.none, GUIStyle.none);
                if (GUILayout.Button("<color=#0dcbec>Create Room</color>", styles[1]))
                    _windowFrame = CreateNewRoomFrame();
                foreach (Room room in Room.GetOrdinatedList(Room.List))
                {
                    if (_filter != string.Empty && !room.RoomName.RemoveColors().ContainsIgnoreCase(_filter) && !room.Map.ContainsIgnoreCase(_filter))
                        continue;
                    if (GUILayout.Button($"<color=#{RefStrings.MessageColor}>{(room.IsProtected ? "<color=#034C94>[</color><color=#1191D1>PW</color><color=#034C94>]</color> " : string.Empty)}{(room.RoomName.RemoveColors().Length > 20 ? room.RoomName.RemoveColors().Substring(0, 20) : room.RoomName.HexColor())} || {room.RoomSettings.Split('`')[1]} || {(room.IsJoinable ? "<color=#00FF00>" : "<color=#FF0000>")}{room.CurrentPlayers}/{room.MaxPlayers}</color></color>", styles[1]) && room.IsJoinable)
                    {
                        Toggle();
                        PhotonNetwork.JoinRoom(room.RoomSettings);
                    }
                }
                GUILayout.EndScrollView();
                GUILayout.EndArea();
            };
        }

        private Action<Rect> CreateNewRoomFrame()
        {
            GUIStyle[] styles =
            {
                _style,
                new GUIStyle(GUI.skin.textArea)
                {
                    fontSize = 25,
                    normal = {textColor = InterfaceManager.FloatColor(9868950), background = _style.normal.background},
                    active = {background = _style.active.background},
                    hover = {background = _style.hover.background},
                    border = new RectOffset(0, 0, 0, 0),
                    alignment = TextAnchor.MiddleCenter
                },
                new GUIStyle(_style)
                {
                    fontSize = 30
                }, 
            };

            int index = 0;
            LevelInfo.Initialize();
            SimpleAES aes = new SimpleAES();
            string[] roomMaps = LevelInfo.Levels.Select(x => x.name).ToArray();
            string roomName = "Italia >> Hawk mod";
            string roomPassword = string.Empty;
            string roomMaxPlayers = "10";
            string roomTime = "9999";
            string roomDifficulty = "normal";
            DayLight roomDayLight = DayLight.Day;
            bool roomVisible = true;
            bool roomOpen = true;

            return window =>
            {
                GUI.Box(new Rect(Screen.width / 100f * 10, window.y + 20, Screen.width - Screen.width / 100f * 20, 800), string.Empty);
                GUI.Label(new Rect(0, window.y + 30, Screen.width, 50), "Creating new Room", styles[2]);
                GUI.Label(rect = new Rect(Screen.width / 100f * 10 + 30, window.y + 160, Screen.width / 2f - (Screen.width / 100f * 10) - 30, 30), "Room name:", styles[0]);
                GUI.Label(rect = rect.MoveY(40), "Map:", styles[0]);
                GUI.Label(rect = rect.MoveY(40), "Max players:", styles[0]);
                GUI.Label(rect = rect.MoveY(40), "Password:", styles[0]);
                GUI.Label(rect = rect.MoveY(40), "Difficulty:", styles[0]);
                GUI.Label(rect = rect.MoveY(40), "Daylight:", styles[0]);
                GUI.Label(rect = rect.MoveY(40), "Time:", styles[0]);
                GUI.Label(rect = rect.MoveY(40), "Visible:", styles[0]);
                GUI.Label(rect.MoveY(40), "Open:", styles[0]);
                roomName = GUI.TextField(rect = new Rect(Screen.width / 2f, window.y + 160, Screen.width / 2f - (Screen.width / 100f * 10) - 30, 35), roomName, styles[1]);
                var btn = CustomButton(rect = rect.MoveY(40), roomMaps[index], styles[0]);
                if (btn == 1)
                    index = roomMaps.Length - 1 == index ? 0 : index + 1;
                else if (btn == -1)
                    index = index == 0 ? roomMaps.Length - 1 : index - 1;
                roomMaxPlayers = GUI.TextField(rect = rect.MoveY(40), roomMaxPlayers, styles[1]);
                roomPassword = GUI.TextField(rect = rect.MoveY(40), roomPassword, styles[1]);
                rect = rect.MoveY(40);
                if (GUI.Button(new Rect(rect.x, rect.y, rect.width / 3 - 20, rect.height), "Easy", styles[0]))
                    roomDifficulty = "easy";
                if (GUI.Button(new Rect(rect.x + rect.width/3 + 10, rect.y, rect.width / 3 - 20, rect.height), "Normal", styles[0]))
                    roomDifficulty = "normal";
                if (GUI.Button(new Rect(rect.x + rect.width/3*2 + 20, rect.y, rect.width / 3 - 20, rect.height), "Hard", styles[0]))
                    roomDifficulty = "hard";
                if (GUI.Button(rect = rect.MoveY(40), roomDayLight.ToString(), styles[0]))
                    roomDayLight = roomDayLight > DayLight.Dawn ? DayLight.Day : ++roomDayLight;
                roomTime = GUI.TextField(rect = rect.MoveY(40), roomTime, styles[1]);
                if (GUI.Button(rect = rect.MoveY(40), roomVisible.ToString(), styles[0]))
                    roomVisible = !roomVisible;
                if (GUI.Button(rect = rect.MoveY(40), roomOpen.ToString(), styles[0]))
                    roomOpen = !roomOpen;
                if (GUI.Button(new Rect(Screen.width / 2f + 25, rect.y + 100, 300, 50), "Discard", styles[0]))
                    _windowFrame = MultiplayerFrame();
                if (GUI.Button(new Rect(Screen.width / 2f - 25 - 300, rect.y + 100, 300, 50), "Create", styles[0]))
                {
                    string roomTotalName = $"{roomName}`{roomMaps[index]}`{roomDifficulty}`{roomMaxPlayers}`{roomDayLight}`{(roomPassword != string.Empty ? aes.Encrypt(roomPassword) : roomPassword)}`{roomTime}";
                    PhotonNetwork.CreateRoom(roomTotalName, roomVisible, roomOpen, roomMaxPlayers.ToInt());
                }
            };
        }

        private static int CustomButton(Rect rect, string txt, GUIStyle style)
        {
            Vector3 pos = Input.mousePosition;
            var y1 = -(Input.mousePosition.y - Screen.height + 1);
            bool x = rect.x <= pos.x && pos.x <= rect.x + rect.width;
            bool y = rect.y <= y1 && y1 <= rect.y + rect.height;
            if (x && y)
            {
                if (GUI.Button(rect, string.Empty, GUIStyle.none))
                    // Make it runs once at frame (otherwise it increments by 4)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        GUI.DrawTexture(rect, style.active.background);
                        GUI.Label(rect, txt, style);
                        return 1;
                    }
                    if (Input.GetMouseButtonUp(1))
                    {
                        GUI.DrawTexture(rect, style.active.background);
                        GUI.Label(rect, txt, style);
                        return -1;
                    }
                }
                GUI.DrawTexture(rect, style.hover.background);
                GUI.Label(rect, txt, style);
                return 0;
            }
            GUI.DrawTexture(rect, style.normal.background);
            GUI.Label(rect, txt, style);
            return 0;
        }

        private Action<Rect> ProfileSelectorFrame()
        {
            GUIStyle[] styles =
            {
                _style,
                new GUIStyle(GUI.skin.textArea)
                {
                    fontSize = 25,
                    normal = {textColor = InterfaceManager.FloatColor(9868950), background = _style.normal.background},
                    active = {background = _style.active.background},
                    hover = {background = _style.hover.background},
                    border = new RectOffset(0, 0, 0, 0),
                    alignment = TextAnchor.MiddleCenter
                },
                new GUIStyle(_style)
                {
                    fontSize = 30
                },
                new GUIStyle(_style)
                {
                    normal = {background = InterfaceManager.CreateTexture(0,0,0,1)},
                    hover = {textColor = Color.white}
                },
            };

            return window =>
            {
                GUI.Label(new Rect(0, window.y + 30, Screen.width, 50), "Profile Selector", styles[2]);
                GUI.Box(new Rect(Screen.width/100f*10, window.y + 20, Screen.width - Screen.width / 100f * 20, 800), string.Empty);
                GUI.Label(rect = new Rect(Screen.width / 100f * 10 + 30, window.y + 160, Screen.width/2f-(Screen.width / 100f * 10)-30, 30), "Profile Name:", styles[0]);
                GUI.Label(rect = rect.MoveY(40), "Player name:", styles[0]);
                GUI.Label(rect = rect.MoveY(40), "Guild:", styles[0]);
                GUI.Label(rect = rect.MoveY(40), "Chat Name:", styles[0]);
                GUI.Label(rect = rect.MoveY(40), "Chat Color:", styles[0]);
                GUI.Label(rect = rect.MoveY(40), "Friend Name:", styles[0]);
                GUI.Label(rect.MoveY(40), "Chat format:", styles[0]);
                var profile = Core.ProfileManager.Profile;
                profile.ProfileName = GUI.TextField(rect = new Rect(Screen.width / 2f, window.y + 160, Screen.width / 2f - (Screen.width / 100f * 10) - 30, 35), profile.ProfileName, styles[1]);
                profile.PlayerName = GUI.TextField(rect = rect.MoveY(40), profile.PlayerName, styles[1]);
                profile.Guild = GUI.TextField(rect = rect.MoveY(40), profile.Guild, styles[1]);
                profile.ChatName = GUI.TextField(rect = rect.MoveY(40), profile.ChatName, styles[1]);
                profile.ChatColor = GUI.TextField(rect = rect.MoveY(40), profile.ChatColor, 8, styles[1]);
                profile.FriendName = GUI.TextField(rect = rect.MoveY(40), profile.FriendName, styles[1]);
                profile.ChatFormat = GUI.TextField(rect = rect.MoveY(40), profile.ChatFormat, styles[1]);
                if (GUI.Button(new Rect(Screen.width / 2f - 325, rect.y + 160, 650, 50), "Save", styles[0]))
                {
                    switch (ProfileManager.IsValid(profile))
                    {
                        case 0:
                            profile.Save();
                            Core.ProfileManager.Update();
                            Core.Log("File saved!");
                            break;
                        case 1:
                            Core.Log("Invalid 'ChatColor'. Should be RRGGBB(AA)", ErrorType.Error);
                            break;
                        case 2:
                            Core.Log("Invalid 'ChatFormat'. Should contains {0}(name), {1}(color), {2}(message)",
                                ErrorType.Error);
                            break;
                        case 3:
                            Core.Log("You can't save a profile with 'Empty' as a name!", ErrorType.Error);
                            break;
                        default:
                            Core.Log("General error!");
                            break;
                    }
                }
                if (GUI.Button(new Rect(Screen.width / 2f + 25, rect.y + 100, 300, 50), "Discard", styles[0]))
                {
                    profile.Discard();
                    Core.ProfileManager.Update();
                }
                if (GUI.Button(new Rect(Screen.width / 2f - 25 - 300, rect.y + 100, 300, 50), "Delete", styles[0]))
                {
                    profile.Delete();
                    Core.ProfileManager.Update();
                }
                var s = Screen.width - Screen.width / 100f * 10 * 2 - 65;
                rect = new Rect(new Rect(Screen.width / 100f * 10 - s / 4 + 20, window.y + 100, s / 4, 40));
                foreach (Profile profile2 in Core.ProfileManager.GetProfiles())
                    if (GUI.Button(rect = rect.MoveX(s / 4 + 5), profile2.ProfileName, styles[3]))
                        Core.ProfileManager.SwitchProfile(profile2);
            };
        }

        private Action<Rect> ModuleManagerFrame()
        {
            GUIStyle[] styles =
            {
                new GUIStyle(_style)
                {
                    fontSize = 22,
                    hover = {textColor = InterfaceManager.FloatColor(16777215)},
                    alignment = TextAnchor.UpperCenter
                }
            };

            return window =>
            {
                _searchQuery = GUI.TextField(new Rect(window.x, window.y, window.width, 30), _searchQuery.Replace("\n", ""), styles[0]);
                CreateButtons(window.x + 10, window.y + 40, window.width - 10, window.height - 40);
            };
        }

        private void CreateButtons(float x, float y, float width, float height)
        {
            int times = Mathf.FloorToInt(width / 200);
            rect = new Rect(x, y, width, height);
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
                        _windowFrame = Core.ModManager.GetGui(mod); //TODO: Make all mods implements abstract class Module (rip attribute) and add GetGui() method;
                }
                if (GUI.Button(new Rect(rect.x, rect.y, rect.width - 30, rect.height), string.Empty, GUIStyle.none)) //MOD: -30 even when the mod hasn't gotten a gui
                    mod.Toggle();


                rect = new Rect(rect.x + (width - 200 * times) / times + 200, rect.y, 0, 0);

                if (rect.x + 200 > width)
                    rect = new Rect(x, rect.y + 50, 0, 0);
            }
        }
    }
}
