using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.animation
{
    public class AnimationNoGameNoLife : Animation
    {
        public AnimationNoGameNoLife(string playerName, AnimationType type, int fadenumber) : base (playerName, type, fadenumber, "FF5FDD", "FF005E", "00AFFF", "63EEFF")
        {
        }
    }
}
