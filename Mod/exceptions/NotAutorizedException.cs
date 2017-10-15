using System;
using System.Runtime.Serialization;

namespace Mod.exceptions
{
    [Serializable]
    public class NotAutorizedException : Exception
    {
        public NotAutorizedException()
        {
        }

        public NotAutorizedException(string message)
        {
            Core.SendMessage(message);
        }
    }
}