using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class BarrelToBeer : MonoBehaviour
{
    [SerializeField] private Regulating _firstSpawn;
    [SerializeField] private Point _barrelPoint;
    [SerializeField] private BeerCreator _beerCreator; 
    [SerializeField] private GameObject _bartenderUiObject;
    [SerializeField] private GameObject _waiterUiObject;
    [SerializeField] private Animator _animator;
    [SerializeField] private Regulating _regulating;
    [SerializeField] private SphereCollider _collider;
    
    
    private WaitForSeconds _wait = new WaitForSeconds(2f);
    private MoverStoper _moverStoper;
    private Props _currentBarrel;
    

    private void OnEnable()
    {
        _regulating.PointFill += TurnObject;
    }

    private void OnDisable()
    {
        _regulating.PointFill -= TurnObject;
    }


    private void Reset(bool value)
    {
       TurnObject(value);
       _waiterUiObject.SetActive(!value);
       _currentBarrel.Reset(_firstSpawn, _barrelPoint);
    }

    
   private void TurnObject(bool value)
    {
        _animator.SetBool("IsOn", value);
        _collider.enabled = value;
        _bartenderUiObject.SetActive(value);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bartender bartender) && _barrelPoint.IsFill == false)
        {
            _collider.enabled = false;
            _bartenderUiObject.SetActive(false);
            StartCoroutine(FillingPoints());
        }

        if (other.TryGetComponent(out Waiter waiter) && _barrelPoint.IsFill)
        {
            Reset(true);
        }
        
    }

    public IEnumerator FillingPoints()
    {
        var currentQueueBarrels = _regulating.GetTo(1);
        if (currentQueueBarrels.Count == 0) yield break;
        
        _currentBarrel = currentQueueBarrels.Peek();
        _currentBarrel.ScaleUp();
        
        while (_barrelPoint.IsFill == false)
        {
            if (_currentBarrel == null) yield break;
            
            StartCoroutine(_currentBarrel.TryMoveTo(_barrelPoint));
            StartCoroutine(_beerCreator.FillingPoints());
            yield return _wait;
        }
        _currentBarrel.BarrelEmpty();
        _collider.enabled = true;
        _waiterUiObject.SetActive(true);
    }
}
