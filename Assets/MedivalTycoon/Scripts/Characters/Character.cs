using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour, ICharacter
{
    [SerializeField] private Worker _worker;
    [SerializeField] private GameObject _barrel;
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
        var normalizeDirection = Vector3.Normalize(direction);
        _controller.Move(normalizeDirection * _worker.Speed * Time.deltaTime);
    }

    public void PutObject(GameObject props)
    {
        props.transform.DOMove(transform.position + new Vector3(0,0.5f,0), 1);
        props.SetActive(false);
        _barrel.SetActive(true);
        
    }

    private void TryRotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}