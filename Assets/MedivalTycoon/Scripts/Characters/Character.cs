using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour, ICharacter
{
    [SerializeField] private Worker _worker;
    [SerializeField] private Transform _pointHand;

    private CharacterController _controller;
    private Queue<Barrel> _barrels = new Queue<Barrel>();
    private float _offset;
    private int _maxCoutBarrel = 5;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public int GetId()
    {
        return _worker.Id;
    }

    public void Move(Vector3 direction)
    {
        TryRotate(direction);
        var normalizeDirection = Vector3.Normalize(direction);
        _controller.Move(normalizeDirection * _worker.Speed * Time.deltaTime);
    }

    public void PutObjects(Barrel barrel)
    {
        PutObject(barrel);
    }

    private void PutObject(Barrel barrel)
    {
        barrel.ChangeOwner(_pointHand.gameObject, CalculatePoint());
        _barrels.Enqueue(barrel);
    }

    private Vector3 CalculatePoint()
    {
        return new Vector3(_pointHand.position.x, _pointHand.position.y + _offset, _pointHand.position.z);
    }

    private void TryRotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}