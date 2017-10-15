using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Mod.commands;
using Mod.exceptions;

namespace Mod.manager
{
    public class CommandManager : List<Command>
    {
        private readonly List<object> _constructors = new List<object>();

        public CommandManager()
        {
            var commandsTypes =
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes(), (assembly, type) => new {assembly, type}) // Gets all types
                    .Select(a => new {a, attributes = a.type.GetCustomAttributes(typeof(Command), inherit: true)}) // Gets all attributes 
                    .Where(b => b.attributes != null && b.attributes.Length > 0) // Checks for attribute exists
                    .Select(c => new {Type = c.a.type, Attributes = c.attributes.Cast<Command>()}); // Casts all attributes to Command

            foreach (var x1 in commandsTypes)
            {
                foreach (Command aCommand in x1.Attributes)
                {
                    aCommand.ClassType = x1.Type;
                    Add(aCommand);
                }
            }

            foreach (Command command in this)
            {
                _constructors.Add(command.ClassType.GetConstructor(Type.EmptyTypes)?.Invoke(new object[0]));
            }
        }

        public void ExecuteCommand(Command cmd, params object[] parameters)
        {
            try
            {
                cmd.ClassType.GetMethod("OnCommand")?.Invoke(_constructors[IndexOf(cmd)], parameters);
            }
            catch (TargetCantBeLocalException) { }
            catch (TargetInvocationException) { }
            catch (PlayerException) { }
            catch (exceptions.ArgumentException) { }
            catch (PlayerNotFoundException)
            {
                Core.SendMessage(Core.Lang["message.playernotfound.text"]);
            }
        }
    }
}
