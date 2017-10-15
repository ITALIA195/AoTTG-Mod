using System.Linq;
using Mod.commands;
using Mod.manager;

namespace Mod.mods
{
    [Module("certifykill")]
    public class ModCertifyKill
    {
        public void OnTitanHit(float dmg)
        {
            Core.SendMessage("Works");
            if (ModManager.Mods.FirstOrDefault(m => m.IsAbusive) != null)
                Core.SendPublicMessage($"{PhotonNetwork.player.HexName} has done {dmg} with {GetAbusiveModules()} active! (With {(Core.Hero.useGun ? "AHSS" : "Blade")})");
            else
                Core.SendPublicMessage($"{PhotonNetwork.player.HexName} has done {dmg} legitly! (With {(Core.Hero.useGun ? "AHSS" : "Blade")})");
        }

        private static string GetAbusiveModules()
        {
            if (CommandDamage.Damage != -1)
                return "<color=#FF0000>!!Custom Damage!!</color>";
            return ModManager.Mods.Where(m => m.Enabled && m.IsAbusive).Aggregate(string.Empty, (current, m) => current + $"<color=#FF0000>{m.Name}</color> & ").DeleteEnd(3);
        }
    }
}
