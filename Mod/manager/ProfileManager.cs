using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Mod.mods;
using UnityEngine;

namespace Mod.manager
{
    public class ProfileManager 
    {
        private const string PATTERN = @"^ProfileName\:\s?([^\\]*)\nPlayerName\:\s?([^\\]*)\nGuild\:\s?([^\\]*)\nChatName\:\s?([^\\]*)\nFriendName\:\s?([^\\]*)\nChatColor\:\s?([^\\]*)\nChatFormat\:\s?([^\\]*)[^\0]*";

        public Profile Profile => _profiles[_currentProfile];
        private readonly string _path = Core.AppdataPath + "Profiles\\";
        public static Profile EmptyProfile => new Profile(null, "Empty", "GUEST0000", "GUEST0000", string.Empty, string.Empty, "FFFFFF", "{0}: <color=#{1}>{2}</color>");
        private int _currentProfile;
        private readonly List<Profile> _profiles = new List<Profile>();

        public ProfileManager()
        {
        	Directory.CreateDirectory(_path);
            _currentProfile = PlayerPrefs.GetInt("M0DcurrentProfile", 0);
            Update();
        }

        public void Update()
        {
            _profiles.Clear();
            var profiles = Directory.GetFiles(_path).Where(x => x.EndsWith(".profile")).Select(Deserialize);
            foreach (var profile in profiles)
                if (_profiles.Count < 4 && profile.ProfileName != "Empty")
                    _profiles.Add(profile);
            while (_profiles.Count < 4)
                _profiles.Add(EmptyProfile);
        }

        public void SwitchProfile(Profile profile)
        {
            _currentProfile = _profiles.IndexOf(profile);
            ModNameAnimation._animation.SetName(profile.PlayerName);
            PlayerPrefs.SetInt("M0DcurrentProfile", _currentProfile);
        }

        public static int IsValid(Profile profile)
        {
            if (!Regex.IsMatch(profile.ChatColor, @"[A-Fa-f0-9]{8}|[A-Fa-f0-9]{6}"))
                return 1;
            if (Regex.Matches(profile.ChatFormat, @"(\{[0-2]\})")[1].Groups.Count >= 3)
                return 2;
            if (Core.ProfileManager.Profile.ProfileName == "Empty")
                return 3;
            return 0;
        }

        public IEnumerable<Profile> GetProfiles()
        {
            return _profiles;
        }

        private static Profile Deserialize(string file)
        {
            if (!File.Exists(file)) return null;
            Match match = Regex.Match(File.ReadAllText(file), PATTERN);
            if (match.Success)
                return new Profile(file, match);
            Console.WriteLine();
            return null;
        }






        //private readonly Dictionary<string, Profile> Profiles = new Dictionary<string, Profile>();

        //private Profile _profile;
        //

        //public ProfileManager()
        //{
        //    Initialize();
        //}

        //private void LoadProfileOrDefault()
        //{
        //    var profile = PlayerPrefs.GetString("M0Dprofile", string.Empty);
        //    if (!Profiles.ContainsKey(profile)) _profile = EmptyProfile;


        //}

        //private Profile DeserializeProfile(string profile)
        //{
        //    if (!File.Exists(_path + profile + ".profile")) return null;
        //    var matches = Regex.Matches(File.ReadAllText(_path + profile + ".profile"), @"\w+\:\s?(.+)");
        //    var obj = new string[7].Select(x => string.Empty).ToArray();
        //    for (int i = 0; i < matches.Count && i < 7; i++)
        //        obj[i] = matches[i].Groups[1].Value;
        //    if (string.IsNullOrEmpty(obj[0]))
        //        return null;
        //    if (obj[0] != profile)
        //        File.Move(_path + profile + ".profile", _path + obj[0] + ".profile");
        //    return new Profile(obj);
        //}

        //public void SelectProfile(Profile profile)
        //{
        //    profile.Load();
        //    _profile = profile;
        //}

        //public void WriteProfile(Profile profile)
        //{
        //    File.WriteAllText(_path + profile.ProfileName + ".profile", profile.ToFile());
        //}

        //private void Initialize()
        //{
        //    WriteProfile(EmptyProfile);
        //    Update();
        //    LoadProfileOrDefault();
        //}

        //public void Update()
        //{
        //    Profiles.Clear();
        //    Profile prof = null;
        //    var files = Directory.GetFiles(_path);
        //    foreach (var file in files)
        //    {
        //        if (file.EndsWith(".profile"))
        //            prof = DeserializeProfile(Path.GetFileNameWithoutExtension(file));
        //        if (prof != null)
        //            Profiles.Add(prof.ProfileName, prof);
        //    }
        //}


        //public const string FileContent = "ProfileName: \"{0}\"\n" +
        //                                  "PlayerName: \"{1}\"\n" +
        //                                  "Guild: \"{2}\"\n" +
        //                                  "ChatName: \"{3}\"\n" +
        //                                  "ChatColor: \"{4}\"\n" +
        //                                  "ChatFormat: \"{5}\"\n" +
        //                                  "FriendName: \"{6}\"";
        //private readonly string ProfileDir = Core.AppdataPath + "Profiles\\";
        //public List<Profile> Profiles { get; } = new List<Profile>(4);

        //public ProfileManager()
        //{
        //    Directory.CreateDirectory(ProfileDir);
        //    Profiles.Clear();
        //    Profiles.AddRange(GetProfiles());
        //}

        //public Profile WriteProfile(Profile profile)
        //{
        //    Directory.CreateDirectory(ProfileDir);
        //    var content = string.Format(FileContent, profile.Properties);
        //    for (int i = 0; i < 4; i++)
        //    {
        //        if (!File.Exists(ProfileDir + $"{profile.ProfileName}.profile"))
        //        {
        //            File.WriteAllText(ProfileDir + $"{profile.ProfileName}.profile", content);
        //            return profile;
        //        }
        //    }
        //    return profile;
        //}

        //private Profile DeserializeProfile(string file)
        //{
        //    Core.Log("Trying to Deserialize: " + file);
        //    var content = File.ReadAllText(file);
        //    Match match = Match(content);
        //    if (!match.Success)
        //        return new Profile("INVALID", "INVALID PROFILE FILE", "INVALID PROFILE FILE", "INVALID PROFILE FILE", "INVALID PROFILE FILE", "INVALID PROFILE FILE", "INVALID PROFILE FILE");
        //    return new Profile(file, match);
        //}

        //private bool Matches(string content)
        //{
        //    Core.Log(Match(content));
        //    return Match(content);
        //}
        //private MatchCollection Match(string content) => Regex.Matches(content, @"\w+\:\s""(.*)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        //private IEnumerable<Profile> GetProfiles()
        //{
        //    var profiles = Directory.GetFiles(ProfileDir).Where(x => x.EndsWith(".profile")).Where(Matches).Select(DeserializeProfile).ToList();
        //    foreach (var profile in profiles)
        //        Core.Log(profile);
        //    while (profiles.Count < 4)
        //        profiles.Add(EmptyProfile);
        //    return profiles;
        //}

        //public void LoadProfile(Profile profile)
        //{
        //    RefStrings.PlayerName = profile.PlayerName;
        //    RefStrings.ChatFormat = profile.ChatFormat;
        //    RefStrings.ChatName = profile.ChatName;
        //    RefStrings.GuildName = profile.GuildName;
        //    RefStrings.FriendName = profile.FriendName;
        //    RefStrings.ChatColor = profile.ChatColor;
        //}

        //public Profile Profile => _profile;
        //public string PlayerName => _profile.PlayerName;
        //public string Guild => _profile.GuildName;
        //public string ChatName => _profile.ChatName;
        //public string ChatColor => _profile.ChatColor;
        //public string ChatFormat => _profile.ChatFormat;
        //public string FriendName => _profile.FriendName;
    }
}
