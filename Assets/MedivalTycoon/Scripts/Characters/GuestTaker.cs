using System;
using System.Collections;
using System.Collections.Generic;
using Propses;
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

        public void Initialize(string sourceId, BarrelPool barrelPool)
        {
            throw new NotImplementedException();
        }

        public void CreatePoints(int cout, float offset, Vector3 spaceSize)
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

        public void RegisterProps(Stack<IProps> props)
        {
            throw new NotImplementedException();
        }

        public void RegisterProp(IProps barrel)
        {
            throw new NotImplementedException();
        }

        public UnityAction<bool> Fulling { get; set; }
        public IEnumerator FillingPoints()
        {
            throw new NotImplementedException();
        }

        public Stack<IProps> GetTo(int amount)
        {
            throw new NotImplementedException();
        }

        public IProps GetTos()
        {
            throw new NotImplementedException();
        }
    }
}

