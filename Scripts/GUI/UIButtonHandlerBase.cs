using UnityEngine;
using UnityEngine.Events;

namespace HyperCasualFramework
{
    /// <summary>
    /// 設定按鈕相關資訊，按鈕文字、圖片、顏色等等
    /// </summary>
    public class UIButtonHandlerBase : MonoBehaviour
    {
        protected virtual void Awake()
        {
            Initialization();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Initialization()
        {

        }

        /// <summary>
        /// 設定文字
        /// </summary>
        public virtual void SetText(string text)
        {
            
        }

        /// <summary>
        /// 設定圖片與顏色
        /// </summary>
        public virtual void SetImage(Sprite image, Color color)
        {

        }

        /// <summary>
        /// 設定顏色
        /// </summary>
        public virtual void SetColor(Color color)
        {

        }

        /// <summary>
        /// 設定按鈕事件
        /// </summary>
        public virtual void SetButtonEvent(UnityAction<GameObject> unityAction)
        {

        }
    }
}
