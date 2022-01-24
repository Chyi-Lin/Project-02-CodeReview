using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualFramework
{
    public abstract class PropsBase : MonoBehaviour
    {
        protected abstract void OnTriggerEnter(Collider other);
    }
}
