using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualFramework
{
    /// <summary>
    /// 雙色地板
    /// </summary>
    public class OverrideColorTile : ColorTile
    {
        [SerializeField, Header("是否完成")]
        protected bool isComplete = false;

        [SerializeField, Header("目標顏色")]
        protected Color targetColor = Color.white;

        protected bool _hasGetPoint;

        protected override void Initialization()
        {
            _isSetColor = false;
            _originColor = Color.white;

            _propertyBlock = new MaterialPropertyBlock();
            _propertyBlock.SetColor("_Color", _originColor);
            _propertyBlock.SetColor("_SecondaryColor", targetColor);
            targetRenderer.SetPropertyBlock(_propertyBlock);
        }

        public override void SetColor(Color color)
        {
            triggerEnterFeedbacks?.PlayFeedbacks(this.transform.position);

            _propertyBlock.SetColor("_Color", color);
            targetRenderer.SetPropertyBlock(_propertyBlock);

            if (IsFinishColor(color) && isComplete == false)
            {
                OnTileCompleteEvent?.Invoke(this.tileType, this);
                isComplete = true;
            }
            else if(!IsFinishColor(color) && isComplete == true)
            {
                OnTileWrongEvent?.Invoke(this.tileType, this);
                isComplete = false;
            }

            if (!_hasGetPoint)
            {
                _hasGetPoint = true;
                TopDownEnginePointEvent.Trigger(PointsMethods.Add, 1);
            }
        }

        public override bool IsFinishColor(Color color)
        {
            return targetColor == color;
        }

        protected override void CollisionEnterEvent(GameObject go)
        {
            collisionEnterFeedbacks?.PlayFeedbacks(this.transform.position);
        }
    }
}

