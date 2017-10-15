using System.Collections;
using System.Linq;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Mod
{
    public class PropSystem : MonoBehaviour
    {
        private readonly Hashtable props;

        public PropSystem(Hashtable props)
        {
            this.props = props;
            
        }

        public void Compare(Hashtable compareTo)
        {
            Hashtable differences = new Hashtable();
            bool match = false;
            foreach (DictionaryEntry entry in props)
            {
                foreach (DictionaryEntry entry2 in compareTo)
                {
                    if (entry.Key.Equals(entry2.Key))
                        match = true;
                }
                if (!match)
                    differences.Add(entry.Key, entry.Value);
                match = false;
            }
        }
    }
}
