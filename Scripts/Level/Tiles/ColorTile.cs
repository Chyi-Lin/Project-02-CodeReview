using MoreMountains.Feedbacks;
using MoreMountains.TopDownEngine;
using UnityEngine;

namespace HyperCasualFramework
{
    public class ColorTile : MaterialTile
    {
        [SerializeField, Header("設定")]
        protected Renderer targetRenderer;

        [SerializeField, Header("Feedbacks")]
        protected MMFeedbacks collisionEnterFeedbacks;

        [SerializeField]
        protected MMFeedbacks triggerEnterFeedbacks;

        protected bool _isSetColor;
        protected Color _originColor;
        protected MaterialPropertyBlock _propertyBlock;

        protected override void Initialization()
        {
            _isSetColor = false;
            _originColor = Color.white;

            _propertyBlock = new MaterialPropertyBlock();
            _propertyBlock.SetColor("_Color", _originColor);
            targetRenderer.SetPropertyBlock(_propertyBlock);
        }

        /// <summary>
        /// 設定顏色
        /// </summary>
        public override void SetColor(Color color)
        {
            triggerEnterFeedbacks?.PlayFeedbacks(this.transform.position);

            // 是否完成
            if (IsFinishColor(color))
                return;

            _propertyBlock.SetColor("_Color", color);
            targetRenderer.SetPropertyBlock(_propertyBlock);

            OnTileCompleteEvent?.Invoke(this.tileType, this);
            TopDownEnginePointEvent.Trigger(PointsMethods.Add, 1);

            _isSetColor = true;
        }

        /// <summary>
        /// 是否完成染色
        /// </summary>
        public virtual bool IsFinishColor(Color color)
        {
            return _isSetColor;
        }

        protected override void CollisionEnterEvent(GameObject go)
        {
            collisionEnterFeedbacks?.PlayFeedbacks(this.transform.position);
        }
    }
}
