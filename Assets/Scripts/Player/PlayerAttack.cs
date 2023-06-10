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

        private bool isAttack = false;
        public bool IsAttack
        {
            get
            {
                return isAttack;
            }
        }
        
        [SerializeField]
        private float attackTime = 1f; // 테스트용 어택을 하는데 걸리는 시간
        private float attackTimer = 0f;

        private void Awake()
        {
            player = GetComponent<Player>();
        }
        void Start()
        {

        }

        void Update()
        {
            CheckTimer();

            CheckAttack();
        }

        private void CheckTimer()
        {
            if(isAttack && attackTimer > 0f)
            {
                attackTimer -= Time.deltaTime;

                if(attackTimer <= 0f)
                {
                    Debug.Log("Attack 타이머가 다 됐단다");

                    isAttack = false;
                    player.SetCurrentState(onEndState);
                }
            }
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
                // attackTimer = attackTime;

                onEndState = PlayerState.Idle;

                player.SetCurrentState(PlayerState.Attack);

                playerBody.AttackPlay(() => {
                    isAttack = false;

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
                isAttack = true;
                attackTimer = attackTime;

                onEndState = PlayerState.Jump;

                Debug.Log("JumpAttack이야 임마!");
            }
            if(player.GetCurrentState() == PlayerState.SprintAttack)
            {
                isAttack = true;
                // attackTimer = attackTime;

                onEndState = PlayerState.Sprint;

                player.SetCurrentState(PlayerState.SprintAttack);

                playerBody.SprintAttackPlay(() => {
                    isAttack = false;
                    
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
