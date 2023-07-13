using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public abstract class UnityFrameworkComponent : MonoBehaviour
    {
        protected virtual void Awake()
        {
            GameEntry.Register(this);
        }
    }
}