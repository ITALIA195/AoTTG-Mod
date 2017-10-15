using System;
using System.Collections;
using System.IO;
using Mod;
using Mod.commands;
using Mod.gui;
using SimpleJSON;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIMainReferences : MonoBehaviour
{
    public static UIMainReferences instance;
    public static string fengVersion;
    public static bool Firstload = true;
    public GameObject panelCredits;
    public GameObject PanelDisconnect;
    public GameObject panelMain;
    public GameObject PanelMultiJoinPrivate;
    public GameObject PanelMultiPWD;
    public GameObject panelMultiROOM;
    public GameObject panelMultiSet;
    public GameObject panelMultiStart;
    public GameObject PanelMultiWait;
    public GameObject panelOption;
    public GameObject panelSingleSet;
    public GameObject PanelSnapShot;
    public static string version = "01042015";
    //public static string version = "12262016";

    public static IEnumerator request()
    {
        while (!Caching.ready)
            yield return null;
        using (WWW assets = WWW.LoadFromCacheOrDownload($"File://{Application.dataPath}/RCAssets.unity3d", 1))
        {
            yield return assets;
            if (assets.error != null)
                Core.LogFile("Errore: " + assets.error);
            FengGameManagerMKII.RCassets = assets.assetBundle;
            FengGameManagerMKII.isAssetLoaded = true;
        }
    }

    //internal IEnumerator WhyTheFuck()
    //{
    //    string[] arrStrings = { "1.png", "2.jpg", "3.png", "4.png", "5.jpg" };
    //    using (WWW www = new WWW(@"http://hdlclan.ovh/AoTTG/Assets/background" + arrStrings[Random.Range(0, arrStrings.Length - 1)]))
    //    {
    //        yield return www;
    //        if (www.error != null)
    //            Core.LogFile(www.error, ErrorType.Error);
    //        Textures.BACKGROUND_IMAGE = www.texture;
    //    }
    //}

    //public IEnumerator LoadBackground()
    //{
    //    JSONNode json;
    //    using (WWW www = new WWW("http://hdlclan.ovh/AoTTG/Assets/background.json"))
    //    {
    //        yield return www;
    //        if (www.error != null)
    //            Core.LogFile(www.error);
    //        json = JSON.Parse(www.text);
    //    }
    //    if (json == null)
    //    {
    //        Core.LogFile("Error reading json.");
    //        yield break;
    //    }

    //    json = json["background"][Random.Range(1, json["images"].AsInt).ToString()];
    //    if (!Directory.Exists(Application.dataPath + "/Assets/"))
    //        Directory.CreateDirectory(Application.dataPath + "/Assets/");
    //    if (File.Exists(Application.dataPath + "/Assets/" + json["url"]) && Core.GetMD5(Application.dataPath + "/Assets/" + json["url"]) != json["md5"].AsString())
    //        File.Delete(Application.dataPath + "/Assets/" + json["url"]);
    //    if (!File.Exists(Application.dataPath + "/Assets/" + json["url"]))
    //        using (WWW www = new WWW("http://hdlclan.ovh/AoTTG/Assets/" + json["url"]))
    //        {
    //            if (www.error != null)
    //                Core.LogFile(www.error);
    //            yield return www;
    //            File.WriteAllBytes(Application.dataPath + "/Assets/" + json["url"], www.bytes);
    //        }
    //    using (WWW www = new WWW("File://" + Application.dataPath + "/Assets/" + json["url"]))
    //    {
    //        if (www.error != null)
    //            Core.LogFile(www.error);
    //        Textures.BACKGROUND_IMAGE = www.texture;
    //        Destroy(www.texture);
    //    }
        
    //}

    private void Start()
    {
        instance = this;
        fengVersion = "01042015";
        if (!Firstload) return;
        version = fengVersion;
        Firstload = false;
        GameObject target = (GameObject)Instantiate(Resources.Load("InputManagerController"));
        target.name = "InputManagerController";
        DontDestroyOnLoad(target);
        FengGameManagerMKII.s = "verified343,hair,character_eye,glass,character_face,character_head,character_hand,character_body,character_arm,character_leg,character_chest,character_cape,character_brand,character_3dmg,r,character_blade_l,character_3dmg_gas_r,character_blade_r,3dmg_smoke,HORSE,hair,body_001,Cube,Plane_031,mikasa_asset,character_cap_,character_gun".Split(new char[] { ',' });
        StartCoroutine(request());
        //StartCoroutine(LoadBackground());
        //StartCoroutine(WhyTheFuck());
        FengGameManagerMKII.loginstate = 0;
    }
}