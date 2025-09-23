using Characters;
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
    private TextShower _textShower;
    private SeatPoint _seatPoint;
    private TavernVisitor _visitor;
    private Stack<IProps> _props = new Stack<IProps>();
    private Stack<IProps> _pointsProps = new Stack<IProps>();
    private List<Point> _points = new List<Point>();
    private SpawnerPoints _spawnerPoints = new SpawnerPoints();
    private bool _isFull;
    private bool _isEmpty;
    private int _index;
    private int _amountPoint;

    public void Initialize(LayerMask visitorMask)
    {
        _textShower = GetComponentInChildren<TextShower>();
        _textShower.Instialize();
        _seatPoint = GetComponentInChildren<SeatPoint>();
        _seatPoint.Initialize(visitorMask);
        _seatPoint.VisitorSet += GetVisitor;
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
        if (_visitor != null) return;
        _visitor = tavernVisitor;
        CreatePoints(_visitor.BeerAmount, 0.1f);
        _textShower.UpdateBeerDisplay(_visitor.BeerAmount);
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
            if (point.IsFill == false) index++;
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

    private void OnDestroy()
    {
        _seatPoint.VisitorSet -= GetVisitor;
    }

    public Stack<IProps> GetTo(int amount)
    {
        throw new NotImplementedException();
    }
}