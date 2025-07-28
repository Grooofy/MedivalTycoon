using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Propses;
using UnityEngine;

namespace Characters
{
    public class Hand : MonoBehaviour, IPropsMover
    {
        public bool IsFull { get; private set; }
        public int Amount => _points.Count;

        private List<Point> _points = new List<Point>();
        private Stack<IProps> _incomingProps = new Stack<IProps>();
        private Stack<IProps> _carriedProps = new Stack<IProps>();
        private WaitForSeconds _wait = new WaitForSeconds(0.2f);

        private int _index;

        
        public void CreatePoints(int count, float offset, Vector3 spaceSize)
        {
            for (int i = 0; i < count; i++)
            {
                var point = ObjectFactory.CreateObjectWithComponent<Point>("Point_" + i);
                point.transform.parent = transform;
                point.transform.localPosition = Vector3.up * (i * offset);
                point.IsFill = false;
                _points.Add(point);
            }
        }
        
        public void RegisterProps(IPropsMover regulating)
        {
            if (regulating == null) return;

            _incomingProps = regulating.GetTo(Amount);
            _index = 0;
            IsFull = false;
        }
        
        public IEnumerator FillingPoints()
        {
            if (_incomingProps.Count == 0 || _points.Count == 0) yield break;

            _carriedProps.Clear();

            while (_index < _points.Count && _incomingProps.Count > 0)
            {
                var prop = _incomingProps.Pop();
                if (prop == null) continue;

                var point = _points[_index];
                StartCoroutine(prop.TryMoveTo(point));
                _carriedProps.Push(prop);
                _index++;

                yield return _wait;
            }

            if (_carriedProps.Count == _points.Count)
            {
                IsFull = true;
            }
        }

        
        public Stack<IProps> GetTo(int amount)
        {
            var result = new Stack<IProps>();
            int takeAmount = Mathf.Min(amount, _carriedProps.Count);

            for (int i = 0; i < takeAmount; i++)
            {
                var prop = _carriedProps.Pop();
                result.Push(prop);

                _index--;
                if (_index >= 0 && _index < _points.Count)
                {
                    _points[_index].Free();
                }
            }

            if (_carriedProps.Count == 0)
            {
                IsFull = false;
                _index = 0;
            }

            return result;
        }

        
        public void Initialize(string sourceId, BarrelPool barrelPool) { }

        public void RegisterProps(Stack<IProps> props) { }

        public void RegisterProp(IProps barrel) { }
    }
}
