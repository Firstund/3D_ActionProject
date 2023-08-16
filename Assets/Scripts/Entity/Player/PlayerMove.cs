using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerMove : MonoBehaviour, IGravityTargetEntity
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

        private Vector3 posMoveValue = Vector3.zero; // 점프, 이동 등 여러 포지션 이동 요소들의 계산에 영향을 받는 포지션 이동 값
        private Vector3 prevPosMoveValue = Vector3.zero;
        private Vector3 prevMoveValue = Vector3.zero; // Sprint, Move등의 단순한 이동에만 관련된 moveValue값의 이전 값을 담아줌

        private Vector3 prevJumpVector = Vector3.zero;

        private float fallenValue = 0f;

        [SerializeField]
        private float gravityPower = 1f;

        [SerializeField]
        private float jumpDuration = 1f;    // 점프 시간
        private float jumpTimer = 0f;

        private bool jumpTimerStart = false;

        private bool isJumping = false;     // 점프 중인지 여부
        public bool IsJumping
        {
            get
            {
                return isJumping;
            }
        }

        private bool isSprintLock = false; // Sprint Move 상태가 Lock되어야 하는 상태
        public bool IsSprintLock
        {
            get
            {
                return isSprintLock;
            }

            set
            {
                isSprintLock = value;
            }
        }

        private bool isMoveLock = false; // Move 상태가 Lock되어야 하는 상태
        public bool IsMoveLock
        {
            get
            {
                return isMoveLock;
            }

            set
            {
                isMoveLock = value;
            }
        }

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

            player.InAirCheckTrigger.OnHitFloorEnter += (obj) =>
            {
                OnJumpEnd();
            };
        }

        void Update()
        {
            CheckJumpTimer();

            posMoveValue = Vector3.zero;

            if (player.GetCurrentState() == PlayerState.Move || player.GetCurrentState() == PlayerState.Sprint || player.GetCurrentState() == PlayerState.Jump ||
                /*player.GetCurrentState() == PlayerState.MoveAttack ||*/ player.GetCurrentState() == PlayerState.SprintAttack || player.GetCurrentState() == PlayerState.JumpAttack)
            {

                CheckMove();
                CheckJump();

                Move();
                Jump();

            }
            else if (isMoveLock || isSprintLock)
            {
                Move();
            }

            Gravity();

            transform.position += posMoveValue;

            if (posMoveValue == Vector3.zero)
            {
                Inertia();
            }
            else
            {
                prevPosMoveValue = posMoveValue;
            }
        }

        public void Gravity()
        {
            if (player.CurrentState == PlayerState.InAirIdle)
            {
                float gravityVec = Mathf.Sqrt((GameManager.Instance.AccelerationOfGravity * fallenValue) + GameManager.Instance.AccelerationOfGravity) * Time.deltaTime;

                posMoveValue.y -= gravityVec;
                fallenValue += Mathf.Abs(gravityVec);
            }
            else
            {
                fallenValue = 0f;
            }
        }

        private void Inertia()
        {
            // 관성을 구현하는 코드, 버그때문에 비활성화
            // transform.position += prevPosMoveValue;

            // prevPosMoveValue -= prevPosMoveValue.normalized;
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

            Vector3 moveValue = Vector3.zero;

            if (isSprintLock)
            {
                moveValue = currentMoveDirection * player.PlayerStats.sprintSpeed * Time.deltaTime;
            }
            else if (isMoveLock)
            {
                moveValue = currentMoveDirection * player.PlayerStats.speed * Time.deltaTime;
            }
            else if (playerInput.GetKeyDict(PlayerKey.Sprint))
            {
                moveValue = currentMoveDirection * player.PlayerStats.sprintSpeed * Time.deltaTime;
            }
            else
            {
                moveValue = currentMoveDirection * player.PlayerStats.speed * Time.deltaTime;
            }

            prevMoveValue = moveValue;
            posMoveValue += moveValue;
        }

        private void CheckJumpTimer()
        {
            if (jumpTimerStart && jumpTimer > 0f)
            {
                jumpTimer -= Time.deltaTime;

                if (jumpTimer <= 0f)
                {
                    OnJumpEnd();
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

        private void OnJumpEnd()
        {
            isJumping = false;
            jumpTimerStart = false;

            player.SetCurrentState(PlayerState.Idle);
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
                posMoveValue += jumpDelta;

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
