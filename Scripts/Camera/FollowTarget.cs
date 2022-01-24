using UnityEngine;

namespace HyperCasualFramework
{
    public class FollowTarget : MonoBehaviour
    {
        protected enum State { Idle, Following}
        [SerializeField]
        protected State state;
        [SerializeField]
        protected bool stateTest = false;

        [Header("Target")]
        public Transform target;

        [Header("Setting")]
        public Vector3 offset;
        [Range(0f, 1f)]
        public float smoothSpeed = 1f;

        [Header("Lock")]
        public bool lockAsixY = true;

        protected Vector3 currentPosition;

        /// <summary>
        /// 攝影機控制邏輯，要使用 LateUpdate
        /// </summary>
        protected virtual void LateUpdate()
        {
            AIBrain();
        }

        /// <summary>
        /// 狀態處理
        /// </summary>
        protected virtual void AIBrain()
        {
            switch (state)
            {
                case State.Idle:

                    if (HasTarget() && HasSeekTarget() && !stateTest)
                    {
                        state = State.Following;
                        break;
                    }

                    break;
                case State.Following:

                    if (!HasTarget() || !HasSeekTarget() && !stateTest)
                    {
                        state = State.Idle;
                        break;
                    }

                    UpdateFollowTarget();

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 更新跟隨目標
        /// </summary>
        protected virtual void UpdateFollowTarget()
        {
            if (lockAsixY)
            {
                Vector3 targetPosition = target.position;
                targetPosition.y = transform.position.y;
                currentPosition = targetPosition + offset;
            }
            else
            {
                currentPosition = target.position + offset;
            }
            
            transform.position = Vector3.Lerp(transform.position, currentPosition, smoothSpeed);
        }

        /// <summary>
        /// 是否有目標
        /// </summary>
        protected virtual bool HasTarget()
        {
            // 如果有跟隨目標就回傳true
            if (target != null)
                return true;

            // 檢查是否有關卡管理器
            if (LevelManager.Instance == null)
                return false;

            // 檢查是否有玩家
            if (LevelManager.Instance.playerCharacter == null)
            {
                target = null;
                return false;
            }

            // 設定跟隨目標
            target = LevelManager.Instance.playerCharacter.transform;
            return true;
        }

        protected virtual bool HasSeekTarget()
        {
            // 檢查是否有關卡管理器
            if (LevelManager.Instance == null)
                return false;

            // 檢查是否有玩家
            if (LevelManager.Instance.playerCharacter == null)
                return false;

            // 檢查玩家是否死亡，如果死亡或隱藏就停止跟隨
            return LevelManager.Instance.playerCharacter.characterState != Character.CharacterConditions.Dead &&
                LevelManager.Instance.playerCharacter.characterState != Character.CharacterConditions.Hide;

        }
    }
}
