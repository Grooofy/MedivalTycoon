using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Characters
{
    public class GuestTaker : MonoBehaviour
    {
        [SerializeField] private Point _point;
    
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out TavernGuest guest))
            {
                guest.InteractWithGuard(_point.transform);
            }
        }
    }
}

