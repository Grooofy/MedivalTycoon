using Characters;
using MedivalTycoon;
using Propses;
using System;
using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

namespace SeatSyst
{
    public class SeatInventory : MonoBehaviour, IPropsMover
    {
        public Action<int> NeedTextUpdate;
        public Action BeersEnded;
        private Stack<IProps> _props = new Stack<IProps>();
        private Stack<IProps> _pointsProps = new Stack<IProps>();
        private List<Point> _points = new List<Point>();
        private Point _resetBeerPoint;
        private IPropsPool _beerPool;
        private int _index;
        private int _amountPoint;
        private bool _isFull;
        private bool _isEmpty = false;
        private float _resetDelay;
        private TableInteractionMode _tableInteractionMode;

        public bool IsDrink { get; private set; }

        public PropsType Type => PropsType.Beer;

        public void Initialize(TableInteractionMode tableInteractionMode, IPropsPool beerPool, float resetDelay)
        {
            _resetBeerPoint = GetComponentInChildren<Point>();
            _tableInteractionMode = tableInteractionMode;
            _beerPool = beerPool;
            _resetDelay = resetDelay;
        }

        public void CreatePoints(int count, float offset, Vector3 spaceSize = new Vector3())
        {
            for (int i = 0; i < count; i++)
            {
                var point = ObjectFactory.CreateObjectWithComponent<Point>("Point_" + i);
                point.transform.parent = transform;
                point.transform.localPosition = Vector3.up * (i * offset);
                point.Free();   
                _points.Add(point);
            }
            _amountPoint = _points.Count;
        }

        public void DeletePoints()
        {
            foreach (var point in _points)
                Destroy(point.gameObject);

            ResetProps();
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
                if (_index == _amountPoint) break;

                if (_props.TryPop(out var prop))
                {
                    StartCoroutine(prop.TryMoveTo(_points[_index]));
                    _pointsProps.Push(prop);
                    _index++;
                    NeedTextUpdate?.Invoke(1);
                }

                if (_index == _amountPoint)
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
            while (_isEmpty == false)
            {
                yield return WaitFor.Seconds(_resetDelay);
                _pointsProps.TryPop(out var props);

                if (props == null) break;

                yield return props.TryMoveTo(_resetBeerPoint);

                _beerPool.Despawn(props);
                _resetBeerPoint.Free();
                _index--;

                if (_index <= 0)
                {                    
                    BeersEnded?.Invoke();
                    _index = 0;
                    _isEmpty = true;
                    ResetPoints();
                    _tableInteractionMode.Switch();
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

        private void ResetProps()
        {
            _pointsProps.Clear();
            _props.Clear();
            _index = 0;
            _points.Clear();
            _amountPoint = _points.Count;
        }

        public Stack<IProps> GetTo(int amount)
        {
            throw new System.NotImplementedException();
        }

    }
}