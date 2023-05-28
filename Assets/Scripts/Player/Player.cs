using UnityEngine;

namespace Player
{
    public enum PlayerState
    {
        Idle,
        Walk,
        Run,
        Attack,
        Jump,

    }
    public class Player : MonoBehaviour
    {
        private PlayerState currentState = default(PlayerState); // 플레이어의 현재 상태

        void Start()
        {
            
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
