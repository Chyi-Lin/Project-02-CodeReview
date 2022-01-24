using UnityEngine;
using UnityEngine.Events;

namespace HyperCasualFramework
{
    [System.Serializable]
    public class PassTileEvent : UnityEvent<TileType, TileBase> { }

    /// <summary>
    /// 地板基礎
    /// </summary>
    public abstract class TileBase : MonoBehaviour
    {
        [SerializeField, Header("互動設定")]
        protected bool interactive = true;

        [SerializeField]
        protected LayerMask interactiveLayer;

        [SerializeField, Header("地板種類")]
        protected TileType tileType = TileType.Normal;

        /// <summary>
        /// 地板完成顏色事件
        /// </summary>
        [HideInInspector]
        public PassTileEvent OnTileCompleteEvent;

        /// <summary>
        /// 地板錯誤顏色事件
        /// </summary>
        [HideInInspector]
        public PassTileEvent OnTileWrongEvent;

        protected virtual void Awake()
        {
            Initialization();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected abstract void Initialization();

        /// <summary>
        /// 設定顏色
        /// </summary>
        public abstract void SetColor(Color color);

        /// <summary>
        /// 物件碰撞進入事件
        /// </summary>
        protected abstract void CollisionEnterEvent(GameObject go);

        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (interactive)
            {
                if (((1 << collision.gameObject.layer) & interactiveLayer) != 0)
                {
                    CollisionEnterEvent(collision.gameObject);
                }
            }
        }
    }
}
