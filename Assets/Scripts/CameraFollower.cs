using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Vector3 _offSet;
    [SerializeField] private float _smoothing;

    private Target _target;
    private Transform _selectCharacter;

    private void Awake()
    {
        _target = GetComponent<Target>();
        ChangeTarget();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 nextPosition = Vector3.Lerp(transform.position, _selectCharacter.position + _offSet,
            Time.deltaTime * _smoothing);

        transform.position = nextPosition;
    }

    private void ChangeTarget()
    {
        _selectCharacter = _target.Get();
    }
}