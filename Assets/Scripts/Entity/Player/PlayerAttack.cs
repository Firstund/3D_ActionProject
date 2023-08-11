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
