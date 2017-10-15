using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class CustomCharacterManager : MonoBehaviour
{
    private int capeId;
    private int[] capeOption;
    public GameObject character;
    private int costumeId = 1;
    private HeroCostume[] costumeOption;
    public HeroCostume currentCostume;
    private string currentSlot = "Set 1";
    private int divisionId;
    private Division[] divisionOption;
    private int eyeId;
    private int[] eyeOption;
    private int faceId;
    private int[] faceOption;
    private int glassId;
    private int[] glassOption;
    public GameObject hairB;
    public GameObject hairG;
    private int hairId;
    private int[] hairOption;
    public GameObject hairR;
    public GameObject labelACL;
    public GameObject labelBLA;
    public GameObject labelCape;
    public GameObject labelCostume;
    public GameObject labelDivision;
    public GameObject labelEye;
    public GameObject labelFace;
    public GameObject labelGAS;
    public GameObject labelGlass;
    public GameObject labelHair;
    public GameObject labelPOINT;
    public GameObject labelPreset;
    public GameObject labelSex;
    public GameObject labelSKILL;
    public GameObject labelSkin;
    public GameObject labelSPD;
    private int presetId;
    private HERO_SETUP setup;
    private int sexId;
    private Sex[] sexOption;
    private int skillId;
    private string[] skillOption;
    private int skinId;
    private int[] skinOption;

    private int calTotalPoints()
    {
        if (this.setup.myCostume != null)
        {
            int num = 0;
            num += this.setup.myCostume.Stat.SPD;
            num += this.setup.myCostume.Stat.GAS;
            num += this.setup.myCostume.Stat.BLA;
            return (num + this.setup.myCostume.Stat.ACL);
        }
        return 400;
    }

    private void copyBodyCostume(HeroCostume from, HeroCostume to)
    {
        to.ArmLMesh = from.ArmLMesh;
        to.ArmRMesh = from.ArmRMesh;
        to.BodyMesh = from.BodyMesh;
        to.BodyTexture = from.BodyTexture;
        to.UniformType = from.UniformType;
        to.PartChest1ObjectMesh = from.PartChest1ObjectMesh;
        to.PartChest1ObjectTexture = from.PartChest1ObjectTexture;
        to.PartChestObjectMesh = from.PartChestObjectMesh;
        to.PartChestObjectTexture = from.PartChestObjectTexture;
        to.PartChestSkinnedClothMesh = from.PartChestSkinnedClothMesh;
        to.PartChestSkinnedClothTexture = from.PartChestSkinnedClothTexture;
        to.Division = from.Division;
        to.Id = from.Id;
        to.CostumeId = from.CostumeId;
    }

    private void copyCostume(HeroCostume from, HeroCostume to, bool init = false)
    {
        this.copyBodyCostume(from, to);
        to.Sex = from.Sex;
        to.HairMesh = from.HairMesh;
        to.Hair1Mesh = from.Hair1Mesh;
        to.HairColor = new Color(from.HairColor.r, from.HairColor.g, from.HairColor.b);
        to.HairInfo = from.HairInfo;
        to.Cape = from.Cape;
        to.CapeMesh = from.CapeMesh;
        to.CapeTexture = from.CapeTexture;
        to.Brand1Mesh = from.Brand1Mesh;
        to.Brand2Mesh = from.Brand2Mesh;
        to.Brand3Mesh = from.Brand3Mesh;
        to.Brand4Mesh = from.Brand4Mesh;
        to.BrandTexture = from.BrandTexture;
        to.DmgTexture = from.DmgTexture;
        to.FaceTexture = from.FaceTexture;
        to.EyeMesh = from.EyeMesh;
        to.GlassMesh = from.GlassMesh;
        to.BeardMesh = from.BeardMesh;
        to.EyeTextureId = from.EyeTextureId;
        to.BeardTextureId = from.BeardTextureId;
        to.GlassTextureId = from.GlassTextureId;
        to.SkinColor = from.SkinColor;
        to.SkinTexture = from.SkinTexture;
        to.BeardTextureId = from.BeardTextureId;
        to.HandLMesh = from.HandLMesh;
        to.HandRMesh = from.HandRMesh;
        to.Mesh3Dmg = from.Mesh3Dmg;
        to.Mesh3DmgGasL = from.Mesh3DmgGasL;
        to.Mesh3DmgGasR = from.Mesh3DmgGasR;
        to.Mesh3DmgBelt = from.Mesh3DmgBelt;
        to.WeaponLMesh = from.WeaponLMesh;
        to.WeaponRMesh = from.WeaponRMesh;
        if (init)
        {
            to.Stat = new HeroStat();
            to.Stat.ACL = 100;
            to.Stat.SPD = 100;
            to.Stat.GAS = 100;
            to.Stat.BLA = 100;
            to.Stat.skillId = "mikasa";
        }
        else
        {
            to.Stat = new HeroStat();
            to.Stat.ACL = from.Stat.ACL;
            to.Stat.SPD = from.Stat.SPD;
            to.Stat.GAS = from.Stat.GAS;
            to.Stat.BLA = from.Stat.BLA;
            to.Stat.skillId = from.Stat.skillId;
        }
    }

    private void CostumeDataToMyID()
    {
        int index = 0;
        for (index = 0; index < this.sexOption.Length; index++)
        {
            if (this.sexOption[index] == this.setup.myCostume.Sex)
            {
                this.sexId = index;
                break;
            }
        }
        index = 0;
        while (index < this.eyeOption.Length)
        {
            if (this.eyeOption[index] == this.setup.myCostume.EyeTextureId)
            {
                this.eyeId = index;
                break;
            }
            index++;
        }
        this.faceId = -1;
        for (index = 0; index < this.faceOption.Length; index++)
        {
            if (this.faceOption[index] == this.setup.myCostume.BeardTextureId)
            {
                this.faceId = index;
                break;
            }
        }
        this.glassId = -1;
        for (index = 0; index < this.glassOption.Length; index++)
        {
            if (this.glassOption[index] == this.setup.myCostume.GlassTextureId)
            {
                this.glassId = index;
                break;
            }
        }
        for (index = 0; index < this.hairOption.Length; index++)
        {
            if (this.hairOption[index] == this.setup.myCostume.HairInfo.id)
            {
                this.hairId = index;
                break;
            }
        }
        for (index = 0; index < this.skinOption.Length; index++)
        {
            if (this.skinOption[index] == this.setup.myCostume.SkinColor)
            {
                this.skinId = index;
                break;
            }
        }
        if (this.setup.myCostume.Cape)
        {
            this.capeId = 1;
        }
        else
        {
            this.capeId = 0;
        }
        index = 0;
        while (index < this.divisionOption.Length)
        {
            if (this.divisionOption[index] == this.setup.myCostume.Division)
            {
                this.divisionId = index;
                break;
            }
            index++;
        }
        this.costumeId = this.setup.myCostume.CostumeId;
        float r = this.setup.myCostume.HairColor.r;
        float g = this.setup.myCostume.HairColor.g;
        float b = this.setup.myCostume.HairColor.b;
        this.hairR.GetComponent<UISlider>().sliderValue = r;
        this.hairG.GetComponent<UISlider>().sliderValue = g;
        this.hairB.GetComponent<UISlider>().sliderValue = b;
        for (index = 0; index < this.skillOption.Length; index++)
        {
            if (this.skillOption[index] == this.setup.myCostume.Stat.skillId)
            {
                this.skillId = index;
                break;
            }
        }
    }

    private void freshLabel()
    {
        this.labelSex.GetComponent<UILabel>().text = this.sexOption[this.sexId].ToString();
        this.labelEye.GetComponent<UILabel>().text = "eye_" + this.eyeId.ToString();
        this.labelFace.GetComponent<UILabel>().text = "face_" + this.faceId.ToString();
        this.labelGlass.GetComponent<UILabel>().text = "glass_" + this.glassId.ToString();
        this.labelHair.GetComponent<UILabel>().text = "hair_" + this.hairId.ToString();
        this.labelSkin.GetComponent<UILabel>().text = "skin_" + this.skinId.ToString();
        this.labelCostume.GetComponent<UILabel>().text = "costume_" + this.costumeId.ToString();
        this.labelCape.GetComponent<UILabel>().text = "cape_" + this.capeId.ToString();
        this.labelDivision.GetComponent<UILabel>().text = this.divisionOption[this.divisionId].ToString();
        this.labelPOINT.GetComponent<UILabel>().text = "Points: " + ((400 - this.calTotalPoints())).ToString();
        this.labelSPD.GetComponent<UILabel>().text = "SPD " + this.setup.myCostume.Stat.SPD.ToString();
        this.labelGAS.GetComponent<UILabel>().text = "GAS " + this.setup.myCostume.Stat.GAS.ToString();
        this.labelBLA.GetComponent<UILabel>().text = "BLA " + this.setup.myCostume.Stat.BLA.ToString();
        this.labelACL.GetComponent<UILabel>().text = "ACL " + this.setup.myCostume.Stat.ACL.ToString();
        this.labelSKILL.GetComponent<UILabel>().text = "SKILL " + this.setup.myCostume.Stat.skillId.ToString();
    }

    public void LoadData()
    {
        HeroCostume from = CostumeConverter.LocalDataToHeroCostume(this.currentSlot);
        if (from != null)
        {
            this.copyCostume(from, this.setup.myCostume, false);
            this.setup.DeleteCharacterComponent2();
            this.setup.SetCharacterComponent();
        }
        this.CostumeDataToMyID();
        this.freshLabel();
    }

    public void nextOption(CreatePart part)
    {
        if (part == CreatePart.Preset)
        {
            this.presetId = this.toNext(this.presetId, HeroCostume.Costume.Length, 0);
            this.copyCostume(HeroCostume.Costume[this.presetId], this.setup.myCostume, true);
            this.CostumeDataToMyID();
            this.setup.DeleteCharacterComponent2();
            this.setup.SetCharacterComponent();
            this.labelPreset.GetComponent<UILabel>().text = HeroCostume.Costume[this.presetId].Name;
            this.freshLabel();
        }
        else
        {
            this.toOption2(part, true);
        }
    }

    public void nextStatOption(CreateStat type)
    {
        if (type == CreateStat.Skill)
        {
            this.skillId = this.toNext(this.skillId, this.skillOption.Length, 0);
            this.setup.myCostume.Stat.skillId = this.skillOption[this.skillId];
            this.character.GetComponent<CharacterCreateAnimationControl>().PlayAttack(this.setup.myCostume.Stat.skillId);
            this.freshLabel();
        }
        else if (this.calTotalPoints() < 400)
        {
            this.setStatPoint(type, 1);
        }
    }

    public void OnHairBChange(float value)
    {
        if (((this.setup != null) && (this.setup.myCostume != null)) && (this.setup.part_hair != null))
        {
            this.setup.myCostume.HairColor = new Color(this.setup.part_hair.renderer.material.color.r, this.setup.part_hair.renderer.material.color.g, value);
            this.setHairColor();
        }
    }

    public void OnHairGChange(float value)
    {
        if ((this.setup.myCostume != null) && (this.setup.part_hair != null))
        {
            this.setup.myCostume.HairColor = new Color(this.setup.part_hair.renderer.material.color.r, value, this.setup.part_hair.renderer.material.color.b);
            this.setHairColor();
        }
    }

    public void OnHairRChange(float value)
    {
        if ((this.setup.myCostume != null) && (this.setup.part_hair != null))
        {
            this.setup.myCostume.HairColor = new Color(value, this.setup.part_hair.renderer.material.color.g, this.setup.part_hair.renderer.material.color.b);
            this.setHairColor();
        }
    }

    public void OnSoltChange(string id)
    {
        this.currentSlot = id;
    }

    public void prevOption(CreatePart part)
    {
        if (part == CreatePart.Preset)
        {
            this.presetId = this.toPrev(this.presetId, HeroCostume.Costume.Length, 0);
            this.copyCostume(HeroCostume.Costume[this.presetId], this.setup.myCostume, true);
            this.CostumeDataToMyID();
            this.setup.DeleteCharacterComponent2();
            this.setup.SetCharacterComponent();
            this.labelPreset.GetComponent<UILabel>().text = HeroCostume.Costume[this.presetId].Name;
            this.freshLabel();
        }
        else
        {
            this.toOption2(part, false);
        }
    }

    public void prevStatOption(CreateStat type)
    {
        if (type == CreateStat.Skill)
        {
            this.skillId = this.toPrev(this.skillId, this.skillOption.Length, 0);
            this.setup.myCostume.Stat.skillId = this.skillOption[this.skillId];
            this.character.GetComponent<CharacterCreateAnimationControl>().PlayAttack(this.setup.myCostume.Stat.skillId);
            this.freshLabel();
        }
        else
        {
            this.setStatPoint(type, -1);
        }
    }

    public void SaveData()
    {
        CostumeConverter.HeroCostumeToLocalData(this.setup.myCostume, this.currentSlot);
    }

    private void setHairColor()
    {
        if (this.setup.part_hair != null)
        {
            this.setup.part_hair.renderer.material.color = this.setup.myCostume.HairColor;
        }
        if (this.setup.part_hair_1 != null)
        {
            this.setup.part_hair_1.renderer.material.color = this.setup.myCostume.HairColor;
        }
    }

    private void setStatPoint(CreateStat type, int pt)
    {
        switch (type)
        {
            case CreateStat.SPD:
                this.setup.myCostume.Stat.SPD += pt;
                break;

            case CreateStat.GAS:
                this.setup.myCostume.Stat.GAS += pt;
                break;

            case CreateStat.BLA:
                this.setup.myCostume.Stat.BLA += pt;
                break;

            case CreateStat.ACL:
                this.setup.myCostume.Stat.ACL += pt;
                break;
        }
        this.setup.myCostume.Stat.SPD = Mathf.Clamp(this.setup.myCostume.Stat.SPD, 75, 125);
        this.setup.myCostume.Stat.GAS = Mathf.Clamp(this.setup.myCostume.Stat.GAS, 75, 125);
        this.setup.myCostume.Stat.BLA = Mathf.Clamp(this.setup.myCostume.Stat.BLA, 75, 125);
        this.setup.myCostume.Stat.ACL = Mathf.Clamp(this.setup.myCostume.Stat.ACL, 75, 125);
        this.freshLabel();
    }

    private void Start()
    {
        int num;
        QualitySettings.SetQualityLevel(5, true);
        this.costumeOption = HeroCostume.CostumeOption;
        this.setup = this.character.GetComponent<HERO_SETUP>();
        this.setup.Initialize();
        this.setup.myCostume = new HeroCostume();
        this.copyCostume(HeroCostume.Costume[2], this.setup.myCostume, false);
        this.setup.myCostume.SetMesh();
        this.setup.SetCharacterComponent();
        Sex[] sexArray1 = new Sex[2];
        sexArray1[1] = Sex.Female;
        this.sexOption = sexArray1;
        this.eyeOption = new int[28];
        for (num = 0; num < 28; num++)
        {
            this.eyeOption[num] = num;
        }
        this.faceOption = new int[14];
        for (num = 0; num < 14; num++)
        {
            this.faceOption[num] = num + 32;
        }
        this.glassOption = new int[10];
        for (num = 0; num < 10; num++)
        {
            this.glassOption[num] = num + 48;
        }
        this.hairOption = new int[11];
        for (num = 0; num < 11; num++)
        {
            this.hairOption[num] = num;
        }
        this.skinOption = new int[3];
        for (num = 0; num < 3; num++)
        {
            this.skinOption[num] = num + 1;
        }
        this.capeOption = new int[2];
        for (num = 0; num < 2; num++)
        {
            this.capeOption[num] = num;
        }
        Division[] divisionArray1 = new Division[4];
        divisionArray1[1] = Division.TheGarrison;
        divisionArray1[2] = Division.TheMilitaryPolice;
        divisionArray1[3] = Division.TheSurveryCorps;
        this.divisionOption = divisionArray1;
        this.skillOption = new string[] { "mikasa", "levi", "sasha", "jean", "marco", "armin", "petra" };
        this.CostumeDataToMyID();
        this.freshLabel();
    }

    private int toNext(int id, int Count, int start = 0)
    {
        id++;
        if (id >= Count)
        {
            id = start;
        }
        id = Mathf.Clamp(id, start, (start + Count) - 1);
        return id;
    }

    public void toOption(CreatePart part, bool next)
    {
        switch (part)
        {
            case CreatePart.Sex:
                this.sexId = !next ? this.toPrev(this.sexId, this.sexOption.Length, 0) : this.toNext(this.sexId, this.sexOption.Length, 0);
                if (this.sexId != 0)
                {
                    this.costumeId = 0;
                    break;
                }
                this.costumeId = 11;
                break;

            case CreatePart.Eye:
                this.eyeId = !next ? this.toPrev(this.eyeId, this.eyeOption.Length, 0) : this.toNext(this.eyeId, this.eyeOption.Length, 0);
                this.setup.myCostume.EyeTextureId = this.eyeId;
                this.setup.SetFacialTexture(this.setup.part_eye, this.eyeOption[this.eyeId]);
                goto Label_06AE;

            case CreatePart.Face:
                this.faceId = !next ? this.toPrev(this.faceId, this.faceOption.Length, 0) : this.toNext(this.faceId, this.faceOption.Length, 0);
                this.setup.myCostume.BeardTextureId = this.faceOption[this.faceId];
                if (this.setup.part_face == null)
                {
                    this.setup.createFace();
                }
                this.setup.SetFacialTexture(this.setup.part_face, this.faceOption[this.faceId]);
                goto Label_06AE;

            case CreatePart.Glass:
                this.glassId = !next ? this.toPrev(this.glassId, this.glassOption.Length, 0) : this.toNext(this.glassId, this.glassOption.Length, 0);
                this.setup.myCostume.GlassTextureId = this.glassOption[this.glassId];
                if (this.setup.part_glass == null)
                {
                    this.setup.createGlass();
                }
                this.setup.SetFacialTexture(this.setup.part_glass, this.glassOption[this.glassId]);
                goto Label_06AE;

            case CreatePart.Hair:
                this.hairId = !next ? this.toPrev(this.hairId, this.hairOption.Length, 0) : this.toNext(this.hairId, this.hairOption.Length, 0);
                if (this.sexId != 0)
                {
                    this.setup.myCostume.HairMesh = CostumeHair.hairsF[this.hairOption[this.hairId]].hair;
                    this.setup.myCostume.Hair1Mesh = CostumeHair.hairsF[this.hairOption[this.hairId]].hair_1;
                    this.setup.myCostume.HairInfo = CostumeHair.hairsF[this.hairOption[this.hairId]];
                }
                else
                {
                    this.setup.myCostume.HairMesh = CostumeHair.hairsM[this.hairOption[this.hairId]].hair;
                    this.setup.myCostume.Hair1Mesh = CostumeHair.hairsM[this.hairOption[this.hairId]].hair_1;
                    this.setup.myCostume.HairInfo = CostumeHair.hairsM[this.hairOption[this.hairId]];
                }
                this.setup.createHair2();
                this.setHairColor();
                goto Label_06AE;

            case CreatePart.Skin:
                if (this.setup.myCostume.UniformType != UniformType.CasualAHSS)
                {
                    this.skinId = !next ? this.toPrev(this.skinId, 2, 0) : this.toNext(this.skinId, 2, 0);
                }
                else
                {
                    this.skinId = 2;
                }
                this.setup.myCostume.SkinColor = this.skinOption[this.skinId];
                this.setup.myCostume.SetTexture();
                this.setup.SetSkin();
                goto Label_06AE;

            case CreatePart.Costume:
                if (this.setup.myCostume.UniformType != UniformType.CasualAHSS)
                {
                    if (this.sexId == 0)
                    {
                        this.costumeId = !next ? this.toPrev(this.costumeId, 24, 10) : this.toNext(this.costumeId, 24, 10);
                    }
                    else
                    {
                        this.costumeId = !next ? this.toPrev(this.costumeId, 10, 0) : this.toNext(this.costumeId, 10, 0);
                    }
                }
                else
                {
                    this.costumeId = 25;
                }
                this.copyBodyCostume(this.costumeOption[this.costumeId], this.setup.myCostume);
                this.setup.myCostume.SetMesh();
                this.setup.myCostume.SetTexture();
                this.setup.CreateUpperBody2();
                this.setup.CreateLeftArm();
                this.setup.CreateRightArm();
                this.setup.CreateLowerBody();
                goto Label_06AE;

            case CreatePart.Cape:
                this.capeId = !next ? this.toPrev(this.capeId, this.capeOption.Length, 0) : this.toNext(this.capeId, this.capeOption.Length, 0);
                this.setup.myCostume.Cape = this.capeId == 1;
                this.setup.myCostume.SetCape();
                this.setup.myCostume.SetTexture();
                this.setup.createCape2();
                goto Label_06AE;

            case CreatePart.Division:
                this.divisionId = !next ? this.toPrev(this.divisionId, this.divisionOption.Length, 0) : this.toNext(this.divisionId, this.divisionOption.Length, 0);
                this.setup.myCostume.Division = this.divisionOption[this.divisionId];
                this.setup.myCostume.SetTexture();
                this.setup.CreateUpperBody2();
                goto Label_06AE;

            default:
                goto Label_06AE;
        }
        this.copyCostume(this.costumeOption[this.costumeId], this.setup.myCostume, true);
        this.setup.myCostume.Sex = this.sexOption[this.sexId];
        this.character.GetComponent<CharacterCreateAnimationControl>().ToStand();
        this.CostumeDataToMyID();
        this.setup.DeleteCharacterComponent2();
        this.setup.SetCharacterComponent();
    Label_06AE:
        this.freshLabel();
    }

    public void toOption2(CreatePart part, bool next)
    {
        switch (part)
        {
            case CreatePart.Sex:
                this.sexId = !next ? this.toPrev(this.sexId, this.sexOption.Length, 0) : this.toNext(this.sexId, this.sexOption.Length, 0);
                if (this.sexId == 0)
                {
                    this.costumeId = 11;
                }
                else
                {
                    this.costumeId = 0;
                }
                this.copyCostume(this.costumeOption[this.costumeId], this.setup.myCostume, true);
                this.setup.myCostume.Sex = this.sexOption[this.sexId];
                this.character.GetComponent<CharacterCreateAnimationControl>().ToStand();
                this.CostumeDataToMyID();
                this.setup.DeleteCharacterComponent2();
                this.setup.SetCharacterComponent();
                goto Label_0750;

            case CreatePart.Eye:
                this.eyeId = !next ? this.toPrev(this.eyeId, this.eyeOption.Length, 0) : this.toNext(this.eyeId, this.eyeOption.Length, 0);
                this.setup.myCostume.EyeTextureId = this.eyeId;
                this.setup.SetFacialTexture(this.setup.part_eye, this.eyeOption[this.eyeId]);
                goto Label_0750;

            case CreatePart.Face:
                this.faceId = !next ? this.toPrev(this.faceId, this.faceOption.Length, 0) : this.toNext(this.faceId, this.faceOption.Length, 0);
                this.setup.myCostume.BeardTextureId = this.faceOption[this.faceId];
                if (this.setup.part_face == null)
                {
                    this.setup.createFace();
                }
                this.setup.SetFacialTexture(this.setup.part_face, this.faceOption[this.faceId]);
                goto Label_0750;

            case CreatePart.Glass:
                this.glassId = !next ? this.toPrev(this.glassId, this.glassOption.Length, 0) : this.toNext(this.glassId, this.glassOption.Length, 0);
                this.setup.myCostume.GlassTextureId = this.glassOption[this.glassId];
                if (this.setup.part_glass == null)
                {
                    this.setup.createGlass();
                }
                this.setup.SetFacialTexture(this.setup.part_glass, this.glassOption[this.glassId]);
                goto Label_0750;

            case CreatePart.Hair:
                this.hairId = !next ? this.toPrev(this.hairId, this.hairOption.Length, 0) : this.toNext(this.hairId, this.hairOption.Length, 0);
                if (this.sexId == 0)
                {
                    this.setup.myCostume.HairMesh = CostumeHair.hairsM[this.hairOption[this.hairId]].hair;
                    this.setup.myCostume.Hair1Mesh = CostumeHair.hairsM[this.hairOption[this.hairId]].hair_1;
                    this.setup.myCostume.HairInfo = CostumeHair.hairsM[this.hairOption[this.hairId]];
                    break;
                }
                this.setup.myCostume.HairMesh = CostumeHair.hairsF[this.hairOption[this.hairId]].hair;
                this.setup.myCostume.Hair1Mesh = CostumeHair.hairsF[this.hairOption[this.hairId]].hair_1;
                this.setup.myCostume.HairInfo = CostumeHair.hairsF[this.hairOption[this.hairId]];
                break;

            case CreatePart.Skin:
                if (this.setup.myCostume.UniformType == UniformType.CasualAHSS)
                {
                    this.skinId = 2;
                }
                else
                {
                    this.skinId = !next ? this.toPrev(this.skinId, 2, 0) : this.toNext(this.skinId, 2, 0);
                }
                this.setup.myCostume.SkinColor = this.skinOption[this.skinId];
                this.setup.myCostume.SetTexture();
                this.setup.SetSkin();
                goto Label_0750;

            case CreatePart.Costume:
                if (this.setup.myCostume.UniformType == UniformType.CasualAHSS)
                {
                    if (this.setup.myCostume.Sex == Sex.Female)
                    {
                        this.costumeId = 26;
                    }
                    else if (this.setup.myCostume.Sex == Sex.Male)
                    {
                        this.costumeId = 25;
                    }
                }
                else if (this.sexId != 0)
                {
                    this.costumeId = !next ? this.toPrev(this.costumeId, 10, 0) : this.toNext(this.costumeId, 10, 0);
                }
                else
                {
                    this.costumeId = !next ? this.toPrev(this.costumeId, 24, 10) : this.toNext(this.costumeId, 24, 10);
                }
                this.copyBodyCostume(this.costumeOption[this.costumeId], this.setup.myCostume);
                this.setup.myCostume.SetMesh();
                this.setup.myCostume.SetTexture();
                this.setup.CreateUpperBody2();
                this.setup.CreateLeftArm();
                this.setup.CreateRightArm();
                this.setup.CreateLowerBody();
                goto Label_0750;

            case CreatePart.Cape:
                this.capeId = !next ? this.toPrev(this.capeId, this.capeOption.Length, 0) : this.toNext(this.capeId, this.capeOption.Length, 0);
                this.setup.myCostume.Cape = this.capeId == 1;
                this.setup.myCostume.SetCape();
                this.setup.myCostume.SetTexture();
                this.setup.createCape2();
                goto Label_0750;

            case CreatePart.Division:
                this.divisionId = !next ? this.toPrev(this.divisionId, this.divisionOption.Length, 0) : this.toNext(this.divisionId, this.divisionOption.Length, 0);
                this.setup.myCostume.Division = this.divisionOption[this.divisionId];
                this.setup.myCostume.SetTexture();
                this.setup.CreateUpperBody2();
                goto Label_0750;

            default:
                goto Label_0750;
        }
        this.setup.createHair2();
        this.setHairColor();
    Label_0750:
        this.freshLabel();
    }

    private int toPrev(int id, int Count, int start = 0)
    {
        id--;
        if (id < start)
        {
            id = Count - 1;
        }
        id = Mathf.Clamp(id, start, (start + Count) - 1);
        return id;
    }
}

