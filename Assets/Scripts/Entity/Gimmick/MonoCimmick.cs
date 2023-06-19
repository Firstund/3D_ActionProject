using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gimmick
{
    [Serializable]
    public class GimmickStats
    {
        public float hp = 1f;
        public float ap = 0.1f;
    }

    public class MonoCimmick : MonoBehaviour, IEntity
    {
        [SerializeField]
        private GimmickStats gimmickStats = default(GimmickStats);

        /// <summary>
        /// 파괴 가능한 기믹이라면, 체력과 연계하여 설정할것
        /// </summary>
        /// <param name="hitDamage"></param>
        public virtual void OnHit(float hitDamage)
        {
            
        }

        /// <summary>
        /// 데미지를 주는 기믹이 아닐경우 0으로 설정해둘것
        /// </summary>
        /// <returns></returns>
        public virtual float GetDamage()
        {
            return gimmickStats.ap;
        }
    }
}
