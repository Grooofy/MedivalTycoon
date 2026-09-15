using System.Collections;
using System.Collections.Generic;
using Events;
using MedivalTycoon;
using Propses;
using UnityEngine;

namespace Beers
{
    public class BarrelBeerBuffer : MonoBehaviour, IPropsMover
    {
    [SerializeField, Min(0.01f), Tooltip("Seconds per item before tavern upgrades. Lower is faster.")]
    private float _fillInterval = 0.1f;
        public int AvailableCount => _pointsProps.Count;
        private bool _isResetting;
        private bool _isFilling;
        public bool IsTake;
        private Stack<IProps> _props = new Stack<IProps>();
        private Stack<IProps> _pointsProps = new Stack<IProps>();
    private readonly Dictionary<IProps, Coroutine> _arrivals = new Dictionary<IProps, Coroutine>();
        private SpawnerPoints _spawnerPoints = new SpawnerPoints();
        private Point _barrelFinishPoint;
        private float _delayBarrelReset;
        private IPropsPool _barrelPool;
        private List<Point> _points;
        private int _index;
        private int _amountPoint;
        private bool _isEmpty = true;

        public PropsType Type => PropsType.Barrel;

        public void Initialize(IPropsPool barrelPool, Point barrelFinishPoint, float delayBarrelReset)
        {
            _barrelPool = barrelPool;
            _barrelFinishPoint = barrelFinishPoint;
            _delayBarrelReset = delayBarrelReset;
        }

        public void CreatePoints(int cout, float offset, Vector3 spaceSize = new Vector3())
        {
            _spawnerPoints.Initialize(cout, offset, transform);
            _points = _spawnerPoints.SpawnObjectsInCube(spaceSize);
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
        
        public int GetEmptyPointsCount() => Mathf.Max(0, _amountPoint - _index - _props.Count);

        public IEnumerator FillingPoints()
        {
            if (_isFilling) yield break;
            _isFilling = true;
            try
            {
                while (_props.Count > 0)
                {
                    if (_index >= _amountPoint) { yield return null; continue; }
                    var prop = _props.Pop();
                    _arrivals[prop] = StartCoroutine(prop.TryMoveTo(_points[_index]));
                    _pointsProps.Push(prop);
                    _index++;
                    if (_isEmpty)
                    {
                        _isEmpty = false;
                        EventBus.Raise(new BeerBufferOpen(true));
                    }
                    yield return WaitFor.Seconds(TavernUpgrades.FillSeconds(_fillInterval));
                }
            }
            finally { _isFilling = false; }
        }
        public IEnumerator ResetBarrel()
        {
            if (_isResetting) yield break;
            _isResetting = true;
            try
            {
                while (IsTake && _pointsProps.Count > 0)
                {
                    var props = _pointsProps.Pop();
                    if (_arrivals.TryGetValue(props, out var arrival))
                    {
                        StopCoroutine(arrival);
                        _arrivals.Remove(props);
                    }
                    // Release the reserved slot before accepting another delivery.
                    _index--;
                    _points[_index].Free();
                    yield return props.TryMoveTo(_barrelFinishPoint);
                    EventBus.Raise(new BeerCreated());
                    yield return WaitFor.Seconds(TavernUpgrades.FillSeconds(_delayBarrelReset));
                    _barrelFinishPoint.Free();
                    _barrelPool.Despawn(props);
                    if (_pointsProps.Count == 0)
                    {
                        _isEmpty = true;
                        EventBus.Raise(new BeerBufferOpen(false));
                    }
                }
            }
            finally { _isResetting = false; }
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
        
        
        public void RegisterProp(IProps barrel)
        {
        }
    }
}