using MoreMountains.Feedbacks;
using UnityEngine;

namespace HyperCasualFramework
{
    public class Bouncy : MonoBehaviour
    {
        [SerializeField]
        protected LayerMask groundLayer;
        [SerializeField]
        protected float gravityScale = -10f;
        [SerializeField]
        protected float fallingGravityScale = -40f;
        [SerializeField]
        protected float jumpHeight = 7f;

        [SerializeField]
        protected MMFeedbacks jumpFeedbacks;

        protected Rigidbody _rigidbody;
        protected float _currentGravity;
        protected float _currentFallingGravity;
        protected float _currentJumpHeight;

        protected void Awake()
        {
            Initialization();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected void Initialization()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _currentGravity = gravityScale;
            _currentFallingGravity = fallingGravityScale;
            _currentJumpHeight = jumpHeight;
        }

        protected void FixedUpdate()
        {
            Gravity();
        }

        /// <summary>
        /// 重新設定
        /// </summary>
        public void ResetGravity()
        {
            _rigidbody.velocity = Vector3.zero;
            _currentGravity = gravityScale;
            _currentFallingGravity = fallingGravityScale;
            _currentJumpHeight = jumpHeight;
        }

        /// <summary>
        /// 重力
        /// </summary>
        protected void Gravity()
        {
            Vector3 velocity = _rigidbody.velocity;

            if (velocity.y >= 0f)
            {
                velocity.y += _currentGravity * Time.deltaTime;
            }
            else if (velocity.y < 0f)
            {
                velocity.y += _currentFallingGravity * Time.deltaTime;
            }

            _rigidbody.velocity = velocity;
        }

        /// <summary>
        /// 彈跳
        /// </summary>
        public void Jump()
        {
            _rigidbody.velocity = Vector3.up * Mathf.Sqrt(-2.0f * _currentGravity * _currentJumpHeight);
            jumpFeedbacks?.PlayFeedbacks(this.transform.position);
        }

        /// <summary>
        /// 增加上升重力
        /// </summary>
        protected void SetGravity(float value)
        {
            if (value >= 0f)
                return;

            _currentGravity = value;
        }

        /// <summary>
        /// 增加下降重力
        /// </summary>
        protected void SetFallingGravity(float value)
        {
            if (value >= 0f)
                return;

            _currentFallingGravity = value;
        }

        /// <summary>
        /// 增加跳躍高度
        /// </summary>
        protected void SetJumpHeight(float value)
        {
            if (value <= 0f)
                return;

            _currentJumpHeight = value;
        }

        /// <summary>
        /// 觸碰到地板
        /// </summary>
        protected void OnCollisionEnter(Collision collision)
        {
            if (((1 << collision.gameObject.layer) & groundLayer) != 0)
            {
                MaterialTile tile = collision.gameObject.GetComponent<MaterialTile>();
                if (tile == false)
                    return;

                SetGravity(tile.GetGravityEffect);
                SetFallingGravity(tile.GetFallingGravityEffect);
                SetJumpHeight(tile.GetHeightEffect);
                Jump();
            }
        }
    }
}
