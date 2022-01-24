using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualFramework
{
    public class SimpleObjectPooler : MMSimpleObjectPooler
    {
        [SerializeField]
        protected Transform targetParent;

        public Transform GetTargetParent() => targetParent;

        public Transform GetPoolerParent() => _waitingPool.transform;
    }
}

