using System;
using System.Collections.Generic;
using UnityEngine;
using Xft;

public class HERO_SETUP : MonoBehaviour
{
    public string aniname;
    public float anitime;
    private List<BoneWeight> boneWeightsList = new List<BoneWeight>();
    public bool change;
    public GameObject chest_info;
    private readonly byte[] config = new byte[4];
    public int currentOne;
    public SkinnedMeshRenderer[][] elements;
    public bool isDeadBody;
    private List<Material> materialList;
    private GameObject mount_3dmg;
    private GameObject mount_3dmg_gas_l;
    private GameObject mount_3dmg_gas_r;
    private GameObject mount_3dmg_gun_mag_l;
    private GameObject mount_3dmg_gun_mag_r;
    private GameObject mount_weapon_l;
    private GameObject mount_weapon_r;
    public HeroCostume myCostume;
    public GameObject part_3dmg;
    public GameObject part_3dmg_belt;
    public GameObject part_3dmg_gas_l;
    public GameObject part_3dmg_gas_r;
    public GameObject part_arm_l;
    public GameObject part_arm_r;
    public GameObject part_asset_1;
    public GameObject part_asset_2;
    public GameObject part_blade_l;
    public GameObject part_blade_r;
    public GameObject part_brand_1;
    public GameObject part_brand_2;
    public GameObject part_brand_3;
    public GameObject part_brand_4;
    public GameObject part_cape;
    public GameObject part_chest;
    public GameObject part_chest_1;
    public GameObject part_chest_2;
    public GameObject part_chest_3;
    public GameObject part_eye;
    public GameObject part_face;
    public GameObject part_glass;
    public GameObject part_hair;
    public GameObject part_hair_1;
    public GameObject part_hair_2;
    public GameObject part_hand_l;
    public GameObject part_hand_r;
    public GameObject part_head;
    public GameObject part_leg;
    public GameObject part_upper_body;
    public GameObject reference;
    public float timer;

    private void Awake()
    {
        part_head.transform.parent = transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head").transform;
        mount_3dmg = new GameObject();
        mount_3dmg_gas_l = new GameObject();
        mount_3dmg_gas_r = new GameObject();
        mount_3dmg_gun_mag_l = new GameObject();
        mount_3dmg_gun_mag_r = new GameObject();
        mount_weapon_l = new GameObject();
        mount_weapon_r = new GameObject();
        mount_3dmg.transform.position = transform.position;
        mount_3dmg.transform.rotation = Quaternion.Euler(270f, transform.rotation.eulerAngles.y, 0f);
        mount_3dmg.transform.parent = transform.Find("Amarture/Controller_Body/hip/spine/chest").transform;
        mount_3dmg_gas_l.transform.position = transform.position;
        mount_3dmg_gas_l.transform.rotation = Quaternion.Euler(270f, transform.rotation.eulerAngles.y, 0f);
        mount_3dmg_gas_l.transform.parent = transform.Find("Amarture/Controller_Body/hip/spine").transform;
        mount_3dmg_gas_r.transform.position = transform.position;
        mount_3dmg_gas_r.transform.rotation = Quaternion.Euler(270f, transform.rotation.eulerAngles.y, 0f);
        mount_3dmg_gas_r.transform.parent = transform.Find("Amarture/Controller_Body/hip/spine").transform;
        mount_3dmg_gun_mag_l.transform.position = transform.position;
        mount_3dmg_gun_mag_l.transform.rotation = Quaternion.Euler(270f, transform.rotation.eulerAngles.y, 0f);
        mount_3dmg_gun_mag_l.transform.parent = transform.Find("Amarture/Controller_Body/hip/thigh_L").transform;
        mount_3dmg_gun_mag_r.transform.position = transform.position;
        mount_3dmg_gun_mag_r.transform.rotation = Quaternion.Euler(270f, transform.rotation.eulerAngles.y, 0f);
        mount_3dmg_gun_mag_r.transform.parent = transform.Find("Amarture/Controller_Body/hip/thigh_R").transform;
        mount_weapon_l.transform.position = transform.position;
        mount_weapon_l.transform.rotation = Quaternion.Euler(270f, transform.rotation.eulerAngles.y, 0f);
        mount_weapon_l.transform.parent = transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L").transform;
        mount_weapon_r.transform.position = transform.position;
        mount_weapon_r.transform.rotation = Quaternion.Euler(270f, transform.rotation.eulerAngles.y, 0f);
        mount_weapon_r.transform.parent = transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R").transform;
    }

