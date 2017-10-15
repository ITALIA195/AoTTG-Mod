using System;
using System.Text.RegularExpressions;

namespace Mod.commands
{
    [Command("room")]
    public class CommandRoom 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            if (args.Length < 2)
                throw new ArgumentException("/room [max/time/visible] [arg]"); //TODO: Not showing up
            int num;
            switch (args[0].ToLower())
            {
                case "players":
                case "max":
                {
                    num = args[1].ToInt();
                    PhotonNetwork.room.maxPlayers = num;
                    Core.SendMessage("Max player cambiati a " + num + ".");
                    break;
                }

                case "add":
                case "time":
                {
                    num = Regex.Match(args[1], @"([0-9]*).{1}.*").Groups[1].Value.ToInt();
                    string str = Regex.Match(args[1], @"[0-9]*(.{1}).*").Groups[1].Value;

                    if (str.EqualsIgnoreCase("m"))
                        FengGameManagerMKII.instance.AddTime(num * 60);
                    else
                        FengGameManagerMKII.instance.AddTime(num);
                    Core.SendMessage($"Sono stati aggiunti {num} {(str.EqualsIgnoreCase("m") ? "minuti" : "secondi")}.");
                    break;
                }

                case "visible":
                {
                    PhotonNetwork.room.visible = args[1].ToBool();
                    PhotonNetwork.room.open = args[1].ToBool();

                    Core.SendMessage($"La stanza ora e' {(!args[1].ToBool() ? "in" : "")}visibile.");
                    break;
                }
            }
        }
    }
}
