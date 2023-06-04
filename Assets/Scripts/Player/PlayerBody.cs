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

        private float targetQuat = 0f;
        private float curQuat = 0f;

        [SerializeField]
        private float quatSpeed = 2f;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            CheckMoveDirection();
            LerpQuat();

            CheckMoveAnime();
        }

        private void CheckMoveDirection()
        {
            #region SetBool
            if (playerInput.GetKeyDict(PlayerKey.MoveForward))
            {
                animator.SetBool("Forward", true);
            }
            else
            {
                animator.SetBool("Forward", false);
            }

            if (playerInput.GetKeyDict(PlayerKey.MoveLeft))
            {
                animator.SetBool("Left", true);
            }
            else
            {
                animator.SetBool("Left", false);
            }

            if (playerInput.GetKeyDict(PlayerKey.MoveRight))
            {
                animator.SetBool("Right", true);
            }
            else
            {
                animator.SetBool("Right", false);
            }

            if (playerInput.GetKeyDict(PlayerKey.MoveBack))
            {
                animator.SetBool("Backward", true);
            }
            else
            {
                animator.SetBool("Backward", false);
            }
            #endregion

            bool quatChanged = false;
            if (!animator.GetBool("Forward") && animator.GetBool("Left"))
            {
                // transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
                if (targetQuat < 0f)
                {
                    targetQuat = -90f;
                }
                else
                {
                    targetQuat = 270f;
                }

                quatChanged = true;
            }
            else if (!quatChanged)
            {
                targetQuat = 0f;
                // transform.localRotation = Quaternion.identity;
            }

            if (!animator.GetBool("Forward") && animator.GetBool("Right"))
            {
                // transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
                if (targetQuat < 0f)
                {
                    targetQuat = -270f;
                }
                else
                {
                    targetQuat = 90f;
                }

                quatChanged = true;
            }
            else if (!quatChanged)
            {
                targetQuat = 0f;
                // transform.localRotation = Quaternion.identity;
            }

            if (animator.GetBool("Backward") && !(animator.GetBool("Left") || animator.GetBool("Right")))
            {
                // transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                if (targetQuat < 0)
                {
                    targetQuat = -180f;
                }
                else
                {
                    targetQuat = 180f;
                }

                quatChanged = true;
            }
            else if (!quatChanged)
            {
                targetQuat = 0f;
                // transform.localRotation = Quaternion.identity;
            }
        }
        private void LerpQuat()
        {
            curQuat = Mathf.Lerp(curQuat, targetQuat, Time.deltaTime * quatSpeed);

            transform.localRotation = Quaternion.Euler(0f, curQuat, 0f);
        }

        private void CheckMoveAnime()
        {
            if (!(currentPlayer.CurrentState == PlayerState.Move || currentPlayer.CurrentState == PlayerState.Sprint))
            {
                ResetParams();

                animator.SetTrigger("GoToWait");

                return;
            }

            animator.ResetTrigger("GoToWait");

            switch (currentPlayer.CurrentState)
            {
                case PlayerState.Move:
                    {
                        if (!animator.GetBool("Move"))
                        {
                            ResetParams();
                            animator.Play("WALK00_F");
                            animator.SetBool("Move", true);
                        }
                    }
                    break;

                case PlayerState.Sprint:
                    {
                        if (!animator.GetBool("Sprint"))
                        {
                            ResetParams();
                            animator.Play("RUN00_F");
                            animator.SetBool("Sprint", true);
                        }
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
