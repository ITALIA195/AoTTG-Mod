using System.Collections.Generic;
using UnityEngine;

namespace Mod.gui.components
{
    public class Colorbar3
    {
        private readonly Texture2D Texture = new Texture2D(0, 0);
        private readonly Pickers _pickers = new Pickers();
        private readonly Rect _rect;

        public Colorbar3(Rect rect)
        {
            _rect = rect;
        }

        public void Picker(float position, Color color)
        {
            _pickers.Add(new Picker(_rect, position, color));
        }

        public void Draw()
        {
            if (Event.current.type != EventType.Repaint) return;
            Graphics.DrawTexture(_rect.Shrink(-3), Texture, new Rect(), 0, 0, 0, 0, Color.white);
            DrawPicker();
            DoDraw();
        }

        public void Update()
        {
            foreach (Picker picker in _pickers)
                picker.Update();
        }

        private void DrawPicker()
        {
            foreach (Picker picker in _pickers)
                picker.Draw();
        }

        private void DrawLine(float x, Color color)
        {
            Graphics.DrawTexture(new Rect(_rect.x + x, _rect.y, 1, _rect.height), Texture, _rect, 0, 0, 0, 0, color);
        }

        private void DoDraw()
        {
            if (_pickers.Count <= 1) return;
            Hehexd(0, 0, _pickers[1].Position);
        }

        private void Hehexd(int id, float start, float end)
        {
            if (_pickers.Count < id + 1) return;
            for (float i = start; i < start + end; i++)
            {
                DrawLine(i, Color.Lerp(_pickers[id].Color, _pickers[id + 1].Color, i / (start + end)));
                Hehexd(id + 1, end, _pickers[id + 2].Position);
            }
        }
    }
}
