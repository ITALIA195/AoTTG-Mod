using ExitGames.Client.Photon;
using System;
using UnityEngine;

public class CostumeConverter
{
    private static int DivisionToInt(Division id)
    {
        return Convert.ToInt32(id);
    }

    public static void HeroCostumeToLocalData(HeroCostume costume, string slot)
    {
        slot = slot.ToUpper();
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.sex, SexToInt(costume.Sex));
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.costumeId, costume.CostumeId);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.heroCostumeId, costume.Id);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.cape, !costume.Cape ? 0 : 1);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.hairInfo, costume.HairInfo.id);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.eye_texture_id, costume.EyeTextureId);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.beard_texture_id, costume.BeardTextureId);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.glass_texture_id, costume.GlassTextureId);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.skin_color, costume.SkinColor);
        PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.hair_color1, costume.HairColor.r);
        PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.hair_color2, costume.HairColor.g);
        PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.hair_color3, costume.HairColor.b);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.division, DivisionToInt(costume.Division));
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statSPD, costume.Stat.SPD);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statGAS, costume.Stat.GAS);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statBLA, costume.Stat.BLA);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statACL, costume.Stat.ACL);
        PlayerPrefs.SetString(slot + PhotonPlayerProperty.statSKILL, costume.Stat.skillId);
    }

    public static void HeroCostumeToPhotonData(HeroCostume costume, PhotonPlayer player)
    {
        Hashtable propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.sex, SexToInt(costume.Sex));
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.costumeId, costume.CostumeId);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.heroCostumeId, costume.Id);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.cape, costume.Cape);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.hairInfo, costume.HairInfo.id);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.eye_texture_id, costume.EyeTextureId);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.beard_texture_id, costume.BeardTextureId);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.glass_texture_id, costume.GlassTextureId);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.skin_color, costume.SkinColor);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.hair_color1, costume.HairColor.r);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.hair_color2, costume.HairColor.g);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.hair_color3, costume.HairColor.b);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.division, DivisionToInt(costume.Division));
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statSPD, costume.Stat.SPD);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statGAS, costume.Stat.GAS);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statBLA, costume.Stat.BLA);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statACL, costume.Stat.ACL);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statSKILL, costume.Stat.skillId);
        player.SetCustomProperties(propertiesToSet);
    }

    public static void HeroCostumeToPhotonData2(HeroCostume costume, PhotonPlayer player)
    {
        Hashtable propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.sex, SexToInt(costume.Sex));
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        int costumeId = costume.CostumeId;
        if (costumeId == 26)
        {
            costumeId = 25;
        }
        propertiesToSet.Add(PhotonPlayerProperty.costumeId, costumeId);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.heroCostumeId, costume.Id);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.cape, costume.Cape);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.hairInfo, costume.HairInfo.id);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.eye_texture_id, costume.EyeTextureId);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.beard_texture_id, costume.BeardTextureId);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.glass_texture_id, costume.GlassTextureId);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.skin_color, costume.SkinColor);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.hair_color1, costume.HairColor.r);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.hair_color2, costume.HairColor.g);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.hair_color3, costume.HairColor.b);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.division, DivisionToInt(costume.Division));
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statSPD, costume.Stat.SPD);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statGAS, costume.Stat.GAS);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statBLA, costume.Stat.BLA);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statACL, costume.Stat.ACL);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statSKILL, costume.Stat.skillId);
        player.SetCustomProperties(propertiesToSet);
    }

    private static Division IntToDivision(int id)
    {
        if (id == 0)
        {
            return Division.TheGarrison;
        }
        if (id == 1)
        {
            return Division.TheMilitaryPolice;
        }
        if ((id != 2) && (id == 3))
        {
            return Division.TraineesSquad;
        }
        return Division.TheSurveryCorps;
    }

    private static Sex IntToSex(int id)
    {
        if (id == 0)
        {
            return Sex.Female;
        }
        if (id == 1)
        {
            return Sex.Male;
        }
        return Sex.Male;
    }

    private static UniformType IntToUniformType(int id)
    {
        if (id == 0)
        {
            return UniformType.CasualA;
        }
        if (id == 1)
        {
            return UniformType.CasualB;
        }
        if (id != 2)
        {
            if (id == 3)
            {
                return UniformType.UniformB;
            }
            if (id == 4)
            {
                return UniformType.CasualAHSS;
            }
        }
        return UniformType.UniformA;
    }

    public static HeroCostume LocalDataToHeroCostume(string slot)
    {
        slot = slot.ToUpper();
        if (!PlayerPrefs.HasKey(slot + PhotonPlayerProperty.sex))
        {
            return HeroCostume.Costume[0];
        }
        HeroCostume costume = new HeroCostume();
        costume = new HeroCostume {
            Sex = IntToSex(PlayerPrefs.GetInt(slot + PhotonPlayerProperty.sex)),
            Id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.heroCostumeId),
            CostumeId = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.costumeId),
            Cape = (PlayerPrefs.GetInt(slot + PhotonPlayerProperty.cape) != 1) ? false : true,
            HairInfo = (costume.Sex != Sex.Male) ? CostumeHair.hairsF[PlayerPrefs.GetInt(slot + PhotonPlayerProperty.hairInfo)] : CostumeHair.hairsM[PlayerPrefs.GetInt(slot + PhotonPlayerProperty.hairInfo)],
            EyeTextureId = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.eye_texture_id),
            BeardTextureId = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.beard_texture_id),
            GlassTextureId = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.glass_texture_id),
            SkinColor = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.skin_color),
            HairColor = new Color(PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.hair_color1), PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.hair_color2), PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.hair_color3)),
            Division = IntToDivision(PlayerPrefs.GetInt(slot + PhotonPlayerProperty.division)),
            Stat = new HeroStat()
        };
        costume.Stat.SPD = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statSPD);
        costume.Stat.GAS = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statGAS);
        costume.Stat.BLA = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statBLA);
        costume.Stat.ACL = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statACL);
        costume.Stat.skillId = PlayerPrefs.GetString(slot + PhotonPlayerProperty.statSKILL);
        costume.SetBodyByCostumeId(-1);
        costume.SetMesh();
        costume.SetTexture();
        return costume;
    }

    public static HeroCostume PhotonDataToHeroCostume(PhotonPlayer player) //BUG: NullReferenceException on accessing customPeroperties
    {
        Sex sex = IntToSex((int) player.customProperties[PhotonPlayerProperty.sex]);
        var costume = new HeroCostume {
            Sex = sex,
            CostumeId = (int) player.customProperties[PhotonPlayerProperty.costumeId],
            Id = (int) player.customProperties[PhotonPlayerProperty.heroCostumeId],
            Cape = (bool) player.customProperties[PhotonPlayerProperty.cape],
            HairInfo = (sex != Sex.Male) ? CostumeHair.hairsF[(int) player.customProperties[PhotonPlayerProperty.hairInfo]] : CostumeHair.hairsM[(int) player.customProperties[PhotonPlayerProperty.hairInfo]],
            EyeTextureId = (int) player.customProperties[PhotonPlayerProperty.eye_texture_id],
            BeardTextureId = (int) player.customProperties[PhotonPlayerProperty.beard_texture_id],
            GlassTextureId = (int) player.customProperties[PhotonPlayerProperty.glass_texture_id],
            SkinColor = (int) player.customProperties[PhotonPlayerProperty.skin_color],
            HairColor = new Color((float) player.customProperties[PhotonPlayerProperty.hair_color1], (float) player.customProperties[PhotonPlayerProperty.hair_color2], (float) player.customProperties[PhotonPlayerProperty.hair_color3]),
            Division = IntToDivision((int) player.customProperties[PhotonPlayerProperty.division]),
            Stat = new HeroStat()
        };
        costume.Stat.SPD = (int) player.customProperties[PhotonPlayerProperty.statSPD];
        costume.Stat.GAS = (int) player.customProperties[PhotonPlayerProperty.statGAS];
        costume.Stat.BLA = (int) player.customProperties[PhotonPlayerProperty.statBLA];
        costume.Stat.ACL = (int) player.customProperties[PhotonPlayerProperty.statACL];
        costume.Stat.skillId = (string) player.customProperties[PhotonPlayerProperty.statSKILL];
        if ((costume.CostumeId == 25) && (costume.Sex == Sex.Female))
        {
            costume.CostumeId = 26;
        }
        costume.SetBodyByCostumeId(-1);
        costume.SetMesh();
        costume.SetTexture();
        return costume;
    }

    private static int SexToInt(Sex id)
    {
        if (id == Sex.Female)
        {
            return 0;
        }
        if (id == Sex.Male)
        {
            return 1;
        }
        return 1;
    }

    private static int UniformTypeToInt(UniformType id)
    {
        if (id == UniformType.CasualA)
        {
            return 0;
        }
        if (id == UniformType.CasualB)
        {
            return 1;
        }
        if (id != UniformType.UniformA)
        {
            if (id == UniformType.UniformB)
            {
                return 3;
            }
            if (id == UniformType.CasualAHSS)
            {
                return 4;
            }
        }
        return 2;
    }
}

