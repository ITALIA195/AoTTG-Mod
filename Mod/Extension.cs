using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Mod.manager;
using SimpleJSON;
using UnityEngine;

namespace Mod
{
    public static class Extension
    {
        public static string DeleteEnd(this string str, int count)
        {
            return str.Substring(0, str.Length - count);
        }

        public static bool ToBool(this string str)
        {
            return str.EqualsIgnoreCase("true");
        }

        /// <summary>
        /// Converts <see cref="string"/> to <see cref="int"/> safely with TryParse
        /// </summary>
        public static int ToInt(this string str)
        {
            if (int.TryParse(str, out int num))
                return num;
            return -1;
        }

        public static string HexColor(this object str)
        {
            StringBuilder builder = new StringBuilder(str as string);
            foreach (Match match in Regex.Matches(builder.ToString(), @"\[([0-9a-fA-F]{6})\]"))
            {
                builder.Replace(match.Value, $"<color=#{match.Groups[1].Value}>");
                builder.Append("</color>");
            }
            return builder.Replace("[-]", string.Empty).ToString();
        }

        public static string AsString(this JSONNode node)
        {
            return node.ToString().Substring(1, node.ToString().Length - 2);
        }
        
        public static bool EqualsIgnoreCase(this string str, string compareTo)
        {
            return str.Equals(compareTo, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool EqualsIgnoreCase(this string str, params string[] strs)
        {
            return strs.Any(str.EqualsIgnoreCase);
        }

        public static bool ContainsIgnoreCase(this string str, string compareTo)
        {
            return str.ToLower().Contains(compareTo.ToLower());
        }

        public static string RemoveColors(this string str)
        {
            if (str == null) return string.Empty;
            return Regex.Replace(str, @"(?:\[[A-Fa-f0-9]{6}\]|\<color=(?:\#[A-Fa-f0-9]{6,8}|\w+)>|<\/color>|\[-\])", string.Empty);
        }

        public static string HexColor(this Color32 color)
        {
            return $"{color.r:X2}{color.g:X2}{color.b:X2}";
        }

        public static int ToInt(this float obj)
        {
            return Convert.ToInt32(obj);
        }

        public static Texture2D Set(this Texture2D texture, Color color)
        {
            texture.SetPixel(1, 1, color);
            texture.Apply();
            return texture;
        }

        public static Texture2D Set(this Texture2D texture, string color)
        {
            return texture.Set(InterfaceManager.FloatColor(color));
        }
    }
}
