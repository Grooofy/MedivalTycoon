using Beers;
using MedivalTycoon;
using Propses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Money
{
    public class CoinBuffer : MonoBehaviour, IPropsMover
    {
        public Action AllCoinsCreated;
        private IPropsPool _coinPool;
        public SpawnerPoints _spawnerPoints;
        private List<Point> _points = new List<Point>();
        private Stack<IProps> _props = new Stack<IProps>();
        private Stack<IProps> _pointsProps = new Stack<IProps>();
        private int _amountPoint;
        private int _amountVisitorWallet;
        private bool _isCreatingCoins;
        private readonly Dictionary<IProps, Coroutine> _arrivalCoroutines = new Dictionary<IProps, Coroutine>();
        private int _index;
        private TableInteractionMode _tableInteractionMode;

        public PropsType Type => PropsType.Coin;

        public bool IsTake { get; private set; }
        public bool HasAvailableCoins => _pointsProps.Count > 0;

        public bool HasUncollectedCoins => _isCreatingCoins || _pointsProps.Count > 0;

        public void Initialize(IPropsPool coinPool, TableInteractionMode tableInteractionMode)
        {
            _coinPool = coinPool;
            _spawnerPoints = new SpawnerPoints();
            _tableInteractionMode = tableInteractionMode;
        }

        public void SetAmountVisitorWallet(int amount)
        {
            // Preserve the existing payout (the old inclusive loop created amount + 1).
            _amountVisitorWallet = amount + 1;
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
            if (_isCreatingCoins) yield break;
            _isCreatingCoins = true;
            while (_amountVisitorWallet > 0)
            {
                if (_index >= _amountPoint)
                {
                    yield return null;
                    continue;
                }

                var prop = _coinPool.Spawn();

                _arrivalCoroutines[prop] = StartCoroutine(prop.TryMoveTo(_points[_index]));

                _pointsProps.Push(prop);
                _index++;
                _amountVisitorWallet--;
                
                yield return WaitFor.QuarterSecond;
            }

            _isCreatingCoins = false;
            if (_amountVisitorWallet == 0)
            {
                _amountVisitorWallet = 0;
                AllCoinsCreated?.Invoke();
            }
                
        }

        public Stack<IProps> GetTo(int amount)
        {
            var result = new Stack<IProps>();
            int itemsToTake = Mathf.Min(amount, _pointsProps.Count);

            for (int i = 0; i < itemsToTake; i++)
            {
                _pointsProps.TryPop(out var prop);

                // A coin must stop moving to the table before the hand takes ownership.
                if (_arrivalCoroutines.TryGetValue(prop, out var arrival))
                {
                    StopCoroutine(arrival);
                    _arrivalCoroutines.Remove(prop);
                }

                result.Push(prop);

                if (_index > 0)
                {
                    _index--;
                    _points[_index].Free();
                }

                if (_pointsProps.Count == 0)
                {
                    _index = 0;
                    ResetPoints();
                }
            }
            result = new Stack<IProps>(result);
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
