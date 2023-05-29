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

        private bool isJump = false;

        [SerializeField]
        private float jumpDelay = 0.1f;
        private float jumpDelayTimer = 0f;

        private bool jumpDelayTimerStart = false;

        [SerializeField]
        private float jumpRayDistance = 0.5f;

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        void Start()
        {

        }

        void Update()
        {
            CheckJumpDelay();

            if (player.GetCurrentState() == PlayerState.Move || player.GetCurrentState() == PlayerState.Sprint || player.GetCurrentState() == PlayerState.Jump ||
                player.GetCurrentState() == PlayerState.MoveAttack || player.GetCurrentState() == PlayerState.SprintAttack || player.GetCurrentState() == PlayerState.JumpAttack)
            {
                CheckMove();
                CheckJump();

                Move();
                Jump();
            }
        }

        /// <summary>
        /// JumpDelay를 체크해줌
        /// </summary>
        private void CheckJumpDelay()
        {
            if(jumpDelayTimerStart && jumpDelayTimer > 0f)
            {
                jumpDelayTimer -= Time.deltaTime;

                if(jumpDelayTimer <= 0f)
                {
                    jumpDelayTimerStart = false;
                }
            }
        }

        /// <summary>
        /// 움직여야하는 방향을 설정해주는 함수
        /// </summary>
        private void CheckMove()
        {
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
        /// 점프를 체크하는 함수
        /// </summary>
        private void CheckJump()
        {
            if(isJump)
            {
                Ray ray = default(Ray);

                ray.origin = transform.position;
                ray.direction = Vector3.down;

                Debug.DrawRay(ray.origin, ray.direction * jumpRayDistance, Color.red, 10f);

                if(Physics.Raycast(ray.origin, ray.direction, jumpRayDistance, player.FloorLayerMask))
                {
                    isJump = false;

                    player.SetCurrentState(PlayerState.Idle);
                }
            }
        }

        /// <summary>
        /// 움직임을 담당하는 함수
        /// </summary>
        private void Move()
        {
            Vector3 currentPosition = transform.position;

            if (player.GetCurrentState() == PlayerState.Sprint || player.GetCurrentState() == PlayerState.SprintAttack)
            {
                currentPosition += currentMoveDirection * player.PlayerStats.sprintSpeed * Time.deltaTime;
            }
            else
            {
                currentPosition += currentMoveDirection * player.PlayerStats.speed * Time.deltaTime;
            }

            transform.position = currentPosition;
        }

        /// <summary>
        /// 점프를 담당하는 함수
        /// </summary>
        private void Jump()
        {
            if((player.GetCurrentState() == PlayerState.Jump || player.GetCurrentState() == PlayerState.JumpAttack) && !isJump && !jumpDelayTimerStart)
            {
                player.PlayerRigidbody.velocity = Vector3.zero;
                player.PlayerRigidbody.AddForce(Vector3.up * player.PlayerStats.jumpPower, ForceMode.Impulse);

                jumpDelayTimer = jumpDelay;

                jumpDelayTimerStart = true;
                isJump = true;
            }
        }
    }
}
