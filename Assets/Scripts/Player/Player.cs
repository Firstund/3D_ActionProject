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
        InAirIdle,

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

        public float speed = 1;
        public float sprintSpeed = 2;
        public float jumpPower = 1;
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
        /// 이 Player오브젝트의 PlayerCamera스크립트
        /// </summary>
        [SerializeField]
        private PlayerCamera playerCamera = null;
        public PlayerCamera PlayerCamera
        {
            get
            {
                return playerCamera;
            }
        }

        /// <summary>
        /// 플레이어의 몸체를 담당하는 오브젝트가 가지고있는 스크립트
        /// </summary>
        [SerializeField]
        private PlayerBody playerBody = null;
        public PlayerBody PlayerBody
        {
            get
            {
                return playerBody;
            }
        }
        
        /// <summary>
        /// 플레이어 클론 몸체의 프리팹을 저장
        /// </summary>
        [SerializeField]
        private GameObject playerBodyCloneObject = null;
        public GameObject PlayerBodyCloneObject
        {
            get
            {
                return playerBodyCloneObject;
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
                if (playerMove == null)
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
                if (playerAttack == null)
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
                if (playerInput == null)
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


        [SerializeField]
        private float inAirRayDistance = 0.5f;

        private void Awake()
        {
            GameManager.Instance.CurrentPlayer = this;

            playerRigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            SetPlayerRot();
            CheckInAir();
        }

        private void SetPlayerRot()
        {
            transform.LookAt(playerCamera.playerForwardVector + transform.position);
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

        /// <summary>
        /// 공중에 떠있는지를 체크하는 함수
        /// </summary>
        private void CheckInAir()
        {
            if(PlayerAttack.IsAttack)
            {
                return;
            }

            Ray ray = default(Ray);

            ray.origin = transform.position;
            ray.direction = Vector3.down;

            Debug.DrawRay(ray.origin, ray.direction * inAirRayDistance, Color.red, Time.deltaTime);

            if (!Physics.Raycast(ray.origin, ray.direction, inAirRayDistance, floorLayerMask))
            {
                if(!(CurrentState == PlayerState.Jump || CurrentState == PlayerState.JumpAttack))
                {
                    SetCurrentState(PlayerState.InAirIdle);
                    playerBody.Animator.SetBool("InAir", true);
                }
            }
            else
            {
                playerBody.Animator.SetBool("InAir", false);
            }
        }
    }
}