    private void combineSMR(GameObject go, GameObject go2)
    {
        if (go.GetComponent<SkinnedMeshRenderer>() == null)
        {
            go.AddComponent<SkinnedMeshRenderer>();
        }
        SkinnedMeshRenderer component = go.GetComponent<SkinnedMeshRenderer>();
        List<CombineInstance> list = new List<CombineInstance>();
        materialList = new List<Material>();
        materialList.Add(component.material);
        boneWeightsList = new List<BoneWeight>();
        Transform[] bones = component.bones;
        SkinnedMeshRenderer renderer2 = go2.GetComponent<SkinnedMeshRenderer>();
        for (int i = 0; i < renderer2.sharedMesh.subMeshCount; i++)
        {
            CombineInstance item = new CombineInstance {
                mesh = renderer2.sharedMesh,
                transform = renderer2.transform.localToWorldMatrix,
                subMeshIndex = i
            };
            list.Add(item);
            for (int k = 0; k < materialList.Count; k++)
            {
                Material material = materialList[k];
                if (material.name != renderer2.material.name)
                {
                    goto Label_00DA;
                }
            }
            continue;
        Label_00DA:
            materialList.Add(renderer2.material);
        }
        Destroy(renderer2.gameObject);
        component.sharedMesh = new Mesh();
        component.sharedMesh.CombineMeshes(list.ToArray(), true, false);
        component.bones = bones;
        component.materials = materialList.ToArray();
        List<Matrix4x4> list2 = new List<Matrix4x4>();
        for (int j = 0; j < bones.Length; j++)
        {
            if (bones[j] != null)
            {
                list2.Add(bones[j].worldToLocalMatrix * transform.localToWorldMatrix);
            }
        }
        component.sharedMesh.bindposes = list2.ToArray();
    }

