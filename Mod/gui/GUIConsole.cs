using Mod.manager;
using UnityEngine;

namespace Mod.gui
{
    public class GUIConsole : Gui
    {
        private readonly GUIStyle _style = new GUIStyle
        {
            alignment = TextAnchor.LowerRight,
            normal = { textColor = Color.white }
        };

        public void OnGUI()
        {
            if (!ModManager.Find("module.showconsole").Enabled) return;
            GUI.Label(new Rect(Screen.width - 500, Screen.height - 300, 500, 300), Core.LogManager.Logs, _style);
        }
    }
}
