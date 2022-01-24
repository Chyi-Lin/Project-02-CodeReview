using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HyperCasualFramework
{
    public class UISettlement : UISettlementBase
    {
        [Header("Button")]
        public Button buttonNext;
        public Button buttonRestart;
        public Button buttonBack;

        [Header("Settlement Text")]
        public Text textTime;
        public Text textPoint;

        [Header("Secne Name")]
        public string nextSceneName;
        public string mainSceneName;

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Initialization()
        {
            
        }

        /// <summary>
        /// 設置結算介面
        /// </summary>
        public override void SetupSettlement(GameManager manager)
        {
            textTime.text = $"{Mathf.FloorToInt(manager.time / 60f).ToString("00")}:{Mathf.FloorToInt(manager.time % 60f).ToString("00")}";
            textPoint.text = manager.points.ToString();
        }

        /// <summary>
        /// 下一關
        /// </summary>
        public virtual void OnNext()
        {
            if (string.IsNullOrEmpty(nextSceneName))
            {
                Debug.LogWarning("Next scene name is null or empty!");
                return;
            }

            SceneManager.LoadScene(nextSceneName);
        }

        /// <summary>
        /// 重新開始
        /// </summary>
        public virtual void OnRestart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// 返回
        /// </summary>
        public virtual void OnBack()
        {
            if (string.IsNullOrEmpty(mainSceneName))
            {
                Debug.LogWarning("Next scene name is null or empty!");
                return;
            }

            SceneManager.LoadScene(mainSceneName);
        }

        /// <summary>
        /// 設定事件
        /// </summary>
        protected virtual void OnEnable()
        {
            buttonNext?.onClick.AddListener(OnNext);
            buttonRestart?.onClick.AddListener(OnRestart);
            buttonBack?.onClick.AddListener(OnBack);
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        protected virtual void OnDisable()
        {
            buttonNext?.onClick.RemoveListener(OnNext);
            buttonRestart?.onClick.RemoveListener(OnRestart);
            buttonBack?.onClick.RemoveListener(OnBack);
        }
    }
}
