using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour, ICharacter
{
    [SerializeField] private Worker _worker;
    [SerializeField] private Transform _pointHand;

    private CharacterController _controller;
    public bool IsCanMove = true;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public int GetId()
    {
        return _worker.Id;
    }

    public int GetNumberWearableObjects()
    {
        return _worker.NumberWearableObjects;
    }

    public void Move(Vector3 direction)
    {
        TryRotate(direction);
        var normalizeDirection = Vector3.Normalize(direction);
        _controller.Move(normalizeDirection * _worker.Speed * Time.deltaTime);
    }

    private void TryRotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}