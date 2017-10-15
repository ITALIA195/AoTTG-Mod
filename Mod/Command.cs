using System;

namespace Mod
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class Command : Attribute
    {
        private readonly string[] _commands;

        public Command(string commands)
        {
            _commands = commands.Split(';');
        }

        public Type ClassType { set; get; }
        public string[] Commands => _commands;
    }
}