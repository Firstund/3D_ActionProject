using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    public void OnHit(float hitDamage);
    public float GetDamage();
}
