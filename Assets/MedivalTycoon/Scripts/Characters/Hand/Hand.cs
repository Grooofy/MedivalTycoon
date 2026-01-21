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

        public PropsType Type => _currentType;

        private List<Point> _points = new List<Point>();
        private Stack<IProps> _incomingProps = new Stack<IProps>();
        private Stack<IProps> _carriedProps = new Stack<IProps>();
        private PropsType _currentType;        

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
            _currentType = PropsType.None;
        }

        public void RegisterProps(IPropsMover regulating)
        {
            if (regulating == null) return;

            if(_currentType == PropsType.None)
                _currentType = regulating.Type;

            _incomingProps = regulating.GetTo(GetEmptyPointsCount());            
        }

        public bool CanAccept(PropsType type)
        {
            Debug.Log(type.ToString());
            return _currentType == PropsType.None || _currentType == type;
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
                StartCoroutine(props.TryMoveTo(point));
                _carriedProps.Push(props);
                _index++;
                yield return WaitFor.TenthSecond; 
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
            _currentType = PropsType.None;
            foreach (var point in _points)
            {
                point.Free();
            }
        }

        
        public void RegisterProps(Stack<IProps> props)
        {
        }       
    }
}