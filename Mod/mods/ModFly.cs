using UnityEngine;

namespace Mod.mods
{
    [Module("fly", true)]
    public class ModFly
    {
        public void Update()
        {
            if (!PhotonNetwork.inGame || !Input.GetKey(KeyCode.LeftAlt)) return;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity) && Core.Hero != null)
            {
                Vector3 vector = hitInfo.point - GameObject.Find("MainCamera").transform.position;
                var body = Core.Hero.GetComponent<Rigidbody>();
                body.velocity = vector.normalized * body.velocity.magnitude;
            }
        }
    }
}
