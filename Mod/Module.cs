using System;
using UnityEngine;

namespace Mod
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class Module : Attribute
    {
        public readonly string I18NEntry;
        private readonly bool _isAbusive;
        private readonly string _moduleDescription;
        private readonly string _modulePlayerPref;
        private bool _enabled;

        public Module(string name, bool isAbusive = false)
        {
            I18NEntry = "module." + name;
            _moduleDescription = Core.Lang[$"module.{name}.description"];
            _isAbusive = isAbusive;
            _modulePlayerPref = "M0D|" + I18NEntry;
        }

        public void Enable()
        {
            if (_enabled) return;
            _enabled = true;
            PlayerPrefs.SetString(_modulePlayerPref, _enabled.ToString());
            Core.ModManager.CallMethod(this, "OnEnable");
        }

        public void Disable()
        {
            if (!_enabled) return;
            _enabled = false;
            PlayerPrefs.SetString(_modulePlayerPref, _enabled.ToString());
            Core.ModManager.CallMethod(this, "OnDisable");
        }

        public void Toggle()
        {
            if (_enabled) Disable();
            else Enable();
        }

        public Type Class;
        public string Name => Core.Lang[I18NEntry + ".name"];
        public string Description => _moduleDescription;
        public bool IsAbusive => _isAbusive;
        public override string ToString() => Name;

        public bool Enabled => _enabled;
        public void Load() => _enabled = PlayerPrefs.GetString(_modulePlayerPref, "False").ToBool();
    }
}
