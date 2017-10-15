using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using Mod;
using UnityEngine;

public class PhotonPlayer
{
    private int actorID;
    public readonly bool isLocal;
    private string nameField;
    public object TagObject;

    protected internal PhotonPlayer(bool isLocal, int actorID, Hashtable properties)
    {
        this.actorID = -1;
        this.nameField = string.Empty;
        this.customProperties = new Hashtable();
        this.isLocal = isLocal;
        SetGuild = x => SetCustomProperties(new Hashtable { { PhotonPlayerProperty.guildName, x } });
        SetName = x => SetCustomProperties(new Hashtable { { PhotonPlayerProperty.name, x } });
        this.actorID = actorID;
        this.InternalCacheProperties(properties);
    }

    public PhotonPlayer(bool isLocal, int actorID, string name)
    {
        this.actorID = -1;
        this.nameField = string.Empty;
        this.customProperties = new Hashtable();
        this.isLocal = isLocal;
        SetGuild = x => SetCustomProperties(new Hashtable { { PhotonPlayerProperty.guildName, x } });
        SetName = x => SetCustomProperties(new Hashtable { { PhotonPlayerProperty.name, x } });
        this.actorID = actorID;
        this.nameField = name;
    }

    public bool Has(string prop)
    {
        return customProperties[prop] != null;
    }

    public PhotonPlayer GetNextFor(PhotonPlayer currentPlayer)
    {
        if (currentPlayer == null)
        {
            return null;
        }
        return this.GetNextFor(currentPlayer.ID);
    }

    public PhotonPlayer GetNextFor(int currentPlayerId)
    {
        if (((PhotonNetwork.networkingPeer == null) || (PhotonNetwork.networkingPeer.mActors == null)) || (PhotonNetwork.networkingPeer.mActors.Count < 2))
        {
            return null;
        }
        Dictionary<int, PhotonPlayer> mActors = PhotonNetwork.networkingPeer.mActors;
        int num = 2147483647;
        int num2 = currentPlayerId;
        foreach (int num3 in mActors.Keys)
        {
            if (num3 < num2)
            {
                num2 = num3;
            }
            else if ((num3 > currentPlayerId) && (num3 < num))
            {
                num = num3;
            }
        }
        return ((num == 2147483647) ? mActors[num2] : mActors[num]);
    }

    internal void InternalCacheProperties(Hashtable properties)
    {
        if (((properties != null) && (properties.Count != 0)) && !this.customProperties.Equals(properties))
        {
            if (properties.ContainsKey((byte) 255))
            {
                this.nameField = (string) properties[(byte) 255];
            }
            this.customProperties.MergeStringKeys(properties);
            this.customProperties.StripKeysWithNullValues();
        }
    }

    internal void InternalChangeLocalID(int newID)
    {
        if (!this.isLocal)
        {
            Debug.LogError("ERROR You should never change PhotonPlayer IDs!");
        }
        else
        {
            this.actorID = newID;
        }
    }

    public void SendPrivateMessage(string msg)
    {
        Core.SendMessage($"<color=#1068D4>PM<color=#108CD4>></color></color> <color=#{RefStrings.MessageColor}>{HexName}: {msg}</color>", PhotonNetwork.player);
        FengGameManagerMKII.instance.photonView.RPC("Chat", this, $"<color=#1068D4>PM<color=#108CD4>></color></color> <color=#{RefStrings.MessageColor}>{HexName}: {msg}</color>", string.Empty);
    }

    public void SetCustomProperties(Hashtable propertiesToSet)
    {
        if (propertiesToSet != null)
        {
            this.customProperties.MergeStringKeys(propertiesToSet);
            this.customProperties.StripKeysWithNullValues();
            Hashtable actorProperties = propertiesToSet.StripToStringKeys();
            if ((this.actorID > 0) && !PhotonNetwork.offlineMode)
                PhotonNetwork.networkingPeer.OpSetCustomPropertiesOfActor(actorNr: this.actorID, actorProperties: actorProperties, broadcast: true, channelId: 0);
            NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, this, propertiesToSet);
        }
    }

    public Hashtable allProperties
    {
        get
        {
            Hashtable target = new Hashtable();
            target.Merge(this.customProperties);
            target[(byte) 255] = this.name;
            return target;
        }
    }

    public Hashtable customProperties { get; }

    public string name { get { return this.nameField; } set { this.nameField = value; } }

    public int ID => this.actorID;

    public bool isMasterClient => Equals(PhotonNetwork.networkingPeer.mMasterClient, this);

    public string ToStringHex() => HexName.Trim() == string.Empty ? $"{ID:00}" : $"{HexName}";

    public override string ToString() => Name.RemoveColors().Trim() == string.Empty ? $"{ID:00}" : $"{Name.RemoveColors()} ({ID:00})";

    public string ToStringFull() => $"#{this.ID:00} '{this.name}' {this.customProperties.ToStringFull()}";

    public override bool Equals(object p) => p is PhotonPlayer && this.GetHashCode() == ((PhotonPlayer)p).GetHashCode();

    public static PhotonPlayer Find(int ID) => PhotonNetwork.playerList.FirstOrDefault(player => player.ID == ID);

    public override int GetHashCode() => this.ID;

    public PhotonPlayer GetNext() => this.GetNextFor(this.ID);


    public readonly Action<string> SetName;
    public string Name => customProperties[PhotonPlayerProperty.name] as string;
    public string HexName => (customProperties[PhotonPlayerProperty.name].HexColor() ?? customProperties[PhotonPlayerProperty.name]) as string;
    public readonly Action<string> SetGuild;
    public string Guild => customProperties[PhotonPlayerProperty.guildName] as string;
    public string HexGuild => (customProperties[PhotonPlayerProperty.guildName].HexColor() ?? customProperties[PhotonPlayerProperty.guildName]) as string;
}

