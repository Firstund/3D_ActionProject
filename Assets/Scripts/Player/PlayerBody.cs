using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerBody : MonoBehaviour
    {
        private Player currentPlayer => GameManager.Instance.CurrentPlayer; // 현재 플레이어
        private PlayerInput playerInput => currentPlayer.PlayerInput; // 현재 플레이어 Input

        [SerializeField]
        private Animator animator = null;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            CheckMoveDirection();

            CheckMoveAnime();
        }

        private void CheckMoveDirection()
        {
            if(playerInput.GetKeyDict(PlayerKey.MoveForward))
            {
                animator.SetBool("Forward", true);
            }
            else
            {
                animator.SetBool("Forward", false);
            }

            if(playerInput.GetKeyDict(PlayerKey.MoveLeft)) 
            {
                animator.SetBool("Left", true);
            }
            else
            {
                animator.SetBool("Left", false);
            }

            if(playerInput.GetKeyDict(PlayerKey.MoveRight))
            {
                animator.SetBool("Right", true);
            }
            else
            {
                animator.SetBool("Right", false);
            }

            if(playerInput.GetKeyDict(PlayerKey.MoveBack))
            {
                animator.SetBool("Backward", true);
            }
            else
            {
               animator.SetBool("Backward", false); 
            }
        }
        private void CheckMoveAnime()
        {
            if(!(currentPlayer.CurrentState == PlayerState.Move || currentPlayer.CurrentState == PlayerState.Sprint))
            {
                ResetParams();

                animator.SetTrigger("GoToWait");

                return;
            }

            switch(currentPlayer.CurrentState)
            {
                case PlayerState.Move:
                {
                    animator.SetBool("Move", true);
                }
                break;

                case PlayerState.Sprint:
                {
                    animator.SetBool("Sprint", true);
                }
                break;
            }
        }

        private void ResetParams()
        {
            animator.SetBool("Move", false);
            animator.SetBool("Sprint", false);
        }
    }
}
