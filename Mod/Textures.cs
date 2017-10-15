using System.Collections.Generic;
using Mod.manager;
using UnityEngine;

namespace Mod
{
    public class Textures : Dictionary<string, Texture2D>
    {
        //TODO: Make a style chooser where u can change Dark to Light color style.

        public static readonly Texture2D WhiteTexture = InterfaceManager.CreateTexture(255, 255, 255);
        public static readonly Texture2D BlackTexture = InterfaceManager.CreateTexture(0, 0, 0);
        public static readonly Texture2D GrayTexture = InterfaceManager.CreateTexture(Color.gray);
        public static readonly Texture2D EnabledTexture = InterfaceManager.CreateTexture("64DCFF");
        public static readonly Texture2D DisabledTexture = InterfaceManager.CreateTexture(0, 20, 0, 80);
    }
}
