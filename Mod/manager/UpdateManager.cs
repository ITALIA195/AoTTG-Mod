using System.Collections;
using System.IO;
using UnityEngine;

namespace Mod.manager
{
    public class UpdateManager : MonoBehaviour
    {
        public void Start()
        {
#if !DEBUG
            StartCoroutine(DoUpdate());
#endif
        }

        private static IEnumerator DoUpdate()
        {
            using (WWW www = new WWW("https://drive.google.com/uc?export=download&id=0B1mUx38J_SMeSTdjMUFYc0RKZFk"))
            {
                if (!string.IsNullOrEmpty(www.error))
                {
                    Core.Log("Error updating!", ErrorType.Error);
                    Core.LogFile(www.error);
                }
                yield return www;
                if (CheckForUpdates(www.bytes))
                {
                    File.WriteAllBytes($"{Application.dataPath}/Managed/Assembly-CSharp.dll", www.bytes);
                    Core.Log(Core.Lang["message.gameupdate.succeed.text"]);
                }
                else
                {
                    Core.Log(Core.Lang["message.gameupdate.noneed.text"]);
                }
            }
        }

        private static bool CheckForUpdates(byte[] bytes) => Core.GetMD5(new MemoryStream(bytes)) != Core.GetMD5($"{Application.dataPath}/Managed/Assembly-CSharp.dll");
    }
}
