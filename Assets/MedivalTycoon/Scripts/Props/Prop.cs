using UnityEngine;

public class Prop : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private const string _isStop = "IsStop";
    private const string _take = "Take";
    protected void StopRotationAnimation()
    {
        _animator.SetBool(_isStop, true);
    }

    protected void PlayTakeAnimation()
    {
        _animator.SetTrigger(_take);
    }

    public void SetActiveValue(bool value)
    {
        gameObject.SetActive(value);
    }
}
