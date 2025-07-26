using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Propses;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class Hand : MonoBehaviour, IPropsMover
    {
        public bool IsFull { get; private set; } 
        public int Amount => _points.Count;

        private List<Point> _points = new List<Point>();
        private Queue<IProps> _props = new Queue<IProps>();
        private Queue<IProps> _handProps = new Queue<IProps>();
        private WaitForSeconds _wait = new WaitForSeconds(0.2f);
        private Barrel _currentBarrel;
        private int _index;


        public void RegisterProps(IPropsMover regulating)
        {
            if (regulating == null)
            {
                return;
            }
            _props = regulating.GetTo(Amount);
            _index = 0;
        }

        public IEnumerator FillingPoints()
        {
            if (_props.Count == 0) yield break;
            var temporaryQueue = new Queue<IProps>();

            while (IsFull == false && _index < _points.Count)
            {
                if (_props.Count == 0) yield break;

                var prop = _props.Peek();
                if (prop == null) yield break;

                StartCoroutine(prop.TryMoveTo(_points[_index]));
                temporaryQueue.Enqueue(_props.Dequeue());
                _index++;

                if (_props.Count == 0)
                {
                    IsFull = true;
                    _handProps = new Queue<IProps>(temporaryQueue.Reverse());
                }

                yield return _wait;
            }
        }

        public Queue<IProps> GetTo(int amount)
        {
            if (_handProps.Count == 0) return new Queue<IProps>();

            if (amount > _handProps.Count)
            {
                amount = _handProps.Count;
                _index = amount;
            }

            var queue = new Queue<IProps>();

            for (int i = 0; i < amount; i++)
            {
                queue.Enqueue(_handProps.Dequeue());
                _points[_index - 1].Free();
                _index--;
                IsFull = false;
                if (_index < 0) _index = 0;
            }

            return queue;
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
        
        public void Initialize(string sourceId, BarrelPool barrelPool)
        {
        }

        public void RegisterProps(Queue<IProps> props)
        {
        }

        public void RegisterProp(IProps barrel)
        {
        }
    }
}