using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ExitGames.Client.Photon;
using Mod;
using Mod.manager;
using UnityEngine;
using Xft;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class HERO : Photon.MonoBehaviour
{
    #region Fields

    private HERO_STATE _state;
    private bool almostSingleHook;
    private string attackAnimation;
    private int attackLoop;
    private bool attackMove;
    private bool attackReleased;
    public AudioSource audio_ally;
    public AudioSource audio_hitwall;
    private GameObject badGuy;
    public Animation baseAnimation;
    public Rigidbody baseRigidBody;
    public Transform baseTransform;
    public bool bigLean;
    public float bombCD;
    public bool bombImmune;
    public float bombRadius;
    public float bombSpeed;
    public float bombTime;
    public float bombTimeMax;
    private float buffTime;
    public GameObject bulletLeft;
    private int bulletMAX = 7;
    public GameObject bulletRight;
    private bool buttonAttackRelease;
    public Dictionary<string, UISprite> cachedSprites;
    public float CameraMultiplier;
    public bool canJump = true;
    public GameObject checkBoxLeft;
    public GameObject checkBoxRight;
    public GameObject cross1;
    public GameObject cross2;
    public GameObject crossL1;
    public GameObject crossL2;
    public GameObject crossR1;
    public GameObject crossR2;
    public string currentAnimation;
    private int currentBladeNum = 5;
    private float currentBladeSta = 100f;
    private BUFF currentBuff;
    public Camera currentCamera;
    private float currentGas = 100f;
    public float currentSpeed;
    public Vector3 currentV;
    private bool dashD;
    private Vector3 dashDirection;
    private bool dashL;
    private bool dashR;
    private float dashTime;
    private bool dashU;
    private Vector3 dashV;
    public bool detonate;
    private float dTapTime = -1f;
    private bool EHold;
    private GameObject eren_titan;
    private int escapeTimes = 1;
    private float facingDirection;
    private float flare1CD;
    private float flare2CD;
    private float flare3CD;
    private float flareTotalCD = 30f;
    private Transform forearmL;
    private Transform forearmR;
    private float gravity = 20f;
    private bool grounded;
    private GameObject gunDummy;
    private Vector3 gunTarget;
    private Transform handL;
    private Transform handR;
    private bool hasDied;
    public bool hasspawn;
    private bool hookBySomeOne = true;
    public GameObject hookRefL1;
    public GameObject hookRefL2;
    public GameObject hookRefR1;
    public GameObject hookRefR2;
    private bool hookSomeOne;
    private GameObject hookTarget;
    public FengCustomInputs inputManager;
    private float invincible = 3f;
    public bool isCannon;
    private bool isLaunchLeft;
    private bool isLaunchRight;
    private bool isLeftHandHooked;
    private bool isMounted;
    public bool isPhotonCamera;
    private bool isRightHandHooked;
    public float jumpHeight = 2f;
    private bool justGrounded;
    public GameObject LabelDistance;
    public Transform lastHook;
    private float launchElapsedTimeL;
    private float launchElapsedTimeR;
    private Vector3 launchForce;
    private Vector3 launchPointLeft;
    private Vector3 launchPointRight;
    private bool leanLeft;
    private bool leftArmAim;
    public XWeaponTrail leftbladetrail;
    public XWeaponTrail leftbladetrail2;
    private int leftBulletLeft = 7;
    private bool leftGunHasBullet = true;
    private float lTapTime = -1f;
    public GameObject maincamera;
    public float maxVelocityChange = 10f;
    public AudioSource meatDie;
    public Bomb myBomb;
    public GameObject myCannon;
    public Transform myCannonBase;
    public Transform myCannonPlayer;
    public CannonPropRegion myCannonRegion;
    public GROUP myGroup;
    private GameObject myHorse;
    public GameObject myNetWorkName;
    public float myScale = 1f;
    public int myTeam = 1;
    public List<TITAN> myTitans;
    private bool needLean;
    private Quaternion oldHeadRotation;
    private float originVM;
    private bool QHold;
    private string reloadAnimation = string.Empty;
    private bool rightArmAim;
    public XWeaponTrail rightbladetrail;
    public XWeaponTrail rightbladetrail2;
    private int rightBulletLeft = 7;
    private bool rightGunHasBullet = true;
    public AudioSource rope;
    private float rTapTime = -1f;
    public HERO_SETUP setup;
    private GameObject skillCD;
    public float skillCDDuration;
    public float skillCDLast;
    public float skillCDLastCannon;
    private string skillId;
    public string skillIDHUD;
    public AudioSource slash;
    public AudioSource slashHit;
    private ParticleSystem smoke_3dmg;
    private ParticleSystem sparks;
    public float speed = 10f;
    public GameObject speedFX;
    public GameObject speedFX1;
    private ParticleSystem speedFXPS;
    public bool spinning;
    private string standAnimation = "stand";
    private Quaternion targetHeadRotation;
    private Quaternion targetRotation;
    public Vector3 targetV;
    private bool throwedBlades;
    public bool titanForm;
    private GameObject titanWhoGrabMe;
    private int titanWhoGrabMeID;
    private int totalBladeNum = 5;
    public float totalBladeSta = 100f;
    public float totalGas = 100f;
    private Transform upperarmL;
    private Transform upperarmR;
    private float useGasSpeed = 0.2f;
    public bool useGun;
    private float uTapTime = -1f;
    private bool wallJump;
    private float wallRunTime;

    #endregion

    #region Photon's methods

    private void Awake()
    {
        Cache();
        setup = gameObject.GetComponent<HERO_SETUP>();
        baseRigidBody.freezeRotation = true;
        baseRigidBody.useGravity = false;
        handL = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
        handR = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
        forearmL = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L");
        forearmR = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R");
        upperarmL = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L");
        upperarmR = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
    }

    #endregion

    #region RPCs

    [RPC]
    private void backToHumanRPC()
    {
        titanForm = false;
        eren_titan = null;
        gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
    }

    [RPC]
    public void badGuyReleaseMe()
    {
        hookBySomeOne = false;
        badGuy = null;
    }

    [RPC]
    public void blowAway(Vector3 force, PhotonMessageInfo info)
    {
        if (!info.sender.isLocal)
            return;
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !base.photonView.isMine) return;
        rigidbody.AddForce(force, ForceMode.Impulse);
        base.transform.LookAt(base.transform.position);
    }

    #endregion

    public HERO GetHERO()
    {
        if (gameObject.GetComponent<Rigidbody>() != null && this != null && gameObject != null)
            return this;
        return null;
    }

    private void ApplyForceToBody(GameObject gameObject, Vector3 force)
    {
        gameObject.rigidbody.AddForce(force);
        gameObject.rigidbody.AddTorque(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f));
    }

    /// <summary>
    /// Imposta <see cref="attackAnimation"/> a "attack2" se il mouse e' sulla sinistra dello schermo altrimenti "attack1"
    /// </summary>
    public void AttackAccordingToMouse()
    {
        attackAnimation = Input.mousePosition.x < Screen.width / 2 ? "attack2" : "attack1";
    }

    public void AttackAccordingToTarget(Transform targetPosition)
    {
        Vector3 vector = targetPosition.position - transform.position;
        float current = -Mathf.Atan2(vector.z, vector.x) * 57.29578f;
        float f = -Mathf.DeltaAngle(current, transform.rotation.eulerAngles.y - 90f);
        if (Mathf.Abs(f) < 90f && vector.magnitude < 6f && targetPosition.position.y <= transform.position.y + 2f && targetPosition.position.y >= transform.position.y - 5f)
        {
            attackAnimation = "attack4";
        }
        else if (f > 0f)
        {
            attackAnimation = "attack1";
        }
        else
        {
            attackAnimation = "attack2";
        }
    }


    public void BackToHuman()
    {
        gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
        rigidbody.velocity = Vector3.zero;
        titanForm = false;
        ungrabbed();
        FalseAttack();
        skillCDDuration = skillCDLast;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(gameObject, true, false);
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            photonView.RPC("backToHumanRPC", PhotonTargets.Others, new object[0]);
        }
    }

    private void BodyLean()
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
        {
            float z = 0f;
            needLean = false;
            if (!useGun && state == HERO_STATE.Attack && attackAnimation != "attack3_1" && attackAnimation != "attack3_2")
            {
                float y = rigidbody.velocity.y;
                float x = rigidbody.velocity.x;
                float num4 = rigidbody.velocity.z;
                float num5 = Mathf.Sqrt(x * x + num4 * num4);
                float num6 = Mathf.Atan2(y, num5) * 57.29578f;
                targetRotation = Quaternion.Euler(-num6 * (1f - Vector3.Angle(rigidbody.velocity, base.transform.forward) / 90f), facingDirection, 0f);
                if (isLeftHandHooked && bulletLeft != null || isRightHandHooked && bulletRight != null)
                {
                    base.transform.rotation = targetRotation;
                }
            }
            else
            {
                if (isLeftHandHooked && bulletLeft != null && isRightHandHooked && bulletRight != null)
                {
                    if (almostSingleHook)
                    {
                        needLean = true;
                        z = getLeanAngle(bulletRight.transform.position, true);
                    }
                }
                else if (isLeftHandHooked && bulletLeft != null)
                {
                    needLean = true;
                    z = getLeanAngle(bulletLeft.transform.position, true);
                }
                else if (isRightHandHooked && bulletRight != null)
                {
                    needLean = true;
                    z = getLeanAngle(bulletRight.transform.position, false);
                }
                if (needLean)
                {
                    float a = 0f;
                    if (!useGun && state != HERO_STATE.Attack)
                    {
                        a = currentSpeed * 0.1f;
                        a = Mathf.Min(a, 20f);
                    }
                    targetRotation = Quaternion.Euler(-a, facingDirection, z);
                }
                else if (state != HERO_STATE.Attack)
                {
                    targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                }
            }
        }
    }

    public void BombInit()
    {
        skillIDHUD = skillId;
        skillCDDuration = skillCDLast;
        if (RCSettings.bombMode == 1)
        {
            int sum = 0;
            for (int i = 250; i < 254; i++)
            {
                int val = (int) FengGameManagerMKII.settings[i];
                if (val < 0 || val > 10)
                    FengGameManagerMKII.settings[i] = val = 5;
                sum += val;
            }
            if (sum > 20)
            {
                FengGameManagerMKII.settings[250] = 5;
                FengGameManagerMKII.settings[251] = 5;
                FengGameManagerMKII.settings[252] = 5;
                FengGameManagerMKII.settings[253] = 5;
            }
            bombTimeMax = ((int) FengGameManagerMKII.settings[251] * 60f + 200f) / ((int) FengGameManagerMKII.settings[252] * 60f + 200f);
            bombRadius = (int) FengGameManagerMKII.settings[250] * 4f + 20f;
            bombCD = (int) FengGameManagerMKII.settings[253] * -0.4f + 5f;
            bombSpeed = (int) FengGameManagerMKII.settings[252] * 60f + 200f;
            PhotonNetwork.player.SetCustomProperties(new Hashtable
            {
                {PhotonPlayerProperty.RCBombR, (float) FengGameManagerMKII.settings[246]},
                {PhotonPlayerProperty.RCBombG, (float) FengGameManagerMKII.settings[247]},
                {PhotonPlayerProperty.RCBombB, (float) FengGameManagerMKII.settings[248]},
                {PhotonPlayerProperty.RCBombA, (float) FengGameManagerMKII.settings[249]},
                {PhotonPlayerProperty.RCBombRadius, bombRadius}
            });
            skillId = "bomb";
            skillIDHUD = "armin";
            skillCDLast = bombCD;
            skillCDDuration = 10f;
            if (FengGameManagerMKII.instance.roundTime > 10f)
            {
                skillCDDuration = 5f;
            }
        }
    }

    private void BreakApart(Vector3 v, bool isBite)
    {
        /*
         * BUG: NullReferenceException: Object reference not set to an instance of an object HERO.BreakApart (Vector3 v, Boolean isBite)
         * HERO.netDie (Vector3 v, Boolean isBite, Int32 viewID, System.String titanName, Boolean killByTitan, .PhotonMessageInfo info)
         * System.Reflection.MonoMethod.Invoke (System.Object obj, BindingFlags invokeAttr, System.Reflection.Binder binder, System.Object[] parameters, System.Globalization.CultureInfo culture)
         * Rethrow as TargetInvocationException: Exception has been thrown by the target of an invocation.
         * System.Reflection.MonoMethod.Invoke (System.Object obj, BindingFlags invokeAttr, System.Reflection.Binder binder, System.Object[] parameters, System.Globalization.CultureInfo culture)
         * System.Reflection.MethodBase.Invoke (System.Object obj, System.Object[] parameters)
         * NetworkingPeer.ExecuteRPC (ExitGames.Client.Photon.Hashtable rpcData, .PhotonPlayer sender)
         * NetworkingPeer.OnEvent (ExitGames.Client.Photon.EventData photonEvent)
         * ExitGames.Client.Photon.PeerBase.DeserializeMessageAndCallback (System.Byte[] inBuff)
         * ExitGames.Client.Photon.EnetPeer.DispatchIncomingCommands ()
         * ExitGames.Client.Photon.PhotonPeer.DispatchIncomingCommands ()
         * PhotonHandler.Update ()
         */

        GameObject leftWeapon;
        GameObject rightWeapon;
        GameObject _3dmg;
        GameObject gasLeft;
        GameObject gasRight;
        GameObject heroBody = (GameObject) Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
        heroBody.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
        heroBody.GetComponent<HERO_SETUP>().isDeadBody = true;
        heroBody.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, base.animation[currentAnimation].normalizedTime, BODY_PARTS.ARM_R);
        if (!isBite)
        {
            GameObject upperPart = (GameObject) Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
            GameObject lowerPart = (GameObject) Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
            GameObject leftArmPart = (GameObject) Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
            upperPart.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            lowerPart.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            leftArmPart.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            upperPart.GetComponent<HERO_SETUP>().isDeadBody = true;
            lowerPart.GetComponent<HERO_SETUP>().isDeadBody = true;
            leftArmPart.GetComponent<HERO_SETUP>().isDeadBody = true;
            upperPart.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, base.animation[currentAnimation].normalizedTime, BODY_PARTS.UPPER);
            lowerPart.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, base.animation[currentAnimation].normalizedTime, BODY_PARTS.LOWER);
            leftArmPart.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, base.animation[currentAnimation].normalizedTime, BODY_PARTS.ARM_L);
            ApplyForceToBody(upperPart, v);
            ApplyForceToBody(lowerPart, v);
            ApplyForceToBody(leftArmPart, v);
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
            {
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(upperPart, false, false);
            }
        }
        else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
        {
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(heroBody, false, false);
        }
        ApplyForceToBody(heroBody, v);
        Transform transform = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L").transform;
        Transform transform2 = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R").transform;
        if (useGun)
        {
            leftWeapon = (GameObject) Instantiate(Resources.Load("Character_parts/character_gun_l"), transform.position, transform.rotation);
            rightWeapon = (GameObject) Instantiate(Resources.Load("Character_parts/character_gun_r"), transform2.position, transform2.rotation);
            _3dmg = (GameObject) Instantiate(Resources.Load("Character_parts/character_3dmg_2"), base.transform.position, base.transform.rotation);
            gasLeft = (GameObject) Instantiate(Resources.Load("Character_parts/character_gun_mag_l"), base.transform.position, base.transform.rotation);
            gasRight = (GameObject) Instantiate(Resources.Load("Character_parts/character_gun_mag_r"), base.transform.position, base.transform.rotation);
        }
        else
        {
            leftWeapon = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_l"), transform.position, transform.rotation);
            rightWeapon = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_r"), transform2.position, transform2.rotation);
            _3dmg = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg"), base.transform.position, base.transform.rotation);
            gasLeft = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg_gas_l"), base.transform.position, base.transform.rotation);
            gasRight = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg_gas_r"), base.transform.position, base.transform.rotation);
        }
        leftWeapon.renderer.material = CharacterMaterials.materials[setup.myCostume.DmgTexture];
        rightWeapon.renderer.material = CharacterMaterials.materials[setup.myCostume.DmgTexture];
        _3dmg.renderer.material = CharacterMaterials.materials[setup.myCostume.DmgTexture];
        gasLeft.renderer.material = CharacterMaterials.materials[setup.myCostume.DmgTexture];
        gasRight.renderer.material = CharacterMaterials.materials[setup.myCostume.DmgTexture];
        ApplyForceToBody(leftWeapon, v);
        ApplyForceToBody(rightWeapon, v);
        ApplyForceToBody(_3dmg, v);
        ApplyForceToBody(gasLeft, v);
        ApplyForceToBody(gasRight, v);
    }

    private void BufferUpdate()
    {
        if (buffTime > 0f)
        {
            buffTime -= Time.deltaTime;
            if (buffTime <= 0f)
            {
                buffTime = 0f;
                if (currentBuff == BUFF.SpeedUp && base.animation.IsPlaying("run_sasha"))
                {
                    CrossFade("run", 0.1f);
                }
                currentBuff = BUFF.NoBuff;
            }
        }
    }

    public void Cache()
    {
        baseTransform = base.transform;
        baseRigidBody = rigidbody;
        maincamera = GameObject.Find("MainCamera");
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
        {
            baseAnimation = base.animation;
            cross1 = GameObject.Find("cross1");
            cross2 = GameObject.Find("cross2");
            crossL1 = GameObject.Find("crossL1");
            crossL2 = GameObject.Find("crossL2");
            crossR1 = GameObject.Find("crossR1");
            crossR2 = GameObject.Find("crossR2");
            LabelDistance = GameObject.Find("LabelDistance");
            cachedSprites = new Dictionary<string, UISprite>();
            foreach (GameObject obj2 in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if (obj2.GetComponent<UISprite>() != null && obj2.activeInHierarchy)
                {
                    string name = obj2.name;
                    if (!(name.Contains("blade") || name.Contains("bullet") || name.Contains("gas") || name.Contains("flare") || name.Contains("skill_cd") ? cachedSprites.ContainsKey(name) : true))
                    {
                        cachedSprites.Add(name, obj2.GetComponent<UISprite>());
                    }
                }
            }
        }
    }

    private void SetFlareCD()
    {
        if (flare1CD > 0f)
        {
            flare1CD -= Time.deltaTime;
            if (flare1CD < 0f)
            {
                flare1CD = 0f;
            }
        }
        if (flare2CD > 0f)
        {
            flare2CD -= Time.deltaTime;
            if (flare2CD < 0f)
            {
                flare2CD = 0f;
            }
        }
        if (flare3CD > 0f)
        {
            flare3CD -= Time.deltaTime;
            if (flare3CD < 0f)
            {
                flare3CD = 0f;
            }
        }
    }

    private void SetSkillCD()
    {
        if (skillCDDuration > 0f)
        {
            skillCDDuration -= Time.deltaTime;
            if (skillCDDuration < 0f || ModManager.Find("module.ignorecountdown").Enabled)
                skillCDDuration = 0f;
        }
    }

    private float CalculateJumpVerticalSpeed()
    {
        return Mathf.Sqrt(2f * jumpHeight * gravity);
    }

    private void ChangeBlade()
    {
        if (!useGun || grounded || LevelInfo.GetInfo(FengGameManagerMKII.level).type != GAMEMODE.PVP_AHSS)
        {
            state = HERO_STATE.ChangeBlade;
            throwedBlades = false;
            if (useGun)
            {
                if (!leftGunHasBullet && !rightGunHasBullet)
                {
                    if (grounded)
                    {
                        reloadAnimation = "AHSS_gun_reload_both";
                    }
                    else
                    {
                        reloadAnimation = "AHSS_gun_reload_both_air";
                    }
                }
                else if (!leftGunHasBullet)
                {
                    if (grounded)
                    {
                        reloadAnimation = "AHSS_gun_reload_l";
                    }
                    else
                    {
                        reloadAnimation = "AHSS_gun_reload_l_air";
                    }
                }
                else if (!rightGunHasBullet)
                {
                    if (grounded)
                    {
                        reloadAnimation = "AHSS_gun_reload_r";
                    }
                    else
                    {
                        reloadAnimation = "AHSS_gun_reload_r_air";
                    }
                }
                else
                {
                    if (grounded)
                    {
                        reloadAnimation = "AHSS_gun_reload_both";
                    }
                    else
                    {
                        reloadAnimation = "AHSS_gun_reload_both_air";
                    }
                    rightGunHasBullet = false;
                    leftGunHasBullet = false;
                }
                CrossFade(reloadAnimation, 0.05f);
            }
            else
            {
                if (!grounded)
                {
                    reloadAnimation = "changeBlade_air";
                }
                else
                {
                    reloadAnimation = "changeBlade";
                }
                CrossFade(reloadAnimation, 0.1f);
            }
        }
    }

    private void GetDashDirection()
    {
        if (uTapTime >= 0f)
        {
            uTapTime += Time.deltaTime;
            if (uTapTime > 0.2f)
            {
                uTapTime = -1f;
            }
        }
        if (dTapTime >= 0f)
        {
            dTapTime += Time.deltaTime;
            if (dTapTime > 0.2f)
            {
                dTapTime = -1f;
            }
        }
        if (lTapTime >= 0f)
        {
            lTapTime += Time.deltaTime;
            if (lTapTime > 0.2f)
            {
                lTapTime = -1f;
            }
        }
        if (rTapTime >= 0f)
        {
            rTapTime += Time.deltaTime;
            if (rTapTime > 0.2f)
            {
                rTapTime = -1f;
            }
        }
        if (inputManager.isInputDown[InputCode.up])
        {
            if (uTapTime == -1f)
            {
                uTapTime = 0f;
            }
            if (uTapTime != 0f)
            {
                dashU = true;
            }
        }
        if (inputManager.isInputDown[InputCode.down])
        {
            if (dTapTime == -1f)
            {
                dTapTime = 0f;
            }
            if (dTapTime != 0f)
            {
                dashD = true;
            }
        }
        if (inputManager.isInputDown[InputCode.left])
        {
            if (lTapTime == -1f)
            {
                lTapTime = 0f;
            }
            if (lTapTime != 0f)
            {
                dashL = true;
            }
        }
        if (inputManager.isInputDown[InputCode.right])
        {
            if (rTapTime == -1f)
            {
                rTapTime = 0f;
            }
            if (rTapTime != 0f)
            {
                dashR = true;
            }
        }
    }

    private void CheckDashRebind()
    {
        if (FengGameManagerMKII.inputRC.isInputHuman(InputCodeRC.dash))
        {
            if (inputManager.isInput[InputCode.up])
            {
                dashU = true;
            }
            else if (inputManager.isInput[InputCode.down])
            {
                dashD = true;
            }
            else if (inputManager.isInput[InputCode.left])
            {
                dashL = true;
            }
            else if (inputManager.isInput[InputCode.right])
            {
                dashR = true;
            }
        }
    }

    public void CheckTitan()
    {
        int count;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        LayerMask mask = 1 << LayerMask.NameToLayer("PlayerAttackBox");
        LayerMask mask2 = 1 << LayerMask.NameToLayer("Ground");
        LayerMask mask3 = 1 << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask4 = mask | mask2 | mask3;
        RaycastHit[] hitArray = Physics.RaycastAll(ray, 180f, mask4.value);
        List<RaycastHit> list = new List<RaycastHit>();
        List<TITAN> list2 = new List<TITAN>();
        for (count = 0; count < hitArray.Length; count++)
        {
            RaycastHit item = hitArray[count];
            list.Add(item);
        }
        list.Sort((x, y) => x.distance.CompareTo(y.distance));
        float num2 = 180f;
        for (count = 0; count < list.Count; count++)
        {
            RaycastHit hit2 = list[count];
            GameObject gameObject = hit2.collider.gameObject;
            if (gameObject.layer == 16)
            {
                if (gameObject.name.Contains("PlayerDetectorRC") && (hit2 = list[count]).distance < num2)
                {
                    num2 -= 60f;
                    if (num2 <= 60f)
                    {
                        count = list.Count;
                    }
                    TITAN component = gameObject.transform.root.gameObject.GetComponent<TITAN>();
                    if (component != null)
                    {
                        list2.Add(component);
                    }
                }
            }
            else
            {
                count = list.Count;
            }
        }
        for (count = 0; count < myTitans.Count; count++)
        {
            TITAN titan2 = myTitans[count];
            if (!list2.Contains(titan2))
            {
                titan2.isLook = false;
            }
        }
        for (count = 0; count < list2.Count; count++)
        {
            TITAN titan3 = list2[count];
            titan3.isLook = true;
        }
        myTitans = list2;
    }

    public void ClearPopup()
    {
        FengGameManagerMKII.instance.ShowHUDInfoCenter(string.Empty);
    }

    public void ContinueAnimation()
    {
        IEnumerator enumerator = base.animation.GetEnumerator();
        
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                if (current != null && current.speed == 1f)
                {
                    return;
                }
                current.speed = 1f;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
            	disposable.Dispose();
            }
        }
        SetAnimationSpeed();
        playAnimation(GetCurrentPlayingClipName());
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && base.photonView.isMine)
        {
            base.photonView.RPC("netContinueAnimation", PhotonTargets.Others, new object[0]);
        }
    }

    public void CrossFade(string aniName, float time)
    {
        currentAnimation = aniName;
        base.animation.CrossFade(aniName, time);
        if (PhotonNetwork.connected && base.photonView.isMine)
        {
            object[] parameters = new object[] { aniName, time };
            base.photonView.RPC("netCrossFade", PhotonTargets.Others, parameters);
        }
    }

    public string GetCurrentPlayingClipName()
    {
        IEnumerator enumerator = base.animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                if (current != null && base.animation.IsPlaying(current.name))
                {
                    return current.name;
                }
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
            	disposable.Dispose();
            }
        }
        return string.Empty;
    }

    private void SetAnimationSpeed()
    {
        base.animation["attack5"].speed = 1.85f; 
        base.animation["changeBlade"].speed = 1.2f;
        base.animation["air_release"].speed = 0.6f;
        base.animation["changeBlade_air"].speed = 0.8f;
        base.animation["AHSS_gun_reload_both"].speed = 0.38f;
        base.animation["AHSS_gun_reload_both_air"].speed = 0.5f;
        base.animation["AHSS_gun_reload_l"].speed = 0.4f;
        base.animation["AHSS_gun_reload_l_air"].speed = 0.5f;
        base.animation["AHSS_gun_reload_r"].speed = 0.4f;
        base.animation["AHSS_gun_reload_r_air"].speed = 0.5f;
    }

    private void Dash(float horizontal, float vertical)
    {
        if (dashTime <= 0f && (currentGas > 0f || ModManager.Find("module.infinitegas").Enabled) && !isMounted)
        {
            useGas(totalGas * 0.04f);
            facingDirection = getGlobalFacingDirection(horizontal, vertical);
            dashV = getGlobaleFacingVector3(facingDirection);
            originVM = currentSpeed;
            Quaternion quaternion = Quaternion.Euler(0f, facingDirection, 0f);
            rigidbody.rotation = quaternion;
            targetRotation = quaternion;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                Instantiate(Resources.Load("FX/boost_smoke"), base.transform.position, base.transform.rotation);
            }
            else
            {
                PhotonNetwork.Instantiate("FX/boost_smoke", base.transform.position, base.transform.rotation, 0);
            }
            dashTime = 0.5f;
            CrossFade("dash", 0.1f);
            base.animation["dash"].time = 0.1f;
            state = HERO_STATE.AirDodge;
            FalseAttack();
            rigidbody.AddForce(dashV * 40f, ForceMode.VelocityChange);
        }
    }

    public void Die(Vector3 v, bool isBite)
    {
        if (invincible <= 0f)
        {
            if (titanForm && eren_titan != null)
            {
                eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (bulletLeft != null)
            {
                bulletLeft.GetComponent<Bullet>().removeMe();
            }
            if (bulletRight != null)
            {
                bulletRight.GetComponent<Bullet>().removeMe();
            }
            meatDie.Play();
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine) && !useGun)
            {
                leftbladetrail.Deactivate();
                rightbladetrail.Deactivate();
                leftbladetrail2.Deactivate();
                rightbladetrail2.Deactivate();
            }
            BreakApart(v, isBite);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameLose2();
            FalseAttack();
            hasDied = true;
            Transform transform = base.transform.Find("audio_die");
            transform.parent = null;
            transform.GetComponent<AudioSource>().Play();
            if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
            {
                GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(base.transform.position, 0, null, 0.02f);
            }
            UnityEngine.Object.Destroy(gameObject);
        }
    }

    public void DieRC(Transform tf)
    {
        if (invincible <= 0f)
        {
            if (titanForm && eren_titan != null)
            {
                eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (bulletLeft != null)
            {
                bulletLeft.GetComponent<Bullet>().removeMe();
            }
            if (bulletRight != null)
            {
                bulletRight.GetComponent<Bullet>().removeMe();
            }
            Transform transform = base.transform.Find("audio_die");
            transform.parent = null;
            transform.GetComponent<AudioSource>().Play();
            meatDie.Play();
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameLose2();
            FalseAttack();
            hasDied = true;
            GameObject obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("hitMeat2"));
            obj2.transform.position = base.transform.position;
            UnityEngine.Object.Destroy(gameObject);
        }
    }
    
    private void Dodge(bool offTheWall = false)
    {
        if (!FengGameManagerMKII.inputRC.isInputHorse(InputCodeRC.horseMount) || myHorse == null || isMounted || Vector3.Distance(myHorse.transform.position, base.transform.position) >= 15f)
        {
            state = HERO_STATE.GroundDodge;
            if (!offTheWall)
            {
                float num;
                float num2;
                if (inputManager.isInput[InputCode.up])
                {
                    num = 1f;
                }
                else if (inputManager.isInput[InputCode.down])
                {
                    num = -1f;
                }
                else
                {
                    num = 0f;
                }
                if (inputManager.isInput[InputCode.left])
                {
                    num2 = -1f;
                }
                else if (inputManager.isInput[InputCode.right])
                {
                    num2 = 1f;
                }
                else
                {
                    num2 = 0f;
                }
                float num3 = getGlobalFacingDirection(num2, num);
                if (num2 != 0f || num != 0f)
                {
                    facingDirection = num3 + 180f;
                    targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                }
                CrossFade("dodge", 0.1f);
            }
            else
            {
                playAnimation("dodge");
                playAnimationAt("dodge", 0.2f);
            }
            sparks.enableEmission = false;
        }
    }

    private void ErenTransform()
    {
        skillCDDuration = skillCDLast;
        if (bulletLeft != null)
        {
            bulletLeft.GetComponent<Bullet>().removeMe();
        }
        if (bulletRight != null)
        {
            bulletRight.GetComponent<Bullet>().removeMe();
        }
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            eren_titan = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("TITAN_EREN"), base.transform.position, base.transform.rotation);
        }
        else
        {
            eren_titan = PhotonNetwork.Instantiate("TITAN_EREN", base.transform.position, base.transform.rotation, 0);
        }
        eren_titan.GetComponent<TITAN_EREN>().realBody = gameObject;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().FlashBlind();
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(eren_titan, true, false);
        eren_titan.GetComponent<TITAN_EREN>().born();
        eren_titan.rigidbody.velocity = rigidbody.velocity;
        rigidbody.velocity = Vector3.zero;
        base.transform.position = eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
        titanForm = true;
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            object[] parameters = new object[] { eren_titan.GetPhotonView().viewID };
            base.photonView.RPC("whoIsMyErenTitan", PhotonTargets.Others, parameters);
        }
        if (smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && base.photonView.isMine)
        {
            object[] objArray2 = new object[] { false };
            base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, objArray2);
        }
        smoke_3dmg.enableEmission = false;
    }

    public void FalseAttack()
    {
        attackMove = false;
        if (useGun)
        {
            if (!attackReleased)
            {
                ContinueAnimation();
                attackReleased = true;
            }
        }
        else
        {
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
            {
                checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                checkBoxLeft.GetComponent<TriggerColliderWeapon>().ClearHits();
                checkBoxRight.GetComponent<TriggerColliderWeapon>().ClearHits();
                leftbladetrail.StopSmoothly(0.2f);
                rightbladetrail.StopSmoothly(0.2f);
                leftbladetrail2.StopSmoothly(0.2f);
                rightbladetrail2.StopSmoothly(0.2f);
            }
            attackLoop = 0;
            if (!attackReleased)
            {
                ContinueAnimation();
                attackReleased = true;
            }
        }
    }

    public void FillGas()
    {
        currentGas = totalGas;
    }

    private GameObject FindNearestTitan()
    {
        GameObject[] objArray = GameObject.FindGameObjectsWithTag("titan"); //TODO: Replace all FindGameObjectsWithTag with the lists of FengGameManager
        GameObject obj2 = null;
        float positiveInfinity = float.PositiveInfinity;
        Vector3 position = base.transform.position;
        foreach (GameObject obj3 in objArray)
        {
            Vector3 vector2 = obj3.transform.position - position;
            float sqrMagnitude = vector2.sqrMagnitude;
            if (sqrMagnitude < positiveInfinity)
            {
                obj2 = obj3;
                positiveInfinity = sqrMagnitude;
            }
        }
        return obj2;
    }

    private void FixedUpdate()
    {
        if (!titanForm && !isCannon && (!IN_GAME_MAIN_CAMERA.isPausing || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE))
        {
            currentSpeed = baseRigidBody.velocity.magnitude;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
            {
                if (!(baseAnimation.IsPlaying("attack3_2") || baseAnimation.IsPlaying("attack5") || baseAnimation.IsPlaying("special_petra")))
                {
                    baseRigidBody.rotation = Quaternion.Lerp(gameObject.transform.rotation, targetRotation, Time.deltaTime * 6f);
                }
                if (state == HERO_STATE.Grab)
                {
                    baseRigidBody.AddForce(-baseRigidBody.velocity, ForceMode.VelocityChange);
                }
                else
                {
                    if (IsGrounded())
                    {
                        if (!grounded)
                        {
                            justGrounded = true;
                        }
                        grounded = true;
                    }
                    else
                    {
                        grounded = false;
                    }
                    if (hookSomeOne)
                    {
                        if (hookTarget != null)
                        {
                            Vector3 vector2 = hookTarget.transform.position - baseTransform.position;
                            float magnitude = vector2.magnitude;
                            if (magnitude > 2f) //MOD: Test if can hook from distance
                            {
                                baseRigidBody.AddForce(vector2.normalized * Mathf.Pow(magnitude, 0.15f) * 30f - baseRigidBody.velocity * 0.95f, ForceMode.VelocityChange);
                            }
                        }
                        else
                        {
                            hookSomeOne = false;
                        }
                    }
                    else if (hookBySomeOne && badGuy != null)
                    {
                        if (badGuy != null)
                        {
                            Vector3 vector3 = badGuy.transform.position - baseTransform.position;
                            float f = vector3.magnitude;
                            if (f > 5f)
                            {
                                baseRigidBody.AddForce(vector3.normalized * Mathf.Pow(f, 0.15f) * 0.2f, ForceMode.Impulse);
                            }
                        }
                        else
                        {
                            hookBySomeOne = false;
                        }
                    }
                    float x = 0f;
                    float z = 0f;
                    if (!IN_GAME_MAIN_CAMERA.isTyping)
                    {
                        if (inputManager.isInput[InputCode.up])
                        {
                            z = 1f;
                        }
                        else if (inputManager.isInput[InputCode.down])
                        {
                            z = -1f;
                        }
                        else
                        {
                            z = 0f;
                        }
                        if (inputManager.isInput[InputCode.left])
                        {
                            x = -1f;
                        }
                        else if (inputManager.isInput[InputCode.right])
                        {
                            x = 1f;
                        }
                        else
                        {
                            x = 0f;
                        }
                    }
                    bool flag2 = false;
                    bool flag3 = false;
                    bool flag4 = false;
                    isLeftHandHooked = false;
                    isRightHandHooked = false;
                    if (isLaunchLeft)
                    {
                        if (bulletLeft != null && bulletLeft.GetComponent<Bullet>().isHooked())
                        {
                            isLeftHandHooked = true;
                            Vector3 to = bulletLeft.transform.position - baseTransform.position;
                            to.Normalize();
                            to = to * 10f;
                            if (!isLaunchRight)
                            {
                                to = to * 2f;
                            }
                            if (Vector3.Angle(baseRigidBody.velocity, to) > 90f && inputManager.isInput[InputCode.jump])
                            {
                                flag3 = true;
                                flag2 = true;
                            }
                            if (!flag3)
                            {
                                baseRigidBody.AddForce(to);
                                if (Vector3.Angle(baseRigidBody.velocity, to) > 90f)
                                {
                                    baseRigidBody.AddForce(-baseRigidBody.velocity * 2f, ForceMode.Acceleration);
                                }
                            }
                        }
                        launchElapsedTimeL += Time.deltaTime;
                        if (QHold && currentGas > 0f)
                        {
                            useGas(useGasSpeed * Time.deltaTime);
                        }
                        else if (launchElapsedTimeL > 0.3f) //MOD: Before was 0.3f
                        {
                            isLaunchLeft = false;
                            if (bulletLeft != null)
                            {
                                bulletLeft.GetComponent<Bullet>().disable();
                                releaseIfIHookSb();
                                bulletLeft = null;
                                flag3 = false;
                            }
                        }
                    }
                    if (isLaunchRight)
                    {
                        if (bulletRight != null && bulletRight.GetComponent<Bullet>().isHooked())
                        {
                            isRightHandHooked = true;
                            Vector3 vector5 = bulletRight.transform.position - baseTransform.position;
                            vector5.Normalize();
                            vector5 = vector5 * 10f;
                            if (!isLaunchLeft)
                            {
                                vector5 = vector5 * 2f;
                            }
                            if (Vector3.Angle(baseRigidBody.velocity, vector5) > 90f && inputManager.isInput[InputCode.jump])
                            {
                                flag4 = true;
                                flag2 = true;
                            }
                            if (!flag4)
                            {
                                baseRigidBody.AddForce(vector5);
                                if (Vector3.Angle(baseRigidBody.velocity, vector5) > 90f)
                                {
                                    baseRigidBody.AddForce(-baseRigidBody.velocity * 2f, ForceMode.Acceleration);
                                }
                            }
                        }
                        launchElapsedTimeR += Time.deltaTime;
                        if (EHold && currentGas > 0f)
                        {
                            useGas(useGasSpeed * Time.deltaTime);
                        }
                        else if (launchElapsedTimeR > 0.3f)
                        {
                            //Called after release the hook
                            isLaunchRight = false;
                            if (bulletRight != null)
                            {
                                bulletRight.GetComponent<Bullet>().disable();
                                releaseIfIHookSb();
                                bulletRight = null;
                                flag4 = false;
                            }
                        }
                    }
                    if (grounded)
                    {
                        Vector3 vector7;
                        Vector3 zero = Vector3.zero;
                        if (state == HERO_STATE.Attack)
                        {
                            if (attackAnimation == "attack5")
                            {
                                if (baseAnimation[attackAnimation].normalizedTime > 0.4f && baseAnimation[attackAnimation].normalizedTime < 0.61f)
                                {
                                    //Called when closing to finishing special (levi 4example)
                                    baseRigidBody.AddForce(gameObject.transform.forward * 200f);
                                }
                            }
                            else if (attackAnimation == "special_petra")
                            {
                                if (baseAnimation[attackAnimation].normalizedTime > 0.35f && baseAnimation[attackAnimation].normalizedTime < 0.48f)
                                {
                                    baseRigidBody.AddForce(gameObject.transform.forward * 200f);
                                }
                            }
                            else if (baseAnimation.IsPlaying("attack3_2"))
                            {
                                zero = Vector3.zero;
                            }
                            else if (baseAnimation.IsPlaying("attack1") || baseAnimation.IsPlaying("attack2"))
                            {
                                //Base attack (left size attack1 right attack2 I think)
                                baseRigidBody.AddForce(gameObject.transform.forward * 200f);
                            }
                            if (baseAnimation.IsPlaying("attack3_2"))
                            {
                                zero = Vector3.zero;
                            }
                        }
                        if (justGrounded)
                        {
                            if (state != HERO_STATE.Attack || attackAnimation != "attack3_1" && attackAnimation != "attack5" && attackAnimation != "special_petra")
                            {
                                if (state != HERO_STATE.Attack && x == 0f && z == 0f && bulletLeft == null && bulletRight == null && state != HERO_STATE.FillGas)
                                {
                                    state = HERO_STATE.Land;
                                    CrossFade("dash_land", 0.01f);
                                }
                                else
                                {
                                    buttonAttackRelease = true;
                                    if (state != HERO_STATE.Attack && baseRigidBody.velocity.x * baseRigidBody.velocity.x + baseRigidBody.velocity.z * baseRigidBody.velocity.z > speed * speed * 1.5f && state != HERO_STATE.FillGas)
                                    {
                                        state = HERO_STATE.Slide;
                                        CrossFade("slide", 0.05f);
                                        facingDirection = Mathf.Atan2(baseRigidBody.velocity.x, baseRigidBody.velocity.z) * 57.29578f;
                                        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                        sparks.enableEmission = true;
                                    }
                                }
                            }
                            justGrounded = false;
                            zero = baseRigidBody.velocity;
                        }
                        if (state == HERO_STATE.Attack && attackAnimation == "attack3_1" && baseAnimation[attackAnimation].normalizedTime >= 1f)
                        {
                            playAnimation("attack3_2");
                            resetAnimationSpeed();
                            vector7 = Vector3.zero;
                            baseRigidBody.velocity = vector7;
                            zero = vector7;
                            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.2f, 0.3f, 0.95f);
                        }
                        if (state == HERO_STATE.GroundDodge)
                        {
                            if (baseAnimation["dodge"].normalizedTime >= 0.2f && baseAnimation["dodge"].normalizedTime < 0.8f)
                            {
                                zero = -baseTransform.forward * 2.4f * speed;
                            }
                            if (baseAnimation["dodge"].normalizedTime > 0.8f)
                            {
                                zero = baseRigidBody.velocity * 0.9f;
                            }
                        }
                        else if (state == HERO_STATE.Idle)
                        {
                            Vector3 vector8 = new Vector3(x, 0f, z);
                            float resultAngle = getGlobalFacingDirection(x, z);
                            zero = getGlobaleFacingVector3(resultAngle);
                            float num6 = vector8.magnitude <= 0.95f ? (vector8.magnitude >= 0.25f ? vector8.magnitude : 0f) : 1f;
                            zero = zero * num6;
                            zero = zero * speed;
                            if (buffTime > 0f && currentBuff == BUFF.SpeedUp)
                            {
                                zero = zero * 4f;
                            }
                            if (x != 0f || z != 0f)
                            {
                                if (!baseAnimation.IsPlaying("run") && !baseAnimation.IsPlaying("jump") && !baseAnimation.IsPlaying("run_sasha") && (!baseAnimation.IsPlaying("horse_geton") || baseAnimation["horse_geton"].normalizedTime >= 0.5f))
                                {
                                    if (buffTime > 0f && currentBuff == BUFF.SpeedUp)
                                    {
                                        CrossFade("run_sasha", 0.1f);
                                    }
                                    else
                                    {
                                        CrossFade("run", 0.1f);
                                    }
                                }
                            }
                            else
                            {
                                if (!(baseAnimation.IsPlaying(standAnimation) || state == HERO_STATE.Land || baseAnimation.IsPlaying("jump") || baseAnimation.IsPlaying("horse_geton") || baseAnimation.IsPlaying("grabbed")))
                                {
                                    CrossFade(standAnimation, 0.1f);
                                    zero = zero * 0f;
                                }
                                resultAngle = -874f;
                            }
                            if (resultAngle != -874f)
                            {
                                facingDirection = resultAngle;
                                targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                            }
                        }
                        else if (state == HERO_STATE.Land)
                        {
                            zero = baseRigidBody.velocity * 0.96f;
                        }
                        else if (state == HERO_STATE.Slide)
                        {
                            zero = baseRigidBody.velocity * 0.99f;
                            if (currentSpeed < speed * 1.2f)
                            {
                                idle();
                                sparks.enableEmission = false;
                            }
                        }
                        Vector3 velocity = baseRigidBody.velocity;
                        Vector3 force = zero - velocity;
                        force.x = Mathf.Clamp(force.x, -maxVelocityChange, maxVelocityChange);
                        force.z = Mathf.Clamp(force.z, -maxVelocityChange, maxVelocityChange);
                        force.y = 0f;
                        if (baseAnimation.IsPlaying("jump") && baseAnimation["jump"].normalizedTime > 0.18f)
                        {
                            force.y += 8f;
                        }
                        if (baseAnimation.IsPlaying("horse_geton") && baseAnimation["horse_geton"].normalizedTime > 0.18f && baseAnimation["horse_geton"].normalizedTime < 1f)
                        {
                            float num7 = 6f;
                            force = -baseRigidBody.velocity;
                            force.y = num7;
                            float num8 = Vector3.Distance(myHorse.transform.position, baseTransform.position);
                            float num9 = 0.6f * gravity * num8 / 12f;
                            vector7 = myHorse.transform.position - baseTransform.position;
                            force += num9 * vector7.normalized;
                        }
                        if (!(state == HERO_STATE.Attack && useGun))
                        {
                            baseRigidBody.AddForce(force, ForceMode.VelocityChange);
                            baseRigidBody.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, facingDirection, 0f), Time.deltaTime * 10f);
                        }
                    }
                    else
                    {
                        if (sparks.enableEmission)
                        {
                            sparks.enableEmission = false;
                        }
                        if (myHorse != null && (baseAnimation.IsPlaying("horse_geton") || baseAnimation.IsPlaying("air_fall")) && baseRigidBody.velocity.y < 0f && Vector3.Distance(myHorse.transform.position + Vector3.up * 1.65f, baseTransform.position) < 0.5f)
                        {
                            baseTransform.position = myHorse.transform.position + Vector3.up * 1.65f;
                            baseTransform.rotation = myHorse.transform.rotation;
                            isMounted = true;
                            CrossFade("horse_idle", 0.1f);
                            myHorse.GetComponent<Horse>().mounted();
                        }
                        if (!(state != HERO_STATE.Idle || baseAnimation.IsPlaying("dash") || baseAnimation.IsPlaying("wallrun") || baseAnimation.IsPlaying("toRoof") || (baseAnimation.IsPlaying("horse_geton") || baseAnimation.IsPlaying("horse_getoff")) || baseAnimation.IsPlaying("air_release") || isMounted || baseAnimation.IsPlaying("air_hook_l_just") && baseAnimation["air_hook_l_just"].normalizedTime < 1f || baseAnimation.IsPlaying("air_hook_r_just") && baseAnimation["air_hook_r_just"].normalizedTime < 1f ? baseAnimation["dash"].normalizedTime < 0.99f : false))
                        {
                            if (!isLeftHandHooked && !isRightHandHooked && (baseAnimation.IsPlaying("air_hook_l") || baseAnimation.IsPlaying("air_hook_r") || baseAnimation.IsPlaying("air_hook")) && baseRigidBody.velocity.y > 20f)
                            {
                                baseAnimation.CrossFade("air_release");
                            }
                            else
                            {
                                bool flag5 = Mathf.Abs(baseRigidBody.velocity.x) + Mathf.Abs(baseRigidBody.velocity.z) > 25f;
                                bool flag6 = baseRigidBody.velocity.y < 0f;
                                if (!flag5)
                                {
                                    if (flag6)
                                    {
                                        if (!baseAnimation.IsPlaying("air_fall"))
                                        {
                                            CrossFade("air_fall", 0.2f);
                                        }
                                    }
                                    else if (!baseAnimation.IsPlaying("air_rise"))
                                    {
                                        CrossFade("air_rise", 0.2f);
                                    }
                                }
                                else if (!isLeftHandHooked && !isRightHandHooked)
                                {
                                    float current = -Mathf.Atan2(baseRigidBody.velocity.z, baseRigidBody.velocity.x) * 57.29578f;
                                    float num11 = -Mathf.DeltaAngle(current, baseTransform.rotation.eulerAngles.y - 90f);
                                    if (Mathf.Abs(num11) < 45f)
                                    {
                                        if (!baseAnimation.IsPlaying("air2"))
                                        {
                                            CrossFade("air2", 0.2f);
                                        }
                                    }
                                    else if (num11 < 135f && num11 > 0f)
                                    {
                                        if (!baseAnimation.IsPlaying("air2_right"))
                                        {
                                            CrossFade("air2_right", 0.2f);
                                        }
                                    }
                                    else if (num11 > -135f && num11 < 0f)
                                    {
                                        if (!baseAnimation.IsPlaying("air2_left"))
                                        {
                                            CrossFade("air2_left", 0.2f);
                                        }
                                    }
                                    else if (!baseAnimation.IsPlaying("air2_backward"))
                                    {
                                        CrossFade("air2_backward", 0.2f);
                                    }
                                }
                                else if (useGun)
                                {
                                    if (!isRightHandHooked)
                                    {
                                        if (!baseAnimation.IsPlaying("AHSS_hook_forward_l"))
                                        {
                                            CrossFade("AHSS_hook_forward_l", 0.1f);
                                        }
                                    }
                                    else if (!isLeftHandHooked)
                                    {
                                        if (!baseAnimation.IsPlaying("AHSS_hook_forward_r"))
                                        {
                                            CrossFade("AHSS_hook_forward_r", 0.1f);
                                        }
                                    }
                                    else if (!baseAnimation.IsPlaying("AHSS_hook_forward_both"))
                                    {
                                        CrossFade("AHSS_hook_forward_both", 0.1f);
                                    }
                                }
                                else if (!isRightHandHooked)
                                {
                                    if (!baseAnimation.IsPlaying("air_hook_l"))
                                    {
                                        CrossFade("air_hook_l", 0.1f);
                                    }
                                }
                                else if (!isLeftHandHooked)
                                {
                                    if (!baseAnimation.IsPlaying("air_hook_r"))
                                    {
                                        CrossFade("air_hook_r", 0.1f);
                                    }
                                }
                                else if (!baseAnimation.IsPlaying("air_hook"))
                                {
                                    CrossFade("air_hook", 0.1f);
                                }
                            }
                        }
                        if (state == HERO_STATE.Idle && baseAnimation.IsPlaying("air_release") && baseAnimation["air_release"].normalizedTime >= 1f)
                        {
                            CrossFade("air_rise", 0.2f);
                        }
                        if (baseAnimation.IsPlaying("horse_getoff") && baseAnimation["horse_getoff"].normalizedTime >= 1f)
                        {
                            CrossFade("air_rise", 0.2f);
                        }
                        if (baseAnimation.IsPlaying("toRoof"))
                        {
                            if (baseAnimation["toRoof"].normalizedTime < 0.22f)
                            {
                                baseRigidBody.velocity = Vector3.zero;
                                baseRigidBody.AddForce(new Vector3(0f, gravity * baseRigidBody.mass, 0f));
                            }
                            else
                            {
                                if (!wallJump)
                                {
                                    wallJump = true;
                                    baseRigidBody.AddForce(Vector3.up * 8f, ForceMode.Impulse);
                                }
                                baseRigidBody.AddForce(baseTransform.forward * 0.05f, ForceMode.Impulse);
                            }
                            if (baseAnimation["toRoof"].normalizedTime >= 1f)
                            {
                                playAnimation("air_rise");
                            }
                        }
                        else if (!(state != HERO_STATE.Idle || !isPressDirectionTowardsHero(x, z) || inputManager.isInput[InputCode.jump] || inputManager.isInput[InputCode.leftRope] || (inputManager.isInput[InputCode.rightRope] || inputManager.isInput[InputCode.bothRope]) || !IsFrontGrounded() || baseAnimation.IsPlaying("wallrun") || baseAnimation.IsPlaying("dodge")))
                        {
                            CrossFade("wallrun", 0.1f);
                            wallRunTime = 0f;
                        }
                        else if (baseAnimation.IsPlaying("wallrun"))
                        {
                            baseRigidBody.AddForce(Vector3.up * speed - baseRigidBody.velocity, ForceMode.VelocityChange);
                            wallRunTime += Time.deltaTime;
                            if (wallRunTime > 1f || z == 0f && x == 0f)
                            {
                                baseRigidBody.AddForce(-baseTransform.forward * speed * 0.75f, ForceMode.Impulse);
                                Dodge(true);
                            }
                            else if (!IsUpFrontGrounded())
                            {
                                wallJump = false;
                                CrossFade("toRoof", 0.1f);
                            }
                            else if (!IsFrontGrounded())
                            {
                                CrossFade("air_fall", 0.1f);
                            }
                        }
                        else if (!baseAnimation.IsPlaying("attack5") && !baseAnimation.IsPlaying("special_petra") && !baseAnimation.IsPlaying("dash") && !baseAnimation.IsPlaying("jump"))
                        {
                            Vector3 vector11 = new Vector3(x, 0f, z);
                            float num12 = getGlobalFacingDirection(x, z);
                            Vector3 vector12 = getGlobaleFacingVector3(num12);
                            float num13 = vector11.magnitude <= 0.95f ? (vector11.magnitude >= 0.25f ? vector11.magnitude : 0f) : 1f;
                            vector12 = vector12 * num13;
                            vector12 = vector12 * ((float)setup.myCostume.Stat.ACL / 10f * 2f);
                            if (x == 0f && z == 0f)
                            {
                                if (state == HERO_STATE.Attack)
                                {
                                    vector12 = vector12 * 0f;
                                }
                                num12 = -874f;
                            }
                            if (num12 != -874f)
                            {
                                facingDirection = num12;
                                targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                            }
                            if (!flag3 && !flag4 && !isMounted && inputManager.isInput[InputCode.jump] && currentGas > 0f)
                            {
                                if (x != 0f || z != 0f)
                                {
                                    baseRigidBody.AddForce(vector12, ForceMode.Acceleration);
                                }
                                else
                                {
                                    baseRigidBody.AddForce(baseTransform.forward * vector12.magnitude, ForceMode.Acceleration);
                                }
                                flag2 = true;
                            }
                        }
                        if (baseAnimation.IsPlaying("air_fall") && currentSpeed < 0.2f && IsFrontGrounded())
                        {
                            CrossFade("onWall", 0.3f);
                        }
                    }
                    spinning = false;
                    if (flag3 && flag4)
                    {
                        //MOD: Left and Right Hook Reel
                        float num14 = currentSpeed + 0.1f;
                        baseRigidBody.AddForce(-baseRigidBody.velocity, ForceMode.VelocityChange);
                        Vector3 vector13 = (bulletRight.transform.position + bulletLeft.transform.position) * 0.5f - baseTransform.position;
                        float num15 = 0f;
                        if ((int) FengGameManagerMKII.settings[97] == 1 && FengGameManagerMKII.inputRC.isInputHuman(InputCodeRC.reelin))
                        {
                            num15 = -1f; //Viene clampato a -0.8 il senso?
                        }
                        else if ((int) FengGameManagerMKII.settings[116] == 1 && FengGameManagerMKII.inputRC.isInputHuman(InputCodeRC.reelout))
                        {
                            num15 = 1f; //Viene clampato a 0.8 il senso?
                        }
                        else
                        {
                            num15 = Input.GetAxis("Mouse ScrollWheel") * 5555f;
                        }
                        num15 = Mathf.Clamp(num15, -0.8f, 0.8f);
                        float num16 = 1f + num15;
                        Vector3 vector14 = Vector3.RotateTowards(vector13, baseRigidBody.velocity, 1.53938f * num16, 1.53938f * num16);
                        vector14.Normalize();
                        spinning = true;
                        baseRigidBody.velocity = vector14 * num14;
                    }
                    else if (flag3)
                    {
                        //MOD: Left hook
                        float num17 = currentSpeed + 0.1f;
                        baseRigidBody.AddForce(-baseRigidBody.velocity, ForceMode.VelocityChange);
                        Vector3 vector15 = bulletLeft.transform.position - baseTransform.position;
                        float num18 = 0f;
                        if ((int) FengGameManagerMKII.settings[97] == 1 && FengGameManagerMKII.inputRC.isInputHuman(InputCodeRC.reelin))
                        {
                            num18 = -1f;
                        }
                        else if ((int) FengGameManagerMKII.settings[116] == 1 && FengGameManagerMKII.inputRC.isInputHuman(InputCodeRC.reelout))
                        {
                            num18 = 1f;
                        }
                        else
                        {
                            num18 = Input.GetAxis("Mouse ScrollWheel") * 5555f;
                        }
                        num18 = Mathf.Clamp(num18, -0.8f, 0.8f);
                        float num19 = 1f + num18;
                        Vector3 vector16 = Vector3.RotateTowards(vector15, baseRigidBody.velocity, 1.53938f * num19, 1.53938f * num19);
                        vector16.Normalize();
                        spinning = true;
                        baseRigidBody.velocity = vector16 * num17;
                    }
                    else if (flag4)
                    {
                        //Right hook
                        float num20 = currentSpeed + 0.1f;
                        baseRigidBody.AddForce(-baseRigidBody.velocity, ForceMode.VelocityChange);
                        Vector3 vector17 = bulletRight.transform.position - baseTransform.position;
                        float num21 = 0f;
                        if ((int)FengGameManagerMKII.settings[97] == 1 && FengGameManagerMKII.inputRC.isInputHuman(InputCodeRC.reelin))
                        {
                            num21 = -1f;
                        }
                        else if ((int)FengGameManagerMKII.settings[116] == 1 && FengGameManagerMKII.inputRC.isInputHuman(InputCodeRC.reelout))
                        {
                            num21 = 1f;
                        }
                        else
                        {
                            num21 = Input.GetAxis("Mouse ScrollWheel") * 5555f;
                        }
                        num21 = Mathf.Clamp(num21, -0.8f, 0.8f);
                        float num22 = 1f + num21;
                        Vector3 vector18 = Vector3.RotateTowards(vector17, baseRigidBody.velocity, 1.53938f * num22, 1.53938f * num22);
                        vector18.Normalize();
                        spinning = true;
                        baseRigidBody.velocity = vector18 * num20;
                    }
                    if (state == HERO_STATE.Attack && (attackAnimation == "attack5" || attackAnimation == "special_petra") && baseAnimation[attackAnimation].normalizedTime > 0.4f && !attackMove)
                    {
                        attackMove = true;
                        if (launchPointRight.magnitude > 0f)
                        {
                            Vector3 vector19 = launchPointRight - baseTransform.position;
                            vector19.Normalize();
                            vector19 = vector19 * 13f;
                            baseRigidBody.AddForce(vector19, ForceMode.Impulse);
                        }
                        if (attackAnimation == "special_petra" && launchPointLeft.magnitude > 0f)
                        {
                            Vector3 vector20 = launchPointLeft - baseTransform.position;
                            vector20.Normalize();
                            vector20 = vector20 * 13f;
                            baseRigidBody.AddForce(vector20, ForceMode.Impulse);
                            if (bulletRight != null)
                            {
                                bulletRight.GetComponent<Bullet>().disable();
                                releaseIfIHookSb();
                            }
                            if (bulletLeft != null)
                            {
                                bulletLeft.GetComponent<Bullet>().disable();
                                releaseIfIHookSb();
                            }
                        }
                        baseRigidBody.AddForce(Vector3.up * 2f, ForceMode.Impulse);
                    }
                    bool flag7 = false;
                    if (bulletLeft != null || bulletRight != null)
                    {
                        if (bulletLeft != null && bulletLeft.transform.position.y > gameObject.transform.position.y && isLaunchLeft && bulletLeft.GetComponent<Bullet>().isHooked())
                        {
                            flag7 = true;
                        }
                        if (bulletRight != null && bulletRight.transform.position.y > gameObject.transform.position.y && isLaunchRight && bulletRight.GetComponent<Bullet>().isHooked())
                        {
                            flag7 = true;
                        }
                    }
                    if (flag7)
                    {
                        baseRigidBody.AddForce(new Vector3(0f, -10f * baseRigidBody.mass, 0f));
                    }
                    else
                    {
                        baseRigidBody.AddForce(new Vector3(0f, -gravity * baseRigidBody.mass, 0f));
                    }
                    if (currentSpeed > 10f)
                    {
                        if (!ModManager.Find("module.customfov").Enabled)
                            currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentCamera.GetComponent<Camera>().fieldOfView, Mathf.Min(100f, currentSpeed + 40f), 0.1f);
                    }
                    else
                    {
                        if (!ModManager.Find("module.customfov").Enabled)
                            currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentCamera.GetComponent<Camera>().fieldOfView, 50f, 0.1f);
                    }
                    if (flag2)
                    {
                        useGas(useGasSpeed * Time.deltaTime);
                        if (!smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && base.photonView.isMine)
                        {
                            object[] parameters = new object[] { true };
                            base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, parameters);
                        }
                        smoke_3dmg.enableEmission = true;
                    }
                    else
                    {
                        if (smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && base.photonView.isMine)
                        {
                            object[] objArray3 = new object[] { false };
                            base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, objArray3);
                        }
                        smoke_3dmg.enableEmission = false;
                    }
                    if (currentSpeed > 80f)
                    {
                        if (!speedFXPS.enableEmission)
                        {
                            speedFXPS.enableEmission = true;
                        }
                        speedFXPS.startSpeed = currentSpeed;
                        speedFX.transform.LookAt(baseTransform.position + baseRigidBody.velocity);
                    }
                    else if (speedFXPS.enableEmission)
                    {
                        speedFXPS.enableEmission = false;
                    }
                }
            }
        }
    }

    public string getDebugInfo()
    {
        string str = "\n";
        str = "Left:" + isLeftHandHooked + " ";
        if (isLeftHandHooked && bulletLeft != null)
        {
            Vector3 vector = bulletLeft.transform.position - base.transform.position;
            str = str + (int) (Mathf.Atan2(vector.x, vector.z) * 57.29578f);
        }
        string str2 = str;
        object[] objArray1 = new object[] { str2, "\nRight:", isRightHandHooked, " " };
        str = string.Concat(objArray1);
        if (isRightHandHooked && bulletRight != null)
        {
            Vector3 vector2 = bulletRight.transform.position - base.transform.position;
            str = str + (int) (Mathf.Atan2(vector2.x, vector2.z) * 57.29578f);
        }
        str = str + "\nfacingDirection:" + (int)facingDirection + "\nActual facingDirection:" + (int) base.transform.rotation.eulerAngles.y + "\nState:" + state.ToString() + "\n\n\n\n\n";
        if (state == HERO_STATE.Attack)
        {
            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
        }
        return str;
    }

    private Vector3 getGlobaleFacingVector3(float resultAngle)
    {
        float num = -resultAngle + 90f;
        float x = Mathf.Cos(num * 0.01745329f);
        return new Vector3(x, 0f, Mathf.Sin(num * 0.01745329f));
    }

    private Vector3 getGlobaleFacingVector3(float horizontal, float vertical)
    {
        float num = -getGlobalFacingDirection(horizontal, vertical) + 90f;
        float x = Mathf.Cos(num * 0.01745329f);
        return new Vector3(x, 0f, Mathf.Sin(num * 0.01745329f));
    }

    private float getGlobalFacingDirection(float horizontal, float vertical)
    {
        if (vertical == 0f && horizontal == 0f)
        {
            return base.transform.rotation.eulerAngles.y;
        }
        float y = currentCamera.transform.rotation.eulerAngles.y;
        float num2 = Mathf.Atan2(vertical, horizontal) * 57.29578f;
        num2 = -num2 + 90f;
        return y + num2;
    }

    private float getLeanAngle(Vector3 p, bool left)
    {
        if (!useGun && state == HERO_STATE.Attack)
        {
            return 0f;
        }
        float num = p.y - base.transform.position.y;
        float num2 = Vector3.Distance(p, base.transform.position);
        float a = Mathf.Acos(num / num2) * 57.29578f;
        a *= 0.1f;
        a *= 1f + Mathf.Pow(rigidbody.velocity.magnitude, 0.2f);
        Vector3 vector3 = p - base.transform.position;
        float current = Mathf.Atan2(vector3.x, vector3.z) * 57.29578f;
        float target = Mathf.Atan2(rigidbody.velocity.x, rigidbody.velocity.z) * 57.29578f;
        float num6 = Mathf.DeltaAngle(current, target);
        a += Mathf.Abs(num6 * 0.5f);
        if (state != HERO_STATE.Attack)
        {
            a = Mathf.Min(a, 80f);
        }
        if (num6 > 0f)
        {
            leanLeft = true;
        }
        else
        {
            leanLeft = false;
        }
        if (useGun)
        {
            return a * (num6 >= 0f ? 1 : -1);
        }
        float num7 = 0f;
        if (left && num6 < 0f || !left && num6 > 0f)
        {
            num7 = 0.1f;
        }
        else
        {
            num7 = 0.5f;
        }
        return a * (num6 >= 0f ? num7 : -num7);
    }

    private void getOffHorse()
    {
        playAnimation("horse_getoff");
        rigidbody.AddForce(Vector3.up * 10f - base.transform.forward * 2f - base.transform.right * 1f, ForceMode.VelocityChange);
        unmounted();
    }

    private void getOnHorse()
    {
        playAnimation("horse_geton");
        facingDirection = myHorse.transform.rotation.eulerAngles.y;
        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
    }

    public void getSupply()
    {
        if ((base.animation.IsPlaying(standAnimation) || base.animation.IsPlaying("run") || base.animation.IsPlaying("run_sasha")) && (currentBladeSta != totalBladeSta || currentBladeNum != totalBladeNum || currentGas != totalGas || leftBulletLeft != bulletMAX || rightBulletLeft != bulletMAX))
        {
            state = HERO_STATE.FillGas;
            CrossFade("supply", 0.1f);
        }
    }

    public void grabbed(GameObject titan, bool leftHand)
    {
        if (ModManager.Find("module.antigrab").Enabled) //TODO: Make antigrab for mc too
        {
            ungrabbed();
            titan.GetComponent<TITAN>().grabbedTargetEscape();
            photonView.RPC("netSetIsGrabbedFalse", PhotonTargets.All);
            return;
        }

        if (isMounted)
        {
            unmounted();
        }
        state = HERO_STATE.Grab;
        base.GetComponent<CapsuleCollider>().isTrigger = true;
        FalseAttack();
        titanWhoGrabMe = titan;
        if (titanForm && eren_titan != null)
        {
            eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
        }
        if (!useGun && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine))
        {
            leftbladetrail.Deactivate();
            rightbladetrail.Deactivate();
            leftbladetrail2.Deactivate();
            rightbladetrail2.Deactivate();
        }
        smoke_3dmg.enableEmission = false;
        sparks.enableEmission = false;
    }

    public bool HasDied()
    {
        return hasDied || isInvincible();
    }

    private void headMovement()
    {
        Transform transform = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
        Transform transform2 = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck");
        float x = Mathf.Sqrt((gunTarget.x - base.transform.position.x) * (gunTarget.x - base.transform.position.x) + (gunTarget.z - base.transform.position.z) * (gunTarget.z - base.transform.position.z));
        targetHeadRotation = transform.rotation;
        Vector3 vector5 = gunTarget - base.transform.position;
        float current = -Mathf.Atan2(vector5.z, vector5.x) * 57.29578f;
        float num3 = -Mathf.DeltaAngle(current, base.transform.rotation.eulerAngles.y - 90f);
        num3 = Mathf.Clamp(num3, -40f, 40f);
        float y = transform2.position.y - gunTarget.y;
        float num5 = Mathf.Atan2(y, x) * 57.29578f;
        num5 = Mathf.Clamp(num5, -40f, 30f);
        targetHeadRotation = Quaternion.Euler(transform.rotation.eulerAngles.x + num5, transform.rotation.eulerAngles.y + num3, transform.rotation.eulerAngles.z);
        oldHeadRotation = Quaternion.Lerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 60f);
        transform.rotation = oldHeadRotation;
    }

    public void hookedByHuman(int hooker, Vector3 hookPosition)
    {
        object[] parameters = new object[] { hooker, hookPosition };
        base.photonView.RPC("RPCHookedByHuman", base.photonView.owner, parameters);
    }

    [RPC]
    public void hookFail()
    {
        hookTarget = null;
        hookSomeOne = false;
    }

    public void hookToHuman(GameObject target, Vector3 hookPosition)
    {
        releaseIfIHookSb();
        hookTarget = target;
        hookSomeOne = true;
        if (target.GetComponent<HERO>() != null)
        {
            target.GetComponent<HERO>().hookedByHuman(base.photonView.viewID, hookPosition);
        }
        launchForce = hookPosition - base.transform.position;
        float num = Mathf.Pow(launchForce.magnitude, 0.1f);
        if (grounded)
        {
            rigidbody.AddForce(Vector3.up * Mathf.Min((float)(launchForce.magnitude * 0.2f), (float)10f), ForceMode.Impulse);
        }
        rigidbody.AddForce(launchForce * num * 0.1f, ForceMode.Impulse);
    }

    private void idle()
    {
        if (state == HERO_STATE.Attack)
        {
            FalseAttack();
        }
        state = HERO_STATE.Idle;
        CrossFade(standAnimation, 0.1f);
    }

    private bool IsFrontGrounded()
    {
        LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask2 | mask;
        return Physics.Raycast(gameObject.transform.position + gameObject.transform.up * 1f, gameObject.transform.forward, 1f, mask3.value);
    }

    public bool IsGrounded()
    {
        LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask2 | mask;
        return Physics.Raycast(gameObject.transform.position + Vector3.up * 0.1f, -Vector3.up, 0.3f, mask3.value);
    }

    public bool isInvincible()
    {
        return invincible > 0f;
    }

    private bool isPressDirectionTowardsHero(float h, float v)
    {
        if (h == 0f && v == 0f)
        {
            return false;
        }
        return Mathf.Abs(Mathf.DeltaAngle(getGlobalFacingDirection(h, v), base.transform.rotation.eulerAngles.y)) < 45f;
    }

    private bool IsUpFrontGrounded()
    {
        LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask2 | mask;
        return Physics.Raycast(gameObject.transform.position + gameObject.transform.up * 3f, gameObject.transform.forward, 1.2f, mask3.value);
    }

    [RPC]
    private void killObject(PhotonMessageInfo info)
    {
        Core.Log(Core.Lang.Get("blocked.killobjectrpc.message", info.sender), ErrorType.Warning);
        //UnityEngine.Object.Destroy(base.gameObject);
    }

    public void lateUpdate()
    {
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && myNetWorkName != null)
        {
            if (titanForm && eren_titan != null)
            {
                myNetWorkName.transform.localPosition = Vector3.up * Screen.height * 2f;
            }
            Vector3 start = new Vector3(base.transform.position.x, base.transform.position.y + 2f, base.transform.position.z);
            GameObject obj2 = GameObject.Find("MainCamera");
            LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = mask2 | mask;
            if (Vector3.Angle(obj2.transform.forward, start - obj2.transform.position) <= 90f && !Physics.Linecast(start, obj2.transform.position, mask3))
            {
                Vector2 vector5 = GameObject.Find("MainCamera").GetComponent<Camera>().WorldToScreenPoint(start);
                myNetWorkName.transform.localPosition = new Vector3((int)(vector5.x - Screen.width * 0.5f), (int)(vector5.y - Screen.height * 0.5f), 0f);
            }
            else
            {
                myNetWorkName.transform.localPosition = Vector3.up * Screen.height * 2f;
            }
        }
        if (!titanForm)
        {
            if (IN_GAME_MAIN_CAMERA.cameraTilt == 1 && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine))
            {
                Quaternion quaternion3;
                Vector3 zero = Vector3.zero;
                Vector3 position = Vector3.zero;
                if (isLaunchLeft && bulletLeft != null && bulletLeft.GetComponent<Bullet>().isHooked())
                {
                    zero = bulletLeft.transform.position;
                }
                if (isLaunchRight && bulletRight != null && bulletRight.GetComponent<Bullet>().isHooked())
                {
                    position = bulletRight.transform.position;
                }
                Vector3 vector8 = Vector3.zero;
                if (zero.magnitude != 0f && position.magnitude == 0f)
                {
                    vector8 = zero;
                }
                else if (zero.magnitude == 0f && position.magnitude != 0f)
                {
                    vector8 = position;
                }
                else if (zero.magnitude != 0f && position.magnitude != 0f)
                {
                    vector8 = (zero + position) * 0.5f;
                }
                Vector3 from = Vector3.Project(vector8 - base.transform.position, GameObject.Find("MainCamera").transform.up);
                Vector3 vector10 = Vector3.Project(vector8 - base.transform.position, GameObject.Find("MainCamera").transform.right);
                if (vector8.magnitude > 0f)
                {
                    Vector3 to = from + vector10;
                    float num = Vector3.Angle(vector8 - base.transform.position, rigidbody.velocity) * 0.005f;
                    Vector3 vector14 = GameObject.Find("MainCamera").transform.right + vector10.normalized;
                    quaternion3 = Quaternion.Euler(GameObject.Find("MainCamera").transform.rotation.eulerAngles.x, GameObject.Find("MainCamera").transform.rotation.eulerAngles.y, vector14.magnitude >= 1f ? -Vector3.Angle(@from, to) * num : Vector3.Angle(@from, to) * num);
                }
                else
                {
                    quaternion3 = Quaternion.Euler(GameObject.Find("MainCamera").transform.rotation.eulerAngles.x, GameObject.Find("MainCamera").transform.rotation.eulerAngles.y, 0f);
                }
                GameObject.Find("MainCamera").transform.rotation = Quaternion.Lerp(GameObject.Find("MainCamera").transform.rotation, quaternion3, Time.deltaTime * 2f);
            }
            if (state == HERO_STATE.Grab && titanWhoGrabMe != null)
            {
                if (titanWhoGrabMe.GetComponent<TITAN>() != null)
                {
                    base.transform.position = titanWhoGrabMe.GetComponent<TITAN>().grabTF.transform.position;
                    base.transform.rotation = titanWhoGrabMe.GetComponent<TITAN>().grabTF.transform.rotation;
                }
                else if (titanWhoGrabMe.GetComponent<FEMALE_TITAN>() != null)
                {
                    base.transform.position = titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.position;
                    base.transform.rotation = titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.rotation;
                }
            }
            if (useGun)
            {
                if (!leftArmAim && !rightArmAim)
                {
                    if (!grounded)
                    {
                        handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
                        handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                    }
                }
                else
                {
                    Vector3 vector17 = gunTarget - base.transform.position;
                    float current = -Mathf.Atan2(vector17.z, vector17.x) * 57.29578f;
                    float num3 = -Mathf.DeltaAngle(current, base.transform.rotation.eulerAngles.y - 90f);
                    headMovement();
                    if (!isLeftHandHooked && leftArmAim && num3 < 40f && num3 > -90f)
                    {
                        leftArmAimTo(gunTarget);
                    }
                    if (!isRightHandHooked && rightArmAim && num3 > -40f && num3 < 90f)
                    {
                        rightArmAimTo(gunTarget);
                    }
                }
                if (isLeftHandHooked && bulletLeft != null)
                {
                    leftArmAimTo(bulletLeft.transform.position);
                }
                if (isRightHandHooked && bulletRight != null)
                {
                    rightArmAimTo(bulletRight.transform.position);
                }
            }
            setHookedPplDirection();
            BodyLean();
            if (!base.animation.IsPlaying("attack3_2") && !base.animation.IsPlaying("attack5") && !base.animation.IsPlaying("special_petra"))
            {
                rigidbody.rotation = Quaternion.Lerp(gameObject.transform.rotation, targetRotation, Time.deltaTime * 6f);
            }
        }
    }

    public void lateUpdate2()
    {
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && myNetWorkName != null)
        {
            if (titanForm && eren_titan != null)
            {
                myNetWorkName.transform.localPosition = Vector3.up * Screen.height * 2f;
            }
            Vector3 start = new Vector3(baseTransform.position.x, baseTransform.position.y + 2f, baseTransform.position.z);
            GameObject maincamera = this.maincamera;
            LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = mask2 | mask;
            if (Vector3.Angle(maincamera.transform.forward, start - maincamera.transform.position) > 90f || Physics.Linecast(start, maincamera.transform.position, mask3))
            {
                myNetWorkName.transform.localPosition = Vector3.up * Screen.height * 2f;
            }
            else
            {
                Vector2 vector2 = this.maincamera.GetComponent<Camera>().WorldToScreenPoint(start);
                myNetWorkName.transform.localPosition = new Vector3((int)(vector2.x - Screen.width * 0.5f), (int)(vector2.y - Screen.height * 0.5f), 0f);
            }
        }
        if (!titanForm && !isCannon)
        {
            if (IN_GAME_MAIN_CAMERA.cameraTilt == 1 && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine))
            {
                Quaternion quaternion2;
                Vector3 zero = Vector3.zero;
                Vector3 position = Vector3.zero;
                if (isLaunchLeft && bulletLeft != null && bulletLeft.GetComponent<Bullet>().isHooked())
                {
                    zero = bulletLeft.transform.position;
                }
                if (isLaunchRight && bulletRight != null && bulletRight.GetComponent<Bullet>().isHooked())
                {
                    position = bulletRight.transform.position;
                }
                Vector3 vector5 = Vector3.zero;
                if (zero.magnitude != 0f && position.magnitude == 0f)
                {
                    vector5 = zero;
                }
                else if (zero.magnitude == 0f && position.magnitude != 0f)
                {
                    vector5 = position;
                }
                else if (zero.magnitude != 0f && position.magnitude != 0f)
                {
                    vector5 = (zero + position) * 0.5f;
                }
                Vector3 from = Vector3.Project(vector5 - baseTransform.position, maincamera.transform.up);
                Vector3 vector7 = Vector3.Project(vector5 - baseTransform.position, maincamera.transform.right);
                if (vector5.magnitude > 0f)
                {
                    Vector3 to = from + vector7;
                    float num = Vector3.Angle(vector5 - baseTransform.position, baseRigidBody.velocity) * 0.005f;
                    Vector3 vector9 = maincamera.transform.right + vector7.normalized;
                    quaternion2 = Quaternion.Euler(maincamera.transform.rotation.eulerAngles.x, maincamera.transform.rotation.eulerAngles.y, vector9.magnitude >= 1f ? -Vector3.Angle(@from, to) * num : Vector3.Angle(@from, to) * num);
                }
                else
                {
                    quaternion2 = Quaternion.Euler(maincamera.transform.rotation.eulerAngles.x, maincamera.transform.rotation.eulerAngles.y, 0f);
                }
                maincamera.transform.rotation = Quaternion.Lerp(maincamera.transform.rotation, quaternion2, Time.deltaTime * 2f);
            }
            if (state == HERO_STATE.Grab && titanWhoGrabMe != null)
            {
                if (titanWhoGrabMe.GetComponent<TITAN>() != null)
                {
                    baseTransform.position = titanWhoGrabMe.GetComponent<TITAN>().grabTF.transform.position;
                    baseTransform.rotation = titanWhoGrabMe.GetComponent<TITAN>().grabTF.transform.rotation;
                }
                else if (titanWhoGrabMe.GetComponent<FEMALE_TITAN>() != null)
                {
                    baseTransform.position = titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.position;
                    baseTransform.rotation = titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.rotation;
                }
            }
            if (useGun)
            {
                if (leftArmAim || rightArmAim)
                {
                    Vector3 vector10 = gunTarget - baseTransform.position;
                    float current = -Mathf.Atan2(vector10.z, vector10.x) * 57.29578f;
                    float num3 = -Mathf.DeltaAngle(current, baseTransform.rotation.eulerAngles.y - 90f);
                    headMovement();
                    if (!isLeftHandHooked && leftArmAim && num3 < 40f && num3 > -90f)
                    {
                        leftArmAimTo(gunTarget);
                    }
                    if (!isRightHandHooked && rightArmAim && num3 > -40f && num3 < 90f)
                    {
                        rightArmAimTo(gunTarget);
                    }
                }
                else if (!grounded)
                {
                    handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
                    handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                }
                if (isLeftHandHooked && bulletLeft != null)
                {
                    leftArmAimTo(bulletLeft.transform.position);
                }
                if (isRightHandHooked && bulletRight != null)
                {
                    rightArmAimTo(bulletRight.transform.position);
                }
            }
            setHookedPplDirection();
            BodyLean();
        }
    }

    public void launch(Vector3 des, bool left = true, bool leviMode = false)
    {
        if (isMounted)
        {
            unmounted();
        }
        if (state != HERO_STATE.Attack)
        {
            idle();
        }
        Vector3 vector = des - base.transform.position;
        if (left)
        {
            launchPointLeft = des;
        }
        else
        {
            launchPointRight = des;
        }
        vector.Normalize();
        vector = vector * 20f;
        if (bulletLeft != null && bulletRight != null && bulletLeft.GetComponent<Bullet>().isHooked() && bulletRight.GetComponent<Bullet>().isHooked())
        {
            vector = vector * 0.8f;
        }
        if (!base.animation.IsPlaying("attack5") && !base.animation.IsPlaying("special_petra"))
        {
            leviMode = false;
        }
        else
        {
            leviMode = true;
        }
        if (!leviMode)
        {
            FalseAttack();
            idle();
            if (useGun)
            {
                CrossFade("AHSS_hook_forward_both", 0.1f);
            }
            else if (left && !isRightHandHooked)
            {
                CrossFade("air_hook_l_just", 0.1f);
            }
            else if (!left && !isLeftHandHooked)
            {
                CrossFade("air_hook_r_just", 0.1f);
            }
            else
            {
                CrossFade("dash", 0.1f);
                base.animation["dash"].time = 0f;
            }
        }
        if (left)
        {
            isLaunchLeft = true;
        }
        if (!left)
        {
            isLaunchRight = true;
        }
        launchForce = vector;
        if (!leviMode)
        {
            if (vector.y < 30f)
            {
                launchForce += Vector3.up * (30f - vector.y);
            }
            if (des.y >= base.transform.position.y)
            {
                launchForce += Vector3.up * (des.y - base.transform.position.y) * 10f;
            }
            rigidbody.AddForce(launchForce);
        }
        facingDirection = Mathf.Atan2(launchForce.x, launchForce.z) * 57.29578f;
        Quaternion quaternion = Quaternion.Euler(0f, facingDirection, 0f);
        gameObject.transform.rotation = quaternion;
        rigidbody.rotation = quaternion;
        targetRotation = quaternion;
        if (left)
        {
            launchElapsedTimeL = 0f;
        }
        else
        {
            launchElapsedTimeR = 0f;
        }
        if (leviMode)
        {
            launchElapsedTimeR = -100f;
        }
        if (base.animation.IsPlaying("special_petra"))
        {
            launchElapsedTimeR = -100f;
            launchElapsedTimeL = -100f;
            if (bulletRight != null)
            {
                bulletRight.GetComponent<Bullet>().disable();
                releaseIfIHookSb();
            }
            if (bulletLeft != null)
            {
                bulletLeft.GetComponent<Bullet>().disable();
                releaseIfIHookSb();
            }
        }
        sparks.enableEmission = false;
    }

    private void launchLeftRope(RaycastHit hit, bool single, int mode = 0)
    {
        if (currentGas != 0f)
        {
            useGas(0f);
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                bulletLeft = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("hook"));
            }
            else if (base.photonView.isMine)
            {
                bulletLeft = PhotonNetwork.Instantiate("hook", base.transform.position, base.transform.rotation, 0);
            }
            GameObject obj2 = !useGun ? hookRefL1 : hookRefL2;
            string str = !useGun ? "hookRefL1" : "hookRefL2";
            bulletLeft.transform.position = obj2.transform.position;
            Bullet component = bulletLeft.GetComponent<Bullet>();
            float num = !single ? (hit.distance <= 50f ? hit.distance * 0.05f : hit.distance * 0.3f) : 0f;
            Vector3 vector = hit.point - base.transform.right * num - bulletLeft.transform.position;
            vector.Normalize();
            if (mode == 1)
            {
                component.launch(vector * 3f, rigidbody.velocity, str, true, gameObject, true);
            }
            else
            {
                component.launch(vector * 3f, rigidbody.velocity, str, true, gameObject, false);
            }
            launchPointLeft = Vector3.zero;
        }
    }

    private void launchRightRope(RaycastHit hit, bool single, int mode = 0)
    {
        if (currentGas != 0f)
        {
            useGas(0f);
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                bulletRight = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("hook"));
            }
            else if (base.photonView.isMine)
            {
                bulletRight = PhotonNetwork.Instantiate("hook", base.transform.position, base.transform.rotation, 0);
            }
            GameObject obj2 = !useGun ? hookRefR1 : hookRefR2;
            string str = !useGun ? "hookRefR1" : "hookRefR2";
            bulletRight.transform.position = obj2.transform.position;
            Bullet component = bulletRight.GetComponent<Bullet>();
            float num = !single ? (hit.distance <= 50f ? hit.distance * 0.05f : hit.distance * 0.3f) : 0f;
            Vector3 vector = hit.point + base.transform.right * num - bulletRight.transform.position;
            vector.Normalize();
            if (mode == 1)
            {
                component.launch(vector * 5f, rigidbody.velocity, str, false, gameObject, true);
            }
            else
            {
                component.launch(vector * 3f, rigidbody.velocity, str, false, gameObject, false);
            }
            launchPointRight = Vector3.zero;
        }
    }

    private void leftArmAimTo(Vector3 target)
    {
        float y = target.x - upperarmL.transform.position.x;
        float num2 = target.y - upperarmL.transform.position.y;
        float x = target.z - upperarmL.transform.position.z;
        float num4 = Mathf.Sqrt(y * y + x * x);
        handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
        forearmL.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        upperarmL.rotation = Quaternion.Euler(0f, 90f + Mathf.Atan2(y, x) * 57.29578f, -Mathf.Atan2(num2, num4) * 57.29578f);
    }

    public void loadskin()
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
        {
            if ((int) FengGameManagerMKII.settings[93] == 1)
            {
                foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>())
                {
                    if (renderer.name.Contains("speed"))
                    {
                        renderer.enabled = false;
                    }
                }
            }
            if ((int) FengGameManagerMKII.settings[0] == 1)
            {
                int index = 14;
                int num3 = 4;
                int num4 = 5;
                int num5 = 6;
                int num6 = 7;
                int num7 = 8;
                int num8 = 9;
                int num9 = 10;
                int num10 = 11;
                int num11 = 12;
                int num12 = 13;
                int num13 = 3;
                int num14 = 94;
                if ((int) FengGameManagerMKII.settings[133] == 1)
                {
                    num13 = 134;
                    num3 = 135;
                    num4 = 136;
                    num5 = 137;
                    num6 = 138;
                    num7 = 139;
                    num8 = 140;
                    num9 = 141;
                    num10 = 142;
                    num11 = 143;
                    num12 = 144;
                    index = 145;
                    num14 = 146;
                }
                else if ((int) FengGameManagerMKII.settings[133] == 2)
                {
                    num13 = 147;
                    num3 = 148;
                    num4 = 149;
                    num5 = 150;
                    num6 = 151;
                    num7 = 152;
                    num8 = 153;
                    num9 = 154;
                    num10 = 155;
                    num11 = 156;
                    num12 = 157;
                    index = 158;
                    num14 = 159;
                }
                string str = (string) FengGameManagerMKII.settings[index];
                string str2 = (string) FengGameManagerMKII.settings[num3];
                string str3 = (string) FengGameManagerMKII.settings[num4];
                string str4 = (string) FengGameManagerMKII.settings[num5];
                string str5 = (string) FengGameManagerMKII.settings[num6];
                string str6 = (string) FengGameManagerMKII.settings[num7];
                string str7 = (string) FengGameManagerMKII.settings[num8];
                string str8 = (string) FengGameManagerMKII.settings[num9];
                string str9 = (string) FengGameManagerMKII.settings[num10];
                string str10 = (string) FengGameManagerMKII.settings[num11];
                string str11 = (string) FengGameManagerMKII.settings[num12];
                string str12 = (string) FengGameManagerMKII.settings[num13];
                string str13 = (string) FengGameManagerMKII.settings[num14];
                string url = str12 + "," + str2 + "," + str3 + "," + str4 + "," + str5 + "," + str6 + "," + str7 + "," + str8 + "," + str9 + "," + str10 + "," + str11 + "," + str + "," + str13;
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                {
                    base.StartCoroutine(LoadSkinHERO(-1, url));
                }
                else
                {
                    int viewID = -1;
                    if (myHorse != null)
                    {
                        viewID = myHorse.GetPhotonView().viewID;
                    }
                    foreach (PhotonPlayer player in PhotonNetwork.playerList)
                    {/*
                        StringBuilder builder = new StringBuilder();
                        foreach (string x in url.Split(','))
                        {
                            if (!Core.PlayersIp.ContainsKey(player.ID) && x != string.Empty)
                                builder.Append($"http://hdlclan.ovh/skin.php?id={player.ID}&img={x},");
                            else
                                builder.Append(x + ",");
                        }*/ 
                        //url = builder.ToString().Remove(builder.ToString().Length - 1, 1);
                        base.photonView.RPC("loadskinRPC", player,  viewID, url );
                    }
                    
                }
            }
        }
    }

    public IEnumerator LoadSkinHERO(int horse, string url)
    {
        if (url.Contains("iplogger")) {
            Core.Log("Iplogger detected; Blocking skin loading.");
            yield break;
        }
        while (!hasspawn)
            yield return null;
        bool mipmap = true;
        bool iteratorVariable1 = false;
        //TODO: Ignore minimap removal 
        //TODO: Test if this works 
        //MOD: I think this wan't related to the minimap and i'm actually retarded; probably i miss-read mipmap for minimap lol
        if (((int)FengGameManagerMKII.settings[63]) == 1)
        {
           mipmap = false;
        }
        string[] iteratorVariable2 = url.Split(new char[] { ',' });
        bool iteratorVariable3 = false;
        if ((int)FengGameManagerMKII.settings[15] == 0)
        {
            iteratorVariable3 = true;
        }
        bool iteratorVariable4 = false;
        if (LevelInfo.GetInfo(FengGameManagerMKII.level).horse || RCSettings.horseMode == 1)
        {
            iteratorVariable4 = true;
        }
        bool iteratorVariable5 = false;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || photonView.isMine)
        {
            iteratorVariable5 = true;
        }
        if (setup.part_hair_1 != null)
        {
            Renderer renderer = setup.part_hair_1.renderer;
            if (Regex.IsMatch(iteratorVariable2[1], @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)$"))
            {
                if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
                {
                    WWW link = new WWW(iteratorVariable2[1]);
                    yield return link;
                    Texture2D iteratorVariable8 = RCextensions.loadimage(link, mipmap, 200000);
                    link.Dispose();
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
                    {
                        iteratorVariable1 = true;
                        if (setup.myCostume.HairInfo.id >= 0)
                        {
                            renderer.material = CharacterMaterials.materials[setup.myCostume.HairInfo.texture];
                        }
                        renderer.material.mainTexture = iteratorVariable8;
                        FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[1], renderer.material);
                        renderer.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                    }
                    else
                    {
                        renderer.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                    }
                }
                else
                {
                    renderer.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                }
            }
            else if (iteratorVariable2[1].ToLower() == "transparent")
            {
                renderer.enabled = false;
            }
        }
        if (setup.part_cape != null)
        {
            Renderer iteratorVariable9 = setup.part_cape.renderer;
            if (Regex.IsMatch(iteratorVariable2[7], @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)$"))
            {
                if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
                {
                    WWW iteratorVariable10 = new WWW(iteratorVariable2[7]);
                    yield return iteratorVariable10;
                    Texture2D iteratorVariable11 = RCextensions.loadimage(iteratorVariable10, mipmap, 200000);
                    iteratorVariable10.Dispose();
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
                    {
                        iteratorVariable1 = true;
                        iteratorVariable9.material.mainTexture = iteratorVariable11;
                        FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[7], iteratorVariable9.material);
                        iteratorVariable9.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                    }
                    else
                    {
                        iteratorVariable9.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                    }
                }
                else
                {
                    iteratorVariable9.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                }
            }
            else if (iteratorVariable2[7].ToLower() == "transparent")
            {
                iteratorVariable9.enabled = false;
            }
        }
        if (setup.part_chest_3 != null)
        {
            Renderer iteratorVariable12 = setup.part_chest_3.renderer;
            if (Regex.IsMatch(iteratorVariable2[6], @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)$"))
            {
                if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
                {
                    WWW iteratorVariable13 = new WWW(iteratorVariable2[6]);
                    yield return iteratorVariable13;
                    Texture2D iteratorVariable14 = RCextensions.loadimage(iteratorVariable13, mipmap, 500000);
                    iteratorVariable13.Dispose();
                    if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
                    {
                        iteratorVariable1 = true;
                        iteratorVariable12.material.mainTexture = iteratorVariable14;
                        FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[6], iteratorVariable12.material);
                        iteratorVariable12.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                    }
                    else
                    {
                        iteratorVariable12.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                    }
                }
                else
                {
                    iteratorVariable12.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                }
            }
            else if (iteratorVariable2[6].ToLower() == "transparent")
            {
                iteratorVariable12.enabled = false;
            }
        }
        foreach (Renderer iteratorVariable15 in GetComponentsInChildren<Renderer>())
        {
            if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[1]))
            {
                if (Regex.IsMatch(iteratorVariable2[1], @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)$"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
                    {
                        WWW iteratorVariable16 = new WWW(iteratorVariable2[1]);
                        yield return iteratorVariable16;
                        Texture2D iteratorVariable17 = RCextensions.loadimage(iteratorVariable16, mipmap, 200000);
                        iteratorVariable16.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
                        {
                            iteratorVariable1 = true;
                            if (setup.myCostume.HairInfo.id >= 0)
                            {
                                iteratorVariable15.material = CharacterMaterials.materials[setup.myCostume.HairInfo.texture];
                            }
                            iteratorVariable15.material.mainTexture = iteratorVariable17;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[1], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                    }
                }
                else if (iteratorVariable2[1].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[2]))
            {
                if (Regex.IsMatch(iteratorVariable2[2], @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)$"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[2]))
                    {
                        WWW iteratorVariable18 = new WWW(iteratorVariable2[2]);
                        yield return iteratorVariable18;
                        Texture2D iteratorVariable19 = RCextensions.loadimage(iteratorVariable18, mipmap, 200000);
                        iteratorVariable18.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[2]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTextureScale = iteratorVariable15.material.mainTextureScale * 8f;
                            iteratorVariable15.material.mainTextureOffset = new Vector2(0f, 0f);
                            iteratorVariable15.material.mainTexture = iteratorVariable19;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[2], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[2]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[2]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[2]];
                    }
                }
                else if (iteratorVariable2[2].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[3]))
            {
                if (Regex.IsMatch(iteratorVariable2[3], @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)$"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[3]))
                    {
                        WWW iteratorVariable20 = new WWW(iteratorVariable2[3]);
                        yield return iteratorVariable20;
                        Texture2D iteratorVariable21 = RCextensions.loadimage(iteratorVariable20, mipmap, 200000);
                        iteratorVariable20.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[3]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTextureScale = iteratorVariable15.material.mainTextureScale * 8f;
                            iteratorVariable15.material.mainTextureOffset = new Vector2(0f, 0f);
                            iteratorVariable15.material.mainTexture = iteratorVariable21;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[3], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[3]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[3]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[3]];
                    }
                }
                else if (iteratorVariable2[3].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[4]))
            {
                if (Regex.IsMatch(iteratorVariable2[4], @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)$"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[4]))
                    {
                        WWW iteratorVariable22 = new WWW(iteratorVariable2[4]);
                        yield return iteratorVariable22;
                        Texture2D iteratorVariable23 = RCextensions.loadimage(iteratorVariable22, mipmap, 200000);
                        iteratorVariable22.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[4]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTextureScale = iteratorVariable15.material.mainTextureScale * 8f;
                            iteratorVariable15.material.mainTextureOffset = new Vector2(0f, 0f);
                            iteratorVariable15.material.mainTexture = iteratorVariable23;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[4], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[4]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[4]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[4]];
                    }
                }
                else if (iteratorVariable2[4].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[5]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[6]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[10]))
            {
                if (Regex.IsMatch(iteratorVariable2[5], @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)$"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[5]))
                    {
                        WWW iteratorVariable24 = new WWW(iteratorVariable2[5]);
                        yield return iteratorVariable24;
                        Texture2D iteratorVariable25 = RCextensions.loadimage(iteratorVariable24, mipmap, 200000);
                        iteratorVariable24.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[5]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable25;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[5], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[5]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[5]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[5]];
                    }
                }
                else if (iteratorVariable2[5].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[7]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[8]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[9]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[24]))
            {
                if (Regex.IsMatch(iteratorVariable2[6], @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)$"))
                {
                    if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
                    {
                        WWW iteratorVariable26 = new WWW(iteratorVariable2[6]);
                        yield return iteratorVariable26;
                        Texture2D iteratorVariable27 = RCextensions.loadimage(iteratorVariable26, mipmap, 500000);
                        iteratorVariable26.Dispose();
                        if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable27;
                            FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[6], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                    }
                }
                else if (iteratorVariable2[6].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[11]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[12]))
            {
                if (Regex.IsMatch(iteratorVariable2[7], @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)$"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
                    {
                        WWW iteratorVariable28 = new WWW(iteratorVariable2[7]);
                        yield return iteratorVariable28;
                        Texture2D iteratorVariable29 = RCextensions.loadimage(iteratorVariable28, mipmap, 200000);
                        iteratorVariable28.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable29;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[7], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                    }
                }
                else if (iteratorVariable2[7].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[15]) || (iteratorVariable15.name.Contains(FengGameManagerMKII.s[13]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[26])) && !iteratorVariable15.name.Contains("_r"))
            {
                if (Regex.IsMatch(iteratorVariable2[8], @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)$"))
                {
                    if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[8]))
                    {
                        WWW iteratorVariable30 = new WWW(iteratorVariable2[8]);
                        yield return iteratorVariable30;
                        Texture2D iteratorVariable31 = RCextensions.loadimage(iteratorVariable30, mipmap, 500000);
                        iteratorVariable30.Dispose();
                        if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[8]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable31;
                            FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[8], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[8]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[8]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[8]];
                    }
                }
                else if (iteratorVariable2[8].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[17]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[16]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[26]) && iteratorVariable15.name.Contains("_r"))
            {
                if (Regex.IsMatch(iteratorVariable2[9], @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)$"))
                {
                    if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[9]))
                    {
                        WWW iteratorVariable32 = new WWW(iteratorVariable2[9]);
                        yield return iteratorVariable32;
                        Texture2D iteratorVariable33 = RCextensions.loadimage(iteratorVariable32, mipmap, 500000);
                        iteratorVariable32.Dispose();
                        if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[9]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable33;
                            FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[9], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[9]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[9]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[9]];
                    }
                }
                else if (iteratorVariable2[9].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name == FengGameManagerMKII.s[18] && iteratorVariable3)
            {
                if (Regex.IsMatch(iteratorVariable2[10], @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)$"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[10]))
                    {
                        WWW iteratorVariable34 = new WWW(iteratorVariable2[10]);
                        yield return iteratorVariable34;
                        Texture2D iteratorVariable35 = RCextensions.loadimage(iteratorVariable34, mipmap, 200000);
                        iteratorVariable34.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[10]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable35;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[10], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[10]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[10]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[10]];
                    }
                }
                else if (iteratorVariable2[10].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[25]))
            {
                if (Regex.IsMatch(iteratorVariable2[11], @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)$"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[11]))
                    {
                        WWW iteratorVariable36 = new WWW(iteratorVariable2[11]);
                        yield return iteratorVariable36;
                        Texture2D iteratorVariable37 = RCextensions.loadimage(iteratorVariable36, mipmap, 200000);
                        iteratorVariable36.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[11]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable37;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[11], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[11]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[11]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[11]];
                    }
                }
                else if (iteratorVariable2[11].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
        }
        if (iteratorVariable4 && horse >= 0)
        {
            GameObject gameObject = PhotonView.Find(horse).gameObject;
            if (gameObject != null)
            {
                foreach (Renderer iteratorVariable39 in gameObject.GetComponentsInChildren<Renderer>())
                {
                    if (iteratorVariable39.name.Contains(FengGameManagerMKII.s[19]))
                    {
                        if (Regex.IsMatch(iteratorVariable2[0], @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)$"))
                        {
                            if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[0]))
                            {
                                WWW iteratorVariable40 = new WWW(iteratorVariable2[0]);
                                yield return iteratorVariable40;
                                Texture2D iteratorVariable41 = RCextensions.loadimage(iteratorVariable40, mipmap, 500000);
                                iteratorVariable40.Dispose();
                                if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[0]))
                                {
                                    iteratorVariable1 = true;
                                    iteratorVariable39.material.mainTexture = iteratorVariable41;
                                    FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[0], iteratorVariable39.material);
                                    iteratorVariable39.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[0]];
                                }
                                else
                                {
                                    iteratorVariable39.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[0]];
                                }
                            }
                            else
                            {
                                iteratorVariable39.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[0]];
                            }
                        }
                        else if (iteratorVariable2[0].ToLower() == "transparent")
                        {
                            iteratorVariable39.enabled = false;
                        }
                    }
                }
            }
        }
        if (iteratorVariable5 && Regex.IsMatch(iteratorVariable2[12], @"^https?:\/\/(?:[a-z0-9\-]+\.)+[a-z]{2,6}(?:\/[^\/#?]+)+\.(?:jpg|gif|png|jpeg)$"))
        {
            if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[12]))
            {
                WWW iteratorVariable42 = new WWW(iteratorVariable2[12]);
                yield return iteratorVariable42;
                Texture2D iteratorVariable43 = RCextensions.loadimage(iteratorVariable42, mipmap, 200000);
                iteratorVariable42.Dispose();
                if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[12]))
                {
                    iteratorVariable1 = true;
                    leftbladetrail.MyMaterial.mainTexture = iteratorVariable43;
                    rightbladetrail.MyMaterial.mainTexture = iteratorVariable43;
                    FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[12], leftbladetrail.MyMaterial);
                    leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    leftbladetrail2.MyMaterial = leftbladetrail.MyMaterial;
                    rightbladetrail2.MyMaterial = leftbladetrail.MyMaterial;
                }
                else
                {
                    leftbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    rightbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                }
            }
            else
            {
                leftbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                rightbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
            }
        }
        if (iteratorVariable1)
        {
            FengGameManagerMKII.instance.unloadAssets();
        }
    }

    [RPC]
    public void loadskinRPC(int horse, string url)
    {
        if ((int) FengGameManagerMKII.settings[0] == 1)
        {
            base.StartCoroutine(LoadSkinHERO(horse, url));
        }
    }

    public void markDie()
    {
        hasDied = true;
        state = HERO_STATE.Die;
    }

    [RPC]
    public void moveToRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            base.transform.position = new Vector3(posX, posY, posZ);
        }
    }

    [RPC]
    private void net3DMGSMOKE(bool ifON)
    {
        if (smoke_3dmg != null)
        {
            smoke_3dmg.enableEmission = ifON;
        }
    }

    [RPC]
    private void netContinueAnimation()
    {
        IEnumerator enumerator = base.animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                if (current == null || current.speed == 1f)
                {
                    return;
                }
                current.speed = 1f;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            disposable?.Dispose();
        }
        playAnimation(GetCurrentPlayingClipName());
    }

    [RPC]
    private void netCrossFade(string aniName, float time)
    {
        currentAnimation = aniName;
        if (base.animation != null)
        {
            base.animation.CrossFade(aniName, time);
        }   
    }

    [RPC]
    public void netDie(Vector3 v, bool isBite, int viewID = -1, string titanName = "", bool killByTitan = true, PhotonMessageInfo info = null)
    {
        if (ModManager.Find("module.immortality").Enabled && base.photonView.isMine) //TODO: Make immortality
        {
            Core.Log("Immortality triggered! (netDie)");
            photonView.RPC("backToHumanRPC", PhotonTargets.Others);
            ungrabbed();
            return;
        }

        if (base.photonView.isMine && info != null && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.BOSS_FIGHT_CT)
        {
            if (FengGameManagerMKII.ignoreList.Contains(info.sender.ID))
            {
                base.photonView.RPC("backToHumanRPC", PhotonTargets.Others, new object[0]);
                return;
            }
            if (!info.sender.isLocal && !info.sender.isMasterClient)
            {
                if (info.sender.Name == null || info.sender.customProperties[PhotonPlayerProperty.isTitan] == null)
                {
                    Core.SendMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
                else if (viewID < 0)
                {
                    if (titanName == "")
                    {
                        Core.SendMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + " (possibly valid).</color>");
                    }
                    else
                    {
                        Core.SendMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                    }
                }
                else if (PhotonView.Find(viewID) == null)
                {
                    Core.SendMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
                else if (PhotonView.Find(viewID).owner.ID != info.sender.ID)
                {
                    Core.SendMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
            }
        }
        if (PhotonNetwork.isMasterClient)
        {
            onDeathEvent(viewID, killByTitan);
            int iD = base.photonView.owner.ID;
            if (FengGameManagerMKII.heroHash.ContainsKey(iD))
            {
                FengGameManagerMKII.heroHash.Remove(iD);
            }
        }
        if (base.photonView.isMine)
        {
            Vector3 vector = Vector3.up * 5000f;
            if (myBomb != null)
            {
                myBomb.destroyMe();
            }
            if (myCannon != null)
            {
                PhotonNetwork.Destroy(myCannon);
            }
            if (titanForm && eren_titan != null)
            {
                eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (skillCD != null)
            {
                skillCD.transform.localPosition = vector;
            }
        }
        if (bulletLeft != null)
        {
            bulletLeft.GetComponent<Bullet>().removeMe();
        }
        if (bulletRight != null)
        {
            bulletRight.GetComponent<Bullet>().removeMe();
        }
        meatDie.Play();
        if (!(useGun || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !base.photonView.isMine))
        {
            leftbladetrail.Deactivate();
            rightbladetrail.Deactivate();
            leftbladetrail2.Deactivate();
            rightbladetrail2.Deactivate();
        }
        FalseAttack();
        BreakApart(v, isBite);
        if (base.photonView.isMine)
        {
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FengGameManagerMKII.instance.myRespawnTime = 0f;
        }
        hasDied = true;
        Transform transform = base.transform.Find("audio_die");
        if (transform != null)
        {
            transform.parent = null;
            transform.GetComponent<AudioSource>().Play();
        }
        gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
        if (base.photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(base.photonView);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.dead, true);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.deaths, RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.deaths]) + 1);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            object[] parameters = new object[] { !(titanName == string.Empty) ? 1 : 0 };
            FengGameManagerMKII.instance.photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, parameters);
            if (viewID != -1)
            {
                PhotonView view2 = PhotonView.Find(viewID);
                if (view2 != null)
                {
                    FengGameManagerMKII.instance.sendKillInfo(killByTitan, "[FFC000][" + info.sender.ID + "][FFFFFF]" + view2.owner.Name, false, PhotonNetwork.player.Name);
                    propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                    propertiesToSet.Add(PhotonPlayerProperty.kills, RCextensions.returnIntFromObject(view2.owner.customProperties[PhotonPlayerProperty.kills]) + 1);
                    view2.owner.SetCustomProperties(propertiesToSet);
                }
            }
            else
            {
                FengGameManagerMKII.instance.sendKillInfo(titanName != string.Empty, "[FFC000][" + info.sender.ID + "][FFFFFF]" + titanName, false, PhotonNetwork.player.Name);
            }
        }
        if (base.photonView.isMine)
        {
            PhotonNetwork.Destroy(base.photonView);
        }
    }

    [RPC]
    private void netDie2(int viewID = -1, string titanName = "", PhotonMessageInfo info = null)
    {
        if (ModManager.Find("module.immortality").Enabled && photonView.isMine) //TODO: Immortality
        {
            Core.Log("Immortality saved you!");
            photonView.RPC("backToHumanRPC", PhotonTargets.Others);
            return;
        }

        GameObject obj2;
        if (base.photonView.isMine && info != null && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.BOSS_FIGHT_CT)
        {
            if (FengGameManagerMKII.ignoreList.Contains(info.sender.ID))
            {
                photonView.RPC("backToHumanRPC", PhotonTargets.Others);
                return;
            }
            if (!info.sender.isLocal && !info.sender.isMasterClient)
            {
                if (info.sender.Name == null || info.sender.customProperties[PhotonPlayerProperty.isTitan] == null)
                {
                    Core.SendMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
                else if (viewID < 0)
                {
                    if (titanName == "")
                    {
                        Core.SendMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + " (possibly valid).</color>");
                    }
                    else if (RCSettings.bombMode == 0 && RCSettings.deadlyCannons == 0)
                    {
                        Core.SendMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                    }
                }
                else if (PhotonView.Find(viewID) == null)
                {
                    Core.SendMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
                else if (PhotonView.Find(viewID).owner.ID != info.sender.ID)
                {
                    Core.SendMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
            }
        }
        if (base.photonView.isMine)
        {
            Vector3 vector = Vector3.up * 5000f;
            if (myBomb != null)
            {
                myBomb.destroyMe();
            }
            if (myCannon != null)
            {
                PhotonNetwork.Destroy(myCannon);
            }
            PhotonNetwork.RemoveRPCs(base.photonView);
            if (titanForm && eren_titan != null)
            {
                eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (skillCD != null)
            {
                skillCD.transform.localPosition = vector;
            }
        }
        meatDie.Play();
        if (bulletLeft != null)
        {
            bulletLeft.GetComponent<Bullet>().removeMe();
        }
        if (bulletRight != null)
        {
            bulletRight.GetComponent<Bullet>().removeMe();
        }
        Transform transform = base.transform.Find("audio_die");
        transform.parent = null;
        transform.GetComponent<AudioSource>().Play();
        if (base.photonView.isMine)
        {
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FengGameManagerMKII.instance.myRespawnTime = 0f;
        }
        FalseAttack();
        hasDied = true;
        gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(base.photonView);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.dead, true);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.deaths, (int) PhotonNetwork.player.customProperties[PhotonPlayerProperty.deaths] + 1);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            if (viewID != -1)
            {
                PhotonView view2 = PhotonView.Find(viewID);
                if (view2 != null)
                {
                    FengGameManagerMKII.instance.sendKillInfo(true, "[FFC000][" + info.sender.ID.ToString() + "][FFFFFF]" + RCextensions.returnStringFromObject(view2.owner.Name), false, RCextensions.returnStringFromObject(PhotonNetwork.player.Name), 0);
                    propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                    propertiesToSet.Add(PhotonPlayerProperty.kills, RCextensions.returnIntFromObject(view2.owner.customProperties[PhotonPlayerProperty.kills]) + 1);
                    view2.owner.SetCustomProperties(propertiesToSet);
                }
            }
            else
            {
                FengGameManagerMKII.instance.sendKillInfo(true, "[FFC000][" + info.sender.ID.ToString() + "][FFFFFF]" + titanName, false, RCextensions.returnStringFromObject(PhotonNetwork.player.Name), 0);
            }
            object[] parameters = new object[] { !(titanName == string.Empty) ? 1 : 0 };
            FengGameManagerMKII.instance.photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, parameters);
        }
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && base.photonView.isMine)
        {
            obj2 = PhotonNetwork.Instantiate("hitMeat2", base.transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
        }
        else
        {
            obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("hitMeat2"));
        }
        obj2.transform.position = base.transform.position;
        if (base.photonView.isMine)
        {
            PhotonNetwork.Destroy(base.photonView);
        }
        if (PhotonNetwork.isMasterClient)
        {
            onDeathEvent(viewID, true);
            int iD = base.photonView.owner.ID;
            if (FengGameManagerMKII.heroHash.ContainsKey(iD))
            {
                FengGameManagerMKII.heroHash.Remove(iD);
            }
        }
    }

    public void netDieLocal(Vector3 v, bool isBite, int viewID = -1, string titanName = "", bool killByTitan = true)
    {
        if (base.photonView.isMine)
        {
            Vector3 vector = Vector3.up * 5000f;
            if (titanForm && eren_titan != null)
            {
                eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (myBomb != null)
            {
                myBomb.destroyMe();
            }
            if (myCannon != null)
            {
                PhotonNetwork.Destroy(myCannon);
            }
            if (skillCD != null)
            {
                skillCD.transform.localPosition = vector;
            }
        }
        if (bulletLeft != null)
        {
            bulletLeft.GetComponent<Bullet>().removeMe();
        }
        if (bulletRight != null)
        {
            bulletRight.GetComponent<Bullet>().removeMe();
        }
        meatDie.Play();
        if (!(useGun || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !base.photonView.isMine))
        {
            leftbladetrail.Deactivate();
            rightbladetrail.Deactivate();
            leftbladetrail2.Deactivate();
            rightbladetrail2.Deactivate();
        }
        FalseAttack();
        BreakApart(v, isBite);
        if (base.photonView.isMine)
        {
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FengGameManagerMKII.instance.myRespawnTime = 0f;
        }
        hasDied = true;
        Transform transform = base.transform.Find("audio_die");
        transform.parent = null;
        transform.GetComponent<AudioSource>().Play();
        gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
        if (base.photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(base.photonView);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.dead, true);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.deaths, RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.deaths]) + 1);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            object[] parameters = new object[] { !(titanName == string.Empty) ? 1 : 0 };
            FengGameManagerMKII.instance.photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, parameters);
            if (viewID != -1)
            {
                PhotonView view = PhotonView.Find(viewID);
                if (view != null)
                {
                    FengGameManagerMKII.instance.sendKillInfo(killByTitan, view.owner.Name, false, PhotonNetwork.player.Name, 0);
                    propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                    propertiesToSet.Add(PhotonPlayerProperty.kills, RCextensions.returnIntFromObject(view.owner.customProperties[PhotonPlayerProperty.kills]) + 1);
                    view.owner.SetCustomProperties(propertiesToSet);
                }
            }
            else
            {
                FengGameManagerMKII.instance.sendKillInfo(!(titanName == string.Empty), titanName, false, RCextensions.returnStringFromObject(PhotonNetwork.player.Name), 0);
            }
        }
        if (base.photonView.isMine)
        {
            PhotonNetwork.Destroy(base.photonView);
        }
        if (PhotonNetwork.isMasterClient)
        {
            onDeathEvent(viewID, killByTitan);
            int iD = base.photonView.owner.ID;
            if (FengGameManagerMKII.heroHash.ContainsKey(iD))
            {
                FengGameManagerMKII.heroHash.Remove(iD);
            }
        }
    }

    [RPC]
    private void netGrabbed(int titanId, bool leftHand)
    {
        if (ModManager.Find("module.antigrab").Enabled)
        {
            Core.Log("You got grabbed by a titan. Ungrabbing...");
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                photonView.RPC("netSetIsGrabbedFalse", PhotonTargets.All);
            PhotonView.Find(titanId).RPC("grabbedTargetEscape", PhotonTargets.MasterClient);
            return;
        }

        titanWhoGrabMeID = titanId;
        grabbed(PhotonView.Find(titanId).gameObject, leftHand);
    }

    [RPC]
    private void netlaughAttack()
    {
        foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
        {
            if (Vector3.Distance(obj2.transform.position, base.transform.position) < 50f && Vector3.Angle(obj2.transform.forward, base.transform.position - obj2.transform.position) < 90f && obj2.GetComponent<TITAN>() != null)
            {
                obj2.GetComponent<TITAN>().beLaughAttacked();
            }
        }
    }

    [RPC]
    private void netPauseAnimation()
    {
        IEnumerator enumerator = base.animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                current.speed = 0f;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
            	disposable.Dispose();
            }
        }
    }

    [RPC]
    private void netPlayAnimation(string aniName)
    {
        currentAnimation = aniName;
        if (base.animation != null)
        {
            base.animation.Play(aniName);
        }
    }

    [RPC]
    private void netPlayAnimationAt(string aniName, float normalizedTime)
    {
        currentAnimation = aniName;
        if (base.animation != null)
        {
            base.animation.Play(aniName);
            base.animation[aniName].normalizedTime = normalizedTime;
        }
    }

    [RPC]
    private void netSetIsGrabbedFalse()
    {
        state = HERO_STATE.Idle;
    }

    [RPC]
    private void netTauntAttack(float tauntTime, float distance = 100f)
    {
        foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
        {
            if (Vector3.Distance(obj2.transform.position, base.transform.position) < distance && obj2.GetComponent<TITAN>() != null)
            {
                obj2.GetComponent<TITAN>().beTauntedBy(gameObject, tauntTime);
            }
        }
    }

    [RPC]
    private void netUngrabbed()
    {
        ungrabbed();
        netPlayAnimation(standAnimation);
        FalseAttack();
    }

    public void onDeathEvent(int viewID, bool isTitan)
    {
        RCEvent event2;
        string[] strArray;
        if (isTitan)
        {
            if (FengGameManagerMKII.RCEvents.ContainsKey("OnPlayerDieByTitan"))
            {
                event2 = (RCEvent) FengGameManagerMKII.RCEvents["OnPlayerDieByTitan"];
                strArray = (string[]) FengGameManagerMKII.RCVariableNames["OnPlayerDieByTitan"];
                if (FengGameManagerMKII.playerVariables.ContainsKey(strArray[0]))
                {
                    FengGameManagerMKII.playerVariables[strArray[0]] = base.photonView.owner;
                }
                else
                {
                    FengGameManagerMKII.playerVariables.Add(strArray[0], base.photonView.owner);
                }
                if (FengGameManagerMKII.titanVariables.ContainsKey(strArray[1]))
                {
                    FengGameManagerMKII.titanVariables[strArray[1]] = PhotonView.Find(viewID).gameObject.GetComponent<TITAN>();
                }
                else
                {
                    FengGameManagerMKII.titanVariables.Add(strArray[1], PhotonView.Find(viewID).gameObject.GetComponent<TITAN>());
                }
                event2.checkEvent();
            }
        }
        else if (FengGameManagerMKII.RCEvents.ContainsKey("OnPlayerDieByPlayer"))
        {
            event2 = (RCEvent) FengGameManagerMKII.RCEvents["OnPlayerDieByPlayer"];
            strArray = (string[]) FengGameManagerMKII.RCVariableNames["OnPlayerDieByPlayer"];
            if (FengGameManagerMKII.playerVariables.ContainsKey(strArray[0]))
            {
                FengGameManagerMKII.playerVariables[strArray[0]] = base.photonView.owner;
            }
            else
            {
                FengGameManagerMKII.playerVariables.Add(strArray[0], base.photonView.owner);
            }
            if (FengGameManagerMKII.playerVariables.ContainsKey(strArray[1]))
            {
                FengGameManagerMKII.playerVariables[strArray[1]] = PhotonView.Find(viewID).owner;
            }
            else
            {
                FengGameManagerMKII.playerVariables.Add(strArray[1], PhotonView.Find(viewID).owner);
            }
            event2.checkEvent();
        }
    }

    private void OnDestroy()
    {
        if (myNetWorkName != null)
        {
            Destroy(myNetWorkName);
        }
        if (gunDummy != null)
        {
            Destroy(gunDummy);
        }
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
            releaseIfIHookSb();
        }
        if (GameObject.Find("MultiplayerManager") != null)
        {
            FengGameManagerMKII.instance.removeHero(this);
        }
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
        {
            Vector3 vector = Vector3.up * 5000f;
            cross1.transform.localPosition = vector;
            cross2.transform.localPosition = vector;
            crossL1.transform.localPosition = vector;
            crossL2.transform.localPosition = vector;
            crossR1.transform.localPosition = vector;
            crossR2.transform.localPosition = vector;
            LabelDistance.transform.localPosition = vector;
        }
        if (setup.part_cape != null)
        {
            ClothFactory.DisposeObject(setup.part_cape);
        }
        if (setup.part_hair_1 != null)
        {
            ClothFactory.DisposeObject(setup.part_hair_1);
        }
        if (setup.part_hair_2 != null)
        {
            ClothFactory.DisposeObject(setup.part_hair_2);
        }
    }

    public void pauseAnimation()
    {
        IEnumerator enumerator = base.animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                if (current != null)
                    current.speed = 0f;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
            	disposable.Dispose();
            }
        }
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && base.photonView.isMine)
        {
            base.photonView.RPC("netPauseAnimation", PhotonTargets.Others, new object[0]);
        }
    }

    public void playAnimation(string aniName)
    {
        currentAnimation = aniName;
        base.animation.Play(aniName);
        if (PhotonNetwork.connected && base.photonView.isMine)
        {
            object[] parameters = new object[] { aniName };
            base.photonView.RPC("netPlayAnimation", PhotonTargets.Others, parameters);
        }
    }

    private void playAnimationAt(string aniName, float normalizedTime)
    {
        currentAnimation = aniName;
        base.animation.Play(aniName);
        base.animation[aniName].normalizedTime = normalizedTime;
        if (PhotonNetwork.connected && base.photonView.isMine)
        {
            object[] parameters = new object[] { aniName, normalizedTime };
            base.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, parameters);
        }
    }

    private void releaseIfIHookSb()
    {
        if (hookSomeOne && hookTarget != null)
        {
            hookTarget.GetPhotonView().RPC("badGuyReleaseMe", hookTarget.GetPhotonView().owner, new object[0]);
            hookTarget = null;
            hookSomeOne = false;
        }
    }

    public IEnumerator reloadSky()
    {
        yield return new WaitForSeconds(0.5f);
        if (FengGameManagerMKII.skyMaterial != null && Camera.main.GetComponent<Skybox>().material != FengGameManagerMKII.skyMaterial)
        {
            Camera.main.GetComponent<Skybox>().material = FengGameManagerMKII.skyMaterial;
        }
    }

    public void resetAnimationSpeed()
    {
        IEnumerator enumerator = base.animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                if (current != null)
                    current.speed = 1f;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
            	disposable.Dispose();
            }
        }
        SetAnimationSpeed();
    }


    [RPC]
    public void ReturnFromCannon(PhotonMessageInfo info)
    {
        if (info.sender == base.photonView.owner)
        {
            isCannon = false;
            gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
        }
    }

    private void rightArmAimTo(Vector3 target)
    {
        float y = target.x - upperarmR.transform.position.x;
        float num2 = target.y - upperarmR.transform.position.y;
        float x = target.z - upperarmR.transform.position.z;
        float num4 = Mathf.Sqrt(y * y + x * x);
        handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        forearmR.localRotation = Quaternion.Euler(90f, 0f, 0f);
        upperarmR.rotation = Quaternion.Euler(180f, 90f + Mathf.Atan2(y, x) * 57.29578f, Mathf.Atan2(num2, num4) * 57.29578f);
    }

    [RPC]
    private void RPCHookedByHuman(int hooker, Vector3 hookPosition)
    {
        hookBySomeOne = true;
        badGuy = PhotonView.Find(hooker).gameObject;
        if (Vector3.Distance(hookPosition, base.transform.position) < 15f)
        {
            launchForce = PhotonView.Find(hooker).gameObject.transform.position - base.transform.position;
            rigidbody.AddForce(-rigidbody.velocity * 0.9f, ForceMode.VelocityChange);
            float num = Mathf.Pow(launchForce.magnitude, 0.1f);
            if (grounded)
            {
                rigidbody.AddForce(Vector3.up * Mathf.Min((float)(launchForce.magnitude * 0.2f), (float)10f), ForceMode.Impulse);
            }
            rigidbody.AddForce(launchForce * num * 0.1f, ForceMode.Impulse);
            if (state != HERO_STATE.Grab)
            {
                dashTime = 1f;
                CrossFade("dash", 0.05f);
                base.animation["dash"].time = 0.1f;
                state = HERO_STATE.AirDodge;
                FalseAttack();
                facingDirection = Mathf.Atan2(launchForce.x, launchForce.z) * 57.29578f;
                Quaternion quaternion = Quaternion.Euler(0f, facingDirection, 0f);
                gameObject.transform.rotation = quaternion;
                rigidbody.rotation = quaternion;
                targetRotation = quaternion;
            }
        }
        else
        {
            hookBySomeOne = false;
            badGuy = null;
            PhotonView.Find(hooker).RPC("hookFail", PhotonView.Find(hooker).owner, new object[0]);
        }
    }

    private void salute()
    {
        state = HERO_STATE.Salute;
        CrossFade("salute", 0.1f);
    }

    private void setHookedPplDirection()
    {
        almostSingleHook = false;
        if (isRightHandHooked && isLeftHandHooked)
        {
            if (bulletLeft != null && bulletRight != null)
            {
                Vector3 normal = bulletLeft.transform.position - bulletRight.transform.position;
                if (normal.sqrMagnitude < 4f)
                {
                    Vector3 vector2 = (bulletLeft.transform.position + bulletRight.transform.position) * 0.5f - base.transform.position;
                    facingDirection = Mathf.Atan2(vector2.x, vector2.z) * 57.29578f;
                    if (useGun && state != HERO_STATE.Attack)
                    {
                        float current = -Mathf.Atan2(rigidbody.velocity.z, rigidbody.velocity.x) * 57.29578f;
                        float target = -Mathf.Atan2(vector2.z, vector2.x) * 57.29578f;
                        float num3 = -Mathf.DeltaAngle(current, target);
                        facingDirection += num3;
                    }
                    almostSingleHook = true;
                }
                else
                {
                    Vector3 to = base.transform.position - bulletLeft.transform.position;
                    Vector3 vector6 = base.transform.position - bulletRight.transform.position;
                    Vector3 vector7 = (bulletLeft.transform.position + bulletRight.transform.position) * 0.5f;
                    Vector3 from = base.transform.position - vector7;
                    if (Vector3.Angle(@from, to) < 30f && Vector3.Angle(@from, vector6) < 30f)
                    {
                        almostSingleHook = true;
                        Vector3 vector9 = vector7 - base.transform.position;
                        facingDirection = Mathf.Atan2(vector9.x, vector9.z) * 57.29578f;
                    }
                    else
                    {
                        almostSingleHook = false;
                        Vector3 forward = base.transform.forward;
                        Vector3.OrthoNormalize(ref normal, ref forward);
                        facingDirection = Mathf.Atan2(forward.x, forward.z) * 57.29578f;
                        float num4 = Mathf.Atan2(to.x, to.z) * 57.29578f;
                        if (Mathf.DeltaAngle(num4, facingDirection) > 0f)
                        {
                            facingDirection += 180f;
                        }
                    }
                }
            }
        }
        else
        {
            almostSingleHook = true;
            Vector3 zero = Vector3.zero;
            if (isRightHandHooked && bulletRight != null)
            {
                zero = bulletRight.transform.position - base.transform.position;
            }
            else
            {
                if (!isLeftHandHooked || bulletLeft == null)
                {
                    return;
                }
                zero = bulletLeft.transform.position - base.transform.position;
            }
            facingDirection = Mathf.Atan2(zero.x, zero.z) * 57.29578f;
            if (state != HERO_STATE.Attack)
            {
                float num6 = -Mathf.Atan2(rigidbody.velocity.z, rigidbody.velocity.x) * 57.29578f;
                float num7 = -Mathf.Atan2(zero.z, zero.x) * 57.29578f;
                float num8 = -Mathf.DeltaAngle(num6, num7);
                if (useGun)
                {
                    facingDirection += num8;
                }
                else
                {
                    float num9 = 0f;
                    if (isLeftHandHooked && num8 < 0f || isRightHandHooked && num8 > 0f)
                    {
                        num9 = -0.1f;
                    }
                    else
                    {
                        num9 = 0.1f;
                    }
                    facingDirection += num8 * num9;
                }
            }
        }
    }

    [RPC]
    public void SetMyCannon(int viewID, PhotonMessageInfo info)
    {
        if (info.sender == base.photonView.owner)
        {
            PhotonView view = PhotonView.Find(viewID);
            if (view != null)
            {
                myCannon = view.gameObject;
                if (myCannon != null)
                {
                    myCannonBase = myCannon.transform;
                    myCannonPlayer = myCannonBase.Find("PlayerPoint");
                    isCannon = true;
                }
            }
        }
    }

    [RPC]
    public void SetMyPhotonCamera(float offset, PhotonMessageInfo info)
    {
        if (base.photonView.owner == info.sender)
        {
            CameraMultiplier = offset;
            base.GetComponent<SmoothSyncMovement>().PhotonCamera = true;
            isPhotonCamera = true;
        }
    }

    [RPC]
    private void setMyTeam(int val) //BUG: NullReferenceException UnityEngine.GameObject.GetComponent[TriggerColliderWeapon] 
    {
        myTeam = val;
        if (checkBoxLeft?.GetComponent<TriggerColliderWeapon>() != null)
            checkBoxLeft.GetComponent<TriggerColliderWeapon>().myTeam = val;
        if (checkBoxRight?.GetComponent<TriggerColliderWeapon>() != null)
            checkBoxRight.GetComponent<TriggerColliderWeapon>().myTeam = val;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
        {
            object[] objArray;
            if (RCSettings.friendlyMode > 0)
            {
                if (val != 1)
                {
                    objArray = new object[] { 1 };
                    base.photonView.RPC("setMyTeam", PhotonTargets.AllBuffered, objArray);
                }
            }
            else if (RCSettings.pvpMode == 1)
            {
                int num = 0;
                if (base.photonView.owner.customProperties[PhotonPlayerProperty.RCteam] != null)
                {
                    num = RCextensions.returnIntFromObject(base.photonView.owner.customProperties[PhotonPlayerProperty.RCteam]);
                }
                if (val != num)
                {
                    objArray = new object[] { num };
                    base.photonView.RPC("setMyTeam", PhotonTargets.AllBuffered, objArray);
                }
            }
            else if (RCSettings.pvpMode == 2 && val != base.photonView.owner.ID)
            {
                objArray = new object[] { base.photonView.owner.ID };
                base.photonView.RPC("setMyTeam", PhotonTargets.AllBuffered, objArray);
            }
        }
    }

    public void setSkillHUDPosition()
    {
        skillCD = GameObject.Find("skill_cd_" + skillId);
        if (skillCD != null)
        {
            skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        }
        if (useGun)
        {
            skillCD.transform.localPosition = Vector3.up * 5000f;
        }
    }

    public void setSkillHUDPosition2()
    {
        skillCD = GameObject.Find("skill_cd_" + skillIDHUD);
        if (skillCD != null)
        {
            skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        }
        if (useGun && RCSettings.bombMode == 0)
        {
            skillCD.transform.localPosition = Vector3.up * 5000f;
        }
    }

    public void setStat()
    {
        skillCDLast = 1.5f;
        skillId = setup.myCostume.Stat.skillId;
        if (skillId == "levi")
        {
            skillCDLast = 3.5f;
        }
        SetAnimationSpeed();
        if (skillId == "armin")
        {
            skillCDLast = 5f;
        }
        if (skillId == "marco")
        {
            skillCDLast = 10f;
        }
        if (skillId == "jean")
        {
            skillCDLast = 0.001f;
        }
        if (skillId == "eren")
        {
            skillCDLast = 120f;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            {
                if (!LevelInfo.GetInfo(FengGameManagerMKII.level).teamTitan && LevelInfo.GetInfo(FengGameManagerMKII.level).type != GAMEMODE.RACING && LevelInfo.GetInfo(FengGameManagerMKII.level).type != GAMEMODE.PVP_CAPTURE && LevelInfo.GetInfo(FengGameManagerMKII.level).type != GAMEMODE.TROST)
                {
                    int num = 0;
                    foreach (PhotonPlayer player in PhotonNetwork.playerList)
                    {
                        if ((int) player.customProperties[PhotonPlayerProperty.isTitan] == 1 && ((string) player.customProperties[PhotonPlayerProperty.character]).ToUpper() == "EREN")
                        {
                            num++;
                        }
                    }
                    if (num > 1)
                    {
                        skillId = "petra";
                        skillCDLast = 1f;
                    }
                }
                else
                {
                    skillId = "petra";
                    skillCDLast = 1f;
                }
            }
        }
        if (skillId == "sasha")
        {
            skillCDLast = 20f;
        }
        if (skillId == "petra")
        {
            skillCDLast = 3.5f;
        }
        skillCDDuration = skillCDLast;
        speed = setup.myCostume.Stat.SPD / 10f;
        totalGas = currentGas = setup.myCostume.Stat.GAS;
        totalBladeSta = currentBladeSta = setup.myCostume.Stat.BLA;
        rigidbody.mass = 0.5f - (setup.myCostume.Stat.ACL - 100) * 0.001f;
        GameObject.Find("skill_cd_bottom").transform.localPosition = new Vector3(0f, -Screen.height * 0.5f + 5f, 0f);
        skillCD = GameObject.Find("skill_cd_" + skillId);
        skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        GameObject.Find("GasUI").transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
        {
            GameObject.Find("bulletL").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL1").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR1").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL2").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR2").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL3").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR3").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL4").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR4").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL5").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR5").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL6").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR6").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL7").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR7").GetComponent<UISprite>().enabled = false;
        }
        if (setup.myCostume.UniformType == UniformType.CasualAHSS)
        {
            standAnimation = "AHSS_stand_gun";
            useGun = true;
            gunDummy = new GameObject();
            gunDummy.name = "gunDummy";
            gunDummy.transform.position = base.transform.position;
            gunDummy.transform.rotation = base.transform.rotation;
            myGroup = GROUP.A;
            setTeam(2);
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
            {
                GameObject.Find("bladeCL").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladeCR").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel1").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader1").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel2").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader2").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel3").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader3").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel4").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader4").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel5").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader5").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bulletL").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL1").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR1").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL2").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR2").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL3").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR3").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL4").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR4").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL5").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR5").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL6").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR6").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL7").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR7").GetComponent<UISprite>().enabled = true;
                skillCD.transform.localPosition = Vector3.up * 5000f;
            }
        }
        else if (setup.myCostume.Sex == Sex.Female)
        {
            standAnimation = "stand";
            setTeam(1);
        }
        else
        {
            standAnimation = "stand_levi";
            setTeam(1);
        }
    }

    public void setStat2()
    {
        skillCDLast = 1.5f;
        skillId = setup.myCostume.Stat.skillId;
        if (skillId == "levi")
        {
            skillCDLast = 3.5f;
        }
        SetAnimationSpeed();
        if (skillId == "armin")
        {
            skillCDLast = 5f;
        }
        if (skillId == "marco")
        {
            skillCDLast = 10f;
        }
        if (skillId == "jean")
        {
            skillCDLast = 0.001f;
        }
        if (skillId == "eren")
        {
            skillCDLast = 120f;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            {
                if (LevelInfo.GetInfo(FengGameManagerMKII.level).teamTitan || LevelInfo.GetInfo(FengGameManagerMKII.level).type == GAMEMODE.RACING || LevelInfo.GetInfo(FengGameManagerMKII.level).type == GAMEMODE.PVP_CAPTURE || LevelInfo.GetInfo(FengGameManagerMKII.level).type == GAMEMODE.TROST)
                {
                    skillId = "petra";
                    skillCDLast = 1f;
                }
                else
                {
                    int num = 0;
                    foreach (PhotonPlayer player in PhotonNetwork.playerList)
                    {
                        if (RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.isTitan]) == 1 && RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.character]).ToUpper() == "EREN")
                        {
                            num++;
                        }
                    }
                    if (num > 1)
                    {
                        skillId = "petra";
                        skillCDLast = 1f;
                    }
                }
            }
        }
        if (skillId == "sasha")
        {
            skillCDLast = 20f;
        }
        if (skillId == "petra")
        {
            skillCDLast = 3.5f;
        }
        BombInit();
        speed = setup.myCostume.Stat.SPD / 10f;
        totalGas = currentGas = setup.myCostume.Stat.GAS;
        totalBladeSta = currentBladeSta = setup.myCostume.Stat.BLA;
        baseRigidBody.mass = 0.5f - (setup.myCostume.Stat.ACL - 100) * 0.001f;
        GameObject.Find("skill_cd_bottom").transform.localPosition = new Vector3(0f, -Screen.height * 0.5f + 5f, 0f);
        skillCD = GameObject.Find("skill_cd_" + skillIDHUD);
        skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        GameObject.Find("GasUI").transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
        {
            GameObject.Find("bulletL").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL1").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR1").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL2").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR2").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL3").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR3").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL4").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR4").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL5").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR5").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL6").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR6").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL7").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR7").GetComponent<UISprite>().enabled = false;
        }
        if (setup.myCostume.UniformType == UniformType.CasualAHSS)
        {
            standAnimation = "AHSS_stand_gun";
            useGun = true;
            gunDummy = new GameObject();
            gunDummy.name = "gunDummy";
            gunDummy.transform.position = baseTransform.position;
            gunDummy.transform.rotation = baseTransform.rotation;
            myGroup = GROUP.A;
            setTeam2(2);
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
            {
                GameObject.Find("bladeCL").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladeCR").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel1").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader1").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel2").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader2").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel3").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader3").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel4").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader4").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel5").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader5").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bulletL").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL1").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR1").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL2").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR2").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL3").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR3").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL4").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR4").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL5").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR5").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL6").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR6").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL7").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR7").GetComponent<UISprite>().enabled = true;
                if (skillId != "bomb")
                {
                    skillCD.transform.localPosition = Vector3.up * 5000f;
                }
            }
        }
        else if (setup.myCostume.Sex == Sex.Female)
        {
            standAnimation = "stand";
            setTeam2(1);
        }
        else
        {
            standAnimation = "stand_levi";
            setTeam2(1);
        }
    }

    public void setTeam(int team)
    {
        setMyTeam(team);
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
        {
            object[] parameters = new object[] { team };
            base.photonView.RPC("setMyTeam", PhotonTargets.OthersBuffered, parameters);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.team, team);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }
    }

    public void setTeam2(int team)
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
        {
            object[] parameters = new object[] { team };
            base.photonView.RPC("setMyTeam", PhotonTargets.AllBuffered, parameters);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.team, team);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }
        else
        {
            setMyTeam(team);
        }
    }

    public void shootFlare(int type)
    {
        bool flag = false;
        if (type == 1 && flare1CD == 0f  || ModManager.Find("module.ignorecountdown").Enabled)
        {
            flare1CD = flareTotalCD;
            flag = true;
        }
        if (type == 2 && flare2CD == 0f || ModManager.Find("module.ignorecountdown").Enabled)
        {
            flare2CD = flareTotalCD;
            flag = true;
        }
        if (type == 3 && flare3CD == 0f || ModManager.Find("module.ignorecountdown").Enabled)
        {
            flare3CD = flareTotalCD;
            flag = true;
        }
        if (flag)
        {
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                GameObject obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("FX/flareBullet" + type), base.transform.position, base.transform.rotation);
                obj2.GetComponent<FlareMovement>().dontShowHint();
                UnityEngine.Object.Destroy(obj2, 25f);
            }
            else
            {
                PhotonNetwork.Instantiate("FX/flareBullet" + type, base.transform.position, base.transform.rotation, 0).GetComponent<FlareMovement>().dontShowHint();
            }
        }
    }

    private void showAimUI()
    {
        Vector3 vector;
        if (Screen.showCursor)
        {
            GameObject obj2 = GameObject.Find("cross1");
            GameObject obj3 = GameObject.Find("cross2");
            GameObject obj4 = GameObject.Find("crossL1");
            GameObject obj5 = GameObject.Find("crossL2");
            GameObject obj6 = GameObject.Find("crossR1");
            GameObject obj7 = GameObject.Find("crossR2");
            GameObject obj8 = GameObject.Find("LabelDistance");
            vector = Vector3.up * 10000f;
            obj7.transform.localPosition = vector;
            obj6.transform.localPosition = vector;
            obj5.transform.localPosition = vector;
            obj4.transform.localPosition = vector;
            obj8.transform.localPosition = vector;
            obj3.transform.localPosition = vector;
            obj2.transform.localPosition = vector;
        }
        else
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = mask2 | mask;
            if (Physics.Raycast(ray, out hit, 1E+07f, mask3.value))
            {
                RaycastHit hit2;
                GameObject obj9 = GameObject.Find("cross1");
                GameObject obj10 = GameObject.Find("cross2");
                obj9.transform.localPosition = Input.mousePosition;
                Transform transform = obj9.transform;
                transform.localPosition -= new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                obj10.transform.localPosition = obj9.transform.localPosition;
                vector = hit.point - base.transform.position;
                float magnitude = vector.magnitude;
                GameObject obj11 = GameObject.Find("LabelDistance");
                string str = magnitude <= 1000f ? ((int) magnitude).ToString() : "???";
                obj11.GetComponent<UILabel>().text = str;
                if (magnitude > 120f)
                {
                    Transform transform2 = obj9.transform;
                    transform2.localPosition += Vector3.up * 10000f;
                    obj11.transform.localPosition = obj10.transform.localPosition;
                }
                else
                {
                    Transform transform3 = obj10.transform;
                    transform3.localPosition += Vector3.up * 10000f;
                    obj11.transform.localPosition = obj9.transform.localPosition;
                }
                Transform transform4 = obj11.transform;
                transform4.localPosition -= new Vector3(0f, 15f, 0f);
                Vector3 vector2 = new Vector3(0f, 0.4f, 0f);
                vector2 -= base.transform.right * 0.3f;
                Vector3 vector3 = new Vector3(0f, 0.4f, 0f);
                vector3 += base.transform.right * 0.3f;
                float num3 = hit.distance <= 50f ? hit.distance * 0.05f : hit.distance * 0.3f;
                Vector3 vector4 = hit.point - base.transform.right * num3 - (base.transform.position + vector2);
                Vector3 vector5 = hit.point + base.transform.right * num3 - (base.transform.position + vector3);
                vector4.Normalize();
                vector5.Normalize();
                vector4 = vector4 * 1000000f;
                vector5 = vector5 * 1000000f;
                if (Physics.Linecast(base.transform.position + vector2, base.transform.position + vector2 + vector4, out hit2, mask3.value))
                {
                    GameObject obj12 = GameObject.Find("crossL1");
                    obj12.transform.localPosition = currentCamera.WorldToScreenPoint(hit2.point);
                    Transform transform5 = obj12.transform;
                    transform5.localPosition -= new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                    obj12.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(obj12.transform.localPosition.y - (Input.mousePosition.y - Screen.height * 0.5f), obj12.transform.localPosition.x - (Input.mousePosition.x - Screen.width * 0.5f)) * 57.29578f + 180f);
                    GameObject obj13 = GameObject.Find("crossL2");
                    obj13.transform.localPosition = obj12.transform.localPosition;
                    obj13.transform.localRotation = obj12.transform.localRotation;
                    if (hit2.distance > 120f)
                    {
                        Transform transform6 = obj12.transform;
                        transform6.localPosition += Vector3.up * 10000f;
                    }
                    else
                    {
                        Transform transform7 = obj13.transform;
                        transform7.localPosition += Vector3.up * 10000f;
                    }
                }
                if (Physics.Linecast(base.transform.position + vector3, base.transform.position + vector3 + vector5, out hit2, mask3.value))
                {
                    GameObject obj14 = GameObject.Find("crossR1");
                    obj14.transform.localPosition = currentCamera.WorldToScreenPoint(hit2.point);
                    Transform transform8 = obj14.transform;
                    transform8.localPosition -= new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                    obj14.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(obj14.transform.localPosition.y - (Input.mousePosition.y - Screen.height * 0.5f), obj14.transform.localPosition.x - (Input.mousePosition.x - Screen.width * 0.5f)) * 57.29578f);
                    GameObject obj15 = GameObject.Find("crossR2");
                    obj15.transform.localPosition = obj14.transform.localPosition;
                    obj15.transform.localRotation = obj14.transform.localRotation;
                    if (hit2.distance > 120f)
                    {
                        Transform transform9 = obj14.transform;
                        transform9.localPosition += Vector3.up * 10000f;
                    }
                    else
                    {
                        Transform transform10 = obj15.transform;
                        transform10.localPosition += Vector3.up * 10000f;
                    }
                }
            }
        }
    }

    private void showAimUI2()
    {
        Vector3 vector;
        if (Screen.showCursor)
        {
            GameObject obj2 = cross1;
            GameObject obj3 = cross2;
            GameObject obj4 = crossL1;
            GameObject obj5 = crossL2;
            GameObject obj6 = crossR1;
            GameObject obj7 = crossR2;
            GameObject labelDistance = LabelDistance;
            vector = Vector3.up * 10000f;
            obj7.transform.localPosition = vector;
            obj6.transform.localPosition = vector;
            obj5.transform.localPosition = vector;
            obj4.transform.localPosition = vector;
            labelDistance.transform.localPosition = vector;
            obj3.transform.localPosition = vector;
            obj2.transform.localPosition = vector;
        }
        else
        {
            CheckTitan();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = mask2 | mask;
            if (Physics.Raycast(ray, out RaycastHit hit, 1E+7f, mask3.value))
            {
                GameObject obj9 = cross1;
                GameObject obj10 = cross2;
                obj9.transform.localPosition = Input.mousePosition;
                Transform transform = obj9.transform;
                transform.localPosition -= new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                obj10.transform.localPosition = obj9.transform.localPosition;
                vector = hit.point - baseTransform.position;
                float magnitude = vector.magnitude;
                GameObject obj11 = LabelDistance;
                string str = magnitude <= 10000f ? ((int) magnitude).ToString() : "???"; //MOD: Changed the hook (label) distance from 1000f to 10000f
                if ((int) FengGameManagerMKII.settings[189] == 1)
                {
                    str = str + "\n" + currentSpeed.ToString("F1") + " u/s";
                }
                else if ((int) FengGameManagerMKII.settings[189] == 2)
                {
                    str = str + "\n" + (currentSpeed / 100f).ToString("F1") + "K";
                }
                obj11.GetComponent<UILabel>().text = str;
                if (magnitude > 120f)
                {
                    Transform transform11 = obj9.transform;
                    transform11.localPosition += Vector3.up * 10000f;
                    obj11.transform.localPosition = obj10.transform.localPosition;
                }
                else
                {
                    Transform transform12 = obj10.transform;
                    transform12.localPosition += Vector3.up * 10000f;
                    obj11.transform.localPosition = obj9.transform.localPosition;
                }
                Transform transform13 = obj11.transform;
                transform13.localPosition -= new Vector3(0f, 15f, 0f);
                Vector3 vector2 = new Vector3(0f, 0.4f, 0f);
                vector2 -= baseTransform.right * 0.3f;
                Vector3 vector3 = new Vector3(0f, 0.4f, 0f);
                vector3 += baseTransform.right * 0.3f;
                float num4 = hit.distance <= 50f ? hit.distance * 0.05f : hit.distance * 0.3f;
                Vector3 vector4 = hit.point - baseTransform.right * num4 - (baseTransform.position + vector2);
                Vector3 vector5 = hit.point + baseTransform.right * num4 - (baseTransform.position + vector3);
                vector4.Normalize();
                vector5.Normalize();
                vector4 = vector4 * 1000000f;
                vector5 = vector5 * 1000000f;
                if (Physics.Linecast(baseTransform.position + vector2, baseTransform.position + vector2 + vector4, out RaycastHit hit2, mask3.value))
                {
                    GameObject obj12 = crossL1;
                    obj12.transform.localPosition = currentCamera.WorldToScreenPoint(hit2.point);
                    Transform transform14 = obj12.transform;
                    transform14.localPosition -= new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                    obj12.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(obj12.transform.localPosition.y - (Input.mousePosition.y - Screen.height * 0.5f), obj12.transform.localPosition.x - (Input.mousePosition.x - Screen.width * 0.5f)) * 57.29578f + 180f);
                    GameObject obj13 = crossL2;
                    obj13.transform.localPosition = obj12.transform.localPosition;
                    obj13.transform.localRotation = obj12.transform.localRotation;
                    if (hit2.distance > 120f)
                    {
                        Transform transform15 = obj12.transform;
                        transform15.localPosition += Vector3.up * 10000f;
                    }
                    else
                    {
                        Transform transform16 = obj13.transform;
                        transform16.localPosition += Vector3.up * 10000f;
                    }
                }
                if (Physics.Linecast(baseTransform.position + vector3, baseTransform.position + vector3 + vector5, out hit2, mask3.value))
                {
                    GameObject obj14 = crossR1;
                    obj14.transform.localPosition = currentCamera.WorldToScreenPoint(hit2.point);
                    Transform transform17 = obj14.transform;
                    transform17.localPosition -= new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                    obj14.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(obj14.transform.localPosition.y - (Input.mousePosition.y - Screen.height * 0.5f), obj14.transform.localPosition.x - (Input.mousePosition.x - Screen.width * 0.5f)) * 57.29578f);
                    GameObject obj15 = crossR2;
                    obj15.transform.localPosition = obj14.transform.localPosition;
                    obj15.transform.localRotation = obj14.transform.localRotation;
                    if (hit2.distance > 120f)
                    {
                        Transform transform18 = obj14.transform;
                        transform18.localPosition += Vector3.up * 10000f;
                    }
                    else
                    {
                        Transform transform19 = obj15.transform;
                        transform19.localPosition += Vector3.up * 10000f;
                    }
                }
            }
        }
    }

    private void showGas()
    {
        float num = currentGas / totalGas;
        float num2 = currentBladeSta / totalBladeSta;
        GameObject.Find("gasL1").GetComponent<UISprite>().fillAmount = currentGas / totalGas;
        GameObject.Find("gasR1").GetComponent<UISprite>().fillAmount = currentGas / totalGas;
        if (!useGun)
        {
            GameObject.Find("bladeCL").GetComponent<UISprite>().fillAmount = currentBladeSta / totalBladeSta;
            GameObject.Find("bladeCR").GetComponent<UISprite>().fillAmount = currentBladeSta / totalBladeSta;
            if (num <= 0f)
            {
                GameObject.Find("gasL").GetComponent<UISprite>().color = Color.red;
                GameObject.Find("gasR").GetComponent<UISprite>().color = Color.red;
            }
            else if (num < 0.3f)
            {
                GameObject.Find("gasL").GetComponent<UISprite>().color = Color.yellow;
                GameObject.Find("gasR").GetComponent<UISprite>().color = Color.yellow;
            }
            else
            {
                GameObject.Find("gasL").GetComponent<UISprite>().color = Color.white;
                GameObject.Find("gasR").GetComponent<UISprite>().color = Color.white;
            }
            if (num2 <= 0f)
            {
                GameObject.Find("bladel1").GetComponent<UISprite>().color = Color.red;
                GameObject.Find("blader1").GetComponent<UISprite>().color = Color.red;
            }
            else if (num2 < 0.3f)
            {
                GameObject.Find("bladel1").GetComponent<UISprite>().color = Color.yellow;
                GameObject.Find("blader1").GetComponent<UISprite>().color = Color.yellow;
            }
            else
            {
                GameObject.Find("bladel1").GetComponent<UISprite>().color = Color.white;
                GameObject.Find("blader1").GetComponent<UISprite>().color = Color.white;
            }
            if (currentBladeNum <= 4)
            {
                GameObject.Find("bladel5").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader5").GetComponent<UISprite>().enabled = false;
            }
            else
            {
                GameObject.Find("bladel5").GetComponent<UISprite>().enabled = true;
                GameObject.Find("blader5").GetComponent<UISprite>().enabled = true;
            }
            if (currentBladeNum <= 3)
            {
                GameObject.Find("bladel4").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader4").GetComponent<UISprite>().enabled = false;
            }
            else
            {
                GameObject.Find("bladel4").GetComponent<UISprite>().enabled = true;
                GameObject.Find("blader4").GetComponent<UISprite>().enabled = true;
            }
            if (currentBladeNum <= 2)
            {
                GameObject.Find("bladel3").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader3").GetComponent<UISprite>().enabled = false;
            }
            else
            {
                GameObject.Find("bladel3").GetComponent<UISprite>().enabled = true;
                GameObject.Find("blader3").GetComponent<UISprite>().enabled = true;
            }
            if (currentBladeNum <= 1)
            {
                GameObject.Find("bladel2").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader2").GetComponent<UISprite>().enabled = false;
            }
            else
            {
                GameObject.Find("bladel2").GetComponent<UISprite>().enabled = true;
                GameObject.Find("blader2").GetComponent<UISprite>().enabled = true;
            }
            if (currentBladeNum <= 0)
            {
                GameObject.Find("bladel1").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader1").GetComponent<UISprite>().enabled = false;
            }
            else
            {
                GameObject.Find("bladel1").GetComponent<UISprite>().enabled = true;
                GameObject.Find("blader1").GetComponent<UISprite>().enabled = true;
            }
        }
        else
        {
            if (leftGunHasBullet)
            {
                GameObject.Find("bulletL").GetComponent<UISprite>().enabled = true;
            }
            else
            {
                GameObject.Find("bulletL").GetComponent<UISprite>().enabled = false;
            }
            if (rightGunHasBullet)
            {
                GameObject.Find("bulletR").GetComponent<UISprite>().enabled = true;
            }
            else
            {
                GameObject.Find("bulletR").GetComponent<UISprite>().enabled = false;
            }
        }
    }

    private void showGas2()
    {
        float num = currentGas / totalGas;
        float num2 = currentBladeSta / totalBladeSta;
        cachedSprites["gasL1"].fillAmount = currentGas / totalGas;
        cachedSprites["gasR1"].fillAmount = currentGas / totalGas;
        if (!useGun)
        {
            cachedSprites["bladeCL"].fillAmount = currentBladeSta / totalBladeSta;
            cachedSprites["bladeCR"].fillAmount = currentBladeSta / totalBladeSta;
            if (num <= 0f)
            {
                cachedSprites["gasL"].color = Color.red;
                cachedSprites["gasR"].color = Color.red;
            }
            else if (num < 0.3f)
            {
                cachedSprites["gasL"].color = Color.yellow;
                cachedSprites["gasR"].color = Color.yellow;
            }
            else
            {
                cachedSprites["gasL"].color = Color.white;
                cachedSprites["gasR"].color = Color.white;
            }
            if (num2 <= 0f)
            {
                cachedSprites["bladel1"].color = Color.red;
                cachedSprites["blader1"].color = Color.red;
            }
            else if (num2 < 0.3f)
            {
                cachedSprites["bladel1"].color = Color.yellow;
                cachedSprites["blader1"].color = Color.yellow;
            }
            else
            {
                cachedSprites["bladel1"].color = Color.white;
                cachedSprites["blader1"].color = Color.white;
            }
            if (currentBladeNum <= 4)
            {
                cachedSprites["bladel5"].enabled = false;
                cachedSprites["blader5"].enabled = false;
            }
            else
            {
                cachedSprites["bladel5"].enabled = true;
                cachedSprites["blader5"].enabled = true;
            }
            if (currentBladeNum <= 3)
            {
                cachedSprites["bladel4"].enabled = false;
                cachedSprites["blader4"].enabled = false;
            }
            else
            {
                cachedSprites["bladel4"].enabled = true;
                cachedSprites["blader4"].enabled = true;
            }
            if (currentBladeNum <= 2)
            {
                cachedSprites["bladel3"].enabled = false;
                cachedSprites["blader3"].enabled = false;
            }
            else
            {
                cachedSprites["bladel3"].enabled = true;
                cachedSprites["blader3"].enabled = true;
            }
            if (currentBladeNum <= 1)
            {
                cachedSprites["bladel2"].enabled = false;
                cachedSprites["blader2"].enabled = false;
            }
            else
            {
                cachedSprites["bladel2"].enabled = true;
                cachedSprites["blader2"].enabled = true;
            }
            if (currentBladeNum <= 0)
            {
                cachedSprites["bladel1"].enabled = false;
                cachedSprites["blader1"].enabled = false;
            }
            else
            {
                cachedSprites["bladel1"].enabled = true;
                cachedSprites["blader1"].enabled = true;
            }
        }
        else
        {
            if (leftGunHasBullet)
            {
                cachedSprites["bulletL"].enabled = true;
            }
            else
            {
                cachedSprites["bulletL"].enabled = false;
            }
            if (rightGunHasBullet)
            {
                cachedSprites["bulletR"].enabled = true;
            }
            else
            {
                cachedSprites["bulletR"].enabled = false;
            }
        }
    }

    [RPC]
    private void showHitDamage()
    {
        GameObject target = GameObject.Find("LabelScore");
        if (target != null)
        {
            speed = Mathf.Max(10f, speed);
            target.GetComponent<UILabel>().text = speed.ToString();
            target.transform.localScale = Vector3.zero;
            speed = (int) (speed * 0.1f);
            speed = Mathf.Clamp(speed, 40f, 150f);
            iTween.Stop(target);
            object[] args = new object[] { "x", speed, "y", speed, "z", speed, "easetype", iTween.EaseType.easeOutElastic, "time", 1f };
            iTween.ScaleTo(target, iTween.Hash(args));
            object[] objArray2 = new object[] { "x", 0, "y", 0, "z", 0, "easetype", iTween.EaseType.easeInBounce, "time", 0.5f, "delay", 2f };
            iTween.ScaleTo(target, iTween.Hash(objArray2));
        }
    }

    private void showSkillCD()
    {
        if (skillCD != null)
        {
            skillCD.GetComponent<UISprite>().fillAmount = (skillCDLast - skillCDDuration) / skillCDLast;
        }
    }

    [RPC]
    public void SpawnCannonRPC(string settings, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient && base.photonView.isMine && myCannon == null)
        {
            if (myHorse != null && isMounted)
            {
                getOffHorse();
            }
            idle();
            if (bulletLeft != null)
            {
                bulletLeft.GetComponent<Bullet>().removeMe();
            }
            if (bulletRight != null)
            {
                bulletRight.GetComponent<Bullet>().removeMe();
            }
            if (smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && base.photonView.isMine)
            {
                object[] parameters = new object[] { false };
                base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, parameters);
            }
            smoke_3dmg.enableEmission = false;
            rigidbody.velocity = Vector3.zero;
            string[] strArray = settings.Split(new char[] { ',' });
            if (strArray.Length > 15)
            {
                myCannon = PhotonNetwork.Instantiate("RCAsset/" + strArray[1], new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[16]), Convert.ToSingle(strArray[17]), Convert.ToSingle(strArray[18])), 0);
            }
            else
            {
                myCannon = PhotonNetwork.Instantiate("RCAsset/" + strArray[1], new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4])), new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])), 0);
            }
            myCannonBase = myCannon.transform;
            myCannonPlayer = myCannon.transform.Find("PlayerPoint");
            isCannon = true;
            myCannon.GetComponent<Cannon>().myHero = this;
            myCannonRegion = null;
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(myCannon.transform.Find("Barrel").Find("FiringPoint").gameObject, true, false);
            Camera.main.fieldOfView = 55f;
            base.photonView.RPC("SetMyCannon", PhotonTargets.OthersBuffered, new object[] { myCannon.GetPhotonView().viewID });
            skillCDLastCannon = skillCDLast;
            skillCDLast = 3.5f;
            skillCDDuration = 3.5f;
        }
    }

    private void Start()
    {
        FengGameManagerMKII.instance.RegisterHero(this);
        if ((LevelInfo.GetInfo(FengGameManagerMKII.level).horse || RCSettings.horseMode == 1) && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
        {
            myHorse = PhotonNetwork.Instantiate("horse", baseTransform.position + Vector3.up * 5f, baseTransform.rotation, 0);
            myHorse.GetComponent<Horse>().myHero = gameObject;
            myHorse.GetComponent<TITAN_CONTROLLER>().isHorse = true;
        }
        sparks = baseTransform.Find("slideSparks").GetComponent<ParticleSystem>();
        smoke_3dmg = baseTransform.Find("3dmg_smoke").GetComponent<ParticleSystem>();
        baseTransform.localScale = new Vector3(myScale, myScale, myScale);
        facingDirection = baseTransform.rotation.eulerAngles.y;
        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
        smoke_3dmg.enableEmission = false;
        sparks.enableEmission = false;
        speedFXPS = speedFX1.GetComponent<ParticleSystem>();
        speedFXPS.enableEmission = false;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
            if (PhotonNetwork.isMasterClient)
            {
                int iD = base.photonView.owner.ID;
                if (FengGameManagerMKII.heroHash.ContainsKey(iD))
                {
                    FengGameManagerMKII.heroHash[iD] = this;
                }
                else
                {
                    FengGameManagerMKII.heroHash.Add(iD, this);
                }
            }
            GameObject obj2 = GameObject.Find("UI_IN_GAME");
            myNetWorkName = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
            myNetWorkName.name = "LabelNameOverHead";
            myNetWorkName.transform.parent = obj2.GetComponent<UIReferArray>().panels[0].transform;
            myNetWorkName.transform.localScale = new Vector3(14f, 14f, 14f);
            myNetWorkName.GetComponent<UILabel>().text = string.Empty;
            if (base.photonView.isMine)
            {
                if (Minimap.instance != null)
                {
                    Minimap.instance.TrackGameObjectOnMinimap(gameObject, Color.green, false, true, Minimap.IconStyle.CIRCLE);
                }
                base.GetComponent<SmoothSyncMovement>().PhotonCamera = true;
                base.photonView.RPC("SetMyPhotonCamera", PhotonTargets.OthersBuffered, new object[] { PlayerPrefs.GetFloat("cameraDistance") + 0.3f });
            }
            else
            {
                bool flag2 = false;
                if (base.photonView.owner.customProperties[PhotonPlayerProperty.RCteam] != null)
                {
                    switch (RCextensions.returnIntFromObject(base.photonView.owner.customProperties[PhotonPlayerProperty.RCteam]))
                    {
                        case 1:
                            flag2 = true;
                            if (Minimap.instance != null)
                            {
                                Minimap.instance.TrackGameObjectOnMinimap(gameObject, Color.cyan, false, true, Minimap.IconStyle.CIRCLE);
                            }
                            break;

                        case 2:
                            flag2 = true;
                            if (Minimap.instance != null)
                            {
                                Minimap.instance.TrackGameObjectOnMinimap(gameObject, Color.magenta, false, true, Minimap.IconStyle.CIRCLE);
                            }
                            break;
                    }
                }
                if (RCextensions.returnIntFromObject(base.photonView.owner.customProperties[PhotonPlayerProperty.team]) == 2)
                {
                    myNetWorkName.GetComponent<UILabel>().text = "[FF0000]AHSS\n[FFFFFF]";
                    if (!flag2 && Minimap.instance != null)
                    {
                        Minimap.instance.TrackGameObjectOnMinimap(gameObject, Color.red, false, true, Minimap.IconStyle.CIRCLE);
                    }
                }
                else if (!flag2 && Minimap.instance != null)
                {
                    Minimap.instance.TrackGameObjectOnMinimap(gameObject, Color.blue, false, true, Minimap.IconStyle.CIRCLE);
                }
            }
            string str = RCextensions.returnStringFromObject(base.photonView.owner.customProperties[PhotonPlayerProperty.guildName]);
            if (str != string.Empty)
            {
                UILabel component = myNetWorkName.GetComponent<UILabel>();
                string text = component.text;
                string[] strArray2 = new string[] { text, "[FFFF00]", str, "\n[FFFFFF]", base.photonView.owner.Name };
                component.text = string.Concat(strArray2);
            }
            else
            {
                UILabel label2 = myNetWorkName.GetComponent<UILabel>();
                label2.text = label2.text + base.photonView.owner.Name;
            }
        }
        else if (Minimap.instance != null)
        {
            Minimap.instance.TrackGameObjectOnMinimap(gameObject, Color.green, false, true, Minimap.IconStyle.CIRCLE);
        }
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !base.photonView.isMine)
        {
            gameObject.layer = LayerMask.NameToLayer("NetworkObject");
            if (IN_GAME_MAIN_CAMERA.dayLight == DayLight.Night)
            {
                GameObject obj3 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("flashlight"));
                obj3.transform.parent = baseTransform;
                obj3.transform.position = baseTransform.position + Vector3.up;
                obj3.transform.rotation = Quaternion.Euler(353f, 0f, 0f);
            }
            setup.Initialize();
            setup.myCostume = new HeroCostume();
            setup.myCostume = CostumeConverter.PhotonDataToHeroCostume(base.photonView.owner);
            setup.SetCharacterComponent();
            UnityEngine.Object.Destroy(checkBoxLeft);
            UnityEngine.Object.Destroy(checkBoxRight);
            UnityEngine.Object.Destroy(leftbladetrail);
            UnityEngine.Object.Destroy(rightbladetrail);
            UnityEngine.Object.Destroy(leftbladetrail2);
            UnityEngine.Object.Destroy(rightbladetrail2);
            hasspawn = true;
        }
        else
        {
            currentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            inputManager = GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();
            loadskin();
            hasspawn = true;
            base.StartCoroutine(reloadSky());
        }
        bombImmune = false;
        if (RCSettings.bombMode == 1)
        {
            bombImmune = true;
            base.StartCoroutine(stopImmunity());
        }
    }

    public IEnumerator stopImmunity()
    {
        yield return new WaitForSeconds(5f);
        bombImmune = false;
    }

    private void Suicide()
    {
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            netDieLocal(rigidbody.velocity * 50f, false, -1, string.Empty);
            FengGameManagerMKII.instance.needChooseSide = true;
            FengGameManagerMKII.instance.justSuicide = true;
        }
    }

    private void throwBlades()
    {
        Transform transform = setup.part_blade_l.transform;
        Transform transform2 = setup.part_blade_r.transform;
        GameObject obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_l"), transform.position, transform.rotation);
        GameObject obj3 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_r"), transform2.position, transform2.rotation);
        obj2.renderer.material = CharacterMaterials.materials[setup.myCostume.DmgTexture];
        obj3.renderer.material = CharacterMaterials.materials[setup.myCostume.DmgTexture];
        Vector3 force = base.transform.forward + base.transform.up * 2f - base.transform.right;
        obj2.rigidbody.AddForce(force, ForceMode.Impulse);
        Vector3 vector2 = base.transform.forward + base.transform.up * 2f + base.transform.right;
        obj3.rigidbody.AddForce(vector2, ForceMode.Impulse);
        Vector3 torque = new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100));
        torque.Normalize();
        obj2.rigidbody.AddTorque(torque);
        torque = new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100));
        torque.Normalize();
        obj3.rigidbody.AddTorque(torque);
        setup.part_blade_l.SetActive(false);
        setup.part_blade_r.SetActive(false);
        currentBladeNum--;
        if (currentBladeNum == 0)
        {
            currentBladeSta = 0f;
        }
        if (state == HERO_STATE.Attack)
        {
            FalseAttack();
        }
    }

    public void ungrabbed()
    {
        facingDirection = 0f;
        targetRotation = Quaternion.Euler(0f, 0f, 0f);
        base.transform.parent = null;
        base.GetComponent<CapsuleCollider>().isTrigger = false;
        state = HERO_STATE.Idle;
    }

    private void unmounted()
    {
        myHorse.GetComponent<Horse>().unmounted();
        isMounted = false;
    }

    public void update()
    {
        if (!IN_GAME_MAIN_CAMERA.isPausing)
        {
            if (invincible > 0f)
            {
                invincible -= Time.deltaTime;
            }
            if (!hasDied)
            {
                if (titanForm && eren_titan != null)
                {
                    baseTransform.position = eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
                    gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
                }
                else if (isCannon && myCannon != null)
                {
                    updateCannon();
                    gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
                }
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
                {
                    if (myCannonRegion != null)
                    {
                        FengGameManagerMKII.instance.ShowHUDInfoCenter("Press 'Cannon Mount' key to use Cannon.");
                        if (FengGameManagerMKII.inputRC.isInputCannonDown(InputCodeRC.cannonMount))
                        {
                            myCannonRegion.photonView.RPC("RequestControlRPC", PhotonTargets.MasterClient, new object[] { base.photonView.viewID });
                        }
                    }
                    if (state == HERO_STATE.Grab && !useGun)
                    {
                        if (skillId == "jean")
                        {
                            if (state != HERO_STATE.Attack && (inputManager.isInputDown[InputCode.attack0] || inputManager.isInputDown[InputCode.attack1]) && escapeTimes > 0 && !baseAnimation.IsPlaying("grabbed_jean"))
                            {
                                playAnimation("grabbed_jean");
                                baseAnimation["grabbed_jean"].time = 0f;
                                escapeTimes--;
                            }
                            if (baseAnimation.IsPlaying("grabbed_jean") && baseAnimation["grabbed_jean"].normalizedTime > 0.64f && titanWhoGrabMe.GetComponent<TITAN>() != null)
                            {
                                ungrabbed();
                                baseRigidBody.velocity = Vector3.up * 30f;
                                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                                {
                                    titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
                                }
                                else
                                {
                                    base.photonView.RPC("netSetIsGrabbedFalse", PhotonTargets.All, new object[0]);
                                    if (PhotonNetwork.isMasterClient)
                                    {
                                        titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
                                    }
                                    else
                                    {
                                        PhotonView.Find(titanWhoGrabMeID).RPC("grabbedTargetEscape", PhotonTargets.MasterClient, new object[0]);
                                    }
                                }
                            }
                        }
                        else if (skillId == "eren")
                        {
                            showSkillCD();
                            if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && !IN_GAME_MAIN_CAMERA.isPausing)
                            {
                                SetSkillCD();
                                SetFlareCD();
                            }
                            if (inputManager.isInputDown[InputCode.attack1])
                            {
                                bool flag2 = false;
                                if (skillCDDuration > 0f || flag2)
                                {
                                    flag2 = true;
                                }
                                else
                                {
                                    skillCDDuration = skillCDLast;
                                    if (skillId == "eren" && titanWhoGrabMe.GetComponent<TITAN>() != null)
                                    {
                                        ungrabbed();
                                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                                        {
                                            titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
                                        }
                                        else
                                        {
                                            base.photonView.RPC("netSetIsGrabbedFalse", PhotonTargets.All, new object[0]);
                                            if (PhotonNetwork.isMasterClient)
                                            {
                                                titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
                                            }
                                            else
                                            {
                                                PhotonView.Find(titanWhoGrabMeID).photonView.RPC("grabbedTargetEscape", PhotonTargets.MasterClient, new object[0]);
                                            }
                                        }
                                        ErenTransform();
                                    }
                                }
                            }
                        }
                    }
                    else if (!titanForm && !isCannon)
                    {
                        System.Boolean ReflectorVariable2;
                        System.Boolean ReflectorVariable1;
                        System.Boolean ReflectorVariable0;
                        BufferUpdate();
                        updateExt();
                        if (!grounded && state != HERO_STATE.AirDodge)
                        {
                            if ((int) FengGameManagerMKII.settings[181] == 1)
                            {
                                CheckDashRebind();
                            }
                            else
                            {
                                GetDashDirection();
                            }
                            if (dashD)
                            {
                                dashD = false;
                                Dash(0f, -1f);
                                return;
                            }
                            if (dashU)
                            {
                                dashU = false;
                                Dash(0f, 1f);
                                return;
                            }
                            if (dashL)
                            {
                                dashL = false;
                                Dash(-1f, 0f);
                                return;
                            }
                            if (dashR)
                            {
                                dashR = false;
                                Dash(1f, 0f);
                                return;
                            }
                        }
                        if (grounded && (state == HERO_STATE.Idle || state == HERO_STATE.Slide))
                        {
                            if (!(!inputManager.isInputDown[InputCode.jump] || baseAnimation.IsPlaying("jump") || baseAnimation.IsPlaying("horse_geton")))
                            {
                                idle();
                                CrossFade("jump", 0.1f);
                                sparks.enableEmission = false;
                            }
                            if (!(!FengGameManagerMKII.inputRC.isInputHorseDown(InputCodeRC.horseMount) || baseAnimation.IsPlaying("jump") || baseAnimation.IsPlaying("horse_geton")) && myHorse != null && !isMounted && Vector3.Distance(myHorse.transform.position, base.transform.position) < 15f)
                            {
                                getOnHorse();
                            }
                            if (!(!inputManager.isInputDown[InputCode.dodge] || baseAnimation.IsPlaying("jump") || baseAnimation.IsPlaying("horse_geton")))
                            {
                                Dodge(false);
                                return;
                            }
                        }
                        if (state == HERO_STATE.Idle)
                        {
                            if (inputManager.isInputDown[InputCode.flare1])
                            {
                                shootFlare(1);
                            }
                            if (inputManager.isInputDown[InputCode.flare2])
                            {
                                shootFlare(2);
                            }
                            if (inputManager.isInputDown[InputCode.flare3])
                            {
                                shootFlare(3);
                            }
                            if (inputManager.isInputDown[InputCode.restart])
                            {
                                Suicide();
                            }
                            if (myHorse != null && isMounted && FengGameManagerMKII.inputRC.isInputHorseDown(InputCodeRC.horseMount))
                            {
                                getOffHorse();
                            }
                            if ((base.animation.IsPlaying(standAnimation) || !grounded) && inputManager.isInputDown[InputCode.reload] && (!useGun || RCSettings.ahssReload != 1 || grounded))
                            {
                                ChangeBlade();
                                return;
                            }
                            if (baseAnimation.IsPlaying(standAnimation) && inputManager.isInputDown[InputCode.salute])
                            {
                                salute();
                                return;
                            }
                            if (!isMounted && (inputManager.isInputDown[InputCode.attack0] || inputManager.isInputDown[InputCode.attack1]) && !useGun)
                            {
                                bool flag3 = false;
                                if (inputManager.isInputDown[InputCode.attack1])
                                {
                                    if (skillCDDuration > 0f || flag3)
                                    {
                                        flag3 = true;
                                    }
                                    else
                                    {
                                        skillCDDuration = skillCDLast;
                                        if (skillId == "eren")
                                        {
                                            ErenTransform();
                                            return;
                                        }
                                        if (skillId == "marco")
                                        {
                                            if (IsGrounded())
                                            {
                                                attackAnimation = UnityEngine.Random.Range(0, 2) != 0 ? "special_marco_1" : "special_marco_0";
                                                playAnimation(attackAnimation);
                                            }
                                            else
                                            {
                                                flag3 = true;
                                                skillCDDuration = 0f;
                                            }
                                        }
                                        else if (skillId == "armin")
                                        {
                                            if (IsGrounded())
                                            {
                                                attackAnimation = "special_armin";
                                                playAnimation("special_armin");
                                            }
                                            else
                                            {
                                                flag3 = true;
                                                skillCDDuration = 0f;
                                            }
                                        }
                                        else if (skillId == "sasha")
                                        {
                                            if (IsGrounded())
                                            {
                                                attackAnimation = "special_sasha";
                                                playAnimation("special_sasha");
                                                currentBuff = BUFF.SpeedUp;
                                                buffTime = 10f;
                                            }
                                            else
                                            {
                                                flag3 = true;
                                                skillCDDuration = 0f;
                                            }
                                        }
                                        else if (skillId == "mikasa")
                                        {
                                            attackAnimation = "attack3_1";
                                            playAnimation("attack3_1");
                                            baseRigidBody.velocity = Vector3.up * 10f;
                                        }
                                        else if (skillId == "levi")
                                        {
                                            RaycastHit hit;
                                            attackAnimation = "attack5";
                                            playAnimation("attack5");
                                            baseRigidBody.velocity += Vector3.up * 5f;
                                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                            LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
                                            LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
                                            LayerMask mask3 = mask2 | mask;
                                            if (Physics.Raycast(ray, out hit, 1E+07f, mask3.value))
                                            {
                                                if (bulletRight != null)
                                                {
                                                    bulletRight.GetComponent<Bullet>().disable();
                                                    releaseIfIHookSb();
                                                }
                                                dashDirection = hit.point - baseTransform.position;
                                                launchRightRope(hit, true, 1);
                                                rope.Play();
                                            }
                                            facingDirection = Mathf.Atan2(dashDirection.x, dashDirection.z) * 57.29578f;
                                            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                            attackLoop = 3;
                                        }
                                        else if (skillId == "petra")
                                        {
                                            RaycastHit hit2;
                                            attackAnimation = "special_petra";
                                            playAnimation("special_petra");
                                            baseRigidBody.velocity += Vector3.up * 5f;
                                            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                            LayerMask mask4 = 1 << LayerMask.NameToLayer("Ground");
                                            LayerMask mask5 = 1 << LayerMask.NameToLayer("EnemyBox");
                                            LayerMask mask6 = mask5 | mask4;
                                            if (Physics.Raycast(ray2, out hit2, 1E+07f, mask6.value))
                                            {
                                                if (bulletRight != null)
                                                {
                                                    bulletRight.GetComponent<Bullet>().disable();
                                                    releaseIfIHookSb();
                                                }
                                                if (bulletLeft != null)
                                                {
                                                    bulletLeft.GetComponent<Bullet>().disable();
                                                    releaseIfIHookSb();
                                                }
                                                dashDirection = hit2.point - baseTransform.position;
                                                launchLeftRope(hit2, true, 0);
                                                launchRightRope(hit2, true, 0);
                                                rope.Play();
                                            }
                                            facingDirection = Mathf.Atan2(dashDirection.x, dashDirection.z) * 57.29578f;
                                            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                            attackLoop = 3;
                                        }
                                        else
                                        {
                                            if (needLean)
                                            {
                                                if (leanLeft)
                                                {
                                                    attackAnimation = UnityEngine.Random.Range(0, 100) >= 50 ? "attack1_hook_l1" : "attack1_hook_l2";
                                                }
                                                else
                                                {
                                                    attackAnimation = UnityEngine.Random.Range(0, 100) >= 50 ? "attack1_hook_r1" : "attack1_hook_r2";
                                                }
                                            }
                                            else
                                            {
                                                attackAnimation = "attack1";
                                            }
                                            playAnimation(attackAnimation);
                                        }
                                    }
                                }
                                else if (inputManager.isInputDown[InputCode.attack0])
                                {
                                    if (needLean)
                                    {
                                        if (inputManager.isInput[InputCode.left])
                                        {
                                            attackAnimation = UnityEngine.Random.Range(0, 100) >= 50 ? "attack1_hook_l1" : "attack1_hook_l2";
                                        }
                                        else if (inputManager.isInput[InputCode.right])
                                        {
                                            attackAnimation = UnityEngine.Random.Range(0, 100) >= 50 ? "attack1_hook_r1" : "attack1_hook_r2";
                                        }
                                        else if (leanLeft)
                                        {
                                            attackAnimation = UnityEngine.Random.Range(0, 100) >= 50 ? "attack1_hook_l1" : "attack1_hook_l2";
                                        }
                                        else
                                        {
                                            attackAnimation = UnityEngine.Random.Range(0, 100) >= 50 ? "attack1_hook_r1" : "attack1_hook_r2";
                                        }
                                    }
                                    else if (inputManager.isInput[InputCode.left])
                                    {
                                        attackAnimation = "attack2";
                                    }
                                    else if (inputManager.isInput[InputCode.right])
                                    {
                                        attackAnimation = "attack1";
                                    }
                                    else if (lastHook != null)
                                    {
                                        if (lastHook.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck") != null)
                                        {
                                            AttackAccordingToTarget(lastHook.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck"));
                                        }
                                        else
                                        {
                                            flag3 = true;
                                        }
                                    }
                                    else if (bulletLeft != null && bulletLeft.transform.parent != null)
                                    {
                                        Transform a = bulletLeft.transform.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                                        if (a != null)
                                        {
                                            AttackAccordingToTarget(a);
                                        }
                                        else
                                        {
                                            AttackAccordingToMouse();
                                        }
                                    }
                                    else if (bulletRight != null && bulletRight.transform.parent != null)
                                    {
                                        Transform transform2 = bulletRight.transform.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                                        if (transform2 != null)
                                        {
                                            AttackAccordingToTarget(transform2);
                                        }
                                        else
                                        {
                                            AttackAccordingToMouse();
                                        }
                                    }
                                    else
                                    {
                                        GameObject obj2 = FindNearestTitan();
                                        if (obj2 != null)
                                        {
                                            Transform transform3 = obj2.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                                            if (transform3 != null)
                                            {
                                                AttackAccordingToTarget(transform3);
                                            }
                                            else
                                            {
                                                AttackAccordingToMouse();
                                            }
                                        }
                                        else
                                        {
                                            AttackAccordingToMouse();
                                        }
                                    }
                                }
                                if (!flag3)
                                {
                                    checkBoxLeft.GetComponent<TriggerColliderWeapon>().ClearHits();
                                    checkBoxRight.GetComponent<TriggerColliderWeapon>().ClearHits();
                                    if (grounded)
                                    {
                                        baseRigidBody.AddForce(gameObject.transform.forward * 200f);
                                    }
                                    playAnimation(attackAnimation);
                                    baseAnimation[attackAnimation].time = 0f;
                                    buttonAttackRelease = false;
                                    state = HERO_STATE.Attack;
                                    if (grounded || attackAnimation == "attack3_1" || attackAnimation == "attack5" || attackAnimation == "special_petra")
                                    {
                                        attackReleased = true;
                                        buttonAttackRelease = true;
                                    }
                                    else
                                    {
                                        attackReleased = false;
                                    }
                                    sparks.enableEmission = false;
                                }
                            }
                            if (useGun)
                            {
                                if (inputManager.isInput[InputCode.attack1])
                                {
                                    leftArmAim = true;
                                    rightArmAim = true;
                                }
                                else if (inputManager.isInput[InputCode.attack0])
                                {
                                    if (leftGunHasBullet)
                                    {
                                        leftArmAim = true;
                                        rightArmAim = false;
                                    }
                                    else
                                    {
                                        leftArmAim = false;
                                        if (rightGunHasBullet)
                                        {
                                            rightArmAim = true;
                                        }
                                        else
                                        {
                                            rightArmAim = false;
                                        }
                                    }
                                }
                                else
                                {
                                    leftArmAim = false;
                                    rightArmAim = false;
                                }
                                if (leftArmAim || rightArmAim)
                                {
                                    RaycastHit hit3;
                                    Ray ray3 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                    LayerMask mask7 = 1 << LayerMask.NameToLayer("Ground");
                                    LayerMask mask8 = 1 << LayerMask.NameToLayer("EnemyBox");
                                    LayerMask mask9 = mask8 | mask7;
                                    if (Physics.Raycast(ray3, out hit3, 1E+07f, mask9.value))
                                    {
                                        gunTarget = hit3.point;
                                    }
                                }
                                bool flag4 = false;
                                bool flag5 = false;
                                bool flag6 = false;
                                if (inputManager.isInputUp[InputCode.attack1] && skillId != "bomb")
                                {
                                    if (leftGunHasBullet && rightGunHasBullet)
                                    {
                                        if (grounded)
                                        {
                                            attackAnimation = "AHSS_shoot_both";
                                        }
                                        else
                                        {
                                            attackAnimation = "AHSS_shoot_both_air";
                                        }
                                        flag4 = true;
                                    }
                                    else if (!(leftGunHasBullet || rightGunHasBullet))
                                    {
                                        flag5 = true;
                                    }
                                    else
                                    {
                                        flag6 = true;
                                    }
                                }
                                if (flag6 || inputManager.isInputUp[InputCode.attack0])
                                {
                                    if (grounded)
                                    {
                                        if (leftGunHasBullet && rightGunHasBullet)
                                        {
                                            if (isLeftHandHooked)
                                            {
                                                attackAnimation = "AHSS_shoot_r";
                                            }
                                            else
                                            {
                                                attackAnimation = "AHSS_shoot_l";
                                            }
                                        }
                                        else if (leftGunHasBullet)
                                        {
                                            attackAnimation = "AHSS_shoot_l";
                                        }
                                        else if (rightGunHasBullet)
                                        {
                                            attackAnimation = "AHSS_shoot_r";
                                        }
                                    }
                                    else if (leftGunHasBullet && rightGunHasBullet)
                                    {
                                        if (isLeftHandHooked)
                                        {
                                            attackAnimation = "AHSS_shoot_r_air";
                                        }
                                        else
                                        {
                                            attackAnimation = "AHSS_shoot_l_air";
                                        }
                                    }
                                    else if (leftGunHasBullet)
                                    {
                                        attackAnimation = "AHSS_shoot_l_air";
                                    }
                                    else if (rightGunHasBullet)
                                    {
                                        attackAnimation = "AHSS_shoot_r_air";
                                    }
                                    if (leftGunHasBullet || rightGunHasBullet)
                                    {
                                        flag4 = true;
                                    }
                                    else
                                    {
                                        flag5 = true;
                                    }
                                }
                                if (flag4)
                                {
                                    state = HERO_STATE.Attack;
                                    CrossFade(attackAnimation, 0.05f);
                                    gunDummy.transform.position = baseTransform.position;
                                    gunDummy.transform.rotation = baseTransform.rotation;
                                    gunDummy.transform.LookAt(gunTarget);
                                    attackReleased = false;
                                    facingDirection = gunDummy.transform.rotation.eulerAngles.y;
                                    targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                }
                                else if (flag5 && (grounded || LevelInfo.GetInfo(FengGameManagerMKII.level).type != GAMEMODE.PVP_AHSS && RCSettings.ahssReload == 0))
                                {
                                    ChangeBlade();
                                }
                            }
                        }
                        else if (state == HERO_STATE.Attack)
                        {
                            if (!useGun)
                            {
                                if (!inputManager.isInput[InputCode.attack0])
                                {
                                    buttonAttackRelease = true;
                                }
                                if (!attackReleased)
                                {
                                    if (buttonAttackRelease)
                                    {
                                        ContinueAnimation();
                                        attackReleased = true;
                                    }
                                    else if (baseAnimation[attackAnimation].normalizedTime >= 0.32f)
                                    {
                                        pauseAnimation();
                                    }
                                }
                                if (attackAnimation == "attack3_1" && currentBladeSta > 0f)
                                {
                                    if (baseAnimation[attackAnimation].normalizedTime >= 0.8f)
                                    {
                                        if (!checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
                                        {
                                            checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = true;
                                            if ((int) FengGameManagerMKII.settings[92] == 0)
                                            {
                                                leftbladetrail2.Activate();
                                                rightbladetrail2.Activate();
                                                leftbladetrail.Activate();
                                                rightbladetrail.Activate();
                                            }
                                            baseRigidBody.velocity = -Vector3.up * 30f;
                                        }
                                        if (!checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me)
                                        {
                                            checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = true;
                                            slash.Play();
                                        }
                                    }
                                    else if (checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
                                    {
                                        checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                                        checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                                        checkBoxLeft.GetComponent<TriggerColliderWeapon>().ClearHits();
                                        checkBoxRight.GetComponent<TriggerColliderWeapon>().ClearHits();
                                        leftbladetrail.StopSmoothly(0.1f);
                                        rightbladetrail.StopSmoothly(0.1f);
                                        leftbladetrail2.StopSmoothly(0.1f);
                                        rightbladetrail2.StopSmoothly(0.1f);
                                    }
                                }
                                else
                                {
                                    float num;
                                    float num2;
                                    if (currentBladeSta == 0f)
                                    {
                                        num = -1f;
                                        num2 = -1f;
                                    }
                                    else if (attackAnimation == "attack5")
                                    {
                                        num2 = 0.35f;
                                        num = 0.5f;
                                    }
                                    else if (attackAnimation == "special_petra")
                                    {
                                        num2 = 0.35f;
                                        num = 0.48f;
                                    }
                                    else if (attackAnimation == "special_armin")
                                    {
                                        num2 = 0.25f;
                                        num = 0.35f;
                                    }
                                    else if (attackAnimation == "attack4")
                                    {
                                        num2 = 0.6f;
                                        num = 0.9f;
                                    }
                                    else if (attackAnimation == "special_sasha")
                                    {
                                        num = -1f;
                                        num2 = -1f;
                                    }
                                    else
                                    {
                                        num2 = 0.5f;
                                        num = 0.85f;
                                    }
                                    if (baseAnimation[attackAnimation].normalizedTime > num2 && baseAnimation[attackAnimation].normalizedTime < num)
                                    {
                                        if (!checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
                                        {
                                            checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = true;
                                            slash.Play();
                                            if ((int) FengGameManagerMKII.settings[92] == 0)
                                            {
                                                leftbladetrail2.Activate();
                                                rightbladetrail2.Activate();
                                                leftbladetrail.Activate();
                                                rightbladetrail.Activate();
                                            }
                                        }
                                        if (!checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me)
                                        {
                                            checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = true;
                                        }
                                    }
                                    else if (checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
                                    {
                                        checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                                        checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                                        checkBoxLeft.GetComponent<TriggerColliderWeapon>().ClearHits();
                                        checkBoxRight.GetComponent<TriggerColliderWeapon>().ClearHits();
                                        leftbladetrail2.StopSmoothly(0.1f);
                                        rightbladetrail2.StopSmoothly(0.1f);
                                        leftbladetrail.StopSmoothly(0.1f);
                                        rightbladetrail.StopSmoothly(0.1f);
                                    }
                                    if (attackLoop > 0 && baseAnimation[attackAnimation].normalizedTime > num)
                                    {
                                        attackLoop--;
                                        playAnimationAt(attackAnimation, num2);
                                    }
                                }
                                if (baseAnimation[attackAnimation].normalizedTime >= 1f)
                                {
                                    if (attackAnimation == "special_marco_0" || attackAnimation == "special_marco_1")
                                    {
                                        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                                        {
                                            if (!PhotonNetwork.isMasterClient)
                                            {
                                                object[] parameters = new object[] { 5f, 100f };
                                                base.photonView.RPC("netTauntAttack", PhotonTargets.MasterClient, parameters);
                                            }
                                            else
                                            {
                                                netTauntAttack(5f, 100f);
                                            }
                                        }
                                        else
                                        {
                                            netTauntAttack(5f, 100f);
                                        }
                                        FalseAttack();
                                        idle();
                                    }
                                    else if (attackAnimation == "special_armin")
                                    {
                                        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                                        {
                                            if (!PhotonNetwork.isMasterClient)
                                            {
                                                base.photonView.RPC("netlaughAttack", PhotonTargets.MasterClient, new object[0]);
                                            }
                                            else
                                            {
                                                netlaughAttack();
                                            }
                                        }
                                        else
                                        {
                                            foreach (GameObject obj3 in GameObject.FindGameObjectsWithTag("titan"))
                                            {
                                                if (Vector3.Distance(obj3.transform.position, baseTransform.position) < 50f && Vector3.Angle(obj3.transform.forward, baseTransform.position - obj3.transform.position) < 90f && obj3.GetComponent<TITAN>() != null)
                                                {
                                                    obj3.GetComponent<TITAN>().beLaughAttacked();
                                                }
                                            }
                                        }
                                        FalseAttack();
                                        idle();
                                    }
                                    else if (attackAnimation == "attack3_1")
                                    {
                                        baseRigidBody.velocity -= Vector3.up * Time.deltaTime * 30f;
                                    }
                                    else
                                    {
                                        FalseAttack();
                                        idle();
                                    }
                                }
                                if (baseAnimation.IsPlaying("attack3_2") && baseAnimation["attack3_2"].normalizedTime >= 1f)
                                {
                                    FalseAttack();
                                    idle();
                                }
                            }
                            else
                            {
                                baseTransform.rotation = Quaternion.Lerp(baseTransform.rotation, gunDummy.transform.rotation, Time.deltaTime * 30f);
                                if (!attackReleased && baseAnimation[attackAnimation].normalizedTime > 0.167f)
                                {
                                    GameObject obj4;
                                    attackReleased = true;
                                    bool flag7 = false;
                                    if (attackAnimation == "AHSS_shoot_both" || attackAnimation == "AHSS_shoot_both_air")
                                    {
                                        flag7 = true;
                                        leftGunHasBullet = false;
                                        rightGunHasBullet = false;
                                        baseRigidBody.AddForce(-baseTransform.forward * 1000f, ForceMode.Acceleration);
                                    }
                                    else
                                    {
                                        if (attackAnimation == "AHSS_shoot_l" || attackAnimation == "AHSS_shoot_l_air")
                                        {
                                            leftGunHasBullet = false;
                                        }
                                        else
                                        {
                                            rightGunHasBullet = false;
                                        }
                                        baseRigidBody.AddForce(-baseTransform.forward * 600f, ForceMode.Acceleration);
                                    }
                                    baseRigidBody.AddForce(Vector3.up * 200f, ForceMode.Acceleration);
                                    string prefabName = "FX/shotGun";
                                    if (flag7)
                                    {
                                        prefabName = "FX/shotGun 1";
                                    }
                                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
                                    {
                                        obj4 = PhotonNetwork.Instantiate(prefabName, baseTransform.position + baseTransform.up * 0.8f - baseTransform.right * 0.1f, baseTransform.rotation, 0);
                                        if (obj4.GetComponent<EnemyfxIDcontainer>() != null)
                                        {
                                            obj4.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = base.photonView.viewID;
                                        }
                                    }
                                    else
                                    {
                                        obj4 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load(prefabName), baseTransform.position + baseTransform.up * 0.8f - baseTransform.right * 0.1f, baseTransform.rotation);
                                    }
                                }
                                if (baseAnimation[attackAnimation].normalizedTime >= 1f)
                                {
                                    FalseAttack();
                                    idle();
                                }
                                if (!baseAnimation.IsPlaying(attackAnimation))
                                {
                                    FalseAttack();
                                    idle();
                                }
                            }
                        }
                        else if (state == HERO_STATE.ChangeBlade)
                        {
                            if (useGun)
                            {
                                if (baseAnimation[reloadAnimation].normalizedTime > 0.22f)
                                {
                                    if (!(leftGunHasBullet || !setup.part_blade_l.activeSelf))
                                    {
                                        setup.part_blade_l.SetActive(false);
                                        Transform transform = setup.part_blade_l.transform;
                                        GameObject obj5 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_l"), transform.position, transform.rotation);
                                        obj5.renderer.material = CharacterMaterials.materials[setup.myCostume.DmgTexture];
                                        Vector3 force = -baseTransform.forward * 10f + baseTransform.up * 5f - baseTransform.right;
                                        obj5.rigidbody.AddForce(force, ForceMode.Impulse);
                                        Vector3 torque = new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100));
                                        obj5.rigidbody.AddTorque(torque, ForceMode.Acceleration);
                                    }
                                    if (!(rightGunHasBullet || !setup.part_blade_r.activeSelf))
                                    {
                                        setup.part_blade_r.SetActive(false);
                                        Transform transform5 = setup.part_blade_r.transform;
                                        GameObject obj6 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_r"), transform5.position, transform5.rotation);
                                        obj6.renderer.material = CharacterMaterials.materials[setup.myCostume.DmgTexture];
                                        Vector3 vector3 = -baseTransform.forward * 10f + baseTransform.up * 5f + baseTransform.right;
                                        obj6.rigidbody.AddForce(vector3, ForceMode.Impulse);
                                        Vector3 vector4 = new Vector3(UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-300, 300));
                                        obj6.rigidbody.AddTorque(vector4, ForceMode.Acceleration);
                                    }
                                }
                                if (baseAnimation[reloadAnimation].normalizedTime > 0.62f && !throwedBlades)
                                {
                                    throwedBlades = true;
                                    if (!(leftBulletLeft <= 0 || leftGunHasBullet))
                                    {
                                        leftBulletLeft--;
                                        setup.part_blade_l.SetActive(true);
                                        leftGunHasBullet = true;
                                    }
                                    if (!(rightBulletLeft <= 0 || rightGunHasBullet))
                                    {
                                        setup.part_blade_r.SetActive(true);
                                        rightBulletLeft--;
                                        rightGunHasBullet = true;
                                    }
                                    updateRightMagUI();
                                    updateLeftMagUI();
                                }
                                if (baseAnimation[reloadAnimation].normalizedTime > 1f)
                                {
                                    idle();
                                }
                            }
                            else
                            {
                                if (!grounded)
                                {
                                    if (!(base.animation[reloadAnimation].normalizedTime < 0.2f || throwedBlades))
                                    {
                                        throwedBlades = true;
                                        if (setup.part_blade_l.activeSelf)
                                        {
                                            throwBlades();
                                        }
                                    }
                                    if (base.animation[reloadAnimation].normalizedTime >= 0.56f && currentBladeNum > 0)
                                    {
                                        setup.part_blade_l.SetActive(true);
                                        setup.part_blade_r.SetActive(true);
                                        currentBladeSta = totalBladeSta;
                                    }
                                }
                                else
                                {
                                    if (!(baseAnimation[reloadAnimation].normalizedTime < 0.13f || throwedBlades))
                                    {
                                        throwedBlades = true;
                                        if (setup.part_blade_l.activeSelf)
                                        {
                                            throwBlades();
                                        }
                                    }
                                    if (baseAnimation[reloadAnimation].normalizedTime >= 0.37f && currentBladeNum > 0)
                                    {
                                        setup.part_blade_l.SetActive(true);
                                        setup.part_blade_r.SetActive(true);
                                        currentBladeSta = totalBladeSta;
                                    }
                                }
                                if (baseAnimation[reloadAnimation].normalizedTime >= 1f)
                                {
                                    idle();
                                }
                            }
                        }
                        else if (state == HERO_STATE.Salute)
                        {
                            if (baseAnimation["salute"].normalizedTime >= 1f)
                            {
                                idle();
                            }
                        }
                        else if (state == HERO_STATE.GroundDodge)
                        {
                            if (baseAnimation.IsPlaying("dodge"))
                            {
                                if (!(grounded || baseAnimation["dodge"].normalizedTime <= 0.6f))
                                {
                                    idle();
                                }
                                if (baseAnimation["dodge"].normalizedTime >= 1f)
                                {
                                    idle();
                                }
                            }
                        }
                        else if (state == HERO_STATE.Land)
                        {
                            if (baseAnimation.IsPlaying("dash_land") && baseAnimation["dash_land"].normalizedTime >= 1f)
                            {
                                idle();
                            }
                        }
                        else if (state == HERO_STATE.FillGas)
                        {
                            if (baseAnimation.IsPlaying("supply") && baseAnimation["supply"].normalizedTime >= 1f)
                            {
                                currentBladeSta = totalBladeSta;
                                currentBladeNum = totalBladeNum;
                                currentGas = totalGas;
                                if (!useGun)
                                {
                                    setup.part_blade_l.SetActive(true);
                                    setup.part_blade_r.SetActive(true);
                                }
                                else
                                {
                                    leftBulletLeft = rightBulletLeft = bulletMAX;
                                    rightGunHasBullet = true;
                                    leftGunHasBullet = true;
                                    setup.part_blade_l.SetActive(true);
                                    setup.part_blade_r.SetActive(true);
                                    updateRightMagUI();
                                    updateLeftMagUI();
                                }
                                idle();
                            }
                        }
                        else if (state == HERO_STATE.Slide)
                        {
                            if (!grounded)
                            {
                                idle();
                            }
                        }
                        else if (state == HERO_STATE.AirDodge)
                        {
                            if (dashTime > 0f)
                            {
                                dashTime -= Time.deltaTime;
                                if (currentSpeed > originVM)
                                {
                                    baseRigidBody.AddForce(-baseRigidBody.velocity * Time.deltaTime * 1.7f, ForceMode.VelocityChange);
                                }
                            }
                            else
                            {
                                dashTime = 0f;
                                idle();
                            }
                        }
                        if (inputManager.isInput[InputCode.leftRope])
                        {
                            ReflectorVariable0 = true;
                        }
                        else
                        {
                            ReflectorVariable0 = false;
                        }
                        if (!(ReflectorVariable0 ? (baseAnimation.IsPlaying("attack3_1") || baseAnimation.IsPlaying("attack5") || baseAnimation.IsPlaying("special_petra") || state == HERO_STATE.Grab ? state != HERO_STATE.Idle : false) : true))
                        {
                            if (bulletLeft != null)
                            {
                                QHold = true;
                            }
                            else
                            {
                                RaycastHit hit4;
                                Ray ray4 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                LayerMask mask10 = 1 << LayerMask.NameToLayer("Ground");
                                LayerMask mask11 = 1 << LayerMask.NameToLayer("EnemyBox");
                                LayerMask mask12 = mask11 | mask10;
                                if (Physics.Raycast(ray4, out hit4, 10000f, mask12.value))
                                {
                                    launchLeftRope(hit4, true, 0);
                                    rope.Play();
                                }
                            }
                        }
                        else
                        {
                            QHold = false;
                        }
                        if (inputManager.isInput[InputCode.rightRope])
                        {
                            ReflectorVariable1 = true;
                        }
                        else
                        {
                            ReflectorVariable1 = false;
                        }
                        if (!(ReflectorVariable1 ? (baseAnimation.IsPlaying("attack3_1") || baseAnimation.IsPlaying("attack5") || baseAnimation.IsPlaying("special_petra") || state == HERO_STATE.Grab ? state != HERO_STATE.Idle : false) : true))
                        {
                            if (bulletRight != null)
                            {
                                EHold = true;
                            }
                            else
                            {
                                RaycastHit hit5;
                                Ray ray5 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                LayerMask mask13 = 1 << LayerMask.NameToLayer("Ground");
                                LayerMask mask14 = 1 << LayerMask.NameToLayer("EnemyBox");
                                LayerMask mask15 = mask14 | mask13;
                                if (Physics.Raycast(ray5, out hit5, 10000f, mask15.value))
                                {
                                    launchRightRope(hit5, true, 0);
                                    rope.Play();
                                }
                            }
                        }
                        else
                        {
                            EHold = false;
                        }
                        if (inputManager.isInput[InputCode.bothRope])
                        {
                            ReflectorVariable2 = true;
                        }
                        else
                        {
                            ReflectorVariable2 = false;
                        }
                        if (!(ReflectorVariable2 ? (baseAnimation.IsPlaying("attack3_1") || baseAnimation.IsPlaying("attack5") || baseAnimation.IsPlaying("special_petra") || state == HERO_STATE.Grab ? state != HERO_STATE.Idle : false) : true))
                        {
                            QHold = true;
                            EHold = true;
                            if (bulletLeft == null && bulletRight == null)
                            {
                                RaycastHit hit6;
                                Ray ray6 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                LayerMask mask16 = 1 << LayerMask.NameToLayer("Ground");
                                LayerMask mask17 = 1 << LayerMask.NameToLayer("EnemyBox");
                                LayerMask mask18 = mask17 | mask16;
                                if (Physics.Raycast(ray6, out hit6, 1000000f, mask18.value))
                                {
                                    launchLeftRope(hit6, false, 0);
                                    launchRightRope(hit6, false, 0);
                                    rope.Play();
                                }
                            }
                        }
                        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && !IN_GAME_MAIN_CAMERA.isPausing)
                        {
                            SetSkillCD();
                            SetFlareCD();
                        }
                        if (!useGun)
                        {
                            if (leftbladetrail.gameObject.GetActive())
                            {
                                leftbladetrail.update();
                                rightbladetrail.update();
                            }
                            if (leftbladetrail2.gameObject.GetActive())
                            {
                                leftbladetrail2.update();
                                rightbladetrail2.update();
                            }
                            if (leftbladetrail.gameObject.GetActive())
                            {
                                leftbladetrail.lateUpdate();
                                rightbladetrail.lateUpdate();
                            }
                            if (leftbladetrail2.gameObject.GetActive())
                            {
                                leftbladetrail2.lateUpdate();
                                rightbladetrail2.lateUpdate();
                            }
                        }
                        if (!IN_GAME_MAIN_CAMERA.isPausing)
                        {
                            showSkillCD();
                            //this.showFlareCD2();
                            showGas2();
                            showAimUI2();
                        }
                    }
                    else if (isCannon && !IN_GAME_MAIN_CAMERA.isPausing)
                    {
                        showAimUI2();
                        SetSkillCD();
                        showSkillCD();
                    }
                }
            }
        }
    }

    public void updateCannon()
    {
        baseTransform.position = myCannonPlayer.position;
        baseTransform.rotation = myCannonBase.rotation;
    }

    public void updateExt()
    {
        if (skillId == "bomb")
        {
            if (inputManager.isInputDown[InputCode.attack1] && skillCDDuration <= 0f)
            {
                if (!(myBomb == null || myBomb.disabled))
                {
                    myBomb.Explode(bombRadius);
                }
                detonate = false;
                skillCDDuration = bombCD;
                RaycastHit hitInfo = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
                LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
                LayerMask mask3 = mask2 | mask;
                currentV = baseTransform.position;
                targetV = currentV + Vector3.forward * 200f;
                if (Physics.Raycast(ray, out hitInfo, 1000000f, mask3.value))
                {
                    targetV = hitInfo.point;
                }
                Vector3 vector = Vector3.Normalize(targetV - currentV);
                GameObject obj2 = PhotonNetwork.Instantiate("RCAsset/BombMain", currentV + vector * 4f, new Quaternion(0f, 0f, 0f, 1f), 0);
                obj2.rigidbody.velocity = vector * bombSpeed;
                myBomb = obj2.GetComponent<Bomb>();
                bombTime = 0f;
            }
            else if (myBomb != null && !myBomb.disabled)
            {
                bombTime += Time.deltaTime;
                bool flag2 = false;
                if (inputManager.isInputUp[InputCode.attack1])
                {
                    detonate = true;
                }
                else if (inputManager.isInputDown[InputCode.attack1] && detonate)
                {
                    detonate = false;
                    flag2 = true;
                }
                if (bombTime >= bombTimeMax)
                {
                    flag2 = true;
                }
                if (flag2)
                {
                    myBomb.Explode(bombRadius);
                    detonate = false;
                }
            }
        }
    }

    private void updateLeftMagUI()
    {
        for (int i = 1; i <= bulletMAX; i++)
        {
            GameObject.Find("bulletL" + i).GetComponent<UISprite>().enabled = false;
        }
        for (int j = 1; j <= leftBulletLeft; j++)
        {
            GameObject.Find("bulletL" + j).GetComponent<UISprite>().enabled = true;
        }
    }

    private void updateRightMagUI()
    {
        for (int i = 1; i <= bulletMAX; i++)
        {
            GameObject.Find("bulletR" + i).GetComponent<UISprite>().enabled = false;
        }
        for (int j = 1; j <= rightBulletLeft; j++)
        {
            GameObject.Find("bulletR" + j).GetComponent<UISprite>().enabled = true;
        }
    }

    public void useBlade(int amount = 0)
    {
        if (ModManager.Find("module.infiniteblade").Enabled) return;
        if (amount == 0)
        {
            amount = 1;
        }
        amount *= 2;
        if (currentBladeSta > 0f)
        {
            currentBladeSta -= amount;
            if (currentBladeSta <= 0f)
            {
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
                {
                    leftbladetrail.Deactivate();
                    rightbladetrail.Deactivate();
                    leftbladetrail2.Deactivate();
                    rightbladetrail2.Deactivate();
                    checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                    checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                }
                currentBladeSta = 0f;
                throwBlades();
            }
        }
    }

    private void useGas(float amount = 0)
    {
        if (ModManager.Find("module.infinitegas").Enabled) return;
        if (amount == 0f)
        {
            amount = useGasSpeed;
        }
        if (currentGas > 0f)
        {
            currentGas -= amount;
            if (currentGas < 0f)
            {
                currentGas = 0f;
            }
        }
    }

    [RPC]
    private void whoIsMyErenTitan(int id)
    {
        eren_titan = PhotonView.Find(id).gameObject;
        titanForm = true;
    }

    public bool isGrabbed
    {
        get
        {
            return state == HERO_STATE.Grab;
        }
    }

    private HERO_STATE state
    {
        get
        {
            return _state;
        }
        set
        {
            if (_state == HERO_STATE.AirDodge || _state == HERO_STATE.GroundDodge)
            {
                dashTime = 0f;
            }
            _state = value;
        }
    }
}