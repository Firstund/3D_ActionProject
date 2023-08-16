using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirCheckTrigger : MonoBehaviour
{
    /// <summary>
    /// 공중에 있는지 체크하는데 쓰이는 땅의 LayerMask
    /// </summary>
    [SerializeField]
    private LayerMask floorMask = default;

    private List<GameObject> hitFloorObjectList = new();

    /// <summary>
    /// 공중에 있다고 Check됐다면 true
    /// </summary>
    /// <value></value>
    public bool IsInAIr
    {
        get
        {
            return hitFloorObjectList.Count <= 0;
        }
    }

    /// <summary>
    /// 바닥 Object와 충돌 후 해당 오브젝트가 조건에 맞으면 List에 추가
    /// </summary>
    /// <param name="other"></param>
    private Action<GameObject> onHitFloorEnter = default;
    public Action<GameObject> OnHitFloorEnter
    {
        get
        {
            return onHitFloorEnter;
        }

        set
        {
            onHitFloorEnter = value;
        }
    }

    private Action<GameObject> onHitFloorExit = default;
    public Action<GameObject> OnHitFloorExit
    {
        get
        {
            return onHitFloorExit;
        }

        set
        {
            onHitFloorExit = value;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (floorMask.CompareGameObjectLayer(other.gameObject))
        {
            if (IsPlayerUnder(other.gameObject))
            {
                hitFloorObjectList.Add(other.gameObject);

                onHitFloorEnter?.Invoke(other.gameObject);
            }
        }
    }

    /// <summary>
    /// List에 있던 바닥 Object를 List에서 제거함
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (floorMask.CompareGameObjectLayer(other.gameObject))
        {
            hitFloorObjectList.Remove(other.gameObject);

            onHitFloorExit?.Invoke(other.gameObject);
        }
    }

    /// <summary>
    /// 충돌한 바닥 Object가 Player 아래에 있는지 체크
    /// </summary>
    /// <param name="targetGameObject"></param>
    /// <returns></returns>
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
