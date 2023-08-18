using UnityEngine;

public class BarrelAnimation : MonoBehaviour
{
    private const string _isStop = "IsStop";

    [SerializeField] private Animator _animator;
    [SerializeField] private Barrel _barrel;


    private void OnEnable()
    {
        _barrel.MoveEnded += StopRotationAnimation;
    }

    private void OnDisable()
    {
        _barrel.MoveEnded -= StopRotationAnimation;
    }

    private void StopRotationAnimation()
    {
        _animator.SetBool(_isStop, true);
    }
}
