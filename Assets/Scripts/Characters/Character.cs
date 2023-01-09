using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour, ICharacter
{
    private CharacterController _controller;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }
    
    public void Move(Vector3 direction, float speed)
    {
        TryRotate(direction);
        _controller.Move(direction * speed * Time.deltaTime);
    }

    private void TryRotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}