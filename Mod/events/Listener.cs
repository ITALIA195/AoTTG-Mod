using System;

namespace Mod.events
{
    public interface IListener
    {
        void OnEvent();
        Type GetEvent();
    }
}