    public void create3DMG()
    {
        Destroy(part_3dmg);
        Destroy(part_3dmg_belt);
        Destroy(part_3dmg_gas_l);
        Destroy(part_3dmg_gas_r);
        Destroy(part_blade_l);
        Destroy(part_blade_r);
        if (myCostume.Mesh3Dmg.Length > 0)
        {
            part_3dmg = (GameObject) Instantiate(Resources.Load("Character/" + myCostume.Mesh3Dmg));
            part_3dmg.transform.position = mount_3dmg.transform.position;
            part_3dmg.transform.rotation = mount_3dmg.transform.rotation;
            part_3dmg.transform.parent = mount_3dmg.transform.parent;
            part_3dmg.renderer.material = CharacterMaterials.materials[myCostume.DmgTexture];
        }
        if (myCostume.Mesh3DmgBelt.Length > 0)
        {
            part_3dmg_belt = GenerateCloth(reference, "Character/" + myCostume.Mesh3DmgBelt);
            part_3dmg_belt.renderer.material = CharacterMaterials.materials[myCostume.DmgTexture];
        }
        if (myCostume.Mesh3DmgGasL.Length > 0)
        {
            part_3dmg_gas_l = (GameObject) Instantiate(Resources.Load("Character/" + myCostume.Mesh3DmgGasL));
            if (myCostume.UniformType != UniformType.CasualAHSS)
            {
                part_3dmg_gas_l.transform.position = mount_3dmg_gas_l.transform.position;
                part_3dmg_gas_l.transform.rotation = mount_3dmg_gas_l.transform.rotation;
                part_3dmg_gas_l.transform.parent = mount_3dmg_gas_l.transform.parent;
            }
            else
            {
                part_3dmg_gas_l.transform.position = mount_3dmg_gun_mag_l.transform.position;
                part_3dmg_gas_l.transform.rotation = mount_3dmg_gun_mag_l.transform.rotation;
                part_3dmg_gas_l.transform.parent = mount_3dmg_gun_mag_l.transform.parent;
            }
            part_3dmg_gas_l.renderer.material = CharacterMaterials.materials[myCostume.DmgTexture];
        }
        if (myCostume.Mesh3DmgGasR.Length > 0)
        {
            part_3dmg_gas_r = (GameObject) Instantiate(Resources.Load("Character/" + myCostume.Mesh3DmgGasR));
            if (myCostume.UniformType != UniformType.CasualAHSS)
            {
                part_3dmg_gas_r.transform.position = mount_3dmg_gas_r.transform.position;
                part_3dmg_gas_r.transform.rotation = mount_3dmg_gas_r.transform.rotation;
                part_3dmg_gas_r.transform.parent = mount_3dmg_gas_r.transform.parent;
            }
            else
            {
                part_3dmg_gas_r.transform.position = mount_3dmg_gun_mag_r.transform.position;
                part_3dmg_gas_r.transform.rotation = mount_3dmg_gun_mag_r.transform.rotation;
                part_3dmg_gas_r.transform.parent = mount_3dmg_gun_mag_r.transform.parent;
            }
            part_3dmg_gas_r.renderer.material = CharacterMaterials.materials[myCostume.DmgTexture];
        }
        if (myCostume.WeaponLMesh.Length > 0)
        {
            part_blade_l = (GameObject) Instantiate(Resources.Load("Character/" + myCostume.WeaponLMesh));
            part_blade_l.transform.position = mount_weapon_l.transform.position;
            part_blade_l.transform.rotation = mount_weapon_l.transform.rotation;
            part_blade_l.transform.parent = mount_weapon_l.transform.parent;
            part_blade_l.renderer.material = CharacterMaterials.materials[myCostume.DmgTexture];
            if (part_blade_l.transform.Find("X-WeaponTrailA") != null)
            {
                part_blade_l.transform.Find("X-WeaponTrailA").GetComponent<XWeaponTrail>().Deactivate();
                part_blade_l.transform.Find("X-WeaponTrailB").GetComponent<XWeaponTrail>().Deactivate();
                if (gameObject.GetComponent<HERO>() != null)
                {
                    gameObject.GetComponent<HERO>().leftbladetrail = part_blade_l.transform.Find("X-WeaponTrailA").GetComponent<XWeaponTrail>();
                    gameObject.GetComponent<HERO>().leftbladetrail2 = part_blade_l.transform.Find("X-WeaponTrailB").GetComponent<XWeaponTrail>();
                }
            }
        }
        if (myCostume.WeaponRMesh.Length > 0)
        {
            part_blade_r = (GameObject) Instantiate(Resources.Load("Character/" + myCostume.WeaponRMesh));
            part_blade_r.transform.position = mount_weapon_r.transform.position;
            part_blade_r.transform.rotation = mount_weapon_r.transform.rotation;
            part_blade_r.transform.parent = mount_weapon_r.transform.parent;
            part_blade_r.renderer.material = CharacterMaterials.materials[myCostume.DmgTexture];
            if (part_blade_r.transform.Find("X-WeaponTrailA") != null)
            {
                part_blade_r.transform.Find("X-WeaponTrailA").GetComponent<XWeaponTrail>().Deactivate();
                part_blade_r.transform.Find("X-WeaponTrailB").GetComponent<XWeaponTrail>().Deactivate();
                if (gameObject.GetComponent<HERO>() != null)
                {
                    gameObject.GetComponent<HERO>().rightbladetrail = part_blade_r.transform.Find("X-WeaponTrailA").GetComponent<XWeaponTrail>();
                    gameObject.GetComponent<HERO>().rightbladetrail2 = part_blade_r.transform.Find("X-WeaponTrailB").GetComponent<XWeaponTrail>();
                }
            }
        }
    }

    public void createCape()
    {
        Destroy(part_cape);
        if (myCostume.CapeMesh.Length > 0)
        {
            part_cape = GenerateCloth(reference, "Character/" + myCostume.CapeMesh);
            part_cape.renderer.material = CharacterMaterials.materials[myCostume.BrandTexture];
        }
    }

