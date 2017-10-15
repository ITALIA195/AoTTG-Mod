using System;

public class PhotonMessageInfo
{
    public PhotonView photonView;
    public readonly PhotonPlayer sender;
    private readonly int timeInt;

    public PhotonMessageInfo()
    {
        this.sender = PhotonNetwork.player;
        this.timeInt = (int) (PhotonNetwork.time * 1000.0);
        this.photonView = null;
    }

    public PhotonMessageInfo(PhotonPlayer player, int timestamp, PhotonView view)
    {
        this.sender = player;
        this.timeInt = timestamp;
        this.photonView = view;
    }

    public override string ToString()
    {
        return $"{sender}'";
    }

    public double Timestamp => timeInt / 1000.0;
}

