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

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        void Start()
        {

        }

        void Update()
        {
            if (player.GetCurrentState() == PlayerState.Move || player.GetCurrentState() == PlayerState.Sprint || player.GetCurrentState() == PlayerState.Jump ||
                player.GetCurrentState() == PlayerState.MoveAttack || player.GetCurrentState() == PlayerState.SprintAttack || player.GetCurrentState() == PlayerState.JumpAttack)
            {
                CheckMove();

                Move();
                Jump();
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
        /// 움직임을 담당하는 함수
        /// </summary>
        private void Move()
        {
            Vector3 currentPosition = transform.position;

            if (playerInput.GetKeyDict(PlayerKey.Sprint))
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

        }
    }
}
