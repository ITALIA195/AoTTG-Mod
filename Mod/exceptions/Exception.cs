using System;

namespace Mod.exceptions
{
    [Serializable]
    public class LogException : Exception
    {
        public LogException(string message)
        {
            Core.Log(message);
        }
    }
}