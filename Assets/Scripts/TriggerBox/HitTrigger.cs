using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrigger : MonoBehaviour
{
    /// <summary>
    /// 해당 트리거의 주인 즉, 공격자 오브젝트
    /// </summary>
    [SerializeField]
    private GameObject triggerOwnerObject = null;

    /// <summary>
    /// 공격자 오브젝트의 IEntity 스크립트
    /// </summary>
    private IEntity triggerOwnerEntity = null;

    [SerializeField]
    private List<Collider> hitTriggers = null;
    [SerializeField]
    private LayerMask hitTargetObjectLayer = default(LayerMask);

    private List<IEntity> inTriggerTargetEntities = new(); // 현재 충돌중인 TargetEntity들을 담아주는 List

    [SerializeField]
    private bool isContinuousDamage = false;

    [Header("isContinuousDamage가 true일경우 작동함, 지속데미지의 딜레이를 설정")]
    [SerializeField]
    private float continuouseDamageDelay = 3f;

    private Dictionary<IEntity, float> continuouseDamageTimer = new(); // 지속 데미지를 체크해주는 타이머
                                                                       // Entity마다 들어온 타이밍이 다르므로, IEntity를 키값으로 활용하여 타이머를 체크한다.

    private float hitDamage = 0f;

    void Start()
    {
        if (triggerOwnerObject == null)
        {
            Debug.LogError("The HitTrigger name: " + gameObject.name + " has no triggerOwnerObject!");

            return;
        }
        else
        {
            triggerOwnerEntity = triggerOwnerObject.GetComponent<IEntity>();

            if (triggerOwnerEntity == null)
            {
                Debug.LogError("The TriggerOwnerEntityGameObject name: " + triggerOwnerObject.name + " has no IEntity Script!");

                return;
            }
        }

        for (int i = 0; i < hitTriggers.Count; i++)
        {
            hitTriggers[i].isTrigger = true;
        }

        // hitTriggers[0].
    }

    void Update()
    {
        CheckContinuouseDamageTimer();
    }

    // Collider 컴포넌트의 is Trigger가 false인 상태로 충돌을 시작했을 때
    private void OnTriggerEnter(Collider collider)
    {
        // 충돌한 개체가 적인지 체크한다.

        if (hitTargetObjectLayer.CompareGameObjectLayer(collider.gameObject))
        {
            // 타겟 데미지 처리

            IEntity targetEntity = collider.gameObject.GetComponent<IEntity>();

            if(targetEntity == null)
            {
                return;
            }

            if (isContinuousDamage)
            {
                if(!continuouseDamageTimer.ContainsKey(targetEntity))
                {
                    // 처음 hit되는 Entity의 경우 시작 데미지 처리
                    targetEntity.OnHit(triggerOwnerEntity.GetDamage());
                }

                inTriggerTargetEntities.Add(targetEntity);
            }
            else
            {
                // 지속데미지가 아닌경우 평범하게 hit체크
                targetEntity.OnHit(triggerOwnerEntity.GetDamage());
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (hitTargetObjectLayer.CompareGameObjectLayer(collider.gameObject))
        {
            IEntity targetEntity = collider.gameObject.GetComponent<IEntity>();

            inTriggerTargetEntities.Remove(targetEntity);
        }
    }

    /// <summary>
    ///  지속데미지 관련 타이머를 체크
    /// </summary>
    private void CheckContinuouseDamageTimer()
    {
        if (!isContinuousDamage)
        {
            return;
        }

        for (int i = 0; i < inTriggerTargetEntities.Count; i++)
        {
            IEntity curEntity = inTriggerTargetEntities[i];

            if (continuouseDamageTimer.ContainsKey(curEntity)) // entity별로 hit 타이머를 체크함
            {
                continuouseDamageTimer[curEntity] -= Time.deltaTime;

                if (continuouseDamageTimer[curEntity] <= 0f)
                {
                    curEntity.OnHit(triggerOwnerEntity.GetDamage());

                    continuouseDamageTimer[curEntity] = continuouseDamageDelay;
                }
            }
            else
            {
                continuouseDamageTimer[curEntity] = continuouseDamageDelay;
            }
        }
    }
}
