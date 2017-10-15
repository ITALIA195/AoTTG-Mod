using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Mod.manager;
using UnityEngine;

namespace Mod.animation
{
    public abstract class Animation
    {
        private readonly AnimationType _type;
        private string _playerName;
        private string[] _colors;
        private int _done;

        protected Animation(string playerName, AnimationType type, int times, params string[] colors)
        {
            _playerName = playerName;
            _type = type;
            _colors = CreateFade(colors, times);
        }

        public void SetName(string name)
        {
            _playerName = name;
        }

        private static string[] CreateFade(IList<string> colors, int times)
        {
            var l = new List<string>();
            for (var i = 0; i < colors.Count; i++)
            {
                if (i > colors.Count-2)
                    l.AddRange(Fade(colors[i], colors[0], times));
                else
                    l.AddRange(Fade(colors[i], colors[i+1], times));
            }
            return l.ToArray();
        }

        private static IEnumerable<string> Fade(string color1, string color2, int times)
        {
            var list = new List<string>();
            for (var i = 0; i < times; i++)
                list.Add(Color32.Lerp(InterfaceManager.FloatColor32(color1), InterfaceManager.FloatColor32(color2), 255f / times * i / 255f).HexColor());
            return list.ToArray();
        }

        private string ObtainColor(int index)
        {
            while (index > _colors.Length-1) index -= _colors.Length;
            return "[" + _colors[index] + "]";
        }

        public string NextFrame()
        {
            _done += 1;
            if (_done > _colors.Length - 1) _done = 0;
            switch (_type)
            {
                case AnimationType.RightToLeft:
                    return LeftToRight(_playerName, _done);
                case AnimationType.LeftToRight:
                    return RightToLeft(_playerName, _done);
                case AnimationType.Cycle:
                    return Cycle(_playerName, _done);
                case AnimationType.Fader:
                    return Fader(_playerName, _done);
                case AnimationType.LeftRightLeft:
                    return LeftRightLeft(_playerName);
                default:
                    return "Type is out of range: " + _type;
            }
        }

        private string LeftRightLeft(string name)
        {
            throw new NotImplementedException("This fade is still work in progress");
        }

        private string LeftToRight(string name, int n) //TODO: Make it a single method that has as 2nd arg the index
        {
            var index = 0;
            while (true)
            {
                if (index != name.Length)
                {
                    name = name.Insert(index, ObtainColor(n));
                    index = index + 9;
                    n += 1;
                    continue;
                }
                return name;
            }
        }

        private string Cycle(string name, int n)
        {
            return name.Insert(0, ObtainColor(n)); ;
        }

        private string RightToLeft(string name, int n)
        {
            var index = name.Length-1;
            while (index >= 0)
            {
                name = name.Insert(index, ObtainColor(n));
                index -= 1;
                n += 1;
            }
            return name;
        }

        private static string[] GetColors(string name)
        {
            name = name.Replace("[-]", "");
            MatchCollection matches = Regex.Matches(name, @"(?:(?:\[([A-Fa-f0-9]{6})\])?(.))");
            return matches.Cast<Match>().Select(match => match.Groups[1].Value == string.Empty ? "000000" : match.Groups[1].Value).ToArray();
        }

        private string Fader(string name, int n)
        {
            _colors = GetColors(name);
            if (_colors.Length == 0)
                return name;
            return RightToLeft(name.RemoveColors(), n);
        }
    }
}
