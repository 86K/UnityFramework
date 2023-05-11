using System.Collections.Generic;
using UnityEngine;

namespace UnityFramework.Editor
{
    public abstract class BaseAsset : ScriptableObject
    {
        public virtual float keyRectWidth
        {
            get { return 0.5f; }
        }

        public abstract bool IsDuplicateKey(int index);
    }
    
    public class BaseAsset<TKey, TValue> : BaseAsset, ISerializationCallbackReceiver
    {
        public List<TKey> keys = new List<TKey>();
        public List<TValue> values = new List<TValue>();

        public Dictionary<TKey, int> keyCount = new Dictionary<TKey, int>();

        public override bool IsDuplicateKey(int index)
        {
            TKey key = keys[index];
            return keyCount.ContainsKey(key) && keyCount[key] > 1;
        }

        public virtual void OnBeforeSerialize()
        {
        }

        public virtual void OnAfterDeserialize()
        {
            keyCount.Clear();
            for (int i = 0; i < keys.Count; i++)
            {
                TKey key = keys[i];
                if (key == null)
                {
                    continue;
                }

                if (keyCount.ContainsKey(key))
                {
                    keyCount[key]++;
                }
                else
                {
                    keyCount[key] = 1;
                }
            }
        }
    }
}