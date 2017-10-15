using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Mod.manager;

namespace Mod
{
    public class Profile
    {
        private const string PATTERN = @"^ProfileName\:\s?([^\\]*)\nPlayerName\:\s?([^\\]*)\nGuild\:\s?([^\\]*)\nChatName\:\s?([^\\]*)\nFriendName\:\s?([^\\]*)\nChatColor\:\s?([^\\]*)\nChatFormat\:\s?([^\\]*)[^\0]*";

        public Profile(string file, string profileName, string playerName, string guild, string chatName, string friendName, string chatColor, string chatFormat)
        {
            Path = file;
            ProfileName = profileName;
            PlayerName = playerName;
            Guild = guild;
            ChatName = chatName;
            FriendName = friendName;
            ChatColor = chatColor;
            ChatFormat = chatFormat;
        }

        public Profile(string file, Match content)
        {
            Path = file;
            ProfileName = content.Groups[1].Value;
            PlayerName = content.Groups[2].Value;
            Guild = content.Groups[3].Value;
            ChatName = content.Groups[4].Value;
            FriendName = content.Groups[5].Value;
            ChatColor = content.Groups[6].Value;
            ChatFormat = content.Groups[7].Value;
        }

        public override string ToString()
        {
            return $"ProfileName: {ProfileName}\n" +
                   $"PlayerName: {PlayerName}\n" +
                   $"Guild: {Guild}\n" +
                   $"ChatName: {ChatName}\n" +
                   $"FriendName: {FriendName}\n" +
                   $"ChatColor: {ChatColor}\n" +
                   $"ChatFormat: {ChatFormat}";
        }

        private void Deserialize(string file)
        {
            Path = file;
            Match match = Regex.Match(File.ReadAllText(file), PATTERN);
            if (match.Success)
            {
                ProfileName = match.Groups[1].Value;
                PlayerName = match.Groups[2].Value;
                Guild = match.Groups[3].Value;
                ChatName = match.Groups[4].Value;
                FriendName = match.Groups[5].Value;
                ChatColor = match.Groups[6].Value;
                ChatFormat = match.Groups[7].Value;
            }
        }

        public Profile LoadFromFile(string file)
        {
            Deserialize(file);
            return this;
        }

        public void Delete()
        {
            if (Path == null || Path.EndsWith("Empty.profile") || !File.Exists(Path)) return;
            File.Delete(Path);
        }

        public Profile Save()
        {
            var name = System.IO.Path.GetInvalidFileNameChars().Aggregate(ProfileName, (current, @char) => current.Replace(@char, '_'));
            File.WriteAllText(Core.AppdataPath + $"Profiles\\{name}.profile", ToString());
            if (Path != Core.AppdataPath + $"Profiles\\{name}.profile")
                File.Delete(Path);
            return this;
        }

        public Profile Discard()
        {
            if (Path == null || !File.Exists(Path))
                return ProfileManager.EmptyProfile;
            Deserialize(Path);
            return this;
        }

        public string Path { get; set; }
        public string ProfileName { get; set; }
        public string PlayerName { get; set; }
        public string Guild { get; set; }
        public string ChatName { get; set; }
        public string FriendName { get; set; }
        public string ChatColor { get; set; }
        public string ChatFormat { get; set; }

    }
}
