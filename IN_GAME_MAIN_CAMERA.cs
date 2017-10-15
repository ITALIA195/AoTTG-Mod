using System;
using System.Runtime.InteropServices;
using Mod;
using Mod.manager;
using UnityEngine;

public class IN_GAME_MAIN_CAMERA : MonoBehaviour
{
    public static IN_GAME_MAIN_CAMERA instance;
    public RotationAxes axes;
    public AudioSource bgmusic;
    public static float cameraDistance = 0.6f;
    public static CAMERA_TYPE cameraMode;
    public static int cameraTilt = 1;
    public static int character = 1;
    private float closestDistance;
    private int currentPeekPlayerIndex;
    public static DayLight dayLight = DayLight.Dawn;
    private float decay;
    public static int difficulty;
    private  float distance = 10f;
    private float distanceMulti;
    private float distanceOffsetMulti;
    private float duration;
    private float flashDuration;
    private bool flip;
    public static GAMEMODE gamemode;
    public bool gameOver;
    public static GAMETYPE gametype = GAMETYPE.STOP;
    private bool hasSnapShot;
    private Transform head;
    public float height = 5f;
    public float heightDamping = 2f;
    private float heightMulti;
    public FengCustomInputs inputManager;
    public static int invertY = 1;
    public static bool isCheating;
    public static bool isPausing;
    public static bool isTyping;
    public float justHit;
    public int lastScore;
    public static int level;
    private bool lockAngle;
    private Vector3 lockCameraPosition;
    private GameObject locker;
    private GameObject lockTarget;
    public GameObject main_object;
    public float maximumX = 360f;
    public float maximumY = 60f;
    public float minimumX = -360f;
    public float minimumY = -60f;
    private bool needSetHUD;
    private float R;
    public float rotationY;
    public int score;
    public static float sensitivityMulti = 0.5f;
    public static string singleCharacter;
    public Material skyBoxDAWN;
    public Material skyBoxDAY;
    public Material skyBoxNIGHT;
    private Texture2D snapshot1;
    private Texture2D snapshot2;
    private Texture2D snapshot3;
    public GameObject snapShotCamera;
    private int snapShotCount;
    private float snapShotCountDown;
    private int snapShotDmg;
    private float snapShotInterval = 0.02f;
    public RenderTexture snapshotRT;
    private float snapShotStartCountDownTime;
    private GameObject snapShotTarget;
    private Vector3 snapShotTargetPosition;
    public bool spectatorMode;
    private bool startSnapShotFrameCount;
    public static STEREO_3D_TYPE stereoType;
    public Transform target;
    public Texture texture;
    public float timer;
    public static bool triggerAutoLock;
    public static bool usingTitan;
    private Vector3 verticalHeightOffset = Vector3.zero;
    public float verticalRotationOffset;
    public float xSpeed = -3f;
    public float ySpeed = -0.8f;

    private void Awake()
    {
        isTyping = false;
        isPausing = false;
        name = "MainCamera";
        if (PlayerPrefs.HasKey("GameQuality"))
        {
            if (PlayerPrefs.GetFloat("GameQuality") >= 0.9f)
            {
                GetComponent<TiltShift>().enabled = true;
            }
            else
            {
                GetComponent<TiltShift>().enabled = false;
            }
        }
        else
        {
            GetComponent<TiltShift>().enabled = true;
        }
        CreateMinimap();
    }

