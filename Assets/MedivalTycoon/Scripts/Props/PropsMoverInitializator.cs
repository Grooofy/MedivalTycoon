using UnityEngine;
using Lever;
using UnityEngine.Serialization;


public class PropsMoverInitializator : MonoBehaviour
{
    [Header("To Barrel")]
    [SerializeField] private PropsTaker _barrelTaker;
    [SerializeField] private PropsGiver _barrelGiver;
    [FormerlySerializedAs("_firstRegulating")] [SerializeField] private BarrelBuffer firstBarrelBuffer;
    [FormerlySerializedAs("_secondRegulating")] [SerializeField] private BarrelBuffer secondBarrelBuffer;
    
    [SerializeField] private LeverInstaller _leverGetBarrel;

    [Header("To Beer")] 
    [SerializeField] private PropsTaker _beerTaker;
    [SerializeField] private PropsGiver _beerGiver;
    [SerializeField] private BeerCreator _beerRegulating;
    
    [Header("To Table")]
    [SerializeField] private PropsTaker _tableTaker;
    [FormerlySerializedAs("_tableRegulating")] [SerializeField] private BarrelBuffer tableBarrelBuffer;
    
    [Header("To 2 Table")]
    [SerializeField] private PropsTaker _tableTaker2;
    [SerializeField] private BarrelBuffer _tableRegulating2;

    private void Start()
    {
        _barrelGiver.Initialize(firstBarrelBuffer);
        _barrelTaker.Initialize(secondBarrelBuffer);
        _leverGetBarrel.Initialize(firstBarrelBuffer);
       // _beerGiver.Initialize(_beerRegulating);
//        _beerTaker.Initialize(_beerRegulating);
        //_tableTaker.Initialize(_tableRegulating);
        //_tableTaker2.Initialize(_tableRegulating2);
    }
}