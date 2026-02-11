using Beers;
using MedivalTycoon;
using Propses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Money
{
    public class CoinBuffer : MonoBehaviour, IPropsMover
    {
        private IPropsPool _coinPool;
        public SpawnerPoints _spawnerPoints;
        private List<Point> _points = new List<Point>();
        private Stack<IProps> _props = new Stack<IProps>();
        private Stack<IProps> _pointsProps = new Stack<IProps>();
        private int _amountPoint;
        private int _amountWallet;
        private bool _isFull;
        private int _index;
        private TableInteractionMode _tableInteractionMode;

        public PropsType Type => PropsType.Coin;

        public bool IsTake { get; private set; }

        public void Initialize(IPropsPool coinPool, TableInteractionMode tableInteractionMode)
        {
            _coinPool = coinPool;
            _spawnerPoints = new SpawnerPoints();
            _tableInteractionMode = tableInteractionMode;
        }

        public void SetAmountVisitorWallet(int amount)
        {
            _amountWallet = amount + _index; 
        }

        public void CreatePoints(int cout, float offset, Vector3 spaceSize = default)
        {
            _spawnerPoints.Initialize(cout, offset, transform);
            _points.AddRange(_spawnerPoints.SpawnVerticalColumn(offset));
            _amountPoint = _points.Count;
        }

        public void RegisterProps(Stack<IProps> props)
        {
            if (props == null) return;
            if (props.Count == 0) return;

            foreach (var prop in props)
            {
                if (prop == null) continue;
                _props.Push(prop);
            }
        }

        public IEnumerator FillingPoints()
        {
            while (_isFull == false && _index <= _amountWallet)
            {
                if (_index >= _amountPoint) break;

                var prop = _coinPool.Spawn();

                StartCoroutine(prop.TryMoveTo(_points[_index]));

                _pointsProps.Push(prop);
                _index++;
                
                if (_index >= _amountPoint)
                    _isFull = true;
                
                yield return WaitFor.QuarterSecond;
            }

            if (_index >= _amountWallet)
                _amountWallet = 0;
        }

        public Stack<IProps> GetTo(int amount)
        {
            var result = new Stack<IProps>();
            int itemsToTake = Mathf.Min(amount, _pointsProps.Count);

            for (int i = 0; i < itemsToTake; i++)
            {
                _pointsProps.TryPop(out var prop);

                result.Push(prop);

                if (_index >= 0)
                {
                    _index--;
                    _points[_index].Free();
                }

                if (_pointsProps.Count == 0)
                {
                    _index = 0;
                    _isFull = false;
                    ResetPoints();
                }
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

        private void ResetProps()
        {
            _pointsProps.Clear();
            _props.Clear();
            _index = 0;
            _points.Clear();
            _amountPoint = _points.Count;
        }

        public int GetEmptyPointsCount()
        {
            throw new System.NotImplementedException();
        }


    }
}