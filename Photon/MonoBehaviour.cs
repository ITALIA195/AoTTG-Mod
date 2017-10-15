namespace Photon
{
    using UnityEngine;

    public class MonoBehaviour : UnityEngine.MonoBehaviour
    {
        protected new PhotonView networkView
        {
            get
            {
                Debug.LogWarning("Why are you still using networkView? should be PhotonView?");
                return PhotonView.Get(this);
            }
        }

        public PhotonView photonView => PhotonView.Get(this);
    }
}

