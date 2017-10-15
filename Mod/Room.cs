using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod
{
    public class Room
    {
        private readonly string _roomSettings;
        private readonly string _roomName;
        private readonly string _roomMap;
        private readonly bool _isProtected;
        private readonly int _currentPlayers;
        private readonly int _maxPlayers;

        public Room(string roomSettings, bool isPasswordProtected, int currentPlayers, int maxPlayers)
        {
            _roomSettings = roomSettings;
            _roomName = _roomSettings.Split('`')[0];
            _roomMap = _roomSettings.Split('`')[1];
            _isProtected = isPasswordProtected;
            _currentPlayers = currentPlayers;
            _maxPlayers = maxPlayers;
        }

        public static List<Room> List => PhotonNetwork.GetRoomList().Select(room => new Room(room.name, room.name.Split('`')[5] != string.Empty, room.playerCount, room.maxPlayers)).ToList();
        public static readonly Func<List<Room>, List<Room>> GetOrdinatedList = list =>
        {
            var value = list.Where(room => room.IsJoinable && !room.IsProtected).ToList();
            value.AddRange(list.Where(room => room.IsProtected && room.IsJoinable));
            value.AddRange(list.Where(room => room.IsProtected && !room.IsJoinable));
            value.AddRange(list.Where(room => !room.IsJoinable && !room.IsProtected));
            return value;
        };

        public string RoomSettings => _roomSettings;
        public string RoomName => _roomName;
        public string Map => _roomMap;
        public bool IsJoinable => _maxPlayers == 0 || _currentPlayers < _maxPlayers;
        public bool IsProtected => _isProtected;
        public int MaxPlayers => _maxPlayers;
        public int CurrentPlayers => _currentPlayers;
    }
}
