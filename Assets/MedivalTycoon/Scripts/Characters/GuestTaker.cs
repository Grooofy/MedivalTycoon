using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Characters
{
    public class GuestTaker : MonoBehaviour, IPropsMover
    {
        [SerializeField] private Point _point;
    
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out TavernGuest guest))
            {
                guest.InteractWithGuard(_point.transform);
            }
        }

        public void CreatePoints(int cout, float offset)
        {
            
        }

        public void RegisterProps(Queue<Props> props)
        {
            throw new NotImplementedException();
        }

        public void RegisterProp(Props props)
        {
            throw new NotImplementedException();
        }

        public Action<bool> Fulling { get; set; }
        public IEnumerator FillingPoints()
        {
            throw new NotImplementedException();
        }

        public Queue<Props> GetTo(int amount)
        {
            throw new NotImplementedException();
        }
    }
}

