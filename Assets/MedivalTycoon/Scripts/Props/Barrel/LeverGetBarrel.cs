using Characters;
using UnityEngine;

public class LeverGetBarrel : MonoBehaviour
{
    private GroundUI _uiObject;
    private Animator _animator;
    private Regulating _regulating;
    private SphereCollider _collider;


    public void Initialize(IPropsMover propsMover)
    {
        _regulating = propsMover as Regulating;
        _collider = GetComponent<SphereCollider>();
        _uiObject = GetComponentInChildren<GroundUI>().Initialize();
        _animator = GetComponentInChildren<Animator>();
        _regulating.Fulling += TurnObject;
        _animator.SetBool("IsOn", true);
    }
   

    private void OnDestroy()
    {
        _regulating.Fulling -= TurnObject;
    }

   
    private void TurnObject(bool value)
    {
        _animator.SetBool("IsOn", value);
        
        _collider.enabled = !value;
        
        if (value)
            _uiObject.FadeIn();
        else
            _uiObject.FadeOut();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bartender bartender))
        {
            _collider.enabled = false;
            _animator.SetBool("IsOn", false);
            StartCoroutine(_regulating.FillingPoints());
        }
    }


}
