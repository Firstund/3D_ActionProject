using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 플레이어의 상태 정의
    /// </summary>
    public enum PlayerState
    {
        Idle,

        Move,
        Jump,
        Sprint,

        Attack,
        MoveAttack,
        JumpAttack,
        SprintAttack,
    }

    /// <summary>
    /// 플레이어 스탯 정의
    /// </summary>
    [Serializable]
    public class PlayerStats
    {
        public int hp = 10;

        public int ap = 2;
        public int dp = 2;

        public int speed = 1;
        public int sprintSpeed = 2;
        public int jumpPower = 1;
    }

    public class Player : MonoBehaviour
    {
        [SerializeField]
        private LayerMask floorLayerMask = default(LayerMask);
        public LayerMask FloorLayerMask
        {
            get
            {
                return floorLayerMask;
            }
        }

        [SerializeField]
        private PlayerStats playerStats = new();
        public PlayerStats PlayerStats
        {
            get
            {
                return playerStats;
            }
        }

        private PlayerState currentState = default(PlayerState); // 플레이어의 현재 상태
        public PlayerState CurrentState
        {
            get
            {
                return currentState;
            }

            set
            {
                currentState = value;
            }
        }

        /// <summary>
        /// PlayerMove 스크립트
        /// </summary>
        private PlayerMove playerMove = null;
        public PlayerMove PlayerMove
        {
            get
            {
                if(playerMove == null)
                {
                    playerMove = GetComponent<PlayerMove>();
                }

                return playerMove;
            }
        }

        /// <summary>
        /// PlayerAttack 스크립트
        /// </summary>
        private PlayerAttack playerAttack = null;
        public PlayerAttack PlayerAttack
        {
            get
            {
                if(playerAttack == null)
                {
                    playerAttack = GetComponent<PlayerAttack>();
                }

                return playerAttack;
            }
        }

        /// <summary>
        /// PlayerInput 스크립트
        /// </summary>
        private PlayerInput playerInput = null;
        public PlayerInput PlayerInput
        {
            get
            {
                if(playerInput == null)
                {
                    playerInput = GetComponent<PlayerInput>();
                }

                return playerInput;
            }
        }

        private Rigidbody playerRigidbody = null;
        public Rigidbody PlayerRigidbody
        {
            get
            {
                return playerRigidbody;
            }
        }

        private void Awake()
        {
            GameManager.Instance.CurrentPlayer = this;

            playerRigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {

        }

        /// <summary>
        /// 플레이어의 현재 상태를 가져옴
        /// </summary>
        /// <returns></returns>
        public PlayerState GetCurrentState()
        {
            return currentState;
        }

        /// <summary>
        /// 플레이어의 현재 상태를 바꿔줌
        /// </summary>
        /// <param name="nState"></param>
        public void SetCurrentState(PlayerState nState)
        {
            // 새로운 State를 설정했을시 처리해줘야 하는 작업들 코드로 작성하기

            currentState = nState;
        }
    }
}
