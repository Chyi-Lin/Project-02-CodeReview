using MoreMountains.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HyperCasualFramework
{
    [CreateAssetMenu(fileName = "New Level List", menuName = "HyperCasual/Level/Create Level List", order = 50)]
    public class LevelList : DataList<LevelData>
    {
        [SerializeField, Header("清單ID"), Tooltip("欄位保留")]
        protected string id;

        [SerializeField, Header("清單編號"), Tooltip("給世界清單使用，每個關卡清單就是一個世界")]
        protected int listNumber;

        [SerializeField, Header("關卡清單")]
        protected List<LevelData> levelList;

        [Header("自動排序")]
        [MMInspectorButton("Sort", order = 0)]
        public bool autoSort;

        /// <summary>
        /// 取得清單 Id
        /// </summary>
        public string GetId { get { return id; } }

        /// <summary>
        /// 取得清單編號
        /// </summary>
        public int GetListNumber { get { return listNumber; } }

        /// <summary>
        /// 透過 Id 來取得資料
        /// </summary>
        public override LevelData GetData(string id)
        {
            return levelList.Find((data) => data.GetId == id);
        }

        /// <summary>
        /// 透過索引來取得資料
        /// </summary>
        public override LevelData GetData(int position)
        {
            if (position >= levelList.Count)
            {
                Debug.LogWarning("索引已超過資料大小!");
                return null;
            }

            return levelList[position];
        }

        /// <summary>
        /// 透過編號來取得資料
        /// </summary>
        public virtual LevelData GetDataByNumber(int levelNum)
        {
            return levelList.Find((data) => data.GetLevelNumber == levelNum);
        }

        /// <summary>
        /// 檢查是否可選擇此關卡清單
        /// </summary>
        public virtual bool CheckSelectLevelList(int levelNum)
        {
            return levelList.Find((data) => data.GetLevelNumber <= levelNum + 1);
        }

        /// <summary>
        /// 取得資料長度
        /// </summary>
        public override int GetLength()
        {
            return levelList.Count;
        }

        /// <summary>
        /// 資料排序，由編號排序
        /// </summary>
        public void Sort()
        {
            levelList = levelList.OrderBy(x => x.GetLevelNumber)
                .ToList();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif

            MMDebug.DebugLogTime("資料排序完成！");
        }
    }
}
