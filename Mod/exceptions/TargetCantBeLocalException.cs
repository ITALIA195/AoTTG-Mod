using System;
using System.Runtime.Serialization;

namespace Mod.exceptions
{
    [Serializable]
    public class TargetCantBeLocalException : System.Exception
    {


        public TargetCantBeLocalException(string message)
        {
            Core.SendMessage(message);
        }
    }
}