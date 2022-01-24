using UnityEngine;
using UnityEngine.UI;

namespace HyperCasualFramework
{
    /// <summary>
    /// 排行榜清單紀錄項目
    /// </summary>
    public class UIRecordItem : MonoBehaviour
    {
        [SerializeField]
        protected bool isMe;

        [SerializeField]
        protected Text textRank;

        [SerializeField]
        protected Text textName;

        [SerializeField]
        protected Text textPoint;

        /// <summary>
        /// 是否是自己的紀錄
        /// </summary>
        public bool IsMe { get { return isMe; } }

        /// <summary>
        /// 設定紀錄，Rank 為 -1 則不顯示
        /// </summary>
        public void SetRecordItem(int rank, string name, string point)
        {
            if(rank == -1)
                textRank.text = "";
            else
                textRank.text = rank.ToString();
            textName.text = name;
            textPoint.text = point;
        }

        /// <summary>
        /// 設定紀錄，Rank 為 -1 則不顯示
        /// </summary>
        public void SetRecordItem(int rank, string name)
        {
            SetRecordItem(rank, name, "");
        }
    }
}
