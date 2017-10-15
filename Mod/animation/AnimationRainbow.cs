using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.animation
{
    public class AnimationRainbow : Animation
    {
        public AnimationRainbow(string playerName, AnimationType type, int fadenumber) : base (playerName, type, fadenumber, "ff0000", "ff7f00", "ffff00", "00ff00", "00ffff", "0000FF", "8b00FF")
        {
        }
    }
}
