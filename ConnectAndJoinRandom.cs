using Photon;
using System;
using Mod.manager;
using UnityEngine;
using Mod;

public class ConnectAndJoinRandom : Photon.MonoBehaviour
{
    public bool AutoConnect = true;
    private bool ConnectInUpdate = true;

    public virtual void OnConnectedToMaster()
    {
        if (PhotonNetwork.networkingPeer.AvailableRegions != null)
        {
            Debug.LogWarning(string.Concat(new object[] { "List of available regions counts ", PhotonNetwork.networkingPeer.AvailableRegions.Count, ". First: ", PhotonNetwork.networkingPeer.AvailableRegions[0], " \t Current Region: ", PhotonNetwork.networkingPeer.CloudRegion }));
        }
        Core.Log("Succesfully connected to Master (OnConnectedToMaster())");
        PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Core.Log("Error connecting to Photon, cause: " + cause);
    }

    public virtual void OnJoinedLobby()
    {
        Core.Log("Successfully joined Lobby.");
        Core.ModManager.CallMethod("OnJoinedLobby");
    }

    public void OnJoinedRoom()
    {
        Core.Log("Successfully joined into a Room.");
        Core.ModManager.CallMethod("OnJoinedRoom");
    }

    public virtual void OnPhotonRandomJoinFailed()
    {
        Core.Log("No room found (Could not join a random room!), PUN Will create one for you!");
        RoomOptions roomOptions = new RoomOptions {
            maxPlayers = 4
        };
        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }

    public virtual void Start()
    {
        PhotonNetwork.autoJoinLobby = false;
    }

    public virtual void Update()
    {
        if ((this.ConnectInUpdate && this.AutoConnect) && !PhotonNetwork.connected)
        {
            this.ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings("2." + Application.loadedLevel);
        }
    }
}

