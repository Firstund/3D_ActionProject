using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerMove : MonoBehaviour
    {
        private Player player = null;
        private PlayerInput playerInput => player.PlayerInput;

        private Vector3 currentMoveDirection = Vector3.zero;
        public Vector3 CurrentMoveDirection
        {
            get
            {
                return currentMoveDirection;
            }

            set
            {
                currentMoveDirection = value;
            }
        }

        private Vector3 moveValue = Vector3.zero;
        private Vector3 prevJumpVector = Vector3.zero;

        private float fallenValue = 0f;

        [SerializeField]
        private float gravityPower = 1f;

        [SerializeField]
        private float jumpDuration = 1f;    // 점프 시간
        private float jumpTimer = 0f;

        private bool jumpTimerStart = false;

        private bool isJumping = false;     // 점프 중인지 여부

        private bool canChangeDirection = true;
        public bool CanChangeDirection
        {
            get
            {
                return canChangeDirection;
            }

            set
            {
                canChangeDirection = value;
            }
        }

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        void Start()
        {

        }

        void Update()
        {
            CheckJumpTimer();

            moveValue = Vector3.zero;

            if (player.GetCurrentState() == PlayerState.Move || player.GetCurrentState() == PlayerState.Sprint || player.GetCurrentState() == PlayerState.Jump ||
                /*player.GetCurrentState() == PlayerState.MoveAttack ||*/ player.GetCurrentState() == PlayerState.SprintAttack || player.GetCurrentState() == PlayerState.JumpAttack)
            {

                CheckMove();
                CheckJump();

                Move();
                Jump();

            }

            Gravity();

            transform.position += moveValue;
        }

        private void Gravity()
        {
            if (player.CurrentState == PlayerState.InAirIdle)
            {
                float gravityVec = Mathf.Sqrt((GameManager.Instance.AccelerationOfGravity * fallenValue) + GameManager.Instance.AccelerationOfGravity) * Time.deltaTime;

                moveValue.y -= gravityVec;
                fallenValue += Mathf.Abs(gravityVec);
            }
            else
            {
                fallenValue = 0f;
            }
        }

        /// <summary>
        /// 움직여야하는 방향을 설정해주는 함수
        /// </summary>
        private void CheckMove()
        {
            if (!canChangeDirection)
            {
                return;
            }

            Vector3 moveDirection = Vector3.zero;

            if (playerInput.GetKeyDict(PlayerKey.MoveForward))
            {
                moveDirection += transform.forward;
            }
            if (playerInput.GetKeyDict(PlayerKey.MoveRight))
            {
                moveDirection += transform.right;
            }
            if (playerInput.GetKeyDict(PlayerKey.MoveLeft))
            {
                moveDirection -= transform.right;
            }
            if (playerInput.GetKeyDict(PlayerKey.MoveBack))
            {
                moveDirection -= transform.forward;
            }

            currentMoveDirection = moveDirection;
        }

        /// <summary>
        /// 움직임을 담당하는 함수
        /// </summary>
        private void Move()
        {
            // Vector3 moveVec = Vector3.zero;

            if (playerInput.GetKeyDict(PlayerKey.Sprint))
            {
                moveValue += currentMoveDirection * player.PlayerStats.sprintSpeed * Time.deltaTime;
            }
            else
            {
                moveValue += currentMoveDirection * player.PlayerStats.speed * Time.deltaTime;
            }
        }

        private void CheckJumpTimer()
        {
            if (jumpTimerStart && jumpTimer > 0f)
            {
                jumpTimer -= Time.deltaTime;

                if (jumpTimer <= 0f)
                {
                    isJumping = false;
                    jumpTimerStart = false;

                    player.SetCurrentState(PlayerState.Idle);
                }
            }
        }

        private void CheckJump()
        {
            if ((player.GetCurrentState() == PlayerState.Jump || player.GetCurrentState() == PlayerState.JumpAttack) && !isJumping)
            {
                isJumping = true;

                jumpTimerStart = true;
                jumpTimer = jumpDuration;
            }

        }

        /// <summary>
        /// 점프를 담당하는 함수
        /// </summary>
        private void Jump()
        {
            if (isJumping)
            {
                // MoveValue값 변경을 통한 구현
                float normalizedTime = jumpTimer / jumpDuration;
                float jumpProgress = JumpGraph(normalizedTime);

                // 2차함수를 이용한 점프 구현 해보기

                Vector3 jumpVector = Vector3.up * player.PlayerStats.jumpPower * jumpProgress;
                Vector3 jumpDelta = jumpVector - prevJumpVector;

                // Debug.Log(jumpVector);
                moveValue += jumpDelta;

                prevJumpVector = jumpVector;
            }
            else
            {
                prevJumpVector = Vector3.zero;
            }
        }

        private float JumpGraph(float x)
        {
            float y = 0f;

            y = (-4) * (Mathf.Pow(x, 2)) + 4 * x;

            return y;
        }
    }
}
