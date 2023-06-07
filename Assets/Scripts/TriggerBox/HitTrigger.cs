using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrigger : MonoBehaviour
{
    [SerializeField]
    private LayerMask hitTargetObjectLayer = default(LayerMask);

    private float hitDamage = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // Hit 체크하는 함수 만들어서 FixedUpdate에 돌리기
}
