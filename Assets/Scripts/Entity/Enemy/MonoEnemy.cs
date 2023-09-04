using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [Serializable]
    public class EnemyStats
    {
        public float hp = 10;

        public float ap = 2;
        public float dp = 2;
    }

    public class MonoEnemy : MonoBehaviour, IEntity
    {
        [SerializeField]
        private EnemyStats enemyStats = default(EnemyStats);

        /// <summary>
        /// Enemy의 공격을 담당하는 함수
        /// </summary>
        public virtual void Attack()
        {
            
        }
        
        /// <summary>
        /// 데미지 처리를 해주는함수, 적 종류별로 상성 등의 요소가 달라질 수 있음, 후에 마저 제작
        /// </summary>
        /// <param name="hitDamage"></param>
        public virtual void OnHit(float hitDamage)
        {
            // 임시 공식
            Debug.Log("적이 데미지를 받음. 받은 데미지: " + hitDamage);

            enemyStats.hp -= hitDamage;
        }

        /// <summary>
        /// 적의 데미지를 반환해주는 함수, 적 종류별로 다른 데미지 공식을 사용해야한다면 이용할것
        /// </summary>
        /// <returns></returns>
        public virtual float GetDamage()
        {
            // 부모는 단순히 ap를 반환하는 형태로 두고, 자식 class가 override했을 때 여러 공식들을 적용할것

            return enemyStats.ap;
        }
    }
}
