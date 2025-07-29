using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Propses.Beer
{
    public class BeerBuffer : MonoBehaviour, IPropsMover
    {
        private WaitForSeconds _wait = new WaitForSeconds(0.3f);
        private Stack<IProps> _props = new Stack<IProps>();
        private Stack<IProps> _pointsProps = new Stack<IProps>();
        private SpawnerPoints _spawnerPoints = new SpawnerPoints();
        private BarrelPool _barrelPool;
        private List<Point> _points;
        private int _index;
        private int _amountPoint;
        private bool _isFull;
        private string _sourceId;
        
        
        public void Initialize(string sourceId, BarrelPool barrelPool)
        {
            _sourceId = sourceId;
            _barrelPool = barrelPool;
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
                if (_index >= _amountPoint) break;
                
                _props.TryPop(out var props);
                if (props == null) break;
                
                yield return  props.TryMoveTo(_points[_index]);

                _pointsProps.Push(props);
                _index++;

                if (_index >= _amountPoint)
                {
                    _isFull = true;
                }
                yield return _wait;
            }
        }

        public Stack<IProps> GetTo(int amount)
        {
            throw new System.NotImplementedException();
        }
        public void RegisterProp(IProps barrel)
        {
            throw new System.NotImplementedException();
        }
    }
}