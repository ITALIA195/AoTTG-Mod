using System;

namespace Mod.commands
{
    [Command("rules;settings")]
    public class CommandRules 
    {
        public void OnCommand(PhotonPlayer sender, string[] args)
        {
            Core.SendMessage("Currently activated gamemodes:");
            if (RCSettings.bombMode > 0)
                Core.SendMessage("Bomb mode is on.");
            if (RCSettings.teamMode > 0)
                Core.SendMessage($"Team mode is on {(RCSettings.teamMode == 1 ? "no sort" : RCSettings.teamMode == 2 ? "sort by size" : "sort by skill")}.");
            if (RCSettings.pointMode > 0)
                Core.SendMessage($"Point mode is on {RCSettings.pointMode}.");
            if (RCSettings.disableRock > 0)
                Core.SendMessage("Punk Rock-Throwing is disabled.");
            if (RCSettings.spawnMode > 0)
                Core.SendMessage($"Custom spawn rate is on {RCSettings.nRate:F2}% Normal, {RCSettings.aRate:F2}% Abnormal, {RCSettings.jRate:F2}% Jumper, {RCSettings.cRate:F2}% Crawler, {RCSettings.pRate:F2}% Punk ");
            if (RCSettings.explodeMode > 0)
                Core.SendMessage($"Titan explode mode is on {RCSettings.explodeMode}.");
            if (RCSettings.healthMode > 0)
                Core.SendMessage($"Titan health mode is on {RCSettings.healthLower}-{RCSettings.healthUpper}.");
            if (RCSettings.infectionMode > 0)
                Core.SendMessage($"Infection mode is on {RCSettings.infectionMode}.");
            if (RCSettings.damageMode > 0)
                Core.SendMessage($"Minimum nape damage is on {RCSettings.damageMode}.");
            if (RCSettings.moreTitans > 0)
                Core.SendMessage($"Custom titan # is on {RCSettings.moreTitans}.");
            if (RCSettings.sizeMode > 0)
                Core.SendMessage($"Custom titan size is on {RCSettings.sizeLower:F2},{RCSettings.sizeUpper:F2}.");
            if (RCSettings.banEren > 0)
                Core.SendMessage("Anti-Eren is on. Using Titan eren will get you kicked.");
            if (RCSettings.waveModeOn == 1)
                Core.SendMessage($"Custom wave mode is on {RCSettings.waveModeNum}.");
            if (RCSettings.friendlyMode > 0)
                Core.SendMessage("Friendly-Fire disabled. PVP is prohibited.");
            if (RCSettings.pvpMode > 0)
                Core.SendMessage($"AHSS/Blade PVP is on {(RCSettings.pvpMode == 1 ? "team-based" : "FFA")}.");
            if (RCSettings.maxWave > 0)
                Core.SendMessage($"Max Wave set to {RCSettings.maxWave}");
            if (RCSettings.horseMode > 0)
                Core.SendMessage("Horses are enabled.");
            if (RCSettings.ahssReload > 0)
                Core.SendMessage("AHSS Air-Reload disabled.");
            if (RCSettings.punkWaves > 0)
                Core.SendMessage("Punk override every 5 waves enabled.");
            if (RCSettings.endlessMode > 0)
                Core.SendMessage($"Endless Respawn is enabled {RCSettings.endlessMode} seconds.");
            if (RCSettings.globalDisableMinimap > 0)
                Core.SendMessage("Minimaps are disabled.");
            if (RCSettings.motd != string.Empty)
                Core.SendMessage($"Motd <color=#FF0000>{RCSettings.motd}</color>");
            if (RCSettings.deadlyCannons > 0)
                Core.SendMessage("Cannons kill humans.");
        }
    }
}
