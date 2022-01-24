using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualFramework
{
    public class ChangeTargetColor : MonoBehaviour
    {
        [SerializeField]
        protected LayerMask targetLayer;

        [SerializeField, Tooltip("改變顏色用觸發器")]
        protected Collider changeColorTrigger;

        [SerializeField]
        protected Color changeColor = Color.white;

        protected ObjectPoolsManager _poolsManager;

        protected void Awake()
        {
            Initialization();
        }

        protected void Initialization()
        {
            _poolsManager = ObjectPoolsManager.Instance;
            changeColorTrigger.enabled = false;
        }

        /// <summary>
        /// 變更顏色
        /// </summary>
        public void SetColor(Color color)
        {
            changeColor = color;
        }

        /// <summary>
        /// 觸發進入事件
        /// </summary>
        protected void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & targetLayer) != 0)
            {
                ColorTile colorTile = other.gameObject.GetComponent<ColorTile>();
                if (colorTile != null)
                {
                    colorTile.SetColor(changeColor);
                }
            }
        }

        /// <summary>
        /// 碰撞進入事件
        /// </summary>
        protected void OnCollisionEnter(Collision collision)
        {
            if (((1 << collision.gameObject.layer) & targetLayer) != 0)
            {
                // 開啟染色用觸發器
                changeColorTrigger.enabled = true;
            }
        }

        /// <summary>
        /// 碰撞離開事件
        /// </summary>
        protected void OnCollisionExit(Collision collision)
        {
            if (((1 << collision.gameObject.layer) & targetLayer) != 0)
            {
                // 關閉染色用觸發器
                changeColorTrigger.enabled = false;

                // 顯示染色粒子特效
                GameObject gameObject = _poolsManager.GetObject("PaintParticle", transform.position);
                if (gameObject != null)
                {
                    gameObject.GetComponent<ChangeParticleColor>().SetColor(changeColor);
                    gameObject.GetComponent<ParticleSystem>().Play();
                }
            }
        }
    }
}
