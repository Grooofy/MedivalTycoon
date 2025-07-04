using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Characters
{
    public class GuestTaker : MonoBehaviour, IPropsMover
    {
         private List<Point> _points = new List<Point>();
    
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out TavernGuest guest))
            {
                guest.InteractWithGuard(_points[0].transform);
            }
        }

        public void CreatePoints(int cout, float offset)
        {
            for (int i = 0; i < cout; i++)
            {
                var point = ObjectFactory.CreateObjectWithComponent<Point>("Point" + i);
                point.transform.parent = transform;
                point.transform.localPosition =  Vector3.up * (i* offset);
                point.IsFill = false;
                _points.Add(point);
            }
        }

        public void RegisterProps(Queue<Props> props)
        {
            throw new NotImplementedException();
        }

        public void RegisterProp(Props props)
        {
            throw new NotImplementedException();
        }

        public UnityAction<bool> Fulling { get; set; }
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

