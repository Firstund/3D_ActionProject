using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFloorCheck : MonoBehaviour
{
    [SerializeField]
    private LayerMask myEntityLayer = default;
    [SerializeField]
    private LayerMask floorLayer = default;

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
