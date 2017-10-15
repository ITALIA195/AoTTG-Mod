using System;
using System.Runtime.Serialization;

namespace Mod.exceptions
{
    [Serializable]
    public class ArgumentException : System.Exception
    {


        public ArgumentException(string message)
        {
            Core.SendMessage("Errore negli argomenti");
            Core.SendMessage(message);
        }
    }
}