using UnityEngine;
using DG.Tweening;

public class Props : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private const string _isStop = "IsStop";

    protected void StopRotationAnimation()
    {
        _animator.SetBool(_isStop, true);
    }
    
    public void SetActiveValue(bool value)
    {
        gameObject.SetActive(value);
    }
}
