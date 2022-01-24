using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasualFramework
{
    /// <summary>
    /// 滑溜地板
    /// </summary>
    public class SlipperyTile : MaterialTile
    {
        [SerializeField, Header("滑溜位移")]
        protected Vector3 slipperyPosition;

        [SerializeField, Header("快速設定")]
        protected float tileRotation = 45f;
        [SerializeField]
        protected Transform tileModel;
        [SerializeField]
        protected Text slipperyText;

        [SerializeField, Header("Feedbacks")]
        protected MMFeedbacks collisionEnterFeedbacks;

        protected MovementController movementController;

        public override void SetColor(Color color)
        {

        }

        protected override void Initialization()
        {

        }

        protected override void CollisionEnterEvent(GameObject go)
        {
            collisionEnterFeedbacks?.PlayFeedbacks(this.transform.position);
            Slippery(go);
        }

        /// <summary>
        /// 滑溜
        /// </summary>
        protected void Slippery(GameObject go)
        {
            movementController = go.GetComponent<MovementController>();
            if (movementController != null)
                movementController.AddMovement(slipperyPosition);

        }

#if UNITY_EDITOR

        /// <summary>
        /// 快速設定地板
        /// </summary>
        protected void OnValidate()
        {
            if(slipperyPosition.x > 0)
            {
                // Right
                if (tileModel)
                    tileModel.localRotation = Quaternion.Euler(0, 0, -tileRotation);

                if (slipperyText)
                    slipperyText.text = slipperyPosition.x.ToString();
            }
            else if(slipperyPosition.x < 0)
            {
                // Left
                if (tileModel)
                    tileModel.localRotation = Quaternion.Euler(0, 0, tileRotation);

                if (slipperyText)
                    slipperyText.text = (-slipperyPosition.x).ToString();
            }
            else if (slipperyPosition.z > 0)
            {
                // Forward
                if (tileModel)
                    tileModel.localRotation = Quaternion.Euler(tileRotation, 0, 0);

                if (slipperyText)
                    slipperyText.text = slipperyPosition.z.ToString();
            }
            else if (slipperyPosition.z < 0)
            {
                // Back
                if (tileModel)
                    tileModel.localRotation = Quaternion.Euler(-tileRotation, 0, 0);

                if (slipperyText)
                    slipperyText.text = (-slipperyPosition.z).ToString();
            }
            else
            {
                if (tileModel)
                    tileModel.localRotation = Quaternion.Euler(0, 0, 0);

                if (slipperyText)
                    slipperyText.text = "0";
            }
        }

#endif

    }
}