using MoreMountains.Feedbacks;
using UnityEngine;

namespace HyperCasualFramework
{
    /// <summary>
    /// 傳送地板
    /// </summary>
    public class ProtolTile : MaterialTile
    {
        [SerializeField, Header("傳送目標")]
        protected ProtolTile targetTile;
        [SerializeField]
        protected Vector3 protolOffset;

        [SerializeField, Header("Feedbacks")]
        protected MMFeedbacks protolFeedbacks;

        protected MovementController movementController;

        public override void SetColor(Color color)
        {

        }

        protected override void Initialization()
        {
            
        }

        /// <summary>
        /// 取得傳送位置
        /// </summary>
        protected Vector3 GetProtolPostion()
        {
            return targetTile.transform.position + protolOffset;
        }

        /// <summary>
        /// 傳送
        /// </summary>
        protected void Protol(GameObject go)
        {
            if (targetTile == null)
                return;

            movementController = go.GetComponent<MovementController>();
            if(movementController != null)
                movementController.SetPosition(GetProtolPostion());

            targetTile.protolFeedbacks?.PlayFeedbacks(transform.position);
        }

        protected void OnDrawGizmosSelected()
        {
            if (targetTile == null)
                return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.localPosition, .25f);
            Gizmos.DrawWireSphere(GetProtolPostion(), .25f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.localPosition, GetProtolPostion());
        }

        protected override void CollisionEnterEvent(GameObject go)
        {
            Protol(go);
        }
    }
}
