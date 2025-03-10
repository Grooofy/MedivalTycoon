using UnityEngine;

public class LeverGetBarrel : MonoBehaviour
{
    [SerializeField] private GameObject _uiObject;
    [SerializeField] private Animator _animator;
    [SerializeField] private Regulating _regulating;
    [SerializeField] private SphereCollider _collider;
    private MoverStoper _moverStoper;


    private void OnEnable()
    {
        _regulating.Fulling += TurnObject;
    }

    private void OnDisable()
    {
        _regulating.Fulling -= TurnObject;
    }

   
    private void TurnObject(bool value)
    {
        _animator.SetBool("IsOn", value);
        _collider.enabled = !value;
        _uiObject.SetActive(!value);
        
        if (_moverStoper != null)
            _moverStoper.TurnOnMove();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MoverStoper moverStoper))
        {
            _moverStoper = moverStoper;
            _moverStoper.TurnOffMove();
            _collider.enabled = false;
            StartCoroutine(_regulating.FillingPoints());
        }
    }


}
