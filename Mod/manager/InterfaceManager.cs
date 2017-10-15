using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using Mod.gui;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Mod.manager
{
    public class InterfaceManager
    {
        public List<Gui> Guis = new List<Gui>();
        public bool Visible;

        public InterfaceManager()
        {
            GameObject obj = new GameObject("Guis");
            Guis.AddRange(new Gui[]
            {
                obj.AddComponent<GUIScoreboard>(),
                obj.AddComponent<GUIReconnect>(),
                obj.AddComponent<GUIChat>(),
                obj.AddComponent<GUIConsole>(),
                obj.AddComponent<GUIModMenu>(),
                obj.AddComponent<GUINavigator>(),
                obj.AddComponent<GUIMainMenu>(),
            });
            Object.DontDestroyOnLoad(obj);
        }

        public void Reset()
        {
            foreach (Gui gui in Guis)
                gui.Disable();
            Enable(typeof(GUIReconnect));
            Enable(typeof(GUIMainMenu));
            Enable(typeof(GUIScoreboard));
            Enable(typeof(GUIConsole));
        }


        public void Disable(string guiName)
        {
            Guis.FirstOrDefault(g => g.Name.EqualsIgnoreCase(guiName))?.Disable();
        }

        public void Disable(Type gui)
        {
            Guis.FirstOrDefault(g => g.GetType() == gui)?.Disable();
        }

        public void Enable(string guiName)
        {
            Guis.FirstOrDefault(g => g.Name.EqualsIgnoreCase(guiName))?.Enable();
        }

        public void Enable(Type gui)
        {
            Guis.FirstOrDefault(g => g.GetType() == gui)?.Enable();
        }

        public void Toggle(string guiName)
        {
            Guis.FirstOrDefault(g => g.Name.EqualsIgnoreCase(guiName))?.Toggle();
        }

        public static Texture2D CreateTexture(int test) => CreateTexture(FloatColor(test));
        public static Texture2D CreateTexture(int r, int g, int b, int a = 255) => CreateTexture(FloatColor(r, g, b, a));
        public static Texture2D CreateTexture(string color) => CreateTexture(FloatColor(color));
        public static Texture2D CreateTexture(Color color) => new Texture2D(1, 1).Set(color); //TODO: Destroy the texture after using them.

        public static Color FloatColor(int r, int g, int b, int a = 255)
        {
            return new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
        }

        public static Color FloatColor(int color)
        {
            return FloatColor(color.ToString("X"));
        }

        public static Color FloatColor(string hexColor)
        {
            if (hexColor.StartsWith("#"))
                hexColor = hexColor.Substring(1);
            return hexColor.Length != 6 ? Color.black : new Color(Int32.Parse(hexColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber) / 255f, Int32.Parse(hexColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber) / 255f, Int32.Parse(hexColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) / 255f);
        }

        public static Color32 FloatColor32(string hexColor)
        {
            if (hexColor.StartsWith("#"))
                hexColor = hexColor.Substring(1);
            return hexColor.Length != 6 ? new Color32(0,0,0,0) : new Color32(byte.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber), byte.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber), byte.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber), 255);
        }
    }
}
