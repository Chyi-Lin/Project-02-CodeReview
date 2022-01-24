using MoreMountains.Feedbacks;
using UnityEngine;

namespace HyperCasualFramework
{
    /// <summary>
    /// 改變自己的模型顏色
    /// </summary>
    public class ChangeSelfColor : MonoBehaviour
    {
        [SerializeField, Header("TargetRenderer")]
        protected Renderer targetRenderer;

        [SerializeField, Header("Default")]
        protected Color defaultColor = Color.white;

        protected ChangeSelfModel model;

        [SerializeField]
        protected MMFeedback changeColorFeedback;

        protected MaterialPropertyBlock _propertyBlock;

        protected void Awake()
        {
            Initialization();
            InitializationColor();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected void Initialization()
        {
            model = GetComponent<ChangeSelfModel>();
        }

        /// <summary>
        /// 初始化顏色
        /// </summary>
        protected void InitializationColor()
        {
            _propertyBlock = new MaterialPropertyBlock();
            _propertyBlock.SetColor("_Color", defaultColor);

            //targetRenderer = model.GetRenderer();
            targetRenderer.SetPropertyBlock(_propertyBlock);
        }

        /// <summary>
        /// 設定顏色
        /// </summary>
        public void SetColor(Color color)
        {
            changeColorFeedback?.Play(this.transform.position);

            _propertyBlock.SetColor("_Color", color);
            targetRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}