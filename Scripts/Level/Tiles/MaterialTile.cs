using UnityEngine;

namespace HyperCasualFramework
{
    /// <summary>
    /// 特殊地板，會影響球的重力與高度
    /// </summary>
    public abstract class MaterialTile : TileBase
    {
        [SerializeField, Header("地板影響"), Tooltip("上升重力")]
        protected float gravityEffect;

        [SerializeField, Tooltip("下墜重力")]
        protected float fallingGravityEffect;

        [SerializeField, Tooltip("跳躍高度")]
        protected float heightEffect;

        [SerializeField, Tooltip("前後左右移動速度")]
        protected float moveSpeedEffect = 1f;

        public float GetGravityEffect { get { return gravityEffect; } }

        public float GetFallingGravityEffect { get { return fallingGravityEffect; } }

        public float GetHeightEffect { get { return heightEffect; } }

        public float GetMoveSpeedEffect { get { return moveSpeedEffect; } }
    }
}
