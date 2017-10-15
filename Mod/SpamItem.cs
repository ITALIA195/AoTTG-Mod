using UnityEngine;

namespace Mod
{
    public class SpamItem
    {
        private readonly PhotonPlayer _sender;
        private readonly string _rpc;
        private readonly long _time;

        public SpamItem(PhotonPlayer sender, string rpc, long time)
        {
            _sender = sender;
            _rpc = rpc;
            _time = time;
        }

        public PhotonPlayer Sender => _sender;
        public string RPC => _rpc;
        public long Time => _time;
    }
}
