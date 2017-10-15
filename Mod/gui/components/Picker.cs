using System;
using System.Collections.Generic;
using Mod.manager;
using UnityEngine;

namespace Mod.gui.components
{
    public class Picker : IComparable
    {
        private readonly Texture2D texture = new Texture2D(1, 1);
        private readonly Rect _colorBar;
        public float Position { get; }
        public Color Color { get; }
        private readonly bool _edge;
        private readonly Texture2D _texture;

        public Picker(Rect colorBar, float position, Color color, bool edge = false)
        {
            _colorBar = colorBar;
            Color = color;
            Position = position * colorBar.width;
            _edge = edge;
            _texture = InterfaceManager.CreateTexture(color);
        }

        public void Draw()
        {
            Rect rect = GetPickerRect();
            Graphics.DrawTexture(new Rect(rect.x + 2, rect.y + 2, rect.width - 4, rect.height - 4), _texture); // Color
            Graphics.DrawTexture(new Rect(rect.x, rect.y, 1, rect.height), Textures.EnabledTexture); // Left
            Graphics.DrawTexture(new Rect(rect.x, rect.y, rect.width, 1), Textures.EnabledTexture); // Upper
            Graphics.DrawTexture(new Rect(rect.x + rect.width - 1, rect.y, 1, rect.width), Textures.EnabledTexture); // Right
            Drawing.DrawLine(new Vector2(rect.x, rect.y + rect.height), new Vector3(_colorBar.x + Position, _colorBar.y), UnityEngine.Color.cyan, 1, false); // Line left
            Drawing.DrawLine(new Vector2(rect.x + rect.width, rect.y + rect.height), new Vector3(_colorBar.x + Position, _colorBar.y), UnityEngine.Color.cyan, 1, false); // Line right
        }

        public void Update()
        {
            Core.Log("MouseOver? : " + IsMouseHover);
        }

        public bool IsMouseHover
        {
            get
            {
                float mouseX = Input.mousePosition.x;
                float mouseY = -(Input.mousePosition.y - Screen.height + 1);
                Rect rect = GetPickerRect();
                if (mouseX > rect.x && mouseX < rect.x + rect.width && mouseY > rect.y && mouseY < rect.y + rect.height)
                    return true;
                return false;


//                Rect hitbox = _colorBar;
//
//                Vector3 pos = Input.mousePosition;
//                var y1 = -(pos.y - Screen.height + 1);
//                bool x = hitbox.x <= pos.x && pos.x <= hitbox.x + hitbox.width;
//                bool y = hitbox.y <= y1 && y1 <= hitbox.y + hitbox.height;
//                return x && y;
            }
        }

        //WIDTH: 12 || 6
        //HEIGHT: 16 || 4

        private Rect GetPickerRect()
        {
            return new Rect(Position + _colorBar.x - 6, _colorBar.y - 16, 12, 12);
        }

        public bool Selected { get; set; }

        private void Texture(Rect rect, Color color) => Texture(rect.x, rect.y, rect.width, rect.height, color);
        private void Texture(float x, float y, float width, float height, Color color)
        {
            Graphics.DrawTexture(new Rect(x, y, width, height), texture, new Rect(x, y, width, height), 0, 0, 0, 0, color);
        }

        public int CompareTo(object obj)
        {
            Picker picker = obj as Picker;
            if (picker == null)
                return 0;
            if (picker.Position > Position)
                return -1;
            if (picker.Position < Position)
                return 1;
            return 0;
        }
    }
}
