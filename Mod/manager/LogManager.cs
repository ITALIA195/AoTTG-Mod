using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Mod.exceptions;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Mod.manager
{
    public class LogManager : List<Log>
    {
        // Cant optimized it (need to write asap)
        private readonly StreamWriter _file;

        public LogManager()
        {
            var path = Application.dataPath + "/Logs/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (File.Exists(path + "latest.log"))
            {
                string date;
                try
                {
                    date =
                        File.ReadAllLines(path + "latest.log")[1].Substring(1)
                            .Replace('/', '_')
                            .Replace(':', '_');
                }
                catch (Exception)
                {
                    date = DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss");
                }
                if (File.ReadAllLines(path + "latest.log").Length > 3)
                    File.Move(path + "latest.log", path + $"{date}.log");
                else File.Delete(path + "latest.log");
            }

            File.WriteAllLines(path + "latest.log", new[] {"#Hawk's AoTTG Mod logs", "#" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), ""});
            _file = new StreamWriter(path + "latest.log", true) {AutoFlush = true};
        }

        public void Log(Log log, Target target)
        {
            if (target == Target.Client || target == Target.Both)
                Add(log);
            if (target == Target.File || target == Target.Both)
                _file.WriteLine(log);
        }

        public string Logs
        {
            get
            {
                if (Count > 20) RemoveRange(0, Count - 20);
                return this.Aggregate(string.Empty, (current, log) => current + Environment.NewLine + log.Message);
            }
        }

        public static void Serialize(PhotonPlayer sender, byte eventCode, Dictionary<byte, object> args)
        {
            if (eventCode == 200 || eventCode == 226 || eventCode == 230 || eventCode == 229)
                return; // No rpcs (They're log is on ExecuteRPC())

            StringBuilder sb = new StringBuilder($"Event {eventCode}");
            if (sender != null)
                sb.Append($", Sent by {sender}");
            
            if (args.Count > 0)
            {
                sb.Append(", Parameters: [ ");
                foreach (var @byte in args.Keys)
                {
                    if (@byte == 254) continue;
                    if (@byte == 245 && (eventCode == 201 || eventCode == 206))
                        sb.Append(SerializeOSR((Hashtable) args[@byte]));

                    sb.Append($"({@byte}) {SerializeType(args[@byte])}");
                }
                sb.Append("]");
            }

            Core.Log(sb.ToString(), ErrorType.UnityEvent);
        }

        private static string SerializeType(object obj)
        {
            if (obj is int)
                return $"{obj} ";
            if (obj is long)
                return $"{obj}L ";
            if (obj is float)
                return $"{obj}F ";
            if (obj is byte)
                return $"{obj}b ";
            if (obj is uint || obj is ulong || obj is ushort || obj is short)
                return $"={obj.GetType()}= {obj}";
            if (obj is string || obj is char)
                return $"{obj}";
            if (obj is Hashtable)
                return SerializeHashtable(obj);
            if (obj is Array)
                return SerializeArray(obj);
            try
            {
                return obj.ToString();
            }
            catch
            {
                return "Unrecognised";
            }
        }

        // ReSharper disable once PossibleNullReferenceException
        public static string SerializeOSR(object obj)
        {
            var hashtable = obj as Hashtable;
            StringBuilder sb = new StringBuilder();
            foreach (var entry in hashtable)
            {
                var index = entry.Key as short?;
                if (!index.HasValue) continue;
                switch (index.Value)
                {
                    case 0:
                        sb.Append("NetTime: " + entry.Value);
                        break;
                    case 1:
                        sb.Append("Prefix: " + entry.Value);
                        break;
                    default:
                        if (index.Value > 1)
                            sb.Append(SerializeHashtable((Hashtable) entry.Value));
                        break;
                }
                sb.Append(", ");
            }
            return sb.ToString();
        }

        public static string SerializeArray(object rawArray)
        {
            StringBuilder sb = new StringBuilder("== Array == ");
            var array = rawArray as IEnumerable;
            if (array == null) return $"== Array: {rawArray.GetType()} ==";
            sb.Append("[");
            foreach (var entry in array)
                sb.Append(" " + SerializeType(entry));
            sb.Append("]");
            return sb.ToString();
        }

        public static string SerializeHashtable(object hashtable1)
        {
            var hashtable = hashtable1 as Hashtable;
            StringBuilder sb = new StringBuilder("== Hashtable == ");
            if (hashtable == null) return sb.ToString();
            sb.Append('[');
            foreach (var entry in hashtable)
                sb.Append($" {{{SerializeType(entry.Key)}: {SerializeType(entry.Value)}}}");
            sb.Append(']');
            return sb.ToString();
        }
    }
}
