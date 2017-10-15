using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class TITAN_SETUP : Photon.MonoBehaviour
{
    public GameObject eye;
    private CostumeHair hair;
    private GameObject hair_go_ref;
    private int hairType;
    public bool haseye;
    private GameObject part_hair;
    public int skin;

    private void Awake()
    {
        CostumeHair.init();
        CharacterMaterials.init();
        HeroCostume.Initialize();
        hair_go_ref = new GameObject();
        eye.transform.parent = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").transform;
        hair_go_ref.transform.position = (Vector3) ((eye.transform.position + (Vector3.up * 3.5f)) + (transform.forward * 5.2f));
        hair_go_ref.transform.rotation = eye.transform.rotation;
        hair_go_ref.transform.RotateAround(eye.transform.position, transform.right, -20f);
        hair_go_ref.transform.localScale = new Vector3(210f, 210f, 210f);
        hair_go_ref.transform.parent = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").transform;
    }

    [RPC]
    public IEnumerator LoadSkinTitan(int hair2, int eye2, string hairlink)
    {
        bool iteratorVariable0 = false;
        Destroy(part_hair);
        hair = CostumeHair.hairsM[hair2];
        hairType = hair2;
        if (hair.hair != string.Empty)
        {
            GameObject partHair = (GameObject) Instantiate(Resources.Load("Character/" + hair.hair));
            partHair.transform.parent = hair_go_ref.transform.parent;
            partHair.transform.position = hair_go_ref.transform.position;
            partHair.transform.rotation = hair_go_ref.transform.rotation;
            partHair.transform.localScale = hair_go_ref.transform.localScale;
            partHair.renderer.material = CharacterMaterials.materials[hair.texture];
            bool mipmap = (int) FengGameManagerMKII.settings[63] != 1;
            if (Regex.IsMatch(hairlink, @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)|transparent$", RegexOptions.IgnoreCase))
            {
                if (hairlink.ToLower() == "transparent")
                {
                    partHair.renderer.enabled = false;
                }
                else if (!FengGameManagerMKII.linkHash[0].ContainsKey(hairlink))
                {
                    WWW link = new WWW(hairlink);
                    yield return link;
                    Texture2D iteratorVariable4 = RCextensions.loadimage(link, mipmap, 200000);
                    link.Dispose();
                    if (FengGameManagerMKII.linkHash[0].ContainsKey(hairlink))
                    {
                        partHair.renderer.material = (Material)FengGameManagerMKII.linkHash[0][hairlink];
                    }
                    else
                    {
                        iteratorVariable0 = true;
                        partHair.renderer.material.mainTexture = iteratorVariable4;
                        FengGameManagerMKII.linkHash[0].Add(hairlink, partHair.renderer.material);
                        partHair.renderer.material = (Material)FengGameManagerMKII.linkHash[0][hairlink];
                    }
                }
                else
                {
                    partHair.renderer.material = (Material) FengGameManagerMKII.linkHash[0][hairlink];
                }
            }
            part_hair = partHair;
        }
        if (eye2 >= 0)
        {
            SetFacialTexture(eye, eye2);
        }
        if (iteratorVariable0)
        {
            FengGameManagerMKII.instance.unloadAssets();
        }
    }

    public void SetFacialTexture(GameObject go, int id)
    {
        if (id < 0) return;
        go.renderer.material.mainTextureOffset = new Vector2(0.125f * (int)(id / 8f), -0.25f * (id % 4));
    }

    public void SetHair()
    {
        object[] objArray2;
        if ((((int) FengGameManagerMKII.settings[1]) == 1) && ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || photonView.isMine))
        {
            Color color;
            int num = Random.Range(0, 9);
            if (num == 3) //? WTF IS THIS
            {
                num = 9;
            }
            int index = skin - 70;
            if (((int) FengGameManagerMKII.settings[32]) == 1)
            {
                index = Random.Range(16, 20);
            }
            if (((int) FengGameManagerMKII.settings[index]) >= 0)
            {
                num = (int) FengGameManagerMKII.settings[index];
            }
            string hairlink = (string) FengGameManagerMKII.settings[index + 5];
            int eye = Random.Range(1, 8);
            if (haseye)
            {
                eye = 0;
            }
            bool flag2 = Regex.IsMatch(hairlink, @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)|transparent$", RegexOptions.IgnoreCase);
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && photonView.isMine)
            {
                if (flag2)
                {
                    objArray2 = new object[] { num, eye, hairlink };
                    photonView.RPC("setHairRPC2", PhotonTargets.AllBuffered, objArray2);
                }
                else
                {
                    color = HeroCostume.Costume[Random.Range(0, HeroCostume.Costume.Length - 5)].HairColor;
                    objArray2 = new object[] { num, eye, color.r, color.g, color.b };
                    photonView.RPC("setHairPRC", PhotonTargets.AllBuffered, objArray2);
                }
            }
            else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                if (flag2)
                {
                    StartCoroutine(LoadSkinTitan(num, eye, hairlink));
                }
                else
                {
                    color = HeroCostume.Costume[Random.Range(0, HeroCostume.Costume.Length - 5)].HairColor;
                    setHairPRC(num, eye, color.r, color.g, color.b);
                }
            }
        }
        else
        {
            int num = Random.Range(0, CostumeHair.hairsM.Length);
            if (num == 3)
            {
                num = 9;
            }
            Destroy(part_hair);
            hairType = num;
            hair = CostumeHair.hairsM[num];
            if (hair.hair == string.Empty)
            {
                hair = CostumeHair.hairsM[9];
                hairType = 9;
            }
            part_hair = (GameObject) Instantiate(Resources.Load("Character/" + hair.hair));
            part_hair.transform.parent = hair_go_ref.transform.parent;
            part_hair.transform.position = hair_go_ref.transform.position;
            part_hair.transform.rotation = hair_go_ref.transform.rotation;
            part_hair.transform.localScale = hair_go_ref.transform.localScale;
            part_hair.renderer.material = CharacterMaterials.materials[hair.texture];
            part_hair.renderer.material.color = HeroCostume.Costume[Random.Range(0, HeroCostume.Costume.Length - 5)].HairColor;
            int id = Random.Range(1, 8);
            SetFacialTexture(eye, id);
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && photonView.isMine)
            {
                objArray2 = new object[] { hairType, id, part_hair.renderer.material.color.r, part_hair.renderer.material.color.g, part_hair.renderer.material.color.b };
                photonView.RPC("setHairPRC", PhotonTargets.OthersBuffered, objArray2);
            }
        }
    }

    [RPC]
    private void setHairPRC(int type, int eye_type, float c1, float c2, float c3)
    {
        Destroy(part_hair);
        hair = CostumeHair.hairsM[type];
        hairType = type;
        if (hair.hair != string.Empty)
        {
            GameObject obj2 = (GameObject) Instantiate(Resources.Load("Character/" + hair.hair));
            obj2.transform.parent = hair_go_ref.transform.parent;
            obj2.transform.position = hair_go_ref.transform.position;
            obj2.transform.rotation = hair_go_ref.transform.rotation;
            obj2.transform.localScale = hair_go_ref.transform.localScale;
            obj2.renderer.material = CharacterMaterials.materials[hair.texture];
            obj2.renderer.material.color = new Color(c1, c2, c3);
            part_hair = obj2;
        }
        SetFacialTexture(eye, eye_type);
    }

    [RPC]
    public void setHairRPC2(int hair, int eye, string hairlink)
    {
        if (((int) FengGameManagerMKII.settings[1]) == 1)
        {
            StartCoroutine(LoadSkinTitan(hair, eye, hairlink));
        }
    }

    public void setVar(int skin, bool haseye)
    {
        this.skin = skin;
        this.haseye = haseye;
    }
}