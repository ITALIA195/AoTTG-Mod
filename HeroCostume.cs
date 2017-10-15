using UnityEngine;

public class HeroCostume
{
    public string DmgTexture = string.Empty;
    public string ArmLMesh = string.Empty;
    public string ArmRMesh = string.Empty;
    public string BeardMesh = string.Empty;
    public int BeardTextureId = -1;
    public static string[] BodyCasualFaTexture;
    public static string[] BodyCasualFbTexture;
    public static string[] BodyCasualMaTexture;
    public static string[] BodyCasualMbTexture;
    public string BodyMesh = string.Empty;
    public string BodyTexture = string.Empty;
    public static string[] BodyUniformFaTexture;
    public static string[] BodyUniformFbTexture;
    public static string[] BodyUniformMaTexture;
    public static string[] BodyUniformMbTexture;
    public string BrandTexture = string.Empty;
    public string Brand1Mesh = string.Empty;
    public string Brand2Mesh = string.Empty;
    public string Brand3Mesh = string.Empty;
    public string Brand4Mesh = string.Empty;
    public bool Cape;
    public string CapeMesh = string.Empty;
    public string CapeTexture = string.Empty;
    public static HeroCostume[] Costume;
    public int CostumeId;
    public static HeroCostume[] CostumeOption;
    public Division Division;
    public string EyeMesh = string.Empty;
    public int EyeTextureId = -1;
    public string FaceTexture = string.Empty;
    public string GlassMesh = string.Empty;
    public int GlassTextureId = -1;
    public string Hair1Mesh = string.Empty;
    public Color HairColor = new Color(0.5f, 0.1f, 0f);
    public string HairMesh = string.Empty;
    public CostumeHair HairInfo;
    public string HandLMesh = string.Empty;
    public string HandRMesh = string.Empty;
    public int Id;
    private static bool _inited; // 
    public string Mesh3Dmg = string.Empty;
    public string Mesh3DmgBelt = string.Empty;
    public string Mesh3DmgGasL = string.Empty;
    public string Mesh3DmgGasR = string.Empty;
    public string Name = string.Empty;
    public string PartChest1ObjectMesh = string.Empty;
    public string PartChest1ObjectTexture = string.Empty;
    public string PartChestObjectMesh = string.Empty;
    public string PartChestObjectTexture = string.Empty;
    public string PartChestSkinnedClothMesh = string.Empty;
    public string PartChestSkinnedClothTexture = string.Empty;
    public Sex Sex;
    public int SkinColor = 1;
    public string SkinTexture = string.Empty;
    public HeroStat Stat;
    public UniformType UniformType = UniformType.CasualA;
    public string WeaponLMesh = string.Empty;
    public string WeaponRMesh = string.Empty;

    /* 
    public string _3dmg_texture = string.Empty;
    public string arm_l_mesh = string.Empty;
    public string arm_r_mesh = string.Empty;
    public string beard_mesh = string.Empty;
    public int beard_texture_id = -1;
    public static string[] body_casual_fa_texture;
    public static string[] body_casual_fb_texture;
    public static string[] body_casual_ma_texture;
    public static string[] body_casual_mb_texture;
    public string body_mesh = string.Empty;
    public string body_texture = string.Empty;
    public static string[] body_uniform_fa_texture;
    public static string[] body_uniform_fb_texture;
    public static string[] body_uniform_ma_texture;
    public static string[] body_uniform_mb_texture;
    public string brand_texture = string.Empty;
    public string brand1_mesh = string.Empty;
    public string brand2_mesh = string.Empty;
    public string brand3_mesh = string.Empty;
    public string brand4_mesh = string.Empty;
    public bool cape;
    public string cape_mesh = string.Empty;
    public string cape_texture = string.Empty;
    public static HeroCostume[] costume;
    public int costumeId;
    public static HeroCostume[] costumeOption;
    public Division division;
    public string eye_mesh = string.Empty;
    public int eye_texture_id = -1;
    public string face_texture = string.Empty;
    public string glass_mesh = string.Empty;
    public int glass_texture_id = -1;
    public string hair_1_mesh = string.Empty;
    public Color hair_color = new Color(0.5f, 0.1f, 0f);
    public string hair_mesh = string.Empty;
    public CostumeHair hairInfo;
    public string hand_l_mesh = string.Empty;
    public string hand_r_mesh = string.Empty;
    public int id;
    private static bool inited; // 
    public string mesh_3dmg = string.Empty;
    public string mesh_3dmg_belt = string.Empty;
    public string mesh_3dmg_gas_l = string.Empty;
    public string mesh_3dmg_gas_r = string.Empty;
    public string name = string.Empty;
    public string part_chest_1_object_mesh = string.Empty;
    public string part_chest_1_object_texture = string.Empty;
    public string part_chest_object_mesh = string.Empty;
    public string part_chest_object_texture = string.Empty;
    public string part_chest_skinned_cloth_mesh = string.Empty;
    public string part_chest_skinned_cloth_texture = string.Empty;
    public Sex sex;
    public int skin_color = 1;
    public string skin_texture = string.Empty;
    public HeroStat stat;
    public UniformType uniform_type = UniformType.CasualA;
    public string weapon_l_mesh = string.Empty;
    public string weapon_r_mesh = string.Empty;
     */

