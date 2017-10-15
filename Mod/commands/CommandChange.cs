using System.Linq;
using System.Text.RegularExpressions;
using ExitGames.Client.Photon;
using Mod.exceptions;
using Mod.gui;
using ArgumentException = Mod.exceptions.ArgumentException;

namespace Mod.commands
{
    [Command("change")]
    public class CommandChange
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            if (args.Length < 3)
                throw new ArgumentException("/change [name/guild] [all/id] [val]");
            args[2] = Regex.Match(GUIChat.Message, @"[\\\/][a-zA-Z]*\s\w*\s\w*\s(.*)").Groups[1].Value;
            Hashtable hash = new Hashtable();
            switch (args[0].ToLower())
            {
                case "name":
                    hash.Add(PhotonPlayerProperty.name, args[2]);
                    break;
                case "guild":
                    hash.Add(PhotonPlayerProperty.guildName, args[2]);
                    break;
                default:
                    throw new ArgumentException("/change [name/guild] [all/id] [val]");
            }
            if (args[1].EqualsIgnoreCase("all"))
            {
                foreach (PhotonPlayer player in PhotonNetwork.playerList.Where(x => !x.isLocal)) 
                        player.SetCustomProperties(hash);
                Core.SendMessage(
                    $"Hai cambiato {(args[0].EqualsIgnoreCase("name") ? "il nome" : "la gilda")} a tutti in {args[2].HexColor()}.");
            }
            else
            {
                PhotonPlayer player = PhotonPlayer.Find(args[1].ToInt());
                if (player == null)
                    throw new PlayerNotFoundException();
                string name = player.HexName;
                player.SetCustomProperties(hash);
                Core.SendMessage($"{name} ha ora {(args[0].EqualsIgnoreCase("name") ? "il nome" : "la gilda")} {args[2].HexColor()}");
            }
        }
    }
}