    public void createCape2()
    {
        if (!isDeadBody)
        {
            ClothFactory.DisposeObject(part_cape);
            if (myCostume.CapeMesh.Length > 0)
            {
                part_cape = ClothFactory.GetCape(reference, "Character/" + myCostume.CapeMesh, CharacterMaterials.materials[myCostume.BrandTexture]);
            }
        }
    }

    public void createFace()
    {
        part_face = (GameObject) Instantiate(Resources.Load("Character/character_face"));
        part_face.transform.position = part_head.transform.position;
        part_face.transform.rotation = part_head.transform.rotation;
        part_face.transform.parent = transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head").transform;
    }

    public void createGlass()
    {
        part_glass = (GameObject) Instantiate(Resources.Load("Character/glass"));
        part_glass.transform.position = part_head.transform.position;
        part_glass.transform.rotation = part_head.transform.rotation;
        part_glass.transform.parent = transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head").transform;
    }

    public void createHair()
    {
        Destroy(part_hair);
        Destroy(part_hair_1);
        if (myCostume.HairMesh != string.Empty)
        {
            part_hair = (GameObject) Instantiate(Resources.Load("Character/" + myCostume.HairMesh));
            part_hair.transform.position = part_head.transform.position;
            part_hair.transform.rotation = part_head.transform.rotation;
            part_hair.transform.parent = transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head").transform;
            part_hair.renderer.material = CharacterMaterials.materials[myCostume.HairInfo.texture];
            part_hair.renderer.material.color = myCostume.HairColor;
        }
        if (myCostume.Hair1Mesh.Length > 0)
        {
            part_hair_1 = GenerateCloth(reference, "Character/" + myCostume.Hair1Mesh);
            part_hair_1.renderer.material = CharacterMaterials.materials[myCostume.HairInfo.texture];
            part_hair_1.renderer.material.color = myCostume.HairColor;
        }
    }

    public void createHair2()
    {
        Destroy(part_hair);
        if (!isDeadBody)
        {
            ClothFactory.DisposeObject(part_hair_1);
        }
        if (myCostume.HairMesh != string.Empty)
        {
            part_hair = (GameObject) Instantiate(Resources.Load("Character/" + myCostume.HairMesh));
            part_hair.transform.position = part_head.transform.position;
            part_hair.transform.rotation = part_head.transform.rotation;
            part_hair.transform.parent = transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head").transform;
            part_hair.renderer.material = CharacterMaterials.materials[myCostume.HairInfo.texture];
            part_hair.renderer.material.color = myCostume.HairColor;
        }
        if ((myCostume.Hair1Mesh.Length > 0) && !isDeadBody)
        {
            string name = "Character/" + myCostume.Hair1Mesh;
            Material material = CharacterMaterials.materials[myCostume.HairInfo.texture];
            part_hair_1 = ClothFactory.GetHair(reference, name, material, myCostume.HairColor);
        }
    }

    public void CreateHead() //BUG: KeyNotFoundException https://prnt.sc/fifpfd
    {
        Destroy(part_eye);
        Destroy(part_face);
        Destroy(part_glass);
        Destroy(part_hair);
        if (!isDeadBody)
        {
            ClothFactory.DisposeObject(part_hair_1);
        }
        createHair2();
        if (myCostume.EyeMesh.Length > 0)
        {
            part_eye = (GameObject) Instantiate(Resources.Load("Character/" + myCostume.EyeMesh));
            part_eye.transform.position = part_head.transform.position;
            part_eye.transform.rotation = part_head.transform.rotation;
            part_eye.transform.parent = transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head").transform;
            SetFacialTexture(part_eye, myCostume.EyeTextureId);
        }
        if (myCostume.BeardTextureId >= 0)
        {
            createFace();
            SetFacialTexture(part_face, myCostume.BeardTextureId);
        }
        if (myCostume.GlassTextureId >= 0)
        {
            createGlass();
            SetFacialTexture(part_glass, myCostume.GlassTextureId);
        }
        part_head.renderer.material = CharacterMaterials.materials[myCostume.SkinTexture]; //Probabilmente e' qui che throwa
        part_chest.renderer.material = CharacterMaterials.materials[myCostume.SkinTexture]; //Probabilmente e' qui che throwa
    }

