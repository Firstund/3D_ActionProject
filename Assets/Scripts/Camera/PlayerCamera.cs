using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera = null;
    private CinemachineTransposer cinemachineTransposer = null;

    [SerializeField]
    private Vector3 cameraPosOffset = Vector3.zero;

    [SerializeField]
    private float cameraDistance = 10f;

    void Awake()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    void Update()
    {
        cinemachineTransposer.m_FollowOffset = cameraPosOffset;
    }
}
