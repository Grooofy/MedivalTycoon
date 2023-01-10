using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour, ICharacter
{
    [SerializeField] private Worker _worker;
    private CharacterController _controller;
    
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
        _controller.Move(direction * _worker.Speed * Time.deltaTime);
    }

    private void TryRotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}