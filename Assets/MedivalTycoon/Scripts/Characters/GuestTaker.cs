using System;
using System.Collections;
using System.Collections.Generic;
using MedivalTycoon;
using Propses;
using UnityEngine;
using UnityEngine.Events;
using Visitors;


namespace Characters
{
    public class GuestTaker : MonoBehaviour, IPropsMover
    {
        public UnityAction<bool> Fulling { get; set; }
        private List<Point> _points = new List<Point>();
        public PropsType Type => throw new NotImplementedException();


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out TavernVisitor guest))
            {
                //  guest.InteractWithGuard(_points[0].transform);
            }
        }


        public void Initialize(string sourceId, IPropsPool barrelPool)
        {

        }


        public void CreatePoints(int cout, float offset, Vector3 spaceSize)
        {
            for (int i = 0; i < cout; i++)
            {
                var point = ObjectFactory.CreateObjectWithComponent<Point>("Point" + i);
                point.transform.parent = transform;
                point.transform.localPosition = Vector3.up * (i * offset);
                point.Free();
                _points.Add(point);
            }
        }

        public int GetEmptyPointsCount()
        {
            var index = 0;

            foreach (var point in _points)
            {
                if (point.IsFill) index++;
            }

            return index;
        }

        public void RegisterProps(Stack<IProps> props)
        {
            throw new NotImplementedException();
        }

        public void RegisterProp(IProps barrel)
        {
            throw new NotImplementedException();
        }



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

