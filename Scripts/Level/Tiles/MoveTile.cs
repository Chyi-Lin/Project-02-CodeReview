using MoreMountains.Feedbacks;
using UnityEngine;

namespace HyperCasualFramework
{
    public class MoveTile : MaterialTile
    {
        [SerializeField, Header("Feedbacks")]
        protected MMFeedbacks collisionEnterFeedbacks;

        protected MovementController playerController;

        protected void FixedUpdate()
        {
            MoveTarget();
        }

        protected void MoveTarget()
        {
            if (playerController == null)
                return;

            if (playerController.GetMoveState() == MovementController.MoveState.AutoMove)
                playerController.SetPosition(transform.position.x, transform.position.z);
            else
                playerController = null;
        }

        /// <summary>
        /// 設定顏色
        /// </summary>
        public override void SetColor(Color color)
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Initialization()
        {

        }

        protected override void CollisionEnterEvent(GameObject go)
        {
            collisionEnterFeedbacks?.PlayFeedbacks(this.transform.position);

            playerController = go.GetComponent<MovementController>();
            if (playerController == null)
                return;

            if (playerController.GetMoveState() == MovementController.MoveState.AutoMove)
                return;

            playerController.SetMoveState(MovementController.MoveState.AutoMove);

            Vector3 movePos = transform.position;
            movePos.y = playerController.transform.position.y;
            playerController.SetPosition(movePos);
        }

    }
}
