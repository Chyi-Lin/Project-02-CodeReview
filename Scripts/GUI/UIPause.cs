using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HyperCasualFramework
{
    public class UIPause : MonoBehaviour
    {
        [Header("Button")]
        public Button buttonResume;
        public Button buttonRestart;
        public Button buttonBack;

        [Header("Secne Name")]
        public string mainSceneName;

        /// <summary>
        /// 返回遊戲
        /// </summary>
        protected virtual void OnResume()
        {
            if (GameManager.Instance == null)
                return;

            GameManager.Instance.Pause();
        }

        /// <summary>
        /// 重新開始
        /// </summary>
        protected virtual void OnRestart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// 返回
        /// </summary>
        protected virtual void OnBack()
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
            if (buttonResume)
                buttonResume.onClick.AddListener(OnResume);

            if (buttonRestart)
                buttonRestart.onClick.AddListener(OnRestart);

            if (buttonBack)
                buttonBack.onClick.AddListener(OnBack);
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        protected virtual void OnDisable()
        {
            if (buttonResume)
                buttonResume.onClick.RemoveListener(OnResume);

            if (buttonRestart)
                buttonRestart.onClick.RemoveListener(OnRestart);

            if (buttonBack)
                buttonBack.onClick.RemoveListener(OnBack);
        }
    }
}