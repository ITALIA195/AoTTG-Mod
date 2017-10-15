using System.Collections;
using Mod.manager;
using UnityEngine;

namespace Mod.gui
{
    public class GUIReconnect : Gui
    {
        private int _asd = 1;
        private string _roomName;

        public void OnGUI()
        {
            return;
            if (!Enabled) return;
            Texture2D background = InterfaceManager.CreateTexture("373737");
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
            Rect rect = new Rect(Screen.width / 2f - 225, Screen.height / 2f - 80, 450, 160);
            GUI.DrawTexture(rect, background);
            GUI.Label(new Rect(rect.x + 10, rect.y + rect.height / 2 - 20, rect.width - 20, 20), "We are trying to reconnect you into the room:", new GUIStyle(style) { fontSize = 20});
            GUI.Label(new Rect(rect.x + 10, rect.y + rect.height / 2, rect.width - 20, 20), _roomName ?? "Unknown roomname!", new GUIStyle(style) { fontSize = 20});
            if (GUI.Button(new Rect(rect.x + rect.width / 2f - 100, rect.y + rect.height - 40, 200, 30), "Cancel", style))
                _asd += 1;
            Drawing.DrawCircle(new Vector2(rect.x, rect.y), 100, Color.red, 50f, _asd);
            Destroy(background);
            Destroy(buttonActive);
            Destroy(buttonHover);
        }

        public void Reconnect()
        {
            if (!PhotonNetwork.connected) return;
            Core.InterfaceManager.Disable(typeof(GUIMainMenu).Name);
            var currentRoom = PhotonNetwork.room.name;
            _roomName = currentRoom.Split('`')[0];
            Enable();
            PhotonNetwork.Disconnect();
            PhotonNetwork.ConnectToMaster("app-eu.exitgamescloud.com", 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
            StartCoroutine(Reconnect(currentRoom));
        }

        public IEnumerator Reconnect(string room)
        {
            yield return PhotonNetwork.connectionStatesDetailed != PeerStates.JoinedLobby;
            PhotonNetwork.JoinRoom(room);
        }
    }
}