    public void CreateLeftArm()
    {
        Destroy(part_arm_l);
        if (myCostume.ArmLMesh.Length > 0)
        {
            part_arm_l = GenerateCloth(reference, "Character/" + myCostume.ArmLMesh);
            part_arm_l.renderer.material = CharacterMaterials.materials[myCostume.BodyTexture];
        }
        Destroy(part_hand_l);
        if (myCostume.HandLMesh.Length > 0)
        {
            part_hand_l = GenerateCloth(reference, "Character/" + myCostume.HandLMesh);
            part_hand_l.renderer.material = CharacterMaterials.materials[myCostume.SkinTexture];
        }
    }

    public void CreateLowerBody()
    {
        part_leg.renderer.material = CharacterMaterials.materials[myCostume.BodyTexture];
    }

    public void CreateRightArm()
    {
        Destroy(part_arm_r);
        if (myCostume.ArmRMesh.Length > 0)
        {
            part_arm_r = GenerateCloth(reference, "Character/" + myCostume.ArmRMesh);
            part_arm_r.renderer.material = CharacterMaterials.materials[myCostume.BodyTexture];
        }
        Destroy(part_hand_r);
        if (myCostume.HandRMesh.Length > 0)
        {
            part_hand_r = GenerateCloth(reference, "Character/" + myCostume.HandRMesh);
            part_hand_r.renderer.material = CharacterMaterials.materials[myCostume.SkinTexture];
        }
    }

    public void CreateUpperBody()
    {
        Destroy(part_upper_body);
        Destroy(part_brand_1);
        Destroy(part_brand_2);
        Destroy(part_brand_3);
        Destroy(part_brand_4);
        Destroy(part_chest_1);
        Destroy(part_chest_2);
        Destroy(part_chest_3);
        createCape2();
        if (myCostume.PartChestObjectMesh.Length > 0)
        {
            part_chest_1 = (GameObject) Instantiate(Resources.Load("Character/" + myCostume.PartChestObjectMesh));
            part_chest_1.transform.position = chest_info.transform.position;
            part_chest_1.transform.rotation = chest_info.transform.rotation;
            part_chest_1.transform.parent = transform.Find("Amarture/Controller_Body/hip/spine/chest").transform;
            part_chest_1.renderer.material = CharacterMaterials.materials[myCostume.PartChestObjectTexture];
        }
        if (myCostume.PartChest1ObjectMesh.Length > 0)
        {
            part_chest_2 = (GameObject) Instantiate(Resources.Load("Character/" + myCostume.PartChest1ObjectMesh));
            part_chest_2.transform.position = chest_info.transform.position;
            part_chest_2.transform.rotation = chest_info.transform.rotation;
            part_chest_2.transform.parent = transform.Find("Amarture/Controller_Body/hip/spine/chest").transform;
            part_chest_2.transform.parent = transform.Find("Amarture/Controller_Body/hip/spine/chest").transform;
            part_chest_2.renderer.material = CharacterMaterials.materials[myCostume.PartChest1ObjectTexture];
        }
        if (myCostume.PartChestSkinnedClothMesh.Length > 0)
        {
            part_chest_3 = GenerateCloth(reference, "Character/" + myCostume.PartChestSkinnedClothMesh);
            part_chest_3.renderer.material = CharacterMaterials.materials[myCostume.PartChestSkinnedClothTexture];
        }
        if (myCostume.BodyMesh.Length > 0)
        {
            part_upper_body = GenerateCloth(reference, "Character/" + myCostume.BodyMesh);
            part_upper_body.renderer.material = CharacterMaterials.materials[myCostume.BodyTexture];
        }
        if (myCostume.Brand1Mesh.Length > 0)
        {
            part_brand_1 = GenerateCloth(reference, "Character/" + myCostume.Brand1Mesh);
            part_brand_1.renderer.material = CharacterMaterials.materials[myCostume.BrandTexture];
        }
        if (myCostume.Brand2Mesh.Length > 0)
        {
            part_brand_2 = GenerateCloth(reference, "Character/" + myCostume.Brand2Mesh);
            part_brand_2.renderer.material = CharacterMaterials.materials[myCostume.BrandTexture];
        }
        if (myCostume.Brand3Mesh.Length > 0)
        {
            part_brand_3 = GenerateCloth(reference, "Character/" + myCostume.Brand3Mesh);
            part_brand_3.renderer.material = CharacterMaterials.materials[myCostume.BrandTexture];
        }
        if (myCostume.Brand4Mesh.Length > 0)
        {
            part_brand_4 = GenerateCloth(reference, "Character/" + myCostume.Brand4Mesh);
            part_brand_4.renderer.material = CharacterMaterials.materials[myCostume.BrandTexture];
        }
        part_head.renderer.material = CharacterMaterials.materials[myCostume.SkinTexture];
        part_chest.renderer.material = CharacterMaterials.materials[myCostume.SkinTexture];
    }

