using Mod.events;
using UnityEngine;

namespace Mod.mods
{
    [Module("wallhack")]
    public class ModWallhack
    {
        public void OnJoinedRoom()
        {
            OnEnable();
        }

        public void OnPlayerRespawn()
        {
            Core.EventManager.Fire(typeof(EventRespawn));
            OnEnable();
        }

        public void OnEnable()
        {
            const string shader = "Shader \"WallhackOverlay\" { Properties { _Color (\"Main Color\", Color) = (1,1,1,1) _MainTex (\"Base (RGB)\", 2D) = \"white\" {} } SubShader{ Pass { Lighting On Cull Back ZTest Always SetTexture [_MainTex] {}}}Fallback \"VertexLit\" }";
            foreach (GameObject o in GameObject.FindGameObjectsWithTag("titan"))
            {
                foreach (var renderComponent in o.transform.root.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    renderComponent.material = new Material(shader) { mainTexture = renderComponent.material.mainTexture, color = renderComponent.material.color };
                }
                foreach (var renderComponent in o.transform.root.GetComponentsInChildren<MeshRenderer>())
                {
                    renderComponent.material = new Material(shader) { mainTexture = renderComponent.material.mainTexture, color = renderComponent.material.color };
                }
            }
        }

        public void OnInstantiate()
        {
            OnEnable(); //TODO: Make args and get shade the titan
        }

        public void OnDisable()
        {
            Core.SendMessage("Il wallhack non puo' essere disattivato. Aspetta un restart o riconnettiti (o /ktitans).");
        }
    }
}
