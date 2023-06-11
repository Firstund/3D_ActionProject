using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Player
{
    public class PlayerBody : MonoBehaviour
    {
        private Player currentPlayer => GameManager.Instance.CurrentPlayer; // 현재 플레이어
        private PlayerInput playerInput => currentPlayer.PlayerInput; // 현재 플레이어 Input

        [SerializeField]
        private Animator animator = null;
        public Animator Animator
        {
            get
            {
                return animator;
            }
        }

        private float targetQuat = 0f;
        private float curQuat = 0f;

        [SerializeField]
        private float quatSpeed = 2f;

        [SerializeField]
        private float jumpAddForceTime = 0.1f;
        private float jumpAddForceTimer = 0f;

        private bool jumpAddForceTimerStart = false;

        [SerializeField]
        private int maxAttackCombo = 4;

        [SerializeField]
        private float comboTime = 1f;
        private float comboCheckTimer = 0f;

        private bool comboCheckTimerStart = false; // 후에 콤보 체크할 때 사용

        private bool animPlayed = false;
        private bool attackAnimPlayed = false;

        private bool canRotate = true;
        public bool CanRotate
        {
            get
            {
                return canRotate;
            }

            set
            {
                canRotate = value;
            }
        }

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            CheckJumpAddForceTimer();
            CheckComboTimer();

            animPlayed = false;

            CheckMoveDirection();
            LerpQuat();

            CheckMoveAnime();
            CheckJumpAnime();

            CheckGoToWait();
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

            float currentQuat = transform.localRotation.y;

            if (!canRotate)
            {
                return;
            }

            #region CheckQaut
            if (animator.GetBool("Forward"))
            {
                if (animator.GetBool("Left"))
                {
                    targetQuat = Mathf.Abs(currentQuat - 315f) > Mathf.Abs(currentQuat + 45f) ? -45f : 315f;
                }
                else if (animator.GetBool("Right"))
                {
                    targetQuat = Mathf.Abs(currentQuat - 45f) > Mathf.Abs(currentQuat + 315f) ? -315f : 45f;
                }
                else
                {
                    targetQuat = Mathf.Abs(currentQuat) > Mathf.Abs(currentQuat + 360f) ? -360f : 0f;
                }
            }
            else if (animator.GetBool("Backward"))
            {
                if (animator.GetBool("Left"))
                {
                    targetQuat = Mathf.Abs(currentQuat - 225f) > Mathf.Abs(currentQuat + 135f) ? -135f : 225f;
                }
                else if (animator.GetBool("Right"))
                {
                    targetQuat = Mathf.Abs(currentQuat - 135f) > Mathf.Abs(currentQuat + 225f) ? -225f : 135f;
                }
                else
                {
                    targetQuat = Mathf.Abs(currentQuat - 180f) > Mathf.Abs(currentQuat + 180f) ? -180f : 180f;
                }
            }
            else if (animator.GetBool("Left"))
            {
                targetQuat = Mathf.Abs(currentQuat - 270f) > Mathf.Abs(currentQuat + 90f) ? -90f : 270f;
            }
            else if (animator.GetBool("Right"))
            {
                targetQuat = Mathf.Abs(currentQuat - 90f) > Mathf.Abs(currentQuat + 270f) ? -270f : 90f;
            }

            #endregion
        }
        private void LerpQuat()
        {
            curQuat = Mathf.Lerp(curQuat, targetQuat, Time.deltaTime * quatSpeed);

            transform.localRotation = Quaternion.Euler(0f, curQuat, 0f);
        }

        private void CheckGoToWait()
        {
            if (!animPlayed && !attackAnimPlayed)
            {
                animator.SetTrigger("GoToWait");
            }
        }

        /// <summary>
        ///  Move, Sprint를 체크한다음 애니메이션을 실행함
        /// </summary>
        private void CheckMoveAnime()
        {
            if (!(currentPlayer.CurrentState == PlayerState.Move || currentPlayer.CurrentState == PlayerState.Sprint))
            {
                ResetMoveParams();

                return;
            }

            animPlayed = true;
            animator.ResetTrigger("GoToWait");

            switch (currentPlayer.CurrentState)
            {
                case PlayerState.Move:
                    {
                        if (!animator.GetBool("Move"))
                        {
                            ResetMoveParams();
                            animator.Play("WALK00_F");
                            animator.SetBool("Move", true);
                        }
                    }
                    break;

                case PlayerState.Sprint:
                    {
                        if (!animator.GetBool("Sprint"))
                        {
                            ResetMoveParams();
                            animator.Play("RUN00_F");
                            animator.SetBool("Sprint", true);
                        }
                    }
                    break;
            }
        }

        private void CheckJumpAnime()
        {
            if (!(currentPlayer.CurrentState == PlayerState.Jump))
            {
                ResetJumpParams();

                return;
            }

            animPlayed = true;
            animator.ResetTrigger("GoToWait");

            switch (currentPlayer.CurrentState)
            {
                case PlayerState.Jump:
                    {
                        if (!animator.GetBool("Jump"))
                        {
                            ResetJumpParams();
                            animator.Play("JumpStart");

                            jumpAddForceTimer = jumpAddForceTime;
                            jumpAddForceTimerStart = true;

                            animator.SetBool("Jump", true);
                        }
                    }
                    break;
            }

            // animator.playbackTime;
        }

        private void JumpAddForce()
        {
            currentPlayer.PlayerRigidbody.velocity = Vector3.zero;
            currentPlayer.PlayerRigidbody.AddForce(Vector3.up * currentPlayer.PlayerStats.jumpPower, ForceMode.Impulse);
        }

        /// <summary>
        /// JumpAddForceTimer를 체크해줌
        /// </summary>
        private void CheckJumpAddForceTimer()
        {
            if (jumpAddForceTimerStart && jumpAddForceTimer > 0f)
            {
                jumpAddForceTimer -= Time.deltaTime;

                if (jumpAddForceTimer <= 0f)
                {
                    animator.SetTrigger("JumpInAir");
                    JumpAddForce();

                    jumpAddForceTimerStart = false;
                }
            }
        }

        /// <summary>
        /// ComboTimer를 체크해줌
        /// </summary>
        private void CheckComboTimer()
        {
            if (comboCheckTimerStart && comboCheckTimer > 0f)
            {
                comboCheckTimer -= Time.deltaTime;

                if (comboCheckTimer <= 0f)
                {
                    comboCheckTimerStart = false;

                    animator.SetInteger("AttackCount", 0);
                }
            }
        }

        /// <summary>
        /// SprintAttack 실행
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async void SprintAttackPlay(Action callback = null)
        {
            animator.Play("SLIDE00");

            canRotate = false;
            attackAnimPlayed = true;
            currentPlayer.PlayerMove.CanChangeDirection = false;

            animator.ResetTrigger("GoToWait");

            // Debug.Log(playTime);

            int spawnDelay = 1; // milliSeconds
            int spawnDelayTimer = 0;

            // 타이머 작동 방식에 대해 고민해볼 필요가 있음
            while (true)
            {
                // Debug.Log("loop");
                await Task.Delay(1);

                float animNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                if (spawnDelayTimer > 0)
                {
                    
                    spawnDelayTimer -= 1;
                }
                else
                {
                    GameObject playerBodyClone = PoolManager.Instance.GetObject<GameObject>("PlayerBodyClone");

                    if (playerBodyClone == null)
                    {
                        playerBodyClone = Instantiate(currentPlayer.PlayerBodyCloneObject);
                    }

                    playerBodyClone.SetActive(true);
                    playerBodyClone.transform.position = transform.position;
                    playerBodyClone.transform.rotation = transform.rotation;

                    PlayerBodyClone bodyScript = playerBodyClone.GetComponent<PlayerBodyClone>();
                    bodyScript.SetMotion("SLIDE00", 0, animNormalizedTime, 0);
                    bodyScript.OnSpawn(0.3f);

                    spawnDelayTimer = spawnDelay;
                }

                if (animNormalizedTime >= 1f)
                {
                    break;
                }
            }

            canRotate = true;
            attackAnimPlayed = false;
            currentPlayer.PlayerMove.CanChangeDirection = true;

            callback?.Invoke();
        }

        /// <summary>
        /// JumpAttack을 실행함
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async void JumpAttackPlay(Action callback = null)
        {
            int attackCount = animator.GetInteger("AttackCount");

            attackCount++;
            animator.SetInteger("AttackCount", attackCount);

            while (true)
            {
                await Task.Delay(1);

                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    break;
                }
            }

            callback?.Invoke();
        }

        /// <summary>
        /// Attack을 실행함
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async void AttackPlay(Action callback = null)
        {
            int attackCount = animator.GetInteger("AttackCount");

            animator.Play("Attack" + attackCount);

            attackCount++;
            animator.SetInteger("AttackCount", attackCount);

            canRotate = false;
            comboCheckTimerStart = false;
            attackAnimPlayed = true;

            animator.ResetTrigger("GoToWait");

            while (true)
            {
                await Task.Delay(1);

                Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    break;
                }
            }

            // Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

            callback?.Invoke();

            canRotate = true;
            attackAnimPlayed = false;

            if (attackCount >= maxAttackCombo)
            {
                animator.SetInteger("AttackCount", 0);
            }
            else
            {
                comboCheckTimer = comboTime;
                comboCheckTimerStart = true;
            }

        }

        private void ResetInAirParams()
        {
            animator.SetBool("InAir", false);
        }

        private void ResetJumpParams()
        {
            animator.SetBool("Jump", false);
        }

        private void ResetMoveParams()
        {
            animator.SetBool("Move", false);
            animator.SetBool("Sprint", false);
        }
    }
}
