using Events;
using Propses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using Visitors;

public class Seat : MonoBehaviour, IPropsMover
{
    private TextMeshProUGUI _beerText;
    private SeatPoint _seatPoint;   
    private TavernVisitor _visitor;
    private Stack<IProps> _props = new Stack<IProps>();
    private Stack<IProps> _pointsProps = new Stack<IProps>();
    private List<Point> _points;
    private SpawnerPoints _spawnerPoints = new SpawnerPoints();
    private bool _isFull;
    private bool _isEmpty;
    private int _index;
    private int _amountPoint;
    private Vector3 _spaceSize = new Vector3(0.25f, 3, 0.25f);

    public void Initialize(LayerMask visitorMask)
    {
        _beerText = GetComponentInChildren<TextMeshProUGUI>();
        _seatPoint = GetComponentInChildren<SeatPoint>();
        _seatPoint.Initialize(visitorMask);
        _seatPoint.VisitorSet += GetVisitor; //ОТПИШИСЬ
    }

    public Vector3 GetPosition()
    {
        return _seatPoint.GetPosition();
    }

    public void CheckHists()
    {
        if (_seatPoint != null)
            _seatPoint.CheckHits();
    }

    private void GetVisitor(TavernVisitor tavernVisitor)
    {       
        _visitor = tavernVisitor;
        if (_visitor != null)
        {
            UpdateBeerDisplay(_visitor.BeerAmount);            
        }
           
    }

    private void SetActiveText(bool value, int remainingBeer = 0)
    {
        _beerText.gameObject.SetActive(value);
        UpdateBeerDisplay(remainingBeer);
    }

    private void UpdateBeerDisplay(int remaining)
    {
        _beerText.text = $"Пиво: {remaining}";
    }   

    public void CreatePoints(int cout, float offset, Vector3 spaceSize = new Vector3())
    {
        _spawnerPoints.Initialize(100, 0.25f, transform);
        _points = _spawnerPoints.SpawnObjectsInCube(_spaceSize);
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

            yield return props.TryMoveTo(_points[_index]);

            _pointsProps.Push(props);
            _index++;

            if (_index >= _amountPoint)
            {
                _index = _amountPoint;
                _isFull = true;
            }
        }
    }

    public Stack<IProps> GetTo(int amount)
    {
        throw new NotImplementedException();
    }
}