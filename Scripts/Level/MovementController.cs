using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HyperCasualFramework
{
    public class MovementController : MonoBehaviour
    {
        public enum MoveType { Keyboard, Mouse, Both}

        public enum MoveState { Idle, Move, AutoMove}

        [SerializeField, Header("Layer")]
        protected LayerMask groundLayer;

        [SerializeField, Header("Control")]
        protected float moveDistance = 1f;

        [SerializeField, Min(0.01f)]
        protected float moveSpeed = 1f;

        [SerializeField]
        protected MoveType inputType = MoveType.Mouse;

        [SerializeField]
        protected MoveState moveState = MoveState.Idle;

        [SerializeField]
        protected float canMoveIntervalByMouse = 100f;

        protected Character _playerCharacter;
        protected Transform _transform;
        protected Rigidbody _rigidbody;
        protected Vector3 _mouseInputPosition;
        protected Vector3 _inputMovement;
        protected Vector3 _nextPosition;

        protected float currentMoveSpeed;

        /// <summary>
        /// 取得移動狀態
        /// </summary>
        public MoveState GetMoveState() => moveState;

        /// <summary>
        /// 設定移動狀態
        /// </summary>
        public void SetMoveState(MoveState state) => moveState = state;

        /// <summary>
        /// 設定球移動的速度
        /// </summary>
        public void SetMoveSpeed(float moveSpeed)
        {
            currentMoveSpeed = moveSpeed;
        }

        /// <summary>
        /// 進入場景立即觸發
        /// </summary>
        protected void Awake()
        {
            Initialization();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected void Initialization()
        {
            _playerCharacter = GetComponent<Character>();
            _rigidbody = GetComponent<Rigidbody>();
            _transform = transform;
            _nextPosition = _transform.position;

            currentMoveSpeed = moveSpeed;
        }

        protected void Update()
        {
            HandleInput();
        }

        protected void FixedUpdate()
        {
            Movement();
        }

        /// <summary>
        /// 處理輸入
        /// </summary>
        protected void HandleInput()
        {
            if (_playerCharacter.characterState == Character.CharacterConditions.Paused ||
                _playerCharacter.characterState == Character.CharacterConditions.Dead)
                return;

            _inputMovement = Vector3.zero;

            switch (inputType)
            {
                case MoveType.Keyboard:
                    KeyBoardInput();

                    break;
                case MoveType.Mouse:
                    MouseInput();

                    break;

                case MoveType.Both:
                    KeyBoardInput();
                    MouseInput();

                    break;
                default:
                    break;
            }

            if (_inputMovement.magnitude > 0f)
                _nextPosition = _nextPosition + (_inputMovement * moveDistance);
        }

        /// <summary>
        /// 鍵盤輸入
        /// </summary>
        protected void KeyBoardInput()
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                _inputMovement.z = 1f;
                moveState = MoveState.Move;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                _inputMovement.z = -1f;
                moveState = MoveState.Move;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _inputMovement.x = -1f;
                moveState = MoveState.Move;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                _inputMovement.x = 1f;
                moveState = MoveState.Move;
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) ||
                Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                moveState = MoveState.Idle;
            }
        }

        /// <summary>
        /// 滑鼠輸入
        /// </summary>
        protected void MouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseInputPosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                if ((Input.mousePosition.y - _mouseInputPosition.y) > canMoveIntervalByMouse)
                {
                    //Debug.Log("Up");
                    _inputMovement.z = -1f;
                    _mouseInputPosition = Input.mousePosition;
                    moveState = MoveState.Move;
                }
                else if ((_mouseInputPosition.y - Input.mousePosition.y) > canMoveIntervalByMouse)
                {
                    //Debug.Log("Down");
                    _inputMovement.z = 1f;
                    _mouseInputPosition = Input.mousePosition;
                    moveState = MoveState.Move;
                }
                else if ((_mouseInputPosition.x - Input.mousePosition.x) > canMoveIntervalByMouse)
                {
                    //Debug.Log("Left");
                    _inputMovement.x = 1f;
                    _mouseInputPosition = Input.mousePosition;
                    moveState = MoveState.Move;
                }
                else if ((Input.mousePosition.x - _mouseInputPosition.x) > canMoveIntervalByMouse)
                {
                    //Debug.Log("Right");
                    _inputMovement.x = -1f;
                    _mouseInputPosition = Input.mousePosition;
                    moveState = MoveState.Move;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                moveState = MoveState.Idle;
            }
        }

        /// <summary>
        /// 移動
        /// </summary>
        protected void Movement()
        {
            if (moveState != MoveState.AutoMove)
            {
                _nextPosition.x = Mathf.Round(_nextPosition.x);
                _nextPosition.z = Mathf.Round(_nextPosition.z);
            }

            _nextPosition.y = _rigidbody.position.y;
            _transform.position = Vector3.MoveTowards(_transform.position, _nextPosition, currentMoveSpeed);
        }

        /// <summary>
        /// 設定位置
        /// </summary>
        public void SetPosition(Vector3 position)
        {
            this._nextPosition = position;
            _transform.position = _nextPosition;
        }

        /// <summary>
        /// 設定位置，但不引響 Y 軸
        /// </summary>
        public void SetPosition(float x, float z)
        {
            this._nextPosition.x = x;
            this._nextPosition.z = z;
            _transform.position = _nextPosition;
        }

        /// <summary>
        /// 增加移動量
        /// </summary>
        public void AddMovement(Vector3 movement)
        {
            this._nextPosition += movement;
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
                //Debug.Log("Setup move speed effect!! " + collision.gameObject.name);
                SetMoveSpeed(tile.GetMoveSpeedEffect);
            }
        }
    }
}
