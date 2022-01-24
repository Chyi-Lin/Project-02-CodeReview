using UnityEngine;

namespace HyperCasualFramework
{
    public abstract class UISettlementBase : MonoBehaviour
    {
        protected void Awake()
        {
            Initialization();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected abstract void Initialization();

        /// <summary>
        /// 設置結算介面
        /// </summary>
        public abstract void SetupSettlement(GameManager manager);
    }
}
