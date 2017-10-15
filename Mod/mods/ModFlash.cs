using UnityEngine;

namespace Mod.mods
{
    [Module("flash")]
    public class ModFlash
    {
        public void Update()
        {
            if (!Input.GetKey(KeyCode.F)) return;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity) && Core.Hero != null)
            {
                var body = Core.Hero.GetComponent<Rigidbody>();
                Vector3 vector = hitInfo.point - GameObject.Find("MainCamera").transform.position;
                Core.Hero.transform.position = Core.Hero.transform.position + vector.normalized * 3f;
                body.velocity = vector.normalized * body.velocity.magnitude;
            }
        }
    }
}