    public void CreateUpperBody2()
    {
        Destroy(part_upper_body);
        Destroy(part_brand_1);
        Destroy(part_brand_2);
        Destroy(part_brand_3);
        Destroy(part_brand_4);
        Destroy(part_chest_1);
        Destroy(part_chest_2);
        if (!isDeadBody)
        {
            ClothFactory.DisposeObject(part_chest_3);
        }
        createCape2();
        if (myCostume.PartChestObjectMesh.Length > 0)
        {
            part_chest_1 = (GameObject) Instantiate(Resources.Load("Character/" + myCostume.PartChestObjectMesh));
            part_chest_1.transform.position = chest_info.transform.position;
            part_chest_1.transform.rotation = chest_info.transform.rotation;
            part_chest_1.transform.parent = transform.Find("Amarture/Controller_Body/hip/spine/chest").transform;
            part_chest_1.renderer.material = CharacterMaterials.materials[myCostume.PartChestObjectTexture];
        }
        if (myCostume.PartChest1ObjectMesh.Length > 0)
        {
            part_chest_2 = (GameObject) Instantiate(Resources.Load("Character/" + myCostume.PartChest1ObjectMesh));
            part_chest_2.transform.position = chest_info.transform.position;
            part_chest_2.transform.rotation = chest_info.transform.rotation;
            part_chest_2.transform.parent = transform.Find("Amarture/Controller_Body/hip/spine/chest").transform;
            part_chest_2.transform.parent = transform.Find("Amarture/Controller_Body/hip/spine/chest").transform;
            part_chest_2.renderer.material = CharacterMaterials.materials[myCostume.PartChest1ObjectTexture];
        }
        if ((myCostume.PartChestSkinnedClothMesh.Length > 0) && !isDeadBody)
        {
            part_chest_3 = ClothFactory.GetCape(reference, "Character/" + myCostume.PartChestSkinnedClothMesh, CharacterMaterials.materials[myCostume.PartChestSkinnedClothTexture]);
        }
        if (myCostume.BodyMesh.Length > 0)
        {
            part_upper_body = GenerateCloth(reference, "Character/" + myCostume.BodyMesh);
            part_upper_body.renderer.material = CharacterMaterials.materials[myCostume.BodyTexture];
        }
        if (myCostume.Brand1Mesh.Length > 0)
        {
            part_brand_1 = GenerateCloth(reference, "Character/" + myCostume.Brand1Mesh);
            part_brand_1.renderer.material = CharacterMaterials.materials[myCostume.BrandTexture];
        }
        if (myCostume.Brand2Mesh.Length > 0)
        {
            part_brand_2 = GenerateCloth(reference, "Character/" + myCostume.Brand2Mesh);
            part_brand_2.renderer.material = CharacterMaterials.materials[myCostume.BrandTexture];
        }
        if (myCostume.Brand3Mesh.Length > 0)
        {
            part_brand_3 = GenerateCloth(reference, "Character/" + myCostume.Brand3Mesh);
            part_brand_3.renderer.material = CharacterMaterials.materials[myCostume.BrandTexture];
        }
        if (myCostume.Brand4Mesh.Length > 0)
        {
            part_brand_4 = GenerateCloth(reference, "Character/" + myCostume.Brand4Mesh);
            part_brand_4.renderer.material = CharacterMaterials.materials[myCostume.BrandTexture];
        }
        part_head.renderer.material = CharacterMaterials.materials[myCostume.SkinTexture];
        part_chest.renderer.material = CharacterMaterials.materials[myCostume.SkinTexture];
    }

