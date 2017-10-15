using System.IO;
using System.Linq;
using Mod.manager;
using UnityEngine;

namespace Mod
{
    public class FriendList : MonoBehaviour
    {
        private static readonly string file = Application.dataPath + "/friends";
        internal static string[] friendList;
        internal Rect FormRect = new Rect(0, Screen.height - Screen.height/100*40, Screen.width / 100 * 10, Screen.height / 100 * 40);
        internal readonly Texture2D background = new Texture2D(1, 1);
        internal GUIStyle form;

        public static void Add(string friendName)
        {
            if (string.IsNullOrEmpty(friendName)) return;
            var list = friendList.ToList();
            if (!list.Contains(friendName))
                list.Add(friendName);
            friendList = list.ToArray();
            File.WriteAllLines(file, friendList);
        }

        public static void Remove(string friendName)
        {
            if (string.IsNullOrEmpty(friendName)) return;
            var list = friendList.ToList();
            if (list.Contains(friendName))
                list.Remove(friendName);
            friendList = list.ToArray();
            File.WriteAllLines(file, friendList);
        }

        public void Update()
        {
            if (PhotonNetwork.connectionStatesDetailed == PeerStates.JoinedLobby && !PhotonNetwork.inRoom)
            {
                friendList = File.ReadAllLines(file);
                PhotonNetwork.FindFriends(friendList);
            }
        }

        public void OnGUI()
        {
            if (PhotonNetwork.connectionStatesDetailed == PeerStates.JoinedLobby && !PhotonNetwork.inRoom)
                FormRect = GUI.Window(205, FormRect, Frame, "<color=#00D5FF>== Friend List ==</color>", form);
        }

        public void Start()
        {
            background.SetPixel(1, 1, InterfaceManager.FloatColor("FF6F00"));
            background.Apply();
            form = new GUIStyle
            {
                normal = { background = background },
                active = { background = background },
                hover = { background = background },
                onActive = { background = background },
                onFocused = { background = background },
                onHover = { background = background },
                onNormal = { background = background },
                focused = { background = background }
            };
        }

        private static void Frame(int id)
        {
            GUI.DragWindow();
        }
    }
}