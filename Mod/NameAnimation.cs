using System;
using System.Linq;
using Mod.commands;
using Mod.manager;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Mod
{
    //TODO: Make this class better
    public sealed class NameAnimation : MonoBehaviour
    {
        private readonly float[] _colors = {0f, 0f, 0f};
        private const int Addby = 1;
        private int _stage;
        private string _color;

        public string Color => _color;

        public string ColorRainbow(string inputString) => inputString.Aggregate(string.Empty,(current, c) => current + $"[{_color = GetColor()}]{c}");
        public string GetColor()
        {
            switch (_stage)
            {
                case 0:
                    _colors[0] += Addby;
                    if (_colors[0] >= 255f)
                        _stage++;
                    break;
                case 1:
                    _colors[2] += Addby;
                    if (_colors[2] >= 255f)
                        _stage++;
                    break;
                case 2:
                    _colors[0] -= Addby;
                    if (_colors[0] <= 0f)
                        _stage++;
                    break;
                case 3:
                    _colors[1] += Addby;
                    if (_colors[1] >= 255f)
                        _stage++;
                    break;
                case 4:
                    _colors[2] -= Addby;
                    if (_colors[2] <= 0f)
                        _stage++;
                    break;
                case 5:
                    _colors[0] += Addby;
                    if (_colors[0] >= 255f)
                        _stage++;
                    break;
                case 6:
                    _colors[1] -= Addby;
                    if (_colors[1] <= 0f)
                        _stage++;
                    break;
                case 7:
                    _colors[0] += Addby;
                    if (_colors[0] >= 255f)
                        _stage = 0;
                    break;
                default:
                    _stage = 0;
                    _colors[0] = _colors[1] = _colors[2] = 0f;
                    break;
            }
            _colors[0] = Mathf.Clamp(_colors[0], 0f, 255f);
            _colors[1] = Mathf.Clamp(_colors[1], 0f, 255f);
            _colors[2] = Mathf.Clamp(_colors[2], 0f, 255f);
            return $"{_colors[0].ToInt():X2}{_colors[1].ToInt():X2}{_colors[2].ToInt():X2}";
        }
        //public static readonly Func<string, string> ColorRainbow2 = name => name.Aggregate(string.Empty, (current, c) => current + $"[{GetColor2.Invoke()}]{c}[-]");

        //private static readonly Func<string> GetColor2 = () =>
        //{
        //    lerp = Mathf.PingPong(UnityEngine.Random.Range(0, 255), 1);
        //    lerp = Mathf.Clamp(lerp, 0f, 1f);
        //    Color col = Color.Lerp(Color.red, Color.black, lerp);
        //    return $"{col.r.toInt():X2}{col.g.toInt():X2}{col.b.toInt():X2}";
        //};
    }
}
