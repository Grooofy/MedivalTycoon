using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;

public class CameraFollower : MonoBehaviour, IDisposable
{
    private Vector3 _offSet = new Vector3(2, 2, -1.73f);
    private float _smoothing = 10;
    private SwitcherSelectedCharacter _target;

    private List<ICharacter> _characters = new List<ICharacter>();
    private ICharacter _selectedCharacter;
   

    public void Initialize(List<ICharacter> characters, SwitcherSelectedCharacter target, ICharacter selectedCharacter = null)
    {
        _characters = characters;  
        _selectedCharacter = selectedCharacter;
        _target = target;
        _target.Activate += ChangeTarget;
    }

    public void Move()
    {
        MovePosition(transform, _selectedCharacter.GetPosition());
    }

    private void MovePosition(Transform startTarget, Vector3 finishTarget)
    {
        Vector3 nextPosition = Vector3.Lerp(startTarget.position, finishTarget + _offSet,
            Time.deltaTime * _smoothing);

        transform.position = nextPosition;
    }

    private void ChangeTarget(int id)
    {
        _selectedCharacter = _characters[id];
    }

    public void Dispose()
    {
        _target.Activate -= ChangeTarget;
    }
}