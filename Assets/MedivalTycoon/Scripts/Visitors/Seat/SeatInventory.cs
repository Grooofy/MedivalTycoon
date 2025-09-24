using Beers;
using Characters;
using Events;
using MedivalTycoon;
using Propses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeatSyst
{
    public class SeatInventory : MonoBehaviour, IPropsMover
    {
        public Action<int> NeedTextUpdate;
        private Stack<IProps> _props = new Stack<IProps>();
        private Stack<IProps> _pointsProps = new Stack<IProps>();
        private List<Point> _points = new List<Point>();
        private Point _resetBeerPoint;
        private IPropsPool _beerPool;
        private int _index;
        private int _amountPoint;
        private bool _isFull;
        private bool _isEmpty = false;

        public bool IsDrink { get; private set; }

        public void Initialize(IPropsPool beerPool)
        {
            _resetBeerPoint = GetComponentInChildren<Point>();
            _beerPool = beerPool;
        }

        public void CreatePoints(int count, float offset, Vector3 spaceSize = new Vector3())
        {
            for (int i = 0; i < count; i++)
            {
                var point = ObjectFactory.CreateObjectWithComponent<Point>("Point_" + i);
                point.transform.parent = transform;
                point.transform.localPosition = Vector3.up * (i * offset);
                _points.Add(point);
            }
            _amountPoint = _points.Count;
        }

        public void RegisterProps(Stack<IProps> props)
        {
            if (props == null || props.Count == 0) return;

            foreach (var prop in props)
                _props.Push(prop);
        }

        public IEnumerator FillingPoints()
        {
            while (_isFull == false && _props.Count > 0)
            {
                if (_index >= _amountPoint) break;

                if (_props.TryPop(out var prop))
                {
                    StartCoroutine(prop.TryMoveTo(_points[_index]));
                    _pointsProps.Push(prop);
                    _index++;
                    NeedTextUpdate?.Invoke(1);
                }

                if (_index >= _amountPoint)
                    _isFull = true;

                yield return WaitFor.TenthSecond;
            }
        }        
       

        public int GetEmptyPointsCount()
        {
            if (_points.Count == 0) return 0;

            var index = 0;

            foreach (var point in _points)
            {
                if (point.IsFill == false) index++;
            }
            return index;
        }

        public IEnumerator ResetBeer()
        {
            while (true)
            {
                _pointsProps.TryPop(out var props);
                if (props == null) break;

                yield return props.TryMoveTo(_resetBeerPoint);

                yield return WaitFor.Seconds(1);

                _resetBeerPoint.Free();
                _beerPool.Despawn(props);
                _index--;
                _points[_index].Free();

                if (_index < 0)
                {
                    _index = 0;                    
                    _isEmpty = true;
                    ResetPoints();
                }
            }
        }

        private void ResetPoints()
        {
            foreach (var point in _points)
            {
                point.Free();
            }
        }

        public Stack<IProps> GetTo(int amount)
        {
            throw new System.NotImplementedException();
        }

    }
}