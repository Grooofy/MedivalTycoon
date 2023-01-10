using System;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Vector3 _offSet;
    [SerializeField] private float _smoothing;
    [SerializeField] private Target _target;

    private Transform _selectCharacter;

    private void OnEnable()
    {
        _target.TargetReady += ChangeTarget;
    }

    private void OnDisable()
    {
        _target.TargetReady += ChangeTarget;
    }

    private void Awake()
    {
        _target = GetComponent<Target>();
        ChangeTarget(0);
    }

    private void Update()
    {
        Move(transform, _selectCharacter);
    }

    private void Move(Transform startTarget, Transform finishTarget)
    {
        Vector3 nextPosition = Vector3.Lerp(startTarget.position, finishTarget.position + _offSet,
            Time.deltaTime * _smoothing);

        transform.position = nextPosition;
    }

    private void ChangeTarget(int id)
    {
        _selectCharacter = _target.Get(id);
    }
}