    public void Checkstat()
    {
        if (Stat.SPD + Stat.GAS + Stat.BLA + Stat.ACL > 400)
        {
            Stat.ACL = 100;
            Stat.BLA = 100;
            Stat.GAS = 100;
            Stat.SPD = 100;
        }
    }
     
    public static void Initialize()
    {
        if (!_inited)
        {
            _inited = true;
            CostumeHair.init();
            BodyUniformMaTexture = new[]
                {"aottg_hero_uniform_ma_1", "aottg_hero_uniform_ma_2", "aottg_hero_uniform_ma_3"};
            BodyUniformFaTexture = new[]
                {"aottg_hero_uniform_fa_1", "aottg_hero_uniform_fa_2", "aottg_hero_uniform_fa_3"};
            BodyUniformMbTexture = new[]
            {
                "aottg_hero_uniform_mb_1", "aottg_hero_uniform_mb_2", "aottg_hero_uniform_mb_3",
                "aottg_hero_uniform_mb_4"
            };
            BodyUniformFbTexture = new[] {"aottg_hero_uniform_fb_1", "aottg_hero_uniform_fb_2"};
            BodyCasualMaTexture = new[]
                {"aottg_hero_casual_ma_1", "aottg_hero_casual_ma_2", "aottg_hero_casual_ma_3"};
            BodyCasualFaTexture = new[]
                {"aottg_hero_casual_fa_1", "aottg_hero_casual_fa_2", "aottg_hero_casual_fa_3"};
            BodyCasualMbTexture = new[]
                {"aottg_hero_casual_mb_1", "aottg_hero_casual_mb_2", "aottg_hero_casual_mb_3", "aottg_hero_casual_mb_4"};
            BodyCasualFbTexture = new[] {"aottg_hero_casual_fb_1", "aottg_hero_casual_fb_2"};
            Costume = new HeroCostume[39];
            Costume[0] = new HeroCostume
            {
                Name = "annie",
                Sex = Sex.Female,
                UniformType = UniformType.UniformB,
                PartChestObjectMesh = "character_cap_uniform",
                PartChestObjectTexture = "aottg_hero_annie_cap_uniform",
                Cape = true,
                BodyTexture = BodyUniformFbTexture[0],
                HairInfo = CostumeHair.hairsF[5],
                EyeTextureId = 0,
                BeardTextureId = 33,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(1f, 0.9f, 0.5f),
                Division = Division.TheMilitaryPolice,
                CostumeId = 0
            };
            Costume[1] = new HeroCostume
            {
                Name = "annie",
                Sex = Sex.Female,
                UniformType = UniformType.UniformB,
                PartChestObjectMesh = "character_cap_uniform",
                PartChestObjectTexture = "aottg_hero_annie_cap_uniform",
                BodyTexture = BodyUniformFbTexture[0],
                Cape = false,
                HairInfo = CostumeHair.hairsF[5],
                EyeTextureId = 0,
                BeardTextureId = 33,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(1f, 0.9f, 0.5f),
                Division = Division.TraineesSquad,
                CostumeId = 0
            };
            Costume[2] = new HeroCostume
            {
                Name = "annie",
                Sex = Sex.Female,
                UniformType = UniformType.CasualB,
                PartChestObjectMesh = "character_cap_casual",
                PartChestObjectTexture = "aottg_hero_annie_cap_causal",
                PartChest1ObjectMesh = "character_body_blade_keeper_f",
                PartChest1ObjectTexture = BodyCasualFbTexture[0],
                BodyTexture = BodyCasualFbTexture[0],
                Cape = false,
                HairInfo = CostumeHair.hairsF[5],
                EyeTextureId = 0,
                BeardTextureId = 33,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(1f, 0.9f, 0.5f),
                CostumeId = 1
            };
            Costume[3] = new HeroCostume
            {
                Name = "mikasa",
                Sex = Sex.Female,
                UniformType = UniformType.UniformB,
                BodyTexture = BodyUniformFbTexture[1],
                Cape = true,
                HairInfo = CostumeHair.hairsF[7],
                EyeTextureId = 2,
                BeardTextureId = 33,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.15f, 0.15f, 0.145f),
                Division = Division.TheSurveryCorps,
                CostumeId = 2
            };
            Costume[4] = new HeroCostume
            {
                Name = "mikasa",
                Sex = Sex.Female,
                UniformType = UniformType.UniformB,
                PartChestSkinnedClothMesh = "mikasa_asset_uni",
                PartChestSkinnedClothTexture = BodyUniformFbTexture[1],
                BodyTexture = BodyUniformFbTexture[1],
                Cape = false,
                HairInfo = CostumeHair.hairsF[7],
                EyeTextureId = 2,
                BeardTextureId = 33,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.15f, 0.15f, 0.145f),
                Division = Division.TraineesSquad,
                CostumeId = 3
            };
            Costume[5] = new HeroCostume
            {
                Name = "mikasa",
                Sex = Sex.Female,
                UniformType = UniformType.CasualB,
                PartChestSkinnedClothMesh = "mikasa_asset_cas",
                PartChestSkinnedClothTexture = BodyCasualFbTexture[1],
                PartChest1ObjectMesh = "character_body_blade_keeper_f",
                PartChest1ObjectTexture = BodyCasualFbTexture[1],
                BodyTexture = BodyCasualFbTexture[1],
                Cape = false,
                HairInfo = CostumeHair.hairsF[7],
                EyeTextureId = 2,
                BeardTextureId = 33,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.15f, 0.15f, 0.145f),
                CostumeId = 4
            };
            Costume[6] = new HeroCostume
            {
                Name = "levi",
                Sex = Sex.Male,
                UniformType = UniformType.UniformB,
                BodyTexture = BodyUniformMbTexture[1],
                Cape = true,
                HairInfo = CostumeHair.hairsM[7],
                EyeTextureId = 1,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.295f, 0.295f, 0.275f),
                Division = Division.TheSurveryCorps,
                CostumeId = 11
            };
            Costume[7] = new HeroCostume
            {
                Name = "levi",
                Sex = Sex.Male,
                UniformType = UniformType.CasualB,
                BodyTexture = BodyCasualMbTexture[1],
                PartChest1ObjectMesh = "character_body_blade_keeper_m",
                PartChest1ObjectTexture = BodyCasualMbTexture[1],
                Cape = false,
                HairInfo = CostumeHair.hairsM[7],
                EyeTextureId = 1,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.295f, 0.295f, 0.275f),
                CostumeId = 12
            };
            Costume[8] = new HeroCostume
            {
                Name = "eren",
                Sex = Sex.Male,
                UniformType = UniformType.UniformB,
                BodyTexture = BodyUniformMbTexture[0],
                Cape = true,
                HairInfo = CostumeHair.hairsM[4],
                EyeTextureId = 3,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.295f, 0.295f, 0.275f),
                Division = Division.TheSurveryCorps,
                CostumeId = 13
            };
            Costume[9] = new HeroCostume
            {
                Name = "eren",
                Sex = Sex.Male,
                UniformType = UniformType.UniformB,
                BodyTexture = BodyUniformMbTexture[0],
                Cape = false,
                HairInfo = CostumeHair.hairsM[4],
                EyeTextureId = 3,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.295f, 0.295f, 0.275f),
                Division = Division.TraineesSquad,
                CostumeId = 13
            };
            Costume[10] = new HeroCostume
            {
                Name = "eren",
                Sex = Sex.Male,
                UniformType = UniformType.CasualB,
                BodyTexture = BodyCasualMbTexture[0],
                PartChest1ObjectMesh = "character_body_blade_keeper_m",
                PartChest1ObjectTexture = BodyCasualMbTexture[0],
                Cape = false,
                HairInfo = CostumeHair.hairsM[4],
                EyeTextureId = 3,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.295f, 0.295f, 0.275f),
                CostumeId = 14
            };
            Costume[11] = new HeroCostume
            {
                Name = "sasha",
                Sex = Sex.Female,
                UniformType = UniformType.UniformA,
                BodyTexture = BodyUniformFaTexture[1],
                Cape = true,
                HairInfo = CostumeHair.hairsF[10],
                EyeTextureId = 4,
                BeardTextureId = 33,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.45f, 0.33f, 0.255f),
                Division = Division.TheSurveryCorps,
                CostumeId = 5
            };
            Costume[12] = new HeroCostume
            {
                Name = "sasha",
                Sex = Sex.Female,
                UniformType = UniformType.UniformA,
                BodyTexture = BodyUniformFaTexture[1],
                Cape = false,
                HairInfo = CostumeHair.hairsF[10],
                EyeTextureId = 4,
                BeardTextureId = 33,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.45f, 0.33f, 0.255f),
                Division = Division.TraineesSquad,
                CostumeId = 5
            };
            Costume[13] = new HeroCostume
            {
                Name = "sasha",
                Sex = Sex.Female,
                UniformType = UniformType.CasualA,
                BodyTexture = BodyCasualFaTexture[1],
                PartChest1ObjectMesh = "character_body_blade_keeper_f",
                PartChest1ObjectTexture = BodyCasualFaTexture[1],
                Cape = false,
                HairInfo = CostumeHair.hairsF[10],
                EyeTextureId = 4,
                BeardTextureId = 33,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.45f, 0.33f, 0.255f),
                CostumeId = 6
            };
            Costume[14] = new HeroCostume
            {
                Name = "hanji",
                Sex = Sex.Female,
                UniformType = UniformType.UniformA,
                BodyTexture = BodyUniformFaTexture[2],
                Cape = true,
                HairInfo = CostumeHair.hairsF[6],
                EyeTextureId = 5,
                BeardTextureId = 33,
                GlassTextureId = 49,
                SkinColor = 1,
                HairColor = new Color(0.45f, 0.33f, 0.255f),
                Division = Division.TheSurveryCorps,
                CostumeId = 7
            };
            Costume[15] = new HeroCostume
            {
                Name = "hanji",
                Sex = Sex.Female,
                UniformType = UniformType.CasualA,
                BodyTexture = BodyCasualFaTexture[2],
                PartChest1ObjectMesh = "character_body_blade_keeper_f",
                PartChest1ObjectTexture = BodyCasualFaTexture[2],
                Cape = false,
                HairInfo = CostumeHair.hairsF[6],
                EyeTextureId = 5,
                BeardTextureId = 33,
                GlassTextureId = 49,
                SkinColor = 1,
                HairColor = new Color(0.295f, 0.23f, 0.17f),
                CostumeId = 8
            };
            Costume[16] = new HeroCostume
            {
                Name = "rico",
                Sex = Sex.Female,
                UniformType = UniformType.UniformA,
                BodyTexture = BodyUniformFaTexture[0],
                Cape = true,
                HairInfo = CostumeHair.hairsF[9],
                EyeTextureId = 6,
                BeardTextureId = 33,
                GlassTextureId = 48,
                SkinColor = 1,
                HairColor = new Color(1f, 1f, 1f),
                Division = Division.TheGarrison,
                CostumeId = 9
            };
            Costume[17] = new HeroCostume
            {
                Name = "rico",
                Sex = Sex.Female,
                UniformType = UniformType.CasualA,
                BodyTexture = BodyCasualFaTexture[0],
                PartChest1ObjectMesh = "character_body_blade_keeper_f",
                PartChest1ObjectTexture = BodyCasualFaTexture[0],
                Cape = false,
                HairInfo = CostumeHair.hairsF[9],
                EyeTextureId = 6,
                BeardTextureId = 33,
                GlassTextureId = 48,
                SkinColor = 1,
                HairColor = new Color(1f, 1f, 1f),
                CostumeId = 10
            };
            Costume[18] = new HeroCostume
            {
                Name = "jean",
                Sex = Sex.Male,
                UniformType = UniformType.UniformA,
                BodyTexture = BodyUniformMaTexture[1],
                Cape = true,
                HairInfo = CostumeHair.hairsM[6],
                EyeTextureId = 7,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.94f, 0.84f, 0.6f),
                Division = Division.TheSurveryCorps,
                CostumeId = 15
            };
            Costume[19] = new HeroCostume
            {
                Name = "jean",
                Sex = Sex.Male,
                UniformType = UniformType.UniformA,
                BodyTexture = BodyUniformMaTexture[1],
                Cape = false,
                HairInfo = CostumeHair.hairsM[6],
                EyeTextureId = 7,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.94f, 0.84f, 0.6f),
                Division = Division.TraineesSquad,
                CostumeId = 15
            };
            Costume[20] = new HeroCostume
            {
                Name = "jean",
                Sex = Sex.Male,
                UniformType = UniformType.CasualA,
                BodyTexture = BodyCasualMaTexture[1],
                PartChest1ObjectMesh = "character_body_blade_keeper_m",
                PartChest1ObjectTexture = BodyCasualMaTexture[1],
                Cape = false,
                HairInfo = CostumeHair.hairsM[6],
                EyeTextureId = 7,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.94f, 0.84f, 0.6f),
                CostumeId = 16
            };
            Costume[21] = new HeroCostume
            {
                Name = "marco",
                Sex = Sex.Male,
                UniformType = UniformType.UniformA,
                BodyTexture = BodyUniformMaTexture[2],
                Cape = false,
                HairInfo = CostumeHair.hairsM[8],
                EyeTextureId = 8,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.295f, 0.295f, 0.275f),
                Division = Division.TraineesSquad,
                CostumeId = 17
            };
            Costume[22] = new HeroCostume
            {
                Name = "marco",
                Sex = Sex.Male,
                UniformType = UniformType.CasualA,
                BodyTexture = BodyCasualMaTexture[2],
                Cape = false,
                HairInfo = CostumeHair.hairsM[8],
                EyeTextureId = 8,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.295f, 0.295f, 0.275f),
                CostumeId = 18
            };
            Costume[23] = new HeroCostume
            {
                Name = "mike",
                Sex = Sex.Male,
                UniformType = UniformType.UniformB,
                BodyTexture = BodyUniformMbTexture[3],
                Cape = true,
                HairInfo = CostumeHair.hairsM[9],
                EyeTextureId = 9,
                BeardTextureId = 32,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.94f, 0.84f, 0.6f),
                Division = Division.TheSurveryCorps,
                CostumeId = 19
            };
            Costume[24] = new HeroCostume
            {
                Name = "mike",
                Sex = Sex.Male,
                UniformType = UniformType.CasualB,
                BodyTexture = BodyCasualMbTexture[3],
                PartChest1ObjectMesh = "character_body_blade_keeper_m",
                PartChest1ObjectTexture = BodyCasualMbTexture[3],
                Cape = false,
                HairInfo = CostumeHair.hairsM[9],
                EyeTextureId = 9,
                BeardTextureId = 32,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.94f, 0.84f, 0.6f),
                Division = Division.TheSurveryCorps,
                CostumeId = 20
            };
            Costume[25] = new HeroCostume
            {
                Name = "connie",
                Sex = Sex.Male,
                UniformType = UniformType.UniformB,
                BodyTexture = BodyUniformMbTexture[2],
                Cape = true,
                HairInfo = CostumeHair.hairsM[10],
                EyeTextureId = 10,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                Division = Division.TheSurveryCorps,
                CostumeId = 21
            };
            Costume[26] = new HeroCostume
            {
                Name = "connie",
                Sex = Sex.Male,
                UniformType = UniformType.UniformB,
                BodyTexture = BodyUniformMbTexture[2],
                Cape = false,
                HairInfo = CostumeHair.hairsM[10],
                EyeTextureId = 10,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                Division = Division.TraineesSquad,
                CostumeId = 21
            };
            Costume[27] = new HeroCostume
            {
                Name = "connie",
                Sex = Sex.Male,
                UniformType = UniformType.CasualB,
                BodyTexture = BodyCasualMbTexture[2],
                PartChest1ObjectMesh = "character_body_blade_keeper_m",
                PartChest1ObjectTexture = BodyCasualMbTexture[2],
                Cape = false,
                HairInfo = CostumeHair.hairsM[10],
                EyeTextureId = 10,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                CostumeId = 22
            };
            Costume[28] = new HeroCostume
            {
                Name = "armin",
                Sex = Sex.Male,
                UniformType = UniformType.UniformA,
                BodyTexture = BodyUniformMaTexture[0],
                Cape = true,
                HairInfo = CostumeHair.hairsM[5],
                EyeTextureId = 11,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.95f, 0.8f, 0.5f),
                Division = Division.TheSurveryCorps,
                CostumeId = 23
            };
            Costume[29] = new HeroCostume
            {
                Name = "armin",
                Sex = Sex.Male,
                UniformType = UniformType.UniformA,
                BodyTexture = BodyUniformMaTexture[0],
                Cape = false,
                HairInfo = CostumeHair.hairsM[5],
                EyeTextureId = 11,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.95f, 0.8f, 0.5f),
                Division = Division.TraineesSquad,
                CostumeId = 23
            };
            Costume[30] = new HeroCostume
            {
                Name = "armin",
                Sex = Sex.Male,
                UniformType = UniformType.CasualA,
                BodyTexture = BodyCasualMaTexture[0],
                PartChest1ObjectMesh = "character_body_blade_keeper_m",
                PartChest1ObjectTexture = BodyCasualMaTexture[0],
                Cape = false,
                HairInfo = CostumeHair.hairsM[5],
                EyeTextureId = 11,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.95f, 0.8f, 0.5f),
                CostumeId = 24
            };
            Costume[31] = new HeroCostume
            {
                Name = "petra",
                Sex = Sex.Female,
                UniformType = UniformType.UniformA,
                BodyTexture = BodyUniformFaTexture[0],
                Cape = true,
                HairInfo = CostumeHair.hairsF[8],
                EyeTextureId = 27,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(1f, 0.725f, 0.376f),
                Division = Division.TheSurveryCorps,
                CostumeId = 9
            };
            Costume[32] = new HeroCostume
            {
                Name = "petra",
                Sex = Sex.Female,
                UniformType = UniformType.CasualA,
                BodyTexture = BodyCasualFaTexture[0],
                PartChest1ObjectMesh = "character_body_blade_keeper_f",
                PartChest1ObjectTexture = BodyCasualFaTexture[0],
                Cape = false,
                HairInfo = CostumeHair.hairsF[8],
                EyeTextureId = 27,
                BeardTextureId = -1,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(1f, 0.725f, 0.376f),
                Division = Division.TheSurveryCorps,
                CostumeId = 10
            };
            Costume[33] = new HeroCostume
            {
                Name = "custom",
                Sex = Sex.Female,
                UniformType = UniformType.CasualB,
                PartChestSkinnedClothMesh = "mikasa_asset_cas",
                PartChestSkinnedClothTexture = BodyCasualFbTexture[1],
                PartChest1ObjectMesh = "character_body_blade_keeper_f",
                PartChest1ObjectTexture = BodyCasualFbTexture[1],
                BodyTexture = BodyCasualFbTexture[1],
                Cape = false,
                HairInfo = CostumeHair.hairsF[2],
                EyeTextureId = 12,
                BeardTextureId = 33,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.15f, 0.15f, 0.145f),
                CostumeId = 4
            };
            Costume[34] = new HeroCostume
            {
                Name = "custom",
                Sex = Sex.Male,
                UniformType = UniformType.CasualA,
                BodyTexture = BodyCasualMaTexture[0],
                PartChest1ObjectMesh = "character_body_blade_keeper_m",
                PartChest1ObjectTexture = BodyCasualMaTexture[0],
                Cape = false,
                HairInfo = CostumeHair.hairsM[3],
                EyeTextureId = 26,
                BeardTextureId = 44,
                GlassTextureId = -1,
                SkinColor = 1,
                HairColor = new Color(0.41f, 1f, 0f),
                CostumeId = 24
            };
            Costume[35] = new HeroCostume
            {
                Name = "custom",
                Sex = Sex.Female,
                UniformType = UniformType.UniformA,
                BodyTexture = BodyUniformFaTexture[1],
                Cape = false,
                HairInfo = CostumeHair.hairsF[4],
                EyeTextureId = 22,
                BeardTextureId = 33,
                GlassTextureId = 56,
                SkinColor = 1,
                HairColor = new Color(0f, 1f, 0.874f),
                CostumeId = 5
            };
            Costume[36] = new HeroCostume
            {
                Name = "feng",
                Sex = Sex.Male,
                UniformType = UniformType.CasualB,
                BodyTexture = BodyCasualMbTexture[3],
                PartChest1ObjectMesh = "character_body_blade_keeper_m",
                PartChest1ObjectTexture = BodyCasualMbTexture[3],
                Cape = true,
                HairInfo = CostumeHair.hairsM[10],
                EyeTextureId = 25,
                BeardTextureId = 39,
                GlassTextureId = 53,
                SkinColor = 1,
                Division = Division.TheSurveryCorps,
                CostumeId = 20
            };
            Costume[37] = new HeroCostume
            {
                Name = "AHSS",
                Sex = Sex.Male,
                UniformType = UniformType.CasualAHSS,
                BodyTexture = BodyCasualMaTexture[0] + "_ahss",
                Cape = false,
                HairInfo = CostumeHair.hairsM[6],
                EyeTextureId = 25,
                BeardTextureId = 39,
                GlassTextureId = 53,
                SkinColor = 3,
                Division = Division.TheMilitaryPolice,
                CostumeId = 25
            };
            Costume[38] = new HeroCostume
            {
                Name = "AHSS (F)",
                Sex = Sex.Female,
                UniformType = UniformType.CasualAHSS,
                BodyTexture = BodyCasualFaTexture[0],
                Cape = false,
                HairInfo = CostumeHair.hairsF[6],
                EyeTextureId = 2,
                BeardTextureId = 33,
                GlassTextureId = -1,
                SkinColor = 3,
                Division = Division.TheMilitaryPolice,
                CostumeId = 26
            };
            for (var i = 0; i < Costume.Length; i++)
            {
                Costume[i].Stat = HeroStat.getInfo("CUSTOM_DEFAULT");
                Costume[i].Id = i;
                Costume[i].SetMesh();
                Costume[i].SetTexture();
            }
            CostumeOption = new[]
            {
                Costume[0], Costume[2], Costume[3], Costume[4], Costume[5], Costume[11], Costume[13], Costume[14],
                Costume[15], Costume[16], Costume[17], Costume[6], Costume[7], Costume[8], Costume[10], Costume[18],
                Costume[19], Costume[21], Costume[22], Costume[23], Costume[24], Costume[25], Costume[27], Costume[28],
                Costume[30], Costume[37], Costume[38]
            };
        }
    }

    public void SetBodyByCostumeId(int costume = -1)
    {
        if (costume == -1)
        {
            costume = CostumeId;
        }
        CostumeId = costume;
        ArmLMesh = CostumeOption[costume].ArmLMesh;
        ArmRMesh = CostumeOption[costume].ArmRMesh;
        BodyMesh = CostumeOption[costume].BodyMesh;
        BodyTexture = CostumeOption[costume].BodyTexture;
        UniformType = CostumeOption[costume].UniformType;
        PartChest1ObjectMesh = CostumeOption[costume].PartChest1ObjectMesh;
        PartChest1ObjectTexture = CostumeOption[costume].PartChest1ObjectTexture;
        PartChestObjectMesh = CostumeOption[costume].PartChestObjectMesh;
        PartChestObjectTexture = CostumeOption[costume].PartChestObjectTexture;
        PartChestSkinnedClothMesh = CostumeOption[costume].PartChestSkinnedClothMesh;
        PartChestSkinnedClothTexture = CostumeOption[costume].PartChestSkinnedClothTexture;
    }

    public void SetCape()
    {
        if (Cape)
        {
            CapeMesh = "character_cape";
        }
        else
        {
            CapeMesh = string.Empty;
        }
    }

    public void SetMesh()
    {
        Brand1Mesh = string.Empty;
        Brand2Mesh = string.Empty;
        Brand3Mesh = string.Empty;
        Brand4Mesh = string.Empty;
        HandLMesh = "character_hand_l";
        HandRMesh = "character_hand_r";
        Mesh3Dmg = "character_3dmg";
        Mesh3DmgBelt = "character_3dmg_belt";
        Mesh3DmgGasL = "character_3dmg_gas_l";
        Mesh3DmgGasR = "character_3dmg_gas_r";
        WeaponLMesh = "character_blade_l";
        WeaponRMesh = "character_blade_r";
        if (UniformType == UniformType.CasualAHSS)
        {
            HandLMesh = "character_hand_l_ah";
            HandRMesh = "character_hand_r_ah";
            ArmLMesh = "character_arm_casual_l_ah";
            ArmRMesh = "character_arm_casual_r_ah";
            if (Sex == Sex.Female)
            {
                BodyMesh = "character_body_casual_FA";
            }
            else
            {
                BodyMesh = "character_body_casual_MA";
            }
            Mesh3Dmg = "character_3dmg_2";
            Mesh3DmgBelt = string.Empty;
            Mesh3DmgGasL = "character_gun_mag_l";
            Mesh3DmgGasR = "character_gun_mag_r";
            WeaponLMesh = "character_gun_l";
            WeaponRMesh = "character_gun_r";
        }
        else if (UniformType == UniformType.UniformA)
        {
            ArmLMesh = "character_arm_uniform_l";
            ArmRMesh = "character_arm_uniform_r";
            Brand1Mesh = "character_brand_arm_l";
            Brand2Mesh = "character_brand_arm_r";
            if (Sex == Sex.Female)
            {
                BodyMesh = "character_body_uniform_FA";
                Brand3Mesh = "character_brand_chest_f";
                Brand4Mesh = "character_brand_back_f";
            }
            else
            {
                BodyMesh = "character_body_uniform_MA";
                Brand3Mesh = "character_brand_chest_m";
                Brand4Mesh = "character_brand_back_m";
            }
        }
        else if (UniformType == UniformType.UniformB)
        {
            ArmLMesh = "character_arm_uniform_l";
            ArmRMesh = "character_arm_uniform_r";
            Brand1Mesh = "character_brand_arm_l";
            Brand2Mesh = "character_brand_arm_r";
            if (Sex == Sex.Female)
            {
                BodyMesh = "character_body_uniform_FB";
                Brand3Mesh = "character_brand_chest_f";
                Brand4Mesh = "character_brand_back_f";
            }
            else
            {
                BodyMesh = "character_body_uniform_MB";
                Brand3Mesh = "character_brand_chest_m";
                Brand4Mesh = "character_brand_back_m";
            }
        }
        else if (UniformType == UniformType.CasualA)
        {
            ArmLMesh = "character_arm_casual_l";
            ArmRMesh = "character_arm_casual_r";
            if (Sex == Sex.Female)
            {
                BodyMesh = "character_body_casual_FA";
            }
            else
            {
                BodyMesh = "character_body_casual_MA";
            }
        }
        else if (UniformType == UniformType.CasualB)
        {
            ArmLMesh = "character_arm_casual_l";
            ArmRMesh = "character_arm_casual_r";
            if (Sex == Sex.Female)
            {
                BodyMesh = "character_body_casual_FB";
            }
            else
            {
                BodyMesh = "character_body_casual_MB";
            }
        }
        if (HairInfo.hair.Length > 0)
        {
            HairMesh = HairInfo.hair;
        }
        if (HairInfo.hasCloth)
        {
            Hair1Mesh = HairInfo.hair_1;
        }
        if (EyeTextureId >= 0)
        {
            EyeMesh = "character_eye";
        }
        if (BeardTextureId >= 0)
        {
            BeardMesh = "character_face";
        }
        else
        {
            BeardMesh = string.Empty;
        }
        if (GlassTextureId >= 0)
        {
            GlassMesh = "glass";
        }
        else
        {
            GlassMesh = string.Empty;
        }
        SetCape();
    }

    public void SetTexture()
    {
        if (UniformType == UniformType.CasualAHSS)
        {
            DmgTexture = "aottg_hero_AHSS_3dmg";
        }
        else
        {
            DmgTexture = "AOTTG_HERO_3DMG";
        }
        FaceTexture = "aottg_hero_eyes";
        if (Division == Division.TheMilitaryPolice)
        {
            BrandTexture = "aottg_hero_brand_mp";
        }
        if (Division == Division.TheGarrison)
        {
            BrandTexture = "aottg_hero_brand_g";
        }
        if (Division == Division.TheSurveryCorps)
        {
            BrandTexture = "aottg_hero_brand_sc";
        }
        if (Division == Division.TraineesSquad)
        {
            BrandTexture = "aottg_hero_brand_ts";
        }
        if (SkinColor == 1)
        {
            SkinTexture = "aottg_hero_skin_1";
        }
        else if (SkinColor == 2)
        {
            SkinTexture = "aottg_hero_skin_2";
        }
        else if (SkinColor == 3)
        {
            SkinTexture = "aottg_hero_skin_3";
        }
    }
}

