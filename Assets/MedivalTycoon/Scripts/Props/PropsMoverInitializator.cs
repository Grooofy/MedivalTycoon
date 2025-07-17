using UnityEngine;
using Lever;


public class PropsMoverInitializator : MonoBehaviour
{
    [Header("To Barrel")]
    [SerializeField] private PropsTaker _barrelTaker;
    [SerializeField] private PropsGiver _barrelGiver;
    [SerializeField] private Regulating _firstRegulating;
    [SerializeField] private Regulating _secondRegulating;
    
    [SerializeField] private LeverInstaller _leverGetBarrel;

    [Header("To Beer")] 
    [SerializeField] private PropsTaker _beerTaker;
    [SerializeField] private PropsGiver _beerGiver;
    [SerializeField] private BeerCreator _beerRegulating;
    
    [Header("To Table")]
    [SerializeField] private PropsTaker _tableTaker;
    [SerializeField] private Regulating _tableRegulating;
    
    [Header("To 2 Table")]
    [SerializeField] private PropsTaker _tableTaker2;
    [SerializeField] private Regulating _tableRegulating2;

    private void Start()
    {
        _barrelGiver.Initialize(_firstRegulating);
        _barrelTaker.Initialize(_secondRegulating);
        _leverGetBarrel.Initialize(_firstRegulating);
       // _beerGiver.Initialize(_beerRegulating);
//        _beerTaker.Initialize(_beerRegulating);
        //_tableTaker.Initialize(_tableRegulating);
        //_tableTaker2.Initialize(_tableRegulating2);
    }
}