    private void CameraMovement()
    {
        distanceOffsetMulti = (cameraDistance * (200f - camera.fieldOfView)) / 150f;
        transform.position = (head == null) ? main_object.transform.position : head.transform.position;
        Transform transform1 = transform;
        transform1.position += (Vector3) (Vector3.up * heightMulti);
        Transform transform2 = transform;
        transform2.position -= (Vector3) ((Vector3.up * (0.6f - cameraDistance)) * 2f);
        if (cameraMode == CAMERA_TYPE.WOW)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                float angle = (Input.GetAxis("Mouse X") * 10f) * getSensitivityMulti();
                float num2 = ((-Input.GetAxis("Mouse Y") * 10f) * getSensitivityMulti()) * getReverse();
                transform.RotateAround(transform.position, Vector3.up, angle);
                transform.RotateAround(transform.position, transform.right, num2);
            }
            Transform transform3 = transform;
            transform3.position -= (Vector3) (((transform.forward * distance) * distanceMulti) * distanceOffsetMulti);
        }
        else if (cameraMode == CAMERA_TYPE.ORIGINAL)
        {
            float num3 = 0f;
            if (Input.mousePosition.x < (Screen.width * 0.4f))
            {
                num3 = (-((((Screen.width * 0.4f) - Input.mousePosition.x) / ((float) Screen.width)) * 0.4f) * getSensitivityMultiWithDeltaTime()) * 150f;
                transform.RotateAround(transform.position, Vector3.up, num3);
            }
            else if (Input.mousePosition.x > (Screen.width * 0.6f))
            {
                num3 = ((((Input.mousePosition.x - (Screen.width * 0.6f)) / ((float) Screen.width)) * 0.4f) * getSensitivityMultiWithDeltaTime()) * 150f;
                transform.RotateAround(transform.position, Vector3.up, num3);
            }
            float x = ((140f * ((Screen.height * 0.6f) - Input.mousePosition.y)) / ((float) Screen.height)) * 0.5f;
            transform.rotation = Quaternion.Euler(x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            Transform transform4 = transform;
            transform4.position -= (Vector3) (((transform.forward * distance) * distanceMulti) * distanceOffsetMulti);
        }
        else if (cameraMode == CAMERA_TYPE.TPS)
        {
            if (!inputManager.menuOn)
            {
                Screen.lockCursor = true;
            }
            float num5 = (Input.GetAxis("Mouse X") * 10f) * getSensitivityMulti();
            float num6 = ((-Input.GetAxis("Mouse Y") * 10f) * getSensitivityMulti()) * getReverse();
            transform.RotateAround(transform.position, Vector3.up, num5);
            float num7 = transform.rotation.eulerAngles.x % 360f;
            float num8 = num7 + num6;
            if (((num6 <= 0f) || (((num7 >= 260f) || (num8 <= 260f)) && ((num7 >= 80f) || (num8 <= 80f)))) && ((num6 >= 0f) || (((num7 <= 280f) || (num8 >= 280f)) && ((num7 <= 100f) || (num8 >= 100f)))))
            {
                transform.RotateAround(transform.position, transform.right, num6);
            }
            Transform transform5 = transform;
            transform5.position -= (Vector3) (((transform.forward * distance) * distanceMulti) * distanceOffsetMulti);
        }
        if (cameraDistance < 0.65f)
        {
            Transform transform6 = transform;
            transform6.position += (Vector3) (transform.right * Mathf.Max((float) ((0.6f - cameraDistance) * 2f), (float) 0.65f));
        }
    }

    public void CameraMovementLive(HERO hero)
    {
        float magnitude = hero.rigidbody.velocity.magnitude;
        if (magnitude > 10f)
        {
                        if (!ModManager.Find("module.customfov").Enabled)
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, Mathf.Min((float) 100f, (float) (magnitude + 40f)), 0.1f);
        }
        else
        {
                        if (!ModManager.Find("module.customfov").Enabled)
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 50f, 0.1f);
        }
        float num2 = (hero.CameraMultiplier * (200f - Camera.main.fieldOfView)) / 150f;
        this.transform.position = (Vector3) ((head.transform.position + (Vector3.up * heightMulti)) - ((Vector3.up * (0.6f - cameraDistance)) * 2f));
        Transform transform = this.transform;
        transform.position -= (Vector3) (((this.transform.forward * distance) * distanceMulti) * num2);
        if (hero.CameraMultiplier < 0.65f)
        {
            Transform transform2 = this.transform;
            transform2.position += (Vector3) (this.transform.right * Mathf.Max((float) ((0.6f - hero.CameraMultiplier) * 2f), (float) 0.65f));
        }
        this.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, hero.GetComponent<SmoothSyncMovement>().correctCameraRot, Time.deltaTime * 5f);
    }

    private void CreateMinimap()
    {
        LevelInfo info = LevelInfo.GetInfo(FengGameManagerMKII.level);
        if (info != null)
        {
            Minimap minimap = gameObject.AddComponent<Minimap>();
            if (Minimap.instance.myCam == null)
            {
                Minimap.instance.myCam = new GameObject().AddComponent<Camera>();
                Minimap.instance.myCam.nearClipPlane = 0.3f;
                Minimap.instance.myCam.farClipPlane = 1000f;
                Minimap.instance.myCam.enabled = false;
            }
            minimap.CreateMinimap(Minimap.instance.myCam, 512, 0.3f, info.minimapPreset);
            if ((((int) FengGameManagerMKII.settings[231]) == 0) || (RCSettings.globalDisableMinimap == 1))
            {
                minimap.SetEnabled(false);
            }
        }
    }

    public void createSnapShotRT()
    {
        if (snapShotCamera.GetComponent<Camera>().targetTexture != null)
        {
            snapShotCamera.GetComponent<Camera>().targetTexture.Release();
        }
        if (QualitySettings.GetQualityLevel() > 3)
        {
            snapShotCamera.GetComponent<Camera>().targetTexture = new RenderTexture((int) (Screen.width * 0.8f), (int) (Screen.height * 0.8f), 24);
        }
        else
        {
            snapShotCamera.GetComponent<Camera>().targetTexture = new RenderTexture((int) (Screen.width * 0.4f), (int) (Screen.height * 0.4f), 24);
        }
    }

    public void createSnapShotRT2()
    {
        if (snapshotRT != null)
        {
            snapshotRT.Release();
        }
        if (QualitySettings.GetQualityLevel() > 3)
        {
            snapshotRT = new RenderTexture((int) (Screen.width * 0.8f), (int) (Screen.height * 0.8f), 24);
            snapShotCamera.GetComponent<Camera>().targetTexture = snapshotRT;
        }
        else
        {
            snapshotRT = new RenderTexture((int) (Screen.width * 0.4f), (int) (Screen.height * 0.4f), 24);
            snapShotCamera.GetComponent<Camera>().targetTexture = snapshotRT;
        }
    }

    private GameObject findNearestTitan()
    {
        GameObject[] objArray = GameObject.FindGameObjectsWithTag("titan");
        GameObject obj2 = null;
        float positiveInfinity = float.PositiveInfinity;
        closestDistance = float.PositiveInfinity;
        float num2 = positiveInfinity;
        Vector3 position = main_object.transform.position;
        foreach (GameObject obj3 in objArray)
        {
            Vector3 vector2 = obj3.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position - position;
            float magnitude = vector2.magnitude;
            if ((magnitude < num2) && ((obj3.GetComponent<TITAN>() == null) || !obj3.GetComponent<TITAN>().hasDie))
            {
                obj2 = obj3;
                num2 = magnitude;
                closestDistance = num2;
            }
        }
        return obj2;
    }

    public void FlashBlind()
    {
        if (!GameObject.Find("flash")) return;
        GameObject.Find("flash").GetComponent<UISprite>().alpha = 1f;
        GameObject.Find("flash").GetComponent<UISprite>().color = InterfaceManager.FloatColor("00FFFF"); //MOD: lul
        flashDuration = 2f;
    }

    private int getReverse()
    {
        return invertY;
    }

    private float getSensitivityMulti()
    {
        return sensitivityMulti;
    }

    private float getSensitivityMultiWithDeltaTime()
    {
        return ((sensitivityMulti * Time.deltaTime) * 62f);
    }

    private void reset()
    {
        if (gametype == GAMETYPE.SINGLE)
        {
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().RestartGameSingle();
        }
    }

    private Texture2D RTImage(Camera cam)
    {
        RenderTexture active = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;
        cam.Render();
        Texture2D textured = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        int num = (int) (cam.targetTexture.width * 0.04f);
        int destX = (int) (cam.targetTexture.width * 0.02f);
        textured.ReadPixels(new Rect((float) num, (float) num, (float) (cam.targetTexture.width - num), (float) (cam.targetTexture.height - num)), destX, destX);
        textured.Apply();
        RenderTexture.active = active;
        return textured;
    }

    private Texture2D RTImage2(Camera cam)
    {
        RenderTexture active = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;
        cam.Render();
        Texture2D textured = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        int num = (int) (cam.targetTexture.width * 0.04f);
        int destX = (int) (cam.targetTexture.width * 0.02f);
        try
        {
            textured.SetPixel(0, 0, Color.white);
            textured.ReadPixels(new Rect((float) num, (float) num, (float) (cam.targetTexture.width - num), (float) (cam.targetTexture.height - num)), destX, destX);
            textured.Apply();
            RenderTexture.active = active;
        }
        catch
        {
            textured = new Texture2D(1, 1);
            textured.SetPixel(0, 0, Color.white);
            return textured;
        }
        return textured;
    }

    public void setDayLight(DayLight val)
    {
        dayLight = val;
        if (dayLight == DayLight.Night)
        {
            GameObject obj2 = (GameObject) Instantiate(Resources.Load("flashlight"));
            obj2.transform.parent = transform;
            obj2.transform.position = transform.position;
            obj2.transform.rotation = Quaternion.Euler(353f, 0f, 0f);
            RenderSettings.ambientLight = FengColor.nightAmbientLight;
            GameObject.Find("mainLight").GetComponent<Light>().color = FengColor.nightLight;
            gameObject.GetComponent<Skybox>().material = skyBoxNIGHT;
        }
        if (dayLight == DayLight.Day)
        {
            RenderSettings.ambientLight = FengColor.dayAmbientLight;
            GameObject.Find("mainLight").GetComponent<Light>().color = FengColor.dayLight;
            gameObject.GetComponent<Skybox>().material = skyBoxDAY;
        }
        if (dayLight == DayLight.Dawn)
        {
            RenderSettings.ambientLight = FengColor.dawnAmbientLight;
            GameObject.Find("mainLight").GetComponent<Light>().color = FengColor.dawnAmbientLight;
            gameObject.GetComponent<Skybox>().material = skyBoxDAWN;
        }
        snapShotCamera.gameObject.GetComponent<Skybox>().material = gameObject.GetComponent<Skybox>().material;
    }

    public void setHUDposition()
    {
        GameObject.Find("Flare").transform.localPosition = new Vector3((float) (((int) (-Screen.width * 0.5f)) + 14), (float) ((int) (-Screen.height * 0.5f)), 0f);
        GameObject obj2 = GameObject.Find("LabelInfoBottomRight");
        obj2.transform.localPosition = new Vector3((float) ((int) (Screen.width * 0.5f)), (float) ((int) (-Screen.height * 0.5f)), 0f);
        obj2.GetComponent<UILabel>().text = "Pause : " + GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().inputString[InputCode.pause] + " ";
        GameObject.Find("LabelInfoTopCenter").transform.localPosition = new Vector3(0f, (float) ((int) (Screen.height * 0.5f)), 0f);
        GameObject.Find("LabelInfoTopRight").transform.localPosition = new Vector3((float) ((int) (Screen.width * 0.5f)), (float) ((int) (Screen.height * 0.5f)), 0f);
        //GameObject.Find("LabelNetworkStatus").transform.localPosition = new Vector3((float) ((int) (-Screen.width * 0.5f)), (float) ((int) (Screen.height * 0.5f)), 0f);
        GameObject.Find("LabelInfoTopLeft").transform.localPosition = new Vector3((float) ((int) (-Screen.width * 0.5f)), (float) ((int) ((Screen.height * 0.5f) - 20f)), 0f);
        //GameObject.Find("Chatroom").transform.localPosition = new Vector3((float) ((int) (-Screen.width * 0.5f)), (float) ((int) (-Screen.height * 0.5f)), 0f);
        //if (GameObject.Find("Chatroom") != null)
        //{
        //    GameObject.Find("Chatroom").GetComponent<InRoomChat>().setPosition();
        //}
        if (usingTitan && (gametype != GAMETYPE.SINGLE))
        {
            Vector3 vector = new Vector3(0f, 9999f, 0f);
            GameObject.Find("skill_cd_bottom").transform.localPosition = vector;
            GameObject.Find("skill_cd_armin").transform.localPosition = vector;
            GameObject.Find("skill_cd_eren").transform.localPosition = vector;
            GameObject.Find("skill_cd_jean").transform.localPosition = vector;
            GameObject.Find("skill_cd_levi").transform.localPosition = vector;
            GameObject.Find("skill_cd_marco").transform.localPosition = vector;
            GameObject.Find("skill_cd_mikasa").transform.localPosition = vector;
            GameObject.Find("skill_cd_petra").transform.localPosition = vector;
            GameObject.Find("skill_cd_sasha").transform.localPosition = vector;
            GameObject.Find("GasUI").transform.localPosition = vector;
            GameObject.Find("stamina_titan").transform.localPosition = new Vector3(-160f, (float) ((int) ((-Screen.height * 0.5f) + 15f)), 0f);
            GameObject.Find("stamina_titan_bottom").transform.localPosition = new Vector3(-160f, (float) ((int) ((-Screen.height * 0.5f) + 15f)), 0f);
        }
        else
        {
            GameObject.Find("skill_cd_bottom").transform.localPosition = new Vector3(0f, (float) ((int) ((-Screen.height * 0.5f) + 5f)), 0f);
            GameObject.Find("GasUI").transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
            GameObject.Find("stamina_titan").transform.localPosition = new Vector3(0f, 9999f, 0f);
            GameObject.Find("stamina_titan_bottom").transform.localPosition = new Vector3(0f, 9999f, 0f);
        }
        if ((main_object != null) && (main_object.GetComponent<HERO>() != null))
        {
            if (gametype == GAMETYPE.SINGLE)
            {
                main_object.GetComponent<HERO>().setSkillHUDPosition2();
            }
            else if ((main_object.GetPhotonView() != null) && main_object.GetPhotonView().isMine)
            {
                main_object.GetComponent<HERO>().setSkillHUDPosition2();
            }
        }
        if (stereoType == STEREO_3D_TYPE.SIDE_BY_SIDE)
        {
            gameObject.GetComponent<Camera>().aspect = Screen.width / Screen.height;
        }
        createSnapShotRT2();
    }

    public GameObject setMainObject(GameObject obj, bool resetRotation = true, bool lockAngle = false)
    {
        float num;
        main_object = obj;
        if (obj == null)
        {
            head = null;
            num = 1f;
            heightMulti = 1f;
            distanceMulti = num;
        }
        else if (main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head") != null)
        {
            head = main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
            distanceMulti = (head != null) ? (Vector3.Distance(head.transform.position, main_object.transform.position) * 0.2f) : 1f;
            heightMulti = (head != null) ? (Vector3.Distance(head.transform.position, main_object.transform.position) * 0.33f) : 1f;
            if (resetRotation)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else if (main_object.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head") != null)
        {
            head = main_object.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
            num = 0.64f;
            heightMulti = 0.64f;
            distanceMulti = num;
            if (resetRotation)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else
        {
            head = null;
            num = 1f;
            heightMulti = 1f;
            distanceMulti = num;
            if (resetRotation)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        this.lockAngle = lockAngle;
        return obj;
    }

    public GameObject setMainObjectASTITAN(GameObject obj)
    {
        main_object = obj;
        if (main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head") != null)
        {
            head = main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
            distanceMulti = (head != null) ? (Vector3.Distance(head.transform.position, main_object.transform.position) * 0.4f) : 1f;
            heightMulti = (head != null) ? (Vector3.Distance(head.transform.position, main_object.transform.position) * 0.45f) : 1f;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        return obj;
    }

    public void setSpectorMode(bool val)
    {
        spectatorMode = val;
        GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = !val;
        GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = !val;
    }

    private void shakeUpdate()
    {
        if (duration > 0f)
        {
            duration -= Time.deltaTime;
            if (flip)
            {
                Transform transform = gameObject.transform;
                transform.position += (Vector3) (Vector3.up * R);
            }
            else
            {
                Transform transform2 = gameObject.transform;
                transform2.position -= (Vector3) (Vector3.up * R);
            }
            flip = !flip;
            R *= decay;
        }
    }

    public void snapShot2(int index)
    {
        Vector3 vector;
        RaycastHit hit;
        snapShotCamera.transform.position = (head == null) ? main_object.transform.position : head.transform.position;
        Transform transform = snapShotCamera.transform;
        transform.position += (Vector3) (Vector3.up * heightMulti);
        Transform transform2 = snapShotCamera.transform;
        transform2.position -= (Vector3) (Vector3.up * 1.1f);
        Vector3 worldPosition = vector = snapShotCamera.transform.position;
        Vector3 vector3 = (Vector3) ((worldPosition + snapShotTargetPosition) * 0.5f);
        snapShotCamera.transform.position = vector3;
        worldPosition = vector3;
        snapShotCamera.transform.LookAt(snapShotTargetPosition);
        if (index == 3)
        {
            snapShotCamera.transform.RotateAround(this.transform.position, Vector3.up, UnityEngine.Random.Range((float) -180f, (float) 180f));
        }
        else
        {
            snapShotCamera.transform.RotateAround(this.transform.position, Vector3.up, UnityEngine.Random.Range((float) -20f, (float) 20f));
        }
        snapShotCamera.transform.LookAt(worldPosition);
        snapShotCamera.transform.RotateAround(worldPosition, this.transform.right, UnityEngine.Random.Range((float) -20f, (float) 20f));
        float num = Vector3.Distance(snapShotTargetPosition, vector);
        if ((snapShotTarget != null) && (snapShotTarget.GetComponent<TITAN>() != null))
        {
            num += ((index - 1) * snapShotTarget.transform.localScale.x) * 10f;
        }
        Transform transform3 = snapShotCamera.transform;
        transform3.position -= (Vector3) (snapShotCamera.transform.forward * UnityEngine.Random.Range((float) (num + 3f), (float) (num + 10f)));
        snapShotCamera.transform.LookAt(worldPosition);
        snapShotCamera.transform.RotateAround(worldPosition, this.transform.forward, UnityEngine.Random.Range((float) -30f, (float) 30f));
        Vector3 end = (head == null) ? main_object.transform.position : head.transform.position;
        Vector3 vector5 = ((head == null) ? main_object.transform.position : head.transform.position) - snapShotCamera.transform.position;
        end -= vector5;
        LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask | mask2;
        if (head != null)
        {
            if (Physics.Linecast(head.transform.position, end, out hit, (int) mask))
            {
                snapShotCamera.transform.position = hit.point;
            }
            else if (Physics.Linecast(head.transform.position - ((Vector3) ((vector5 * distanceMulti) * 3f)), end, out hit, (int) mask3))
            {
                snapShotCamera.transform.position = hit.point;
            }
        }
        else if (Physics.Linecast(main_object.transform.position + Vector3.up, end, out hit, (int) mask3))
        {
            snapShotCamera.transform.position = hit.point;
        }
        switch (index)
        {
            case 1:
                snapshot1 = RTImage2(snapShotCamera.GetComponent<Camera>());
                SnapShotSaves.addIMG(snapshot1, snapShotDmg);
                break;

            case 2:
                snapshot2 = RTImage2(snapShotCamera.GetComponent<Camera>());
                SnapShotSaves.addIMG(snapshot2, snapShotDmg);
                break;

            case 3:
                snapshot3 = RTImage2(snapShotCamera.GetComponent<Camera>());
                SnapShotSaves.addIMG(snapshot3, snapShotDmg);
                break;
        }
        snapShotCount = index;
        hasSnapShot = true;
        snapShotCountDown = 2f;
        if (index == 1)
        {
            GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().mainTexture = snapshot1;
            GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().transform.localScale = new Vector3(Screen.width * 0.4f, Screen.height * 0.4f, 1f);
            GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().transform.localPosition = new Vector3(-Screen.width * 0.225f, Screen.height * 0.225f, 0f);
            GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().transform.rotation = Quaternion.Euler(0f, 0f, 10f);
            if (PlayerPrefs.HasKey("showSSInGame") && (PlayerPrefs.GetInt("showSSInGame") == 1))
            {
                GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().enabled = true;
            }
            else
            {
                GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().enabled = false;
            }
        }
    }

    public void snapShotUpdate()
    {
        if (startSnapShotFrameCount)
        {
            snapShotStartCountDownTime -= Time.deltaTime;
            if (snapShotStartCountDownTime <= 0f)
            {
                snapShot2(1);
                startSnapShotFrameCount = false;
            }
        }
        if (hasSnapShot)
        {
            snapShotCountDown -= Time.deltaTime;
            if (snapShotCountDown <= 0f)
            {
                GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().enabled = false;
                hasSnapShot = false;
                snapShotCountDown = 0f;
            }
            else if (snapShotCountDown < 1f)
            {
                GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().mainTexture = snapshot3;
            }
            else if (snapShotCountDown < 1.5f)
            {
                GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().mainTexture = snapshot2;
            }
            if (snapShotCount < 3)
            {
                snapShotInterval -= Time.deltaTime;
                if (snapShotInterval <= 0f)
                {
                    snapShotInterval = 0.05f;
                    snapShotCount++;
                    snapShot2(snapShotCount);
                }
            }
        }
    }

    private void Start()
    {
        instance = this;
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().SetMainCamera(this);
        isPausing = false;
        sensitivityMulti = PlayerPrefs.GetFloat("MouseSensitivity");
        invertY = PlayerPrefs.GetInt("invertMouseY");
        inputManager = GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();
        setDayLight(dayLight);
        locker = GameObject.Find("locker");
        cameraTilt = PlayerPrefs.GetInt("cameraTilt", 1);
        if (PlayerPrefs.HasKey("cameraDistance"))
            cameraDistance = PlayerPrefs.GetFloat("cameraDistance") + 0.3f;
        createSnapShotRT2();
    }

    public void startShake(float R, float duration, float decay = 0.95f)
    {
        if (this.duration < duration)
        {
            this.R = R;
            this.duration = duration;
            this.decay = decay;
        }
    }

    public void startSnapShot(Vector3 p, int dmg, GameObject target = null, float startTime = 0.02f)
    {
        snapShotCount = 1;
        startSnapShotFrameCount = true;
        snapShotTargetPosition = p;
        snapShotTarget = target;
        snapShotStartCountDownTime = startTime;
        snapShotInterval = 0.05f + UnityEngine.Random.Range((float) 0f, (float) 0.03f);
        snapShotDmg = dmg;
    }

    public void startSnapShot2(Vector3 p, int dmg, GameObject target, float startTime)
    {
        int num;
        if (int.TryParse((string) FengGameManagerMKII.settings[95], out num))
        {
            if (dmg >= num)
            {
                snapShotCount = 1;
                startSnapShotFrameCount = true;
                snapShotTargetPosition = p;
                snapShotTarget = target;
                snapShotStartCountDownTime = startTime;
                snapShotInterval = 0.05f + UnityEngine.Random.Range((float) 0f, (float) 0.03f);
                snapShotDmg = dmg;
            }
        }
        else
        {
            snapShotCount = 1;
            startSnapShotFrameCount = true;
            snapShotTargetPosition = p;
            snapShotTarget = target;
            snapShotStartCountDownTime = startTime;
            snapShotInterval = 0.05f + UnityEngine.Random.Range((float) 0f, (float) 0.03f);
            snapShotDmg = dmg;
        }
    }

    public void update()
    {
        if (flashDuration > 0f)
        {
            flashDuration -= Time.deltaTime;
            if (flashDuration <= 0f)
            {
                flashDuration = 0f;
            }
            GameObject.Find("flash").GetComponent<UISprite>().alpha = flashDuration * 0.5f;
        }
        if (gametype == GAMETYPE.STOP)
        {
            Screen.showCursor = true;
            Screen.lockCursor = false;
        }
        else
        {
            if ((gametype != GAMETYPE.SINGLE) && gameOver)
            {
                if (inputManager.isInputDown[InputCode.attack1])
                {
                    if (spectatorMode)
                    {
                        setSpectorMode(false);
                    }
                    else
                    {
                        setSpectorMode(true);
                    }
                }
                if (inputManager.isInputDown[InputCode.flare1])
                {
                    currentPeekPlayerIndex++;
                    int length = GameObject.FindGameObjectsWithTag("Player").Length;
                    if (currentPeekPlayerIndex >= length)
                    {
                        currentPeekPlayerIndex = 0;
                    }
                    if (length > 0)
                    {
                        setMainObject(GameObject.FindGameObjectsWithTag("Player")[currentPeekPlayerIndex], true, false);
                        setSpectorMode(false);
                        lockAngle = false;
                    }
                }
                if (inputManager.isInputDown[InputCode.flare2])
                {
                    currentPeekPlayerIndex--;
                    int num2 = GameObject.FindGameObjectsWithTag("Player").Length;
                    if (currentPeekPlayerIndex >= num2)
                    {
                        currentPeekPlayerIndex = 0;
                    }
                    if (currentPeekPlayerIndex < 0)
                    {
                        currentPeekPlayerIndex = num2;
                    }
                    if (num2 > 0)
                    {
                        setMainObject(GameObject.FindGameObjectsWithTag("Player")[currentPeekPlayerIndex], true, false);
                        setSpectorMode(false);
                        lockAngle = false;
                    }
                }
                if (spectatorMode)
                {
                    return;
                }
            }
            if (inputManager.isInputDown[InputCode.pause])
            {
                if (isPausing)
                {
                    if (main_object != null)
                    {
                        Vector3 position = transform.position;
                        position = (head == null) ? main_object.transform.position : head.transform.position;
                        position += (Vector3) (Vector3.up * heightMulti);
                        transform.position = Vector3.Lerp(transform.position, position - ((Vector3) (transform.forward * 5f)), 0.2f);
                    }
                    return;
                }
                isPausing = !isPausing;
                if (isPausing)
                {
                    if (gametype == GAMETYPE.SINGLE)
                    {
                        Time.timeScale = 0f;
                    }
                    GameObject obj3 = GameObject.Find("UI_IN_GAME");
                    NGUITools.SetActive(obj3.GetComponent<UIReferArray>().panels[0], false);
                    NGUITools.SetActive(obj3.GetComponent<UIReferArray>().panels[1], true);
                    NGUITools.SetActive(obj3.GetComponent<UIReferArray>().panels[2], false);
                    NGUITools.SetActive(obj3.GetComponent<UIReferArray>().panels[3], false);
                    GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().showKeyMap();
                    GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().justUPDATEME();
                    GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = true;
                    Screen.showCursor = true;
                    Screen.lockCursor = false;
                }
            }
            if (needSetHUD)
            {
                needSetHUD = false;
                setHUDposition();
            }
            if (inputManager.isInputDown[InputCode.fullscreen])
            {
                Screen.fullScreen = !Screen.fullScreen;
                if (Screen.fullScreen)
                {
                    Screen.SetResolution(960, 600, false);
                }
                else
                {
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                }
                needSetHUD = true;
            }
            if (inputManager.isInputDown[InputCode.restart])
            {
                reset();
            }
            if (main_object != null)
            {
                RaycastHit hit;
                if (inputManager.isInputDown[InputCode.camera])
                {
                    if (cameraMode == CAMERA_TYPE.ORIGINAL)
                    {
                        cameraMode = CAMERA_TYPE.WOW;
                        Screen.lockCursor = false;
                    }
                    else if (cameraMode == CAMERA_TYPE.WOW)
                    {
                        cameraMode = CAMERA_TYPE.TPS;
                        Screen.lockCursor = true;
                    }
                    else if (cameraMode == CAMERA_TYPE.TPS)
                    {
                        cameraMode = CAMERA_TYPE.ORIGINAL;
                        Screen.lockCursor = false;
                    }
                    verticalRotationOffset = 0f;
                }
                if (inputManager.isInputDown[InputCode.hideCursor])
                {
                    Screen.showCursor = !Screen.showCursor;
                }
                if (inputManager.isInputDown[InputCode.focus])
                {
                    triggerAutoLock = !triggerAutoLock;
                    if (triggerAutoLock)
                    {
                        lockTarget = findNearestTitan();
                        if (closestDistance >= 150f)
                        {
                            lockTarget = null;
                            triggerAutoLock = false;
                        }
                    }
                }
                if ((gameOver && lockAngle) && (main_object != null))
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, main_object.transform.rotation, 0.2f);
                    transform.position = Vector3.Lerp(transform.position, main_object.transform.position - ((Vector3) (main_object.transform.forward * 5f)), 0.2f);
                }
                else
                {
                    CameraMovement();
                }
                if (triggerAutoLock && (lockTarget != null))
                {
                    float z = this.transform.eulerAngles.z;
                    Transform transform = lockTarget.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                    Vector3 vector3 = transform.position - ((head == null) ? main_object.transform.position : head.transform.position);
                    vector3.Normalize();
                    lockCameraPosition = (head == null) ? main_object.transform.position : head.transform.position;
                    lockCameraPosition -= (Vector3) (((vector3 * distance) * distanceMulti) * distanceOffsetMulti);
                    lockCameraPosition += (Vector3) (((Vector3.up * 3f) * heightMulti) * distanceOffsetMulti);
                    this.transform.position = Vector3.Lerp(this.transform.position, lockCameraPosition, Time.deltaTime * 4f);
                    if (head != null)
                    {
                        this.transform.LookAt((Vector3) ((head.transform.position * 0.8f) + (transform.position * 0.2f)));
                    }
                    else
                    {
                        this.transform.LookAt((Vector3) ((main_object.transform.position * 0.8f) + (transform.position * 0.2f)));
                    }
                    this.transform.localEulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, z);
                    Vector2 vector7 = camera.WorldToScreenPoint(transform.position - ((Vector3) (transform.forward * lockTarget.transform.localScale.x)));
                    locker.transform.localPosition = new Vector3(vector7.x - (Screen.width * 0.5f), vector7.y - (Screen.height * 0.5f), 0f);
                    if ((lockTarget.GetComponent<TITAN>() != null) && lockTarget.GetComponent<TITAN>().hasDie)
                    {
                        lockTarget = null;
                    }
                }
                else
                {
                    locker.transform.localPosition = new Vector3(0f, (-Screen.height * 0.5f) - 50f, 0f);
                }
                Vector3 end = (head == null) ? main_object.transform.position : head.transform.position;
                Vector3 vector9 = ((head == null) ? main_object.transform.position : head.transform.position) - this.transform.position;
                Vector3 normalized = vector9.normalized;
                end -= (Vector3) ((distance * normalized) * distanceMulti);
                LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
                LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
                LayerMask mask3 = mask | mask2;
                if (head != null)
                {
                    if (Physics.Linecast(head.transform.position, end, out hit, (int) mask))
                    {
                        transform.position = hit.point;
                    }
                    else if (Physics.Linecast(head.transform.position - ((Vector3) ((normalized * distanceMulti) * 3f)), end, out hit, (int) mask2))
                    {
                        transform.position = hit.point;
                    }
                    Debug.DrawLine(head.transform.position - ((Vector3) ((normalized * distanceMulti) * 3f)), end, Color.red);
                }
                else if (Physics.Linecast(main_object.transform.position + Vector3.up, end, out hit, (int) mask3))
                {
                    transform.position = hit.point;
                }
                shakeUpdate();
            }
        }
    }

    public void update2()
    {
        if (flashDuration > 0f)
        {
            flashDuration -= Time.deltaTime;
            if (flashDuration <= 0f)
            {
                flashDuration = 0f;
            }
            GameObject.Find("flash").GetComponent<UISprite>().alpha = flashDuration * 0.5f;
        }
        if (gametype == GAMETYPE.STOP)
        {
            Screen.showCursor = true;
            Screen.lockCursor = false;
        }
        else
        {
            if ((gametype != GAMETYPE.SINGLE) && gameOver)
            {
                if (inputManager.isInputDown[InputCode.attack1])
                {
                    if (spectatorMode)
                    {
                        setSpectorMode(false);
                    }
                    else
                    {
                        setSpectorMode(true);
                    }
                }
                if (inputManager.isInputDown[InputCode.flare1])
                {
                    currentPeekPlayerIndex++;
                    int length = GameObject.FindGameObjectsWithTag("Player").Length;
                    if (currentPeekPlayerIndex >= length)
                    {
                        currentPeekPlayerIndex = 0;
                    }
                    if (length > 0)
                    {
                        setMainObject(GameObject.FindGameObjectsWithTag("Player")[currentPeekPlayerIndex], true, false);
                        setSpectorMode(false);
                        lockAngle = false;
                    }
                }
                if (inputManager.isInputDown[InputCode.flare2])
                {
                    currentPeekPlayerIndex--;
                    int num2 = GameObject.FindGameObjectsWithTag("Player").Length;
                    if (currentPeekPlayerIndex >= num2)
                    {
                        currentPeekPlayerIndex = 0;
                    }
                    if (currentPeekPlayerIndex < 0)
                    {
                        currentPeekPlayerIndex = num2;
                    }
                    if (num2 > 0)
                    {
                        setMainObject(GameObject.FindGameObjectsWithTag("Player")[currentPeekPlayerIndex], true, false);
                        setSpectorMode(false);
                        lockAngle = false;
                    }
                }
                if (spectatorMode)
                {
                    return;
                }
            }
            if (inputManager.isInputDown[InputCode.pause])
            {
                if (isPausing)
                {
                    if (main_object != null)
                    {
                        Vector3 position = transform.position;
                        position = (head == null) ? main_object.transform.position : head.transform.position;
                        position += (Vector3) (Vector3.up * heightMulti);
                        transform.position = Vector3.Lerp(transform.position, position - ((Vector3) (transform.forward * 5f)), 0.2f);
                    }
                    return;
                }
                isPausing = !isPausing;
                if (isPausing)
                {
                    if (gametype == GAMETYPE.SINGLE)
                    {
                        Time.timeScale = 0f;
                    }
                    GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = true;
                    Screen.showCursor = true;
                    Screen.lockCursor = false;
                }
            }
            if (needSetHUD)
            {
                needSetHUD = false;
                setHUDposition();
                Screen.lockCursor = !Screen.lockCursor;
                Screen.lockCursor = !Screen.lockCursor;
            }
            if (inputManager.isInputDown[InputCode.fullscreen])
            {
                Screen.fullScreen = !Screen.fullScreen;
                if (Screen.fullScreen)
                {
                    Screen.SetResolution(960, 600, false);
                }
                else
                {
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                }
                needSetHUD = true;
                Minimap.OnScreenResolutionChanged();
            }
            if (inputManager.isInputDown[InputCode.restart])
            {
                reset();
            }
            if (main_object != null)
            {
                RaycastHit hit;
                if (inputManager.isInputDown[InputCode.camera])
                {
                    if (cameraMode == CAMERA_TYPE.ORIGINAL)
                    {
                        cameraMode = CAMERA_TYPE.WOW;
                        Screen.lockCursor = false;
                    }
                    else if (cameraMode == CAMERA_TYPE.WOW)
                    {
                        cameraMode = CAMERA_TYPE.TPS;
                        Screen.lockCursor = true;
                    }
                    else if (cameraMode == CAMERA_TYPE.TPS)
                    {
                        cameraMode = CAMERA_TYPE.ORIGINAL;
                        Screen.lockCursor = false;
                    }
                    verticalRotationOffset = 0f;
                    if ((((int) FengGameManagerMKII.settings[245]) == 1) || (main_object.GetComponent<HERO>() == null))
                    {
                        Screen.showCursor = false;
                    }
                }
                if (inputManager.isInputDown[InputCode.hideCursor])
                {
                    Screen.showCursor = !Screen.showCursor;
                }
                if (inputManager.isInputDown[InputCode.focus])
                {
                    triggerAutoLock = !triggerAutoLock;
                    if (triggerAutoLock)
                    {
                        lockTarget = findNearestTitan();
                        if (closestDistance >= 150f)
                        {
                            lockTarget = null;
                            triggerAutoLock = false;
                        }
                    }
                }
                if (gameOver && (main_object != null))
                {
                    if (FengGameManagerMKII.inputRC.isInputHumanDown(InputCodeRC.liveCam))
                    {
                        if (((int) FengGameManagerMKII.settings[263]) == 0)
                        {
                            FengGameManagerMKII.settings[263] = 1;
                        }
                        else
                        {
                            FengGameManagerMKII.settings[263] = 0;
                        }
                    }
                    HERO component = main_object.GetComponent<HERO>();
                    if ((((component != null) && (((int) FengGameManagerMKII.settings[263]) == 1)) && component.GetComponent<SmoothSyncMovement>().enabled) && component.isPhotonCamera)
                    {
                        CameraMovementLive(component);
                    }
                    else if (lockAngle)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, main_object.transform.rotation, 0.2f);
                        transform.position = Vector3.Lerp(transform.position, main_object.transform.position - ((Vector3) (main_object.transform.forward * 5f)), 0.2f);
                    }
                    else
                    {
                        CameraMovement();
                    }
                }
                else
                {
                    CameraMovement();
                }
                if (triggerAutoLock && (lockTarget != null))
                {
                    float z = this.transform.eulerAngles.z;
                    Transform transform = lockTarget.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                    Vector3 vector2 = transform.position - ((head == null) ? main_object.transform.position : head.transform.position);
                    vector2.Normalize();
                    lockCameraPosition = (head == null) ? main_object.transform.position : head.transform.position;
                    lockCameraPosition -= (Vector3) (((vector2 * distance) * distanceMulti) * distanceOffsetMulti);
                    lockCameraPosition += (Vector3) (((Vector3.up * 3f) * heightMulti) * distanceOffsetMulti);
                    this.transform.position = Vector3.Lerp(this.transform.position, lockCameraPosition, Time.deltaTime * 4f);
                    if (head != null)
                    {
                        this.transform.LookAt((Vector3) ((head.transform.position * 0.8f) + (transform.position * 0.2f)));
                    }
                    else
                    {
                        this.transform.LookAt((Vector3) ((main_object.transform.position * 0.8f) + (transform.position * 0.2f)));
                    }
                    this.transform.localEulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, z);
                    Vector2 vector3 = camera.WorldToScreenPoint(transform.position - ((Vector3) (transform.forward * lockTarget.transform.localScale.x)));
                    locker.transform.localPosition = new Vector3(vector3.x - (Screen.width * 0.5f), vector3.y - (Screen.height * 0.5f), 0f);
                    if ((lockTarget.GetComponent<TITAN>() != null) && lockTarget.GetComponent<TITAN>().hasDie)
                    {
                        lockTarget = null;
                    }
                }
                else
                {
                    locker.transform.localPosition = new Vector3(0f, (-Screen.height * 0.5f) - 50f, 0f);
                }
                Vector3 end = (head == null) ? main_object.transform.position : head.transform.position;
                Vector3 vector5 = ((head == null) ? main_object.transform.position : head.transform.position) - this.transform.position;
                Vector3 normalized = vector5.normalized;
                end -= (Vector3) ((distance * normalized) * distanceMulti);
                LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
                LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
                LayerMask mask3 = mask | mask2;
                if (head != null)
                {
                    if (Physics.Linecast(head.transform.position, end, out hit, (int) mask))
                    {
                        transform.position = hit.point;
                    }
                    else if (Physics.Linecast(head.transform.position - ((Vector3) ((normalized * distanceMulti) * 3f)), end, out hit, (int) mask2))
                    {
                        transform.position = hit.point;
                    }
                    Debug.DrawLine(head.transform.position - ((Vector3) ((normalized * distanceMulti) * 3f)), end, Color.red);
                }
                else if (Physics.Linecast(main_object.transform.position + Vector3.up, end, out hit, (int) mask3))
                {
                    transform.position = hit.point;
                }
                shakeUpdate();
            }
        }
    }

    public enum RotationAxes
    {
        MouseXAndY,
        MouseX,
        MouseY
    }
}

