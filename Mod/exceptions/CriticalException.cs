using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace Mod.exceptions
{
    [Serializable]
    public class CriticalException : Exception
    {
        public CriticalException()
        {
            Application.Quit();
        }

        public CriticalException(string message)
        {
            Debug.LogError(message);
            Core.LogFile(message, ErrorType.Error);
            Application.Quit();
        }
    }
}