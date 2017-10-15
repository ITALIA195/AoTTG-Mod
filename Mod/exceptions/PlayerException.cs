using System;
using System.Runtime.Serialization;

namespace Mod.exceptions
{
    [Serializable]
    public class PlayerException : System.Exception
    {
        public PlayerException()
        {

        }

        public PlayerException(string message)
        {
            Core.Log(message);
        }
    }
}