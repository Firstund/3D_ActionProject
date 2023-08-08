using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirCheckTrigger : MonoBehaviour
{
    [SerializeField]
    private LayerMask floorMask = default;

    private List<GameObject> hitFloorObjectList = new();

    public bool IsInAIr
    {
        get
        {
            return hitFloorObjectList.Count <= 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (floorMask.CompareGameObjectLayer(other.gameObject))
        {
            if (IsPlayerUnder(other.gameObject))
            {
                hitFloorObjectList.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (floorMask.CompareGameObjectLayer(other.gameObject))
        {
            hitFloorObjectList.Remove(other.gameObject);
        }
    }

    private bool IsPlayerUnder(GameObject targetGameObject)
    {
        bool result = false;

        Ray ray = new Ray();
        RaycastHit hit = new RaycastHit();
        ray.origin = transform.position;
        ray.direction = (targetGameObject.transform.position - transform.position).normalized;

        Physics.Raycast(ray.origin, ray.direction, out hit, Vector3.Distance(transform.position, targetGameObject.transform.position), floorMask);

        if (hit.point.y <= transform.position.y)
        {
            result = true;
        }

        return result;
    }
}
