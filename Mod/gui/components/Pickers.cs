using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.gui.components
{
    public class Pickers : List<Picker>
    {
        public new void Add(Picker picker)
        {
            base.Add(picker);
            Sort();
        }
    }
}
