using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Mod
{
    public class Antispam
    {
        private const long TOLLERANCE = 100;
        private static readonly List<SpamItem> Items = new List<SpamItem>();

        public static bool IsSpamming(PhotonPlayer sender, string rpc)
        {
            var playerItems = Get(sender, rpc).Where(item => Stopwatch.GetTimestamp() - item.Time <= TOLLERANCE).ToList();
            if (playerItems.Count >= 3)
            {
                RemoveItems(sender, rpc);   
                Core.SendPublicMessage($"Spamming rpc ({rpc}) by {sender.HexName}. ({playerItems[2].Time - playerItems[1].Time} millis)");
                return true;
            }
            return false;
        }
        
        public static void Add(SpamItem item)
        {
            Items.Add(item);
        }

        private static IEnumerable<SpamItem> Get(PhotonPlayer sender, string rpc)
        {
            foreach (var item in Items.ToList())
                if (Stopwatch.GetTimestamp() - item.Time > TOLLERANCE)
                    Items.Remove(item);
            return Items.Where(item => Equals(item.Sender, sender) && Equals(item.RPC, rpc)).ToList();
        }

        private static void RemoveItems(PhotonPlayer sender, string rpc)
        {
            foreach (var item in Items.ToList())
                if (Equals(item.Sender, sender) && Equals(item.RPC, rpc))
                    Items.Remove(item);
        }
    }
}