    public void DeleteCharacterComponent()
    {
        Destroy(part_eye);
        Destroy(part_face);
        Destroy(part_glass);
        Destroy(part_hair);
        Destroy(part_hair_1);
        Destroy(part_upper_body);
        Destroy(part_arm_l);
        Destroy(part_arm_r);
        Destroy(part_hair_2);
        Destroy(part_cape);
        Destroy(part_brand_1);
        Destroy(part_brand_2);
        Destroy(part_brand_3);
        Destroy(part_brand_4);
        Destroy(part_chest_1);
        Destroy(part_chest_2);
        Destroy(part_chest_3);
        Destroy(part_3dmg);
        Destroy(part_3dmg_belt);
        Destroy(part_3dmg_gas_l);
        Destroy(part_3dmg_gas_r);
        Destroy(part_blade_l);
        Destroy(part_blade_r);
    }

    public void DeleteCharacterComponent2()
    {
        Destroy(part_eye);
        Destroy(part_face);
        Destroy(part_glass);
        Destroy(part_hair);
        if (!isDeadBody)
        {
            ClothFactory.DisposeObject(part_hair_1);
        }
        Destroy(part_upper_body);
        Destroy(part_arm_l);
        Destroy(part_arm_r);
        if (!isDeadBody)
        {
            ClothFactory.DisposeObject(part_hair_2);
            ClothFactory.DisposeObject(part_cape);
        }
        Destroy(part_brand_1);
        Destroy(part_brand_2);
        Destroy(part_brand_3);
        Destroy(part_brand_4);
        Destroy(part_chest_1);
        Destroy(part_chest_2);
        Destroy(part_chest_3);
        Destroy(part_3dmg);
        Destroy(part_3dmg_belt);
        Destroy(part_3dmg_gas_l);
        Destroy(part_3dmg_gas_r);
        Destroy(part_blade_l);
        Destroy(part_blade_r);
    }

    private static GameObject GenerateCloth(GameObject go, string res)
    {
        if (go.GetComponent<SkinnedMeshRenderer>() == null)
        {
            go.AddComponent<SkinnedMeshRenderer>();
        }
        SkinnedMeshRenderer component = go.GetComponent<SkinnedMeshRenderer>();
        Transform[] bones = component.bones;
        SkinnedMeshRenderer renderer2 = ((GameObject) Instantiate(Resources.Load(res))).GetComponent<SkinnedMeshRenderer>();
        renderer2.gameObject.transform.parent = component.gameObject.transform.parent;
        renderer2.transform.localPosition = Vector3.zero;
        renderer2.transform.localScale = Vector3.one;
        renderer2.bones = bones;
        renderer2.quality = SkinQuality.Bone4;
        return renderer2.gameObject;
    }

    private byte[] GetCurrentConfig()
    {
        return config;
    }

    public void Initialize()
    {
        CharacterMaterials.init();
    }

    public void SetCharacterComponent()
    {
        CreateHead();
        CreateUpperBody2();
        CreateLeftArm();
        CreateRightArm();
        CreateLowerBody();
        create3DMG();
    }

    public void SetFacialTexture(GameObject go, int id)
    {
        if (id >= 0)
        {
            go.renderer.material = CharacterMaterials.materials[myCostume.FaceTexture];
            float num = 0.125f;
            float x = num * ((int) (((float) id) / 8f));
            float y = -0.125f * (id % 8);
            go.renderer.material.mainTextureOffset = new Vector2(x, y);
        }
    }

    public void SetSkin()
    {
        part_head.renderer.material = CharacterMaterials.materials[myCostume.SkinTexture];
        part_chest.renderer.material = CharacterMaterials.materials[myCostume.SkinTexture];
        part_hand_l.renderer.material = CharacterMaterials.materials[myCostume.SkinTexture];
        part_hand_r.renderer.material = CharacterMaterials.materials[myCostume.SkinTexture];
    }
}

