using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.commands
{
    [Command("fakedmg;dmg")]
    public class CommandDamage 
    {
        public static int Damage = -1;

        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("/dmg [damage]"); //TODO: Not showing up
            if (args[0].ToInt() <= -1)
                Damage = -1;
            else
                Damage = args[0].ToInt();
            if (Damage != -1)
                Core.SendMessage("Il danno sarà sempre " + Damage);
            else
                Core.SendMessage("Il danno  e' stato ripristinato.");
        }
    }
}
