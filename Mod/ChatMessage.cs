using UnityEngine;

namespace Mod
{
    public class ChatMessage
    {
        internal readonly string _message;
        internal readonly PhotonPlayer _sender;
        internal readonly float _time;
        internal readonly bool _localOnly;
        public bool visible = true;

        public ChatMessage(object message, PhotonPlayer sender, bool local = false)
        {
            _message = message as string;
            _sender = sender;
            _time = Time.time;
            _localOnly = local;
        }

        public string Message => _message;
        public PhotonPlayer GetSender => _sender;
        public float GetTime => _time;
        public bool IsVisible => visible;
        public bool IsLocalOnly => _localOnly;
    }
}
