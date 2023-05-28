using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerScript<T> : MonoBehaviour
    {
        protected Player player = null;

        protected virtual void Awake()
        {
            player = GetComponent<Player>();
        }
    }
}
