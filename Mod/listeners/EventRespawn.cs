using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.listeners
{
    public class EventRespawn : events.IListener
    {
        public void OnEvent()
        {
            Core.SendMessage("Respawned");
        }

        public Type GetEvent() => typeof(EventRespawn);
    }
}
