using System.Collections;
using Mod;
using Mod.commands;
using Mod.manager;
using UnityEngine;

public class TriggerColliderWeapon : MonoBehaviour
{
    public bool active_me;
    public GameObject currentCamera;
    public ArrayList currentHits = new ArrayList();
    public ArrayList currentHitsII = new ArrayList();
    public AudioSource meatDie;
    public int myTeam = 1;
    public float scoreMulti = 1f;

    private bool CheckIfBehind(GameObject titan)
    {
        Transform transform = titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
        Vector3 to = this.transform.position - transform.transform.position;
        return (Vector3.Angle(-transform.transform.forward, to) < 70f);
    }

    public void ClearHits()
    {
        currentHitsII = new ArrayList();
        currentHits = new ArrayList();
    }

    private static void NapeMeat(Vector3 vkill, Transform titan)
    {
        Transform transform = titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
        GameObject obj2 = (GameObject) Instantiate(Resources.Load("titanNapeMeat"), transform.position, transform.rotation);
        obj2.transform.localScale = titan.localScale;
        obj2.rigidbody.AddForce(vkill.normalized * 15f, ForceMode.Impulse);
        obj2.rigidbody.AddForce(-titan.forward * 10f, ForceMode.Impulse);
        obj2.rigidbody.AddTorque(new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)), ForceMode.Impulse);
    }

    private void OnTriggerStay(Collider other)
    {
        if (active_me)
        {
            if (!currentHitsII.Contains(other.gameObject))
            {
                currentHitsII.Add(other.gameObject);
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.1f, 0.1f, 0.95f);
                if (other.gameObject.transform.root.gameObject.tag == "titan")
                {
                    GameObject obj2;
                    currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<HERO>().slashHit.Play();
                    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                    {   
                        obj2 = PhotonNetwork.Instantiate("hitMeat", transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
                    }
                    else
                    {
                        obj2 = (GameObject) Instantiate(Resources.Load("hitMeat"));
                    }
                    obj2.transform.position = transform.position;
                    transform.root.GetComponent<HERO>().useBlade(0);
                }
            }
            switch (other.gameObject.tag)
            {
                case "playerHitbox":
                    if (LevelInfo.GetInfo(FengGameManagerMKII.level).pvp || ModManager.Find("module.pvpeverywhere").Enabled)
                    {
                        float b = 1f - (Vector3.Distance(other.gameObject.transform.position, transform.position) * 0.05f);
                        b = Mathf.Min(1f, b);
                        HitBox component = other.gameObject.GetComponent<HitBox>();
                        if (component != null && !component.transform.root.gameObject.GetPhotonView().isMine && (component.transform.root != null && component.transform.root.GetComponent<HERO>().myTeam != myTeam && !component.transform.root.GetComponent<HERO>().isInvincible() || ModManager.Find("module.pvpeverywhere").Enabled))
                        {
                            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                            {
                                if (!component.transform.root.GetComponent<HERO>().isGrabbed)
                                {
                                    Vector3 vector = component.transform.root.transform.position - transform.position;
                                    component.transform.root.GetComponent<HERO>().Die(((vector.normalized * b) * 1000f) + (Vector3.up * 50f), false);
                                }
                            }
                            else if (((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && !component.transform.root.GetComponent<HERO>().HasDied()) && !component.transform.root.GetComponent<HERO>().isGrabbed)
                            {
                                component.transform.root.GetComponent<HERO>().markDie();
                                object[] parameters = new object[5];
                                Vector3 vector2 = component.transform.root.position - transform.position;
                                parameters[0] = ((vector2.normalized * b) * 1000f) + (Vector3.up * 50f);
                                parameters[1] = false;
                                parameters[2] = transform.root.gameObject.GetPhotonView().viewID;
                                parameters[3] = PhotonView.Find(transform.root.gameObject.GetPhotonView().viewID).owner.Name;
                                parameters[4] = false;
                                component.transform.root.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters);
                            }
                        }
                    }
                    break;
                case "titanneck":
                    HitBox item = other.gameObject.GetComponent<HitBox>();
                    if (((item != null) && CheckIfBehind(item.transform.root.gameObject)) && !currentHits.Contains(item))
                    {
                        item.hitPosition = (transform.position + item.transform.position) * 0.5f;
                        currentHits.Add(item);
                        meatDie.Play();


                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                        {
                            if ((item.transform.root.GetComponent<TITAN>() != null) && !item.transform.root.GetComponent<TITAN>().hasDie)
                            {
                                Vector3 vector3 = currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - item.transform.root.rigidbody.velocity;
                                int num2 = (int) ((vector3.magnitude * 10f) * scoreMulti);
                                num2 = CommandDamage.Damage == -1 ? Mathf.Max(10, num2) : CommandDamage.Damage;
                                num2 = CommandDamage.Damage == -1 ? (ModManager.Find("module.doubledamage").Enabled ? num2*2 : num2) : num2;
                                if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
                                {
                                    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(item.transform.position, num2, item.transform.root.gameObject, 0.02f);
                                }
                                item.transform.root.GetComponent<TITAN>().die();
                                NapeMeat(currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, item.transform.root);
                                GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().netShowDamage(num2);
                                GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().playerKillInfoSingleUpdate(num2);
                            }
                        }
                        else if (!PhotonNetwork.isMasterClient)
                        {
                            if (item.transform.root.GetComponent<TITAN>() != null)
                            {
                                if (!item.transform.root.GetComponent<TITAN>().hasDie)
                                {
                                    Vector3 vector4 = currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - item.transform.root.rigidbody.velocity;
                                    int num3 = (int) ((vector4.magnitude * 10f) * scoreMulti);
                                    num3 = CommandDamage.Damage == -1 ? Mathf.Max(10, num3) : CommandDamage.Damage;
                                    num3 = CommandDamage.Damage == -1 ? (ModManager.Find("module.doubledamage").Enabled ? num3 * 2 : num3) : num3;
                                    if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
                                    {
                                        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(item.transform.position, num3, item.transform.root.gameObject, 0.02f);
                                        item.transform.root.GetComponent<TITAN>().asClientLookTarget = false;
                                    }
                                    object[] objArray2 = { transform.root.gameObject.GetPhotonView().viewID, num3 };
                                    item.transform.root.GetComponent<TITAN>().photonView.RPC("titanGetHit", item.transform.root.GetComponent<TITAN>().photonView.owner, objArray2);
                                }
                            }
                            else if (item.transform.root.GetComponent<FEMALE_TITAN>() != null)
                            {
                                transform.root.GetComponent<HERO>().useBlade(2147483647);
                                Vector3 vector5 = currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - item.transform.root.rigidbody.velocity;
                                int num4 = (int) ((vector5.magnitude * 10f) * scoreMulti);
                                num4 = Mathf.Max(10, num4);
                                if (!item.transform.root.GetComponent<FEMALE_TITAN>().hasDie)
                                {
                                    object[] objArray3 = { transform.root.gameObject.GetPhotonView().viewID, num4 };
                                    item.transform.root.GetComponent<FEMALE_TITAN>().photonView.RPC("titanGetHit", item.transform.root.GetComponent<FEMALE_TITAN>().photonView.owner, objArray3);
                                }
                            }
                            else if (item.transform.root.GetComponent<COLOSSAL_TITAN>() != null)
                            {
                                transform.root.GetComponent<HERO>().useBlade(2147483647);
                                if (!item.transform.root.GetComponent<COLOSSAL_TITAN>().hasDie)
                                {
                                    Vector3 vector6 = currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - item.transform.root.rigidbody.velocity;
                                    int num5 = (int) ((vector6.magnitude * 10f) * scoreMulti);
                                    num5 = Mathf.Max(10, num5);
                                    object[] objArray4 = { transform.root.gameObject.GetPhotonView().viewID, num5 };
                                    item.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.RPC("titanGetHit", item.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.owner, objArray4);
                                }
                            }
                        }
                        else if (item.transform.root.GetComponent<TITAN>() != null)
                        {
                            if (!item.transform.root.GetComponent<TITAN>().hasDie)
                            {
                                Vector3 vector7 = currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - item.transform.root.rigidbody.velocity;
                                int num6 = (int) ((vector7.magnitude * 10f) * scoreMulti);
                                num6 = Mathf.Max(10, num6);
                                if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
                                {
                                    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(item.transform.position, num6, item.transform.root.gameObject, 0.02f);
                                }
                                item.transform.root.GetComponent<TITAN>().titanGetHit(transform.root.gameObject.GetPhotonView().viewID, num6);
                            }
                        }
                        else if (item.transform.root.GetComponent<FEMALE_TITAN>() != null)
                        {
                            transform.root.GetComponent<HERO>().useBlade(2147483647);
                            if (!item.transform.root.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                Vector3 vector8 = currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - item.transform.root.rigidbody.velocity;
                                int num7 = (int) ((vector8.magnitude * 10f) * scoreMulti);
                                num7 = Mathf.Max(10, num7);
                                if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
                                {
                                    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(item.transform.position, num7, null, 0.02f);
                                }
                                item.transform.root.GetComponent<FEMALE_TITAN>().titanGetHit(transform.root.gameObject.GetPhotonView().viewID, num7);
                            }
                        }
                        else if (item.transform.root.GetComponent<COLOSSAL_TITAN>() != null)
                        {
                            transform.root.GetComponent<HERO>().useBlade(2147483647);
                            if (!item.transform.root.GetComponent<COLOSSAL_TITAN>().hasDie)
                            {
                                Vector3 vector9 = currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - item.transform.root.rigidbody.velocity;
                                int num8 = (int) ((vector9.magnitude * 10f) * scoreMulti);
                                num8 = Mathf.Max(10, num8);
                                if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
                                {
                                    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(item.transform.position, num8, null, 0.02f);
                                }
                                item.transform.root.GetComponent<COLOSSAL_TITAN>().titanGetHit(transform.root.gameObject.GetPhotonView().viewID, num8);
                            }
                        }
                        showCriticalHitFX();
                    }
                    break;
                case "titaneye":
                    if (!currentHits.Contains(other.gameObject))
                    {
                        currentHits.Add(other.gameObject);
                        GameObject gameObject = other.gameObject.transform.root.gameObject;
                        if (gameObject.GetComponent<FEMALE_TITAN>() != null)
                        {
                            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                            {
                                if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
                                {
                                    gameObject.GetComponent<FEMALE_TITAN>().hitEye();
                                }
                            }
                            else if (!PhotonNetwork.isMasterClient)
                            {
                                if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
                                {
                                    object[] objArray5 = { transform.root.gameObject.GetPhotonView().viewID };
                                    gameObject.GetComponent<FEMALE_TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, objArray5);
                                }
                            }
                            else if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                gameObject.GetComponent<FEMALE_TITAN>().hitEyeRPC(transform.root.gameObject.GetPhotonView().viewID);
                            }
                        }
                        else if (gameObject.GetComponent<TITAN>().abnormalType != AbnormalType.TYPE_CRAWLER)
                        {
                            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                            {
                                if (!gameObject.GetComponent<TITAN>().hasDie)
                                {
                                    gameObject.GetComponent<TITAN>().hitEye();
                                }
                            }
                            else if (!PhotonNetwork.isMasterClient)
                            {
                                if (!gameObject.GetComponent<TITAN>().hasDie)
                                {
                                    object[] objArray6 = { transform.root.gameObject.GetPhotonView().viewID };
                                    gameObject.GetComponent<TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, objArray6);
                                }
                            }
                            else if (!gameObject.GetComponent<TITAN>().hasDie)
                            {
                                gameObject.GetComponent<TITAN>().hitEyeRPC(transform.root.gameObject.GetPhotonView().viewID);
                            }
                            showCriticalHitFX();
                        }
                    }
                    break;
                default:
                    if ((other.gameObject.tag == "titanankle") && !currentHits.Contains(other.gameObject))
                    {
                        currentHits.Add(other.gameObject);
                        GameObject obj4 = other.gameObject.transform.root.gameObject;
                        Vector3 vector10 = currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - obj4.rigidbody.velocity;
                        int num9 = (int) ((vector10.magnitude * 10f) * scoreMulti);
                        num9 = Mathf.Max(10, num9);
                        if ((obj4.GetComponent<TITAN>() != null) && (obj4.GetComponent<TITAN>().abnormalType != AbnormalType.TYPE_CRAWLER))
                        {
                            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                            {
                                if (!obj4.GetComponent<TITAN>().hasDie)
                                {
                                    obj4.GetComponent<TITAN>().hitAnkle();
                                }
                            }
                            else
                            {
                                if (!PhotonNetwork.isMasterClient)
                                {
                                    if (!obj4.GetComponent<TITAN>().hasDie)
                                    {
                                        object[] objArray7 = { transform.root.gameObject.GetPhotonView().viewID };
                                        obj4.GetComponent<TITAN>().photonView.RPC("hitAnkleRPC", PhotonTargets.MasterClient, objArray7);
                                    }
                                }
                                else if (!obj4.GetComponent<TITAN>().hasDie)
                                {
                                    obj4.GetComponent<TITAN>().hitAnkle();
                                }
                                showCriticalHitFX();
                            }
                        }
                        else if (obj4.GetComponent<FEMALE_TITAN>() != null)
                        {
                            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                            {
                                if (other.gameObject.name == "ankleR")
                                {
                                    if ((obj4.GetComponent<FEMALE_TITAN>() != null) && !obj4.GetComponent<FEMALE_TITAN>().hasDie)
                                    {
                                        obj4.GetComponent<FEMALE_TITAN>().hitAnkleR(num9);
                                    }
                                }
                                else if ((obj4.GetComponent<FEMALE_TITAN>() != null) && !obj4.GetComponent<FEMALE_TITAN>().hasDie)
                                {
                                    obj4.GetComponent<FEMALE_TITAN>().hitAnkleL(num9);
                                }
                            }
                            else if (other.gameObject.name == "ankleR")
                            {
                                if (!PhotonNetwork.isMasterClient)
                                {
                                    if (!obj4.GetComponent<FEMALE_TITAN>().hasDie)
                                    {
                                        object[] objArray8 = { transform.root.gameObject.GetPhotonView().viewID, num9 };
                                        obj4.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleRRPC", PhotonTargets.MasterClient, objArray8);
                                    }
                                }
                                else if (!obj4.GetComponent<FEMALE_TITAN>().hasDie)
                                {
                                    obj4.GetComponent<FEMALE_TITAN>().hitAnkleRRPC(transform.root.gameObject.GetPhotonView().viewID, num9);
                                }
                            }
                            else if (!PhotonNetwork.isMasterClient)
                            {
                                if (!obj4.GetComponent<FEMALE_TITAN>().hasDie)
                                {
                                    object[] objArray9 = { transform.root.gameObject.GetPhotonView().viewID, num9 };
                                    obj4.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleLRPC", PhotonTargets.MasterClient, objArray9);
                                }
                            }
                            else if (!obj4.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                obj4.GetComponent<FEMALE_TITAN>().hitAnkleLRPC(transform.root.gameObject.GetPhotonView().viewID, num9);
                            }
                            showCriticalHitFX();
                        }
                    }
                    break;
            }
        }
    }

    private void showCriticalHitFX()
    {
        GameObject obj2;
        currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.2f, 0.3f, 0.95f);
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            obj2 = PhotonNetwork.Instantiate("redCross", transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
        }
        else
        {
            obj2 = (GameObject) Instantiate(Resources.Load("redCross"));
        }
        obj2.transform.position = transform.position;
    }

    private void Start()
    {
        currentCamera = GameObject.Find("MainCamera");
    }
}

