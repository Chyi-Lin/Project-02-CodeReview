using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualFramework
{
    public class ObjectPoolsManager : MMPersistentSingleton<ObjectPoolsManager>
    {
        [SerializeField]
        protected List<MMObjectPooler> objectPoolList;

        protected Dictionary<string, MMObjectPooler> objectPoolsDict = new Dictionary<string, MMObjectPooler>();

        protected override void Awake()
        {
            Initialization();
        }

        protected void Initialization()
        {
            for (int i = 0; i < objectPoolList.Count; i++)
            {
                objectPoolsDict.Add(objectPoolList[i].name, objectPoolList[i]);
            }
        }

        protected void InitializationPoolerParent()
        {
            for (int i = 0; i < objectPoolList.Count; i++)
            {
                SetObjectPoolerParent(objectPoolList[i]);
            }
        }

        protected void Start()
        {
            InitializationPoolerParent();
        }

        public MMObjectPooler GetObjectPools(string poolName)
        {
            if (objectPoolsDict.TryGetValue(poolName, out MMObjectPooler pooler))
                return pooler;

            return null;
        }

        public GameObject GetObject(string poolName, Vector3 position)
        {
            if (objectPoolsDict.TryGetValue(poolName, out MMObjectPooler pooler))
            {
                GameObject gameObject = pooler.GetPooledGameObject();

                if (gameObject == null)
                    return null;

                gameObject.transform.position = position;
                gameObject.SetActive(true);
                return gameObject;
            }

            return null;
        }

        protected void SetObjectPoolerParent(MMObjectPooler objectpooler)
        {
            if (objectpooler.GetType() != typeof(SimpleObjectPooler))
                return;

            SimpleObjectPooler pooler = objectpooler as SimpleObjectPooler;

            if (pooler.GetTargetParent() == null)
                return;

            pooler.GetPoolerParent().SetParent(pooler.GetTargetParent());
        }

    }
}

