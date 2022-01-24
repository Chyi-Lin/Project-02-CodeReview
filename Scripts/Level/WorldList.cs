using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualFramework
{
    [CreateAssetMenu(fileName = "New World List", menuName = "HyperCasual/Level/Create World List", order = 50)]
    public class WorldList : DataList<LevelList>
    {
        [SerializeField, Header("世界清單清單")]
        protected List<LevelList> worldList;

        /// <summary>
        /// 透過 Id 來取得資料
        /// </summary>
        public override LevelList GetData(string id)
        {
            return worldList.Find((i) => i.GetId == id);
        }

        /// <summary>
        /// 透過索引來取得資料
        /// </summary>
        public override LevelList GetData(int position)
        {
            return worldList[position];
        }

        /// <summary>
        /// 取得關卡清單，由關卡 ID 來尋找
        /// </summary>
        public virtual LevelList GetListByLevelId(string levelId)
        {
            return worldList.Find((i) => i.GetData(levelId) != null);
        }

        public virtual LevelData GetDataByLevelId(string levelId)
        {
            return GetListByLevelId(levelId).GetData(levelId);
        }

        /// <summary>
        /// 取得資料長度
        /// </summary>
        public override int GetLength()
        {
            return worldList.Count;
        }
    }
}
