using UnityEngine;

public class Prop : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private const string _animationParametrName = "IsStop";
    protected void StopAnimation()
    {
        _animator.SetBool(_animationParametrName, true);
    }
    
    public void SetActiveValue(bool value)
    {
        gameObject.SetActive(value);
    }
}
