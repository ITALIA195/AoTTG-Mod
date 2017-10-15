using System.Linq;
using Object = UnityEngine.Object;

namespace Mod.mods
{
    [Module("fireworks")]
    public class ModFireworks
    {
        private HERO _hero;

        public void Update()
        {
            if ((_hero = Core.Hero) == null)
                return;
            Object.Destroy(PhotonNetwork.Instantiate("FX/flareBullet1", _hero.transform.position, _hero.transform.rotation,
                    PhotonNetwork.playerList.Where(player => !player.isMasterClient && !player.isLocal)
                        .Select(player => player.ID)
                        .ToArray()).GetComponent<FlareMovement>());
        }
    }
}
