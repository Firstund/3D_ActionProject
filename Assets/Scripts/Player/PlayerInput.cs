using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 플레이어 Key 종류 모음
    /// </summary>
    public enum PlayerKey
    {
        MoveForward,    // 앞 이동
        MoveLeft,       // 왼쪽 이동    
        MoveRight,      // 오른쪽 이동
        MoveBack,       // 뒤 이동

        Sprint,         // 전력질주로 이동
        Jump,           // 점프

        Attack,         // 기본공격
    }

    [RequireComponent(typeof(Player))]
    public class PlayerInput : MonoBehaviour
    {
        private Player player = null;
        private PlayerState currentPlayerState => player.CurrentState;

        #region 움직임 키를 담는 변수
        [SerializeField]
        private KeyCode forwardKey = default(KeyCode);
        [SerializeField]
        private KeyCode leftKey = default(KeyCode);
        [SerializeField]
        private KeyCode rightKey = default(KeyCode);
        [SerializeField]
        private KeyCode backKey = default(KeyCode);

        [SerializeField]
        private KeyCode sprintKey = default(KeyCode);
        [SerializeField]
        private KeyCode jumpKey = default(KeyCode);
        #endregion

        #region 공격 키를 담는 변수
        [SerializeField]
        private KeyCode attackKey = default(KeyCode);
        #endregion

        private Dictionary<PlayerKey, bool> keyDict = new(); // 현재 키의 상태를 담는 Dictionary

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        void Update()
        {
            CheckMoveInput();
            CheckAttackInput();

            CheckState();
        }

        /// <summary>
        /// 움직임 Input을 체크함
        /// </summary>
        private void CheckMoveInput()
        {
            #region GetKeyDown
            if (Input.GetKeyDown(forwardKey))
            {
                SetKeyDict(PlayerKey.MoveForward, true);
            }
            if (Input.GetKeyDown(leftKey))
            {
                SetKeyDict(PlayerKey.MoveLeft, true);
            }
            if (Input.GetKeyDown(rightKey))
            {
                SetKeyDict(PlayerKey.MoveRight, true);
            }
            if (Input.GetKeyDown(backKey))
            {
                SetKeyDict(PlayerKey.MoveBack, true);
            }

            if (Input.GetKeyDown(sprintKey))
            {
                SetKeyDict(PlayerKey.Sprint, true);
            }
            if (Input.GetKeyDown(jumpKey))
            {
                SetKeyDict(PlayerKey.Jump, true);
            }
            #endregion

            #region GetKeyUp
            if (Input.GetKeyUp(forwardKey))
            {
                SetKeyDict(PlayerKey.MoveForward, false);
            }
            if (Input.GetKeyUp(leftKey))
            {
                SetKeyDict(PlayerKey.MoveLeft, false);
            }
            if (Input.GetKeyUp(rightKey))
            {
                SetKeyDict(PlayerKey.MoveRight, false);
            }
            if (Input.GetKeyUp(backKey))
            {
                SetKeyDict(PlayerKey.MoveBack, false);
            }

            if (Input.GetKeyUp(sprintKey))
            {
                SetKeyDict(PlayerKey.Sprint, false);
            }
            if (Input.GetKeyUp(jumpKey))
            {
                SetKeyDict(PlayerKey.Jump, false);
            }
            #endregion
        }

        /// <summary>
        /// 공격 Input을 체크함
        /// </summary>
        private void CheckAttackInput()
        {
            #region GetKeyDown
            if (Input.GetKeyDown(attackKey))
            {
                SetKeyDict(PlayerKey.Attack, true);
            }
            #endregion

            #region GetKeyUp
            if (Input.GetKeyUp(attackKey))
            {
                SetKeyDict(PlayerKey.Attack, false);
            }
            #endregion
        }

        /// <summary>
        /// KeyInput에 따른 CurrentState를 변경해주는 함수
        /// </summary>
        private void CheckState()
        {
            if(player.GetCurrentState() == PlayerState.Attack || player.GetCurrentState() == PlayerState.MoveAttack ||
             player.GetCurrentState() == PlayerState.JumpAttack || player.GetCurrentState() == PlayerState.SprintAttack)
            {
                return;
            }

            if(player.GetCurrentState() == PlayerState.Jump)
            {
                if(GetKeyDict(PlayerKey.Attack))
                {
                    player.SetCurrentState(PlayerState.JumpAttack);
                }

                return;
            }

/////////////////////////////////////////////////////////////////////////////////////

            player.SetCurrentState(PlayerState.Idle);

            if(GetKeyDict(PlayerKey.MoveForward) || GetKeyDict(PlayerKey.MoveLeft) || GetKeyDict(PlayerKey.MoveRight) || GetKeyDict(PlayerKey.MoveBack))
            {
                player.SetCurrentState(PlayerState.Move);
            }

            if(GetKeyDict(PlayerKey.Sprint))
            {
                if(player.GetCurrentState() == PlayerState.Move)
                {
                    player.SetCurrentState(PlayerState.Sprint);
                }
            }

            if(GetKeyDict(PlayerKey.Jump))
            {
                player.SetCurrentState(PlayerState.Jump);
            }

            if(GetKeyDict(PlayerKey.Attack))
            {
                if(player.GetCurrentState() == PlayerState.Idle)
                {
                    player.SetCurrentState(PlayerState.Attack);
                }
                else if(player.GetCurrentState() == PlayerState.Move)
                {
                    player.SetCurrentState(PlayerState.MoveAttack);
                } 
                else if(player.GetCurrentState() == PlayerState.Sprint)
                {
                    player.SetCurrentState(PlayerState.SprintAttack);
                }
            }
        }

        /// <summary>
        /// KeyDict를 Set해줌
        /// value값은 GetKeyDown일땐 true, GetKeyUp일땐 false
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">GetKeyDown일땐 true, GetKeyUp일땐 false</param>
        private void SetKeyDict(PlayerKey key, bool value)
        {
            if (keyDict.ContainsKey(key))
            {
                keyDict[key] = value;
            }
            else
            {
                keyDict.Add(key, value);
            }
        }
        public bool GetKeyDict(PlayerKey key)
        {
            if (keyDict.ContainsKey(key))
            {
                return keyDict[key];
            }
            else
            {
                return false;
            }
        }
    }
}
