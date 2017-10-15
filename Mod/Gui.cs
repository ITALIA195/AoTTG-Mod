using System;
using System.Linq;
using Mod.manager;
using Photon;

namespace Mod
{
    public class Gui : MonoBehaviour
    {
        public string Name;
        private bool _enabled;

        public Gui()
        {
            Name = GetType().Name;
        }

        public void Toggle()
        {
            _enabled = !_enabled;
        }

        public void Enable()
        {
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
        }

        public bool Enabled => _enabled;

        public void Switch(string newGui)
        {
            _enabled = false;
            Core.InterfaceManager.Guis.FirstOrDefault(x => x.Name == newGui)?.Enable();
        }
    }
}