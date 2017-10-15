using UnityEngine;

namespace Mod.mods
{
    [Module("scrollmoves")]
    public class ModScrollMoves
    {
        private HERO _hero;
        private float x;

        public void Update()
        {
            if (_hero == null) _hero = Core.GetHero(PhotonNetwork.player.ID);
            if (_hero == null) return;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity) || Input.mouseScrollDelta.y < 0.1f) return;
            x += Input.mouseScrollDelta.y;
            Vector3 vector = hitInfo.point - GameObject.Find("MainCamera").transform.position;
            _hero.transform.position = _hero.transform.position + vector.normalized * x * 4f;
            var body = _hero.GetComponent<Rigidbody>();
            body.velocity = vector.normalized * body.velocity.magnitude;
        }
    }
}
