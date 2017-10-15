using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Mod
{
    public class I18N : Dictionary<string, string>
    {
        private const string Pattern = @"^(\S+)\s*\=\s*(.*)$";
        private readonly string _path = Application.dataPath + "/Lang/";

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public I18N()
        {
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            if (File.Exists(_path + "messages.lang") && Core.GetMD5(_path + "messages.lang") == Core.GetMD5(Core.Assembly.GetManifestResourceStream(@"Mod.resources.messages.lang"))) //TODO: That's for debugging. Remove the check for equality
                Read(File.ReadAllLines(_path + "messages.lang"));
            else
            {
                var list = new List<string>();
                if (Core.Assembly.GetManifestResourceInfo(@"Mod.resources.messages.lang") == null) return;
                using (var stream = new StreamReader(Core.Assembly.GetManifestResourceStream(@"Mod.resources.messages.lang")))
                {
                    string line;
                    while ((line = stream.ReadLine()) != null)
                        list.Add(line);
                }
                File.WriteAllLines(_path + "messages.lang", list.ToArray());
                Read(list.ToArray());
            }
        }

        private void Read(IEnumerable<string> content)
        {
            this.Clear();
            foreach (var line in content)
            {
                if (line.StartsWith("==") || line.StartsWith("#")) continue;
                Match match = Regex.Match(line, Pattern);
                if (!match.Success) continue;
                Add(match.Groups[1].Value, match.Groups[2].Value);
            }
        }

        public string Get(string key, params object[] replacements)
        {
            return ContainsKey(key) ? string.Format(base[key], replacements) : string.Empty;
        }

        public new string this[string key] => ContainsKey(key) ? base[key] : string.Empty;
    }
}
