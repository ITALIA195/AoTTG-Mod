using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Mod.exceptions;
using Mod.gui;
using UnityEngine;
using ArgumentException = Mod.exceptions.ArgumentException;

namespace Mod
{
    public sealed class Chat
    {
        public static Color32 _FadeStartColor = Color.yellow;
        public static Color32 _FadeEndColor = Color.red;

        public static readonly Func<string, string> RemoveSize = message => Regex.Replace(message, @"\</?size(?:={1}\d+)?\>", "", RegexOptions.IgnoreCase);

        public static readonly Func<string, string> Fade = message =>
        {
            message = message.RemoveColors();
            bool increasing = true;
            float color = 0f;
            return message.Aggregate(string.Empty, (msg, c) => msg + $"<color=#{Color32.Lerp(_FadeStartColor, _FadeEndColor, color = ((increasing = color < 0.1f) || color <= 0.9f && increasing) ? color + 0.1f : color - 0.1f).HexColor()}>{c}</color>");
        };

        public static readonly Func<string, string> Fade2 = message =>
        {
            message = message.RemoveColors();
            StringBuilder builder = new StringBuilder();
            float color = 0f;
            foreach (char c in message)
                builder.Append($"<color=#{Color32.Lerp(_FadeStartColor, _FadeEndColor, color += 1f/message.Length).HexColor()}>{c}</color>");
            return builder.ToString();
        };

        public static readonly Func<string, string> AdjustTags = message => Resolve(message, CreateTagList(message));
        internal static readonly Func<string, List<Entry>> CreateTagList = message => (from Match match in Regex.Matches(message, @"\<(\/?)\w+(?:\=*\#*\w*)\>") select new Entry(match.Value, match.Groups[2].Value, match.Groups[1].Value != "/", match.Index, match.Length)).ToList();

        private static string Resolve(string message, List<Entry> foundTags)
        {
            var openTag = new List<Entry>();
            var closeTag = new List<Entry>();
            foreach (Entry entry in foundTags)
            {
                if (entry.isStartTag)
                    openTag.Add(entry);
                else
                    closeTag.Add(entry);
            }
            openTag.Reverse();

            if (closeTag.Count > 0)
                for (int i = 0; i < closeTag.Count; i++)
                {
                    if (!openTag[i].tagName.EqualsIgnoreCase(closeTag[i].tagName))
                        return Resolve(closeTag[i].Remove(message), CreateTagList(closeTag[i].Remove(message)));
                    openTag[i].resolved = true;
                    closeTag[i].resolved = true;
                }

            foreach (Entry t in foundTags)
                if (!t.resolved)
                    return Resolve(t.Remove(message), CreateTagList(t.Remove(message)));

            return message;
        }

        public class Entry
        {
            public readonly bool isStartTag;
            public readonly string tagFull;
            public readonly string tagName;
            public readonly int index;
            public readonly int length;
            public bool resolved;   

            public Entry(string tagFull, string tagName, bool isStartTag, int index, int length)
            {
                this.tagName = tagName;
                this.isStartTag = isStartTag;
                this.index = index;
                this.length = length;
                this.tagFull = tagFull;
            }

            public string Remove(string str)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(str);
                stringBuilder.Replace(tagFull, string.Empty, index, length);
                return stringBuilder.ToString();
            }
        }
    }
}
