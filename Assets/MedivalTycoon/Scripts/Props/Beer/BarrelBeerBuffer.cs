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
        public bool IsTake;
        private Stack<IProps> _props = new Stack<IProps>();
        private Stack<IProps> _pointsProps = new Stack<IProps>();
        private SpawnerPoints _spawnerPoints = new SpawnerPoints();
        private Point _barrelFinishPoint;
        private WaitForSeconds _delayBarrelReset;
        private IPropsPool _barrelPool;
        private List<Point> _points;
        private int _index;
        private int _amountPoint;
        private bool _isFull;
        private bool _isEmpty = true;
        private int _amountBeerToBarrel;
        private string _sourceId;


        public void Initialize(string sourceId, IPropsPool barrelPool, Point barrelFinishPoint, WaitForSeconds delayBarrelReset, int amountBeerToBarrel)
        {
            _sourceId = sourceId;
            _barrelPool = barrelPool;
            _barrelFinishPoint = barrelFinishPoint;
            _delayBarrelReset = delayBarrelReset;
            _amountBeerToBarrel = amountBeerToBarrel;
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
        
        public int GetEmptyPointsCount()
        {
            var index = 0;

            foreach (var point in _points)
            {
                if (point.IsFill) index++;
            }
            return index;
        }


        public IEnumerator FillingPoints()
        {
            while (_isFull == false && _props.Count > 0)
            {
                if (_isEmpty)
                {
                    EventBus.Raise(new BeerBufferOpen(_isEmpty));
                    _isEmpty = false;
                }
                if (_index >= _amountPoint) break;
                
                _props.TryPop(out var props);
                if (props == null) break;
                
                yield return  props.TryMoveTo(_points[_index]);

                _pointsProps.Push(props);
                _index++;

                if (_index >= _amountPoint)
                {
                    _index = _amountPoint;
                    _isFull = true;
                }
            }
        }

        public IEnumerator ResetBarrel()
        {
            while (IsTake && _isEmpty == false)
            {
                _pointsProps.TryPop(out var props);
                if (props == null) break;
                
                yield return props.TryMoveTo(_barrelFinishPoint);
                EventBus.Raise(new BeerCreated());
                yield return _delayBarrelReset;
                _barrelFinishPoint.Free();
                _barrelPool.Despawn(props);
                _index--;
                _points[_index].Free();
                if (_index < 0)
                {
                    _index = 0;
                    EventBus.Raise(new BeerBufferOpen(_isEmpty));
                    _isEmpty = true;
                }
            }
        }
        
        
        
        public Stack<IProps> GetTo(int amount)
        {
            throw new System.NotImplementedException();
        }
        
        private void ResetPoints()
        {
            foreach (var point in _points)
            {
                point.Free();
            }
        }
        
        public void RegisterProp(IProps barrel)
        {
        }
    }
}