using UnityEngine;
using Lever;
using UnityEngine.Serialization;


public class PropsMoverInitializator : MonoBehaviour
{
    [Header("To Barrel")]
    [SerializeField] private BarrelTaker _barrelTaker;
    [SerializeField] private BarrelGiver _barrelGiver;
    [SerializeField] private BarrelBuffer firstBarrelBuffer;
    [SerializeField] private BarrelBuffer secondBarrelBuffer;
    
    [SerializeField] private LeverInstaller _leverGetBarrel;

    [Header("To Beer")] 
    [SerializeField] private BarrelTaker _beerTaker;
    [SerializeField] private BarrelGiver _beerGiver;
    [SerializeField] private BeerBuffer _beerRegulating;
    
    [Header("To Table")]
    [SerializeField] private BarrelTaker _tableTaker;
    [FormerlySerializedAs("_tableRegulating")] [SerializeField] private BarrelBuffer tableBarrelBuffer;
    
    [Header("To 2 Table")]
    [SerializeField] private BarrelTaker _tableTaker2;
    [SerializeField] private BarrelBuffer _tableRegulating2;

   /* private void Start()
    {
        _barrelGiver.Initialize(firstBarrelBuffer);
        _barrelTaker.Initialize(secondBarrelBuffer);
        _leverGetBarrel.Initialize(firstBarrelBuffer);
       // _beerGiver.Initialize(_beerRegulating);
//        _beerTaker.Initialize(_beerRegulating);
        //_tableTaker.Initialize(_tableRegulating);
        //_tableTaker2.Initialize(_tableRegulating2);
    }*/
}