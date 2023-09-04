using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerAttack : MonoBehaviour
    {
        private Player player = null;
        private PlayerInput playerInput => player.PlayerInput;
        private PlayerBody playerBody => player.PlayerBody;

        private PlayerState onEndState = default(PlayerState); // 공격이 끝났을 떄 어떤 State로 바뀌어야하는가

        [SerializeField]
        private HitTrigger playerHitTrigger = null; // Player가 사용하는 hitTrigger 스크립트를 담는 변수

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        public class ColliderStateValue{
            public string key;
            public int colliderIdx;
            public bool enableValue;
        }

        [SerializeField]
        private List<ColliderStateValue> colliderStateList = new();
        [SerializeField]
        private Dictionary<string, (int, bool)> colliderStateDict = new(); // string key값으로 특정 attackState의 이름이 들어간다. 
                                                                           // state가 바뀌었을 때, 해당 attackState의 이름을 key값으로 가지는 (int, bool)값을 가져온다. 해당 값은 특정 collider의 enable 여부를 정해준다.

        private bool isAttack = false;
        public bool IsAttack
        {
            get
            {
                return isAttack;
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
            CheckAttack();
        }

        private void CheckAttack()
        {
            if(isAttack)
            {
                return;
            }

            if(player.GetCurrentState() == PlayerState.Attack)
            {
                isAttack = true;
                player.PlayerMove.IsMoveLock = true;
                // attackTimer = attackTime;

                onEndState = PlayerState.Idle;

                player.SetCurrentState(PlayerState.Attack);

                playerBody.AttackPlay(() => {
                    isAttack = false;
                    player.PlayerMove.IsMoveLock = false;

                    player.SetCurrentState(onEndState);
                });

                Debug.Log("그냥 Attack이야 임마!");
            }
            if(player.GetCurrentState() == PlayerState.MoveAttack)
            {
                isAttack = true;
                // attackTimer = attackTime;

                onEndState = PlayerState.Move;

                player.SetCurrentState(PlayerState.MoveAttack);

                playerBody.AttackPlay(() => {
                    isAttack = false;

                    player.SetCurrentState(onEndState);
                });

                Debug.Log("MoveAttack이야 임마!");
            }
            if(player.GetCurrentState() == PlayerState.JumpAttack)
            {
                // jumpAttack 모션이 없어서 그런지 고장남, 임시 비활성화
                // isAttack = true;

                // onEndState = PlayerState.Jump;

                // playerBody.JumpAttackPlay(() =>{
                //     isAttack = false;

                //     player.SetCurrentState(onEndState);
                // });

                Debug.Log("JumpAttack이야 임마!");
            }
            if(player.GetCurrentState() == PlayerState.SprintAttack)
            {
                isAttack = true;
                player.PlayerMove.IsSprintLock = true;
                // attackTimer = attackTime;

                onEndState = PlayerState.Sprint;

                player.SetCurrentState(PlayerState.SprintAttack);

                playerBody.SprintAttackPlay(() => {
                    isAttack = false;
                    player.PlayerMove.IsSprintLock = false;
                    
                    player.SetCurrentState(onEndState);
                });

                // Debug.Log("SprintAttack이야 임마!");
            }
        }

        public void ResetIsAttack()
        {
            isAttack = false;
        }
    }
}
