using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera cinemachineVirtualCamera = null;
        private CinemachineTransposer cinemachineTransposer = null;

        private PlayerInput playerInput => GameManager.Instance.CurrentPlayer.PlayerInput;

        public float distance = 3;

        public Vector3 playerForwardVector // 카메라 회전에 따른 플레이어의 Forward벡터
        {
            get
            {
                return Vector3.Cross(cinemachineVirtualCamera.transform.right, Vector3.up);
            }
        }

        [SerializeField]
        private Vector3 cameraPosOffset = Vector3.zero;
        private Vector3 cameraPos = Vector3.zero;

        [SerializeField]
        private float cameraDistance = 10f;


        [SerializeField]
        private float minPosOffsetZ = 0f;
        [SerializeField]
        private float maxPosOffsetZ = 10f;

        [SerializeField]
        private float posOffsetZSpeed = 1f;

        void Awake()
        {
            cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        }

        void Update()
        {
            CheckCameraPos();
            CheckFollowPosOffsetZ();

            SetFollowPosOffset();

            // DrawLine(GameManager.Instance.CurrentPlayer.transform.position, GameManager.Instance.CurrentPlayer.transform.position + drawLineTest.normalized * cameraDistance);
        }

        /// <summary>
        /// 마우스 스크롤을 통해 카메라 줌 값을 체크해줌
        /// </summary>
        private void CheckFollowPosOffsetZ()
        {
            float offsetZ = cameraDistance;

            offsetZ -= Input.mouseScrollDelta.y * posOffsetZSpeed;

            if (-offsetZ < minPosOffsetZ)
            {
                offsetZ = -minPosOffsetZ;
            }

            if (-offsetZ > maxPosOffsetZ)
            {
                offsetZ = -maxPosOffsetZ;
            }

            cameraDistance = offsetZ;
        }

        private void CheckCameraPos()
        {
            transform.rotation = Quaternion.Euler(playerInput.YMove, playerInput.XMove, 0); // 카메라를 우선 xmove, ymove값에 따라 회전시킨다.
                                                                                            // 위, 아래로 회전하려면 회전값중 x가 변해야 하므로 yMove값은 x위치에,
                                                                                            // 좌, 우로 회전하려면 회전값중 y가 변해야하므로 xMove값은 y위치에

            Vector3 reverseDistance = new Vector3(0.0f, 0.0f, cameraDistance); // 카메라의 z 이동량을 Vector3로 바꾼다.
            cameraPos = transform.rotation * reverseDistance; // 카메라의 회전값에 z 이동량을 담은 Vector3를 곱하면
                                                                                                                            // 플레이어에 대한 카메라의 상대 좌표를 구하기 위해 플레이어의 Position에서 빼야하는 값이 나온다.
        }
        private void SetFollowPosOffset()
        {
            Vector3 targetPos = GameManager.Instance.CurrentPlayer.transform.position - cameraPos;
            transform.position = targetPos;
        }
    }
}
