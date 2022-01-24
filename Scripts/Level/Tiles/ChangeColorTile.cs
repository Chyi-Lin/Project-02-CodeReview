using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualFramework
{
    /// <summary>
    /// 改變玩家顏色地板
    /// </summary>
    public class ChangeColorTile : MaterialTile
    {
        [SerializeField, Header("目標顏色")]
        protected Color targetColor = Color.white;

        [SerializeField, Header("設定")]
        protected Renderer targetRenderer;

        [SerializeField, Header("Feedbacks")]
        protected MMFeedbacks collisionEnterFeedbacks;

        protected MaterialPropertyBlock _propertyBlock;

        public override void SetColor(Color color)
        {
            
        }

        protected override void Initialization()
        {
            _propertyBlock = new MaterialPropertyBlock();
            _propertyBlock.SetColor("_Color", targetColor);
            targetRenderer.SetPropertyBlock(_propertyBlock);
        }

        protected override void CollisionEnterEvent(GameObject go)
        {
            collisionEnterFeedbacks?.PlayFeedbacks(this.transform.position);
            ChangeColor(go);
        }

        /// <summary>
        /// 改變顏色
        /// </summary>
        protected virtual void ChangeColor(GameObject go)
        {
            ChangeSelfColor changeSelfColor = go.GetComponent<ChangeSelfColor>();
            if (changeSelfColor != null)
            {
                changeSelfColor.SetColor(targetColor);
            }

            ChangeTargetColor changeTargetColor = go.GetComponent<ChangeTargetColor>();
            if (changeTargetColor != null)
            {
                changeTargetColor.SetColor(targetColor);
            }
        }
    }
}
