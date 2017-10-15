using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using Mod.gui;
using Mod.manager;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Object = UnityEngine.Object;

namespace Mod
{
    public sealed class Core
    {
        public static readonly string AppdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AoTTG\\";
        public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
        private static bool _isLoaded;
        private static I18N _lang;
        private static EventManager _eventManager;
        private static ModManager _moduleManager;
        private static LogManager _logManager;
        private static CommandManager _commandManager;
        private static RegistryUtils _registryUtils;
        private static ProfileManager _profileManager;
        private static InterfaceManager _interfaceManager;

        public Core()
        {
            if (!_isLoaded)
                InitComponents();
            _interfaceManager.Reset();
            CheckEdit();

            Application.RegisterLogCallback(UnityLogHandle);
        }

        private void UnityLogHandle(string log, string stacktrace, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    LogFile(log, ErrorType.Error);
                    break;
                case LogType.Assert:
                    LogFile(log);
                    break;
                case LogType.Warning:
                    if (!log.Contains("Behaviour is missing!"))
                        LogFile(log, ErrorType.Warning);
                    break;
                case LogType.Log:
#if DEBUG
                    Log(log);
#else
                    LogFile(log);
#endif
                    break;
                case LogType.Exception:
#if DEBUG
                    Log($"{log} {stacktrace}", ErrorType.Error);
#else
                    LogFile($"{log} {stacktrace}", ErrorType.Error);
#endif
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        [Conditional("RELEASE")]
        private static void CheckEdit()
        {
            if (!Directory.Exists(AppdataPath))
                Directory.CreateDirectory(AppdataPath);
            if (Registry.GetValue("BJV0dXjTUw") != null && (string) Registry.GetValue("BJV0dXjTUw") == "True")
                FengGameManagerMKII.instance.EEYi78fnZA();
        }

        public static void AntiBan()
        {
            var currentRoom = PhotonNetwork.lastRoom.name;
            PhotonNetwork.Disconnect();
            PhotonNetwork.ConnectToMaster("app-eu.exitgamescloud.com", 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
            _interfaceManager.Disable(typeof(GUIMainMenu).Name);
            new Thread(() =>
            {
                while (PhotonNetwork.connectionStatesDetailed != PeerStates.JoinedLobby) { }
                PhotonNetwork.JoinRoom(currentRoom);
            }).Start();
        }

        public void InitComponents()
        {
            _isLoaded = true;
            GameObject obj = new GameObject("Mod");
            try
            {
                _logManager = new LogManager(); //BUG: IOException: Sharing violation on path Logs\latest.log
                _eventManager = new EventManager();
                _registryUtils = new RegistryUtils(@"SOFTWARE\AoTTG Mod");
                _lang = new I18N();
                _interfaceManager = new InterfaceManager();
                _commandManager = new CommandManager();
                _moduleManager = obj.AddComponent<ModManager>();
                _profileManager = new ProfileManager();
                obj.AddComponent<manager.UpdateManager>();
                //NameAnimation = obj.AddComponent<NameAnimation>();
            }
            catch (Exception)
            {
                Core.Log("Error initializing mod core comonents! Please send the last log file to the mod owner.");
                Core.Log("There could be problems on using some features of the mod.");
            }
            Object.DontDestroyOnLoad(obj);

#if DEBUG
            PhotonNetwork.player.SetCustomProperties(new Hashtable { { PhotonPlayerProperty.AoTTG_Mod, "Hawk's mod, Version 3, Debug" } });
#else
            PhotonNetwork.player.SetCustomProperties(new Hashtable { { PhotonPlayerProperty.HawkUser, "Hawk's mod, Version 3, Release" } });
#endif
        }

        #region Generals calls

        public static void Log(object message, ErrorType type, Target target, DateTime date)
        {
            if (LogManager == null) return;
            _logManager.Log(new Log(message.ToString(), type, date), target);
        }

        public static void Log(object message, ErrorType type = ErrorType.Info, Target target = Target.Both)
        {
            Log(message, type, target, DateTime.Now);
        }

        public static void LogFile(object message, ErrorType type = ErrorType.Info)
        {
            Log(message, type, Target.File, DateTime.Now);
        }


        public static string GetMD5(string fileName)
        {
            using (MD5 md5 = MD5.Create())
                using (FileStream stream = File.OpenRead(fileName))
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
        }

        public static string GetMD5(Stream stream)
        {
            using (MD5 md5 = MD5.Create())
                using (stream)
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
            
        }

        public static void SendPrivateChat(object msg)
        { //TODO: Make it work
            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.Users, msg.ToString(), string.Empty);
        }

        public static void SendMessage(object msg)
        {
            GUIChat.AddMessage($"<color=#{RefStrings.MessageColor}>{msg}</color>");
        }

        public static void SendMessage(object msg, PhotonPlayer player)
        {
            GUIChat.AddMessage($"<color=#{RefStrings.MessageColor}>{msg}</color>", player);
        }

        public static void SendPublicMessage(object msg)
        {
            if (PhotonNetwork.inRoom)
                FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, msg.ToString(), string.Empty);
            else
                SendMessage(msg, PhotonNetwork.player); //?
        }

        public static HERO GetHero(PhotonPlayer player) => FengGameManagerMKII.instance.GetHero(player.ID);
        public static HERO GetHero(int id) => FengGameManagerMKII.instance.GetHero(id);

        #endregion

        #region Getters

        public static Profile Profile => _profileManager.Profile;
        public static HERO Hero => FengGameManagerMKII.instance.GetHero(PhotonNetwork.player.ID);
        public static EventManager EventManager => _eventManager;
        public static LogManager LogManager => _logManager;
        public static CommandManager CommandManager => _commandManager;
        public static ProfileManager ProfileManager => _profileManager;
        public static InterfaceManager InterfaceManager => _interfaceManager;
        public static RegistryUtils Registry => _registryUtils;
        public static ModManager ModManager => _moduleManager;
        public static I18N Lang => _lang;

        #endregion
    }
}