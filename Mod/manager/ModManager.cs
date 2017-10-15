using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Mod.manager
{
    public class ModManager : MonoBehaviour
    {
        private readonly List<ConstructorInfo> _info = new List<ConstructorInfo>();
        private readonly List<Module> _mods = new List<Module>();

        public ModManager()
        {
            var commandsTypes =
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes(), (a, t) => new {a, t})
                    .Select(t1 => new {aa = t1, attributes = t1.t.GetCustomAttributes(typeof(Module), true)})
                    .Where(t1 => t1.attributes != null && t1.attributes.Length > 0)
                    .Select(t1 => new {Type = t1.aa.t, Attributes = t1.attributes.Cast<Module>()});
            foreach (var x1 in commandsTypes)
            {
                foreach (var aCommand in x1.Attributes)
                {
                    aCommand.Class = x1.Type;
                    _mods.Add(aCommand);
                    _info.Add(x1.Type.GetConstructor(Type.EmptyTypes));
                }
            }

        }

        private void Start()
        {
            foreach (var m in _mods)
            {
                m.Load();
                CallMethod(m, "Start");
            }
        }

        private void Update()
        {
            CallMethod("Update");
        }

        public bool HasGui(Module module)
        {
            return module.Class.GetMethod("GetGui") != null;
        }

        public Action<Rect> GetGui(Module module)
        {
            try
            {
                return (Action<Rect>)module.Class.GetMethod("GetGui")?.Invoke(_info[_mods.IndexOf(module)]?.Invoke(new object[0]), new object[0]);
            }
            catch (Exception e)
            {
                Core.Log(e);
            }
            return null;
        }

        public void CallMethod(Module module, string methodName)
        {
            try
            {
                module.Class.GetMethod(methodName)?.Invoke(_info[_mods.IndexOf(module)]?.Invoke(new object[0]), new object[0]);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void CallMethod(string methodName, params object[] args)
        {
            try
            {
                for (var index = 0; index < Mods.Count; index++)
                {
                    if (!_mods[index].Enabled) continue;
                    _mods[index].Class.GetMethod(methodName)?.Invoke(_info[index]?.Invoke(new object[0]), args);
                }
            }
            catch (Exception e)
            {
                Core.Log($"Caught {e.GetBaseException()} when calling {methodName} in {GetType().Name}");
                Core.LogFile(e, ErrorType.Error);
            }
        }

        public static Module Find(string modName)
        {
            return Mods.FirstOrDefault(mod => mod.I18NEntry.EqualsIgnoreCase(modName));
        }

        public static List<Module> Mods => Core.ModManager._mods;
    }
}
