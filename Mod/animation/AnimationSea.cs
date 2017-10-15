using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.animation
{
    public class AnimationSea : Animation
    {
        public AnimationSea(string playerName, AnimationType type, int fadenumber) : base(playerName, type, fadenumber, "00aaff", "00e3ff", "00ffe3", "cbffee", "dbffe8", "ffffff", "0052cc")
        {
        }
    }
}
