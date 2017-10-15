using System;
using Microsoft.Win32;

namespace Mod
{
    public class RegistryUtils
    {
        private readonly RegistryKey _key;

        public RegistryUtils(string path)
        {
            if ((_key = Registry.CurrentUser.OpenSubKey(path, true)) == null)
                _key = Registry.CurrentUser.CreateSubKey(path);
            if (_key == null)
                Core.Log("Impossibile creare chiave nel registro.");
        }

        public object GetValue(string keyName)
        {
            return _key.GetValue(keyName);
        }

        public bool SetValue(string keyName, object value)
        {
            try
            {
                _key.SetValue(keyName, value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteKey(string keyName)
        {
            try
            {
                _key.DeleteValue(keyName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
