using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelInfo
{
    public string desc;
    public int enemyNumber;
    public bool hint;
    public bool horse;
    private static bool init;
    public bool lavaMode;
    public static LevelInfo[] Levels;
    public string mapName;
    public Minimap.Preset minimapPreset;
    public string name;
    public bool noCrawler;
    public bool punk = true;
    public bool pvp;
    public RespawnMode respawnMode;
    public bool supply = true;
    public bool teamTitan;
    public GAMEMODE type;

    public static LevelInfo GetInfo(string name)
    {
        Initialize(); // Make a class that manages levelinfos and make all the thingy in the ctor
        return Levels.FirstOrDefault(info => info.name == name);
    }
    
    public static void Initialize() 
    {
        if (!init)
        {
            init = true;
            Levels = new List<LevelInfo>(27)
            {
                new LevelInfo
                {
                    name = "The City II",
                    mapName = "The City I",
                    desc = "Fight the titans with your friends.(RESPAWN AFTER 10 SECONDS/SUPPLY/TEAM TITAN)",
                    enemyNumber = 10,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.DEATHMATCH,
                    supply = true,
                    teamTitan = true,
                    pvp = true
                },
                new LevelInfo
                {
                    name = "The Forest III",
                    mapName = "The Forest",
                    desc = "Survive for 20 waves.player will respawn in every new wave",
                    enemyNumber = 3,
                    type = GAMEMODE.SURVIVE_MODE,
                    respawnMode = RespawnMode.NEWROUND,
                    supply = true
                },
                new LevelInfo
                {
                    name = "Custom",
                    mapName = "The Forest",
                    desc = "Custom Map.",
                    enemyNumber = 1,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    teamTitan = true,
                    pvp = true,
                    punk = true
                },
                new LevelInfo
                {
                    name = "Custom (No PT)",
                    mapName = "The Forest",
                    desc = "Custom Map (No Player Titans).",
                    enemyNumber = 1,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    pvp = true,
                    punk = true,
                    supply = true,
                    teamTitan = false
                },
                new LevelInfo
                {
                    name = "The City",
                    mapName = "The City I",
                    desc = "Kill all the titans. No respawn, play as titan.",
                    enemyNumber = 10,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    teamTitan = true,
                    pvp = true,
                    minimapPreset = new Minimap.Preset(new Vector3(22.6f, 0f, 13f), 731.9738f)
                },
                new LevelInfo
                {
                    name = "The City III",
                    mapName = "The City I",
                    desc = "Capture Checkpoint mode.",
                    enemyNumber = 0,
                    type = GAMEMODE.PVP_CAPTURE,
                    respawnMode = RespawnMode.DEATHMATCH,
                    supply = true,
                    horse = false,
                    teamTitan = true
                },
                new LevelInfo
                {
                    name = "Cage Fighting",
                    mapName = "Cage Fighting",
                    desc =
                        "2 players in different cages. when you kill a titan,  one or more titan will spawn to your opponent's cage.",
                    enemyNumber = 1,
                    type = GAMEMODE.CAGE_FIGHT,
                    respawnMode = RespawnMode.NEVER
                },
                new LevelInfo
                {
                    name = "The Forest",
                    mapName = "The Forest",
                    desc = "The Forest Of Giant Trees.(No RESPAWN/SUPPLY/PLAY AS TITAN)",
                    enemyNumber = 5,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    teamTitan = true,
                    pvp = true
                },
                new LevelInfo
                {
                    name = "The Forest II",
                    mapName = "The Forest",
                    desc = "Survive for 20 waves.",
                    enemyNumber = 3,
                    type = GAMEMODE.SURVIVE_MODE,
                    respawnMode = RespawnMode.NEVER,
                    supply = true
                },
                new LevelInfo
                {
                    name = "Annie",
                    mapName = "The Forest",
                    desc =
                        "Nape Armor/ Ankle Armor:\nNormal:1000/50\nHard:2500/100\nAbnormal:4000/200\nYou only have 1 life.Don't do this alone.",
                    enemyNumber = 15,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    punk = false,
                    pvp = true
                },
                new LevelInfo
                {
                    name = "Annie II",
                    mapName = "The Forest",
                    desc =
                        "Nape Armor/ Ankle Armor:\nNormal:1000/50\nHard:3000/200\nAbnormal:6000/1000\n(RESPAWN AFTER 10 SECONDS)",
                    enemyNumber = 15,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.DEATHMATCH,
                    punk = false,
                    pvp = true
                },
                new LevelInfo
                {
                    name = "Colossal Titan",
                    mapName = "Colossal Titan",
                    desc =
                        "Defeat the Colossal Titan.\nPrevent the abnormal titan from running to the north gate.\n Nape Armor:\n Normal:2000\nHard:3500\nAbnormal:5000\n",
                    enemyNumber = 2,
                    type = GAMEMODE.BOSS_FIGHT_CT,
                    respawnMode = RespawnMode.NEVER,
                    minimapPreset = new Minimap.Preset(new Vector3(8.8f, 0f, 65f), 765.5751f)
                },
                new LevelInfo
                {
                    name = "Colossal Titan II",
                    mapName = "Colossal Titan",
                    desc =
                        "Defeat the Colossal Titan.\nPrevent the abnormal titan from running to the north gate.\n Nape Armor:\n Normal:5000\nHard:8000\nAbnormal:12000\n(RESPAWN AFTER 10 SECONDS)",
                    enemyNumber = 2,
                    type = GAMEMODE.BOSS_FIGHT_CT,
                    respawnMode = RespawnMode.DEATHMATCH,
                    minimapPreset = new Minimap.Preset(new Vector3(8.8f, 0f, 65f), 765.5751f)
                },
                new LevelInfo
                {
                    name = "Trost",
                    mapName = "Colossal Titan",
                    desc = "Escort Titan Eren",
                    enemyNumber = 2,
                    type = GAMEMODE.TROST,
                    respawnMode = RespawnMode.NEVER,
                    punk = false
                },
                new LevelInfo
                {
                    name = "Trost II",
                    mapName = "Colossal Titan",
                    desc = "Escort Titan Eren(RESPAWN AFTER 10 SECONDS)",
                    enemyNumber = 2,
                    type = GAMEMODE.TROST,
                    respawnMode = RespawnMode.DEATHMATCH,
                    punk = false
                },
                new LevelInfo
                {
                    name = "[S]City",
                    mapName = "The City I",
                    desc = "Kill all 15 Titans",
                    enemyNumber = 15,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    supply = true
                },
                new LevelInfo
                {
                    name = "[S]Forest",
                    mapName = "The Forest",
                    desc = string.Empty,
                    enemyNumber = 15,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    supply = true
                },
                new LevelInfo
                {
                    name = "[S]Forest Survive(no crawler)",
                    mapName = "The Forest",
                    desc = string.Empty,
                    enemyNumber = 3,
                    type = GAMEMODE.SURVIVE_MODE,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    noCrawler = true,
                    punk = true
                },
                new LevelInfo
                {
                    name = "[S]Tutorial",
                    mapName = "tutorial",
                    desc = string.Empty,
                    enemyNumber = 1,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    hint = true,
                    punk = false
                },
                new LevelInfo
                {
                    name = "[S]Battle training",
                    mapName = "tutorial 1",
                    desc = string.Empty,
                    enemyNumber = 7,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    punk = false
                },
                new LevelInfo
                {
                    name = "The Forest IV  - LAVA",
                    mapName = "The Forest",
                    desc =
                        "Survive for 20 waves.player will respawn in every new wave.\nNO CRAWLERS\n***YOU CAN'T TOUCH THE GROUND!***",
                    enemyNumber = 3,
                    type = GAMEMODE.SURVIVE_MODE,
                    respawnMode = RespawnMode.NEWROUND,
                    supply = true,
                    noCrawler = true,
                    lavaMode = true
                },
                new LevelInfo
                {
                    name = "[S]Racing - Akina",
                    mapName = "track - akina",
                    desc = string.Empty,
                    enemyNumber = 0,
                    type = GAMEMODE.RACING,
                    respawnMode = RespawnMode.NEVER,
                    supply = false,
                    minimapPreset = new Minimap.Preset(new Vector3(443.2f, 0f, 1912.6f), 1929.042f)
                },
                new LevelInfo
                {
                    name = "Racing - Akina",
                    mapName = "track - akina",
                    desc = string.Empty,
                    enemyNumber = 0,
                    type = GAMEMODE.RACING,
                    respawnMode = RespawnMode.NEVER,
                    supply = false,
                    pvp = true,
                    minimapPreset = new Minimap.Preset(new Vector3(443.2f, 0f, 1912.6f), 1929.042f)
                },
                new LevelInfo
                {
                    name = "Outside The Walls",
                    mapName = "OutSide",
                    desc = "Capture Checkpoint mode.",
                    enemyNumber = 0,
                    type = GAMEMODE.PVP_CAPTURE,
                    respawnMode = RespawnMode.DEATHMATCH,
                    supply = true,
                    horse = true,
                    teamTitan = true,
                    minimapPreset = new Minimap.Preset(new Vector3(2549.4f, 0f, 3042.4f), 3697.16f)
                },
                new LevelInfo
                {
                    name = "Cave Fight",
                    mapName = "CaveFight",
                    desc = "***Spoiler Alarm!***",
                    enemyNumber = -1,
                    type = GAMEMODE.PVP_AHSS,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    horse = false,
                    teamTitan = true,
                    pvp = true,
                    minimapPreset = new Minimap.Preset(new Vector3(22.6f, 0f, 13f), 734.9738f)
                },
                new LevelInfo
                {
                    name = "House Fight",
                    mapName = "HouseFight",
                    desc = "***Spoiler Alarm!***",
                    enemyNumber = -1,
                    type = GAMEMODE.PVP_AHSS,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    horse = false,
                    teamTitan = true,
                    pvp = true
                },
                new LevelInfo
                {
                    name = "[S]Forest Survive(no crawler no punk)",
                    mapName = "The Forest",
                    desc = string.Empty,
                    enemyNumber = 3,
                    type = GAMEMODE.SURVIVE_MODE,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    noCrawler = true,
                    punk = false
                },
            }.ToArray();

        }
    }
}

