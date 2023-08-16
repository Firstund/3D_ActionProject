using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFloorCheck : MonoBehaviour
{
    // 해당 Entity가 바닥 오브젝트 밑에 빠졌을 때 바닥 위로 Entity의 위치를 옮겨줌

    /// <summary>
    /// 내 Entity의 LayerMask
    /// </summary>
    [SerializeField]
    private LayerMask myEntityLayer = default;
    /// <summary>
    /// 바닥 Entity의 LayerMask
    /// </summary>
    [SerializeField]
    private LayerMask floorLayer = default;

    /// <summary>
    /// 바닥 오브젝트 밑에 빠졌는지 Check해야 하는 상태면 true
    /// </summary>
    private bool checkFloor = false;
    public bool CheckFloor
    {
        get
        {
            return checkFloor;
        }

        set
        {
            checkFloor = value;
        }
    }

    void Update()
    {
        if (checkFloor)
        {
            FloorCheck();
        }
    }

    /// <summary>
    /// Entity가 바닥 밑에 빠졌는지 체크 후 빠졌다면 Entity를 꺼내줌
    /// </summary>
    private void FloorCheck()
    {
        Ray myEntityCheckRay = new Ray();
        RaycastHit myEntityRayHit = default;
        myEntityCheckRay.origin = transform.position + Vector3.down * (float.MaxValue / 2f);
        myEntityCheckRay.direction = (transform.position - myEntityCheckRay.origin).normalized;

        Physics.Raycast(myEntityCheckRay.origin, myEntityCheckRay.direction, out myEntityRayHit, float.MaxValue, myEntityLayer);

        Ray floorCheckRay = new Ray();
        RaycastHit floorCheckRayHit = default;
        floorCheckRay.origin = transform.position + Vector3.up * (float.MaxValue / 2f);
        floorCheckRay.direction = (transform.position - floorCheckRay.origin).normalized;

        Physics.Raycast(floorCheckRay.origin, floorCheckRay.direction, out floorCheckRayHit, float.MaxValue, floorLayer);

        if (myEntityRayHit.point.y < floorCheckRayHit.point.y)
        {
            float distance = Vector3.Distance(myEntityRayHit.point, floorCheckRayHit.point);

            Vector3 curPos = transform.position;
            curPos.y += distance;
            transform.position = curPos;
        }
    }
}
