using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MedivalTycoon;
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
                point.Free();
                _points.Add(point);
            }
        }

        public void RegisterProps(IPropsMover regulating)
        {
            if (regulating == null) return;

            _incomingProps = regulating.GetTo(GetEmptyPointsCount());
        }

        public int GetEmptyPointsCount()
        {
            var index = 0;

            foreach (var point in _points)
            {
                if (point.IsFill == false) index++;
            }

            return index;
        }

        public IEnumerator FillingPoints()
        {
            if (_incomingProps.Count == 0 || _points.Count == 0) yield break;

            while (_index < _points.Count && _incomingProps.Count > 0)
            {
                _incomingProps.TryPop(out var props);
                var point = _points[_index];
                yield return props.TryMoveTo(point);
                _carriedProps.Push(props);
                _index++;
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
                if (_carriedProps.TryPop(out var props))
                {
                    result.Push(props);
                    _index--;
                    _points[_index].Free();
                }
            }

            if (_carriedProps.Count == 0)
            {
                IsFull = false;
                _index = 0;
                ResetPoints();
            }

            return result;
        }

        private void ResetPoints()
        {
            foreach (var point in _points)
            {
                point.Free();
            }
        }

        public void Initialize(string sourceId, IPropsPool barrelPool)
        {
        }
      
        public void RegisterProps(Stack<IProps> props)
        {
        }

        public void RegisterProp(IProps barrel)
        {
        }
    }
}