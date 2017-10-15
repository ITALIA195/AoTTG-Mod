using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mod.gui.components
{
    public class ColorBar
    {
        private readonly List<Picker> _pickers = new List<Picker>();
        private readonly Texture2D texture = new Texture2D(1, 1);
        private readonly Rect _rect;

        public ColorBar(Rect rect, params Picker[] pickers)
        {
            _rect = rect;
            _pickers.AddRange(pickers);
            _pickers.Add(new Picker(rect, 0f, Color.black, true));
            _pickers.Add(new Picker(rect, 1f, Color.black, true));
            _pickers.Sort();
        }

        public void AddPicker(Picker picker)
        {
            _pickers.Add(picker);
            _pickers.Sort();
        }

        private void DrawColor()
        {
            for (int i = 0; i < _pickers.Count - 1; i++)
            {
                Picker current = _pickers[i];
                Picker following = _pickers[i + 1];
                for (float j = current.Position; j < following.Position; j++)
                    DrawVerticalLine(j, Color.Lerp(current.Color, following.Color, (j - current.Position) / (following.Position - current.Position)));
            }
        }


        public void Draw()
        {
            if (Event.current.type != EventType.Repaint) return;
            Graphics.DrawTexture(_rect.Shrink(-3), texture, new Rect(), 0, 0, 0, 0, Color.white);
            DrawColor();
            DrawPickers();
        }

        private void DrawVerticalLine(float x, Color color)
        { //                                          * _rect.width 
            Graphics.DrawTexture(new Rect(_rect.x + x, _rect.y, 1, _rect.height), texture, _rect, 0, 0, 0, 0, color);
        }

        private void DrawPickers()
        {
            foreach (Picker picker in _pickers)
                picker.Draw();
        }
    }
}
