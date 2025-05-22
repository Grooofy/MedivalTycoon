using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PropsMoverInitilizator : MonoBehaviour
{
    [Header("To Barrel")]
    [SerializeField] private PropsTaker _barrelTaker;
    [SerializeField] private PropsGiver _barrelGiver;
    [SerializeField] private Regulating _firstRegulating;
    [SerializeField] private Regulating _secondRegulating;

    [Header("To Beer")] 
    [SerializeField] private PropsTaker _beerTaker;
    [SerializeField] private PropsGiver _beerGiver;
    [SerializeField] private BeerCreator _beerRegulating;

    private void Awake()
    {
        _barrelGiver.Initialize(_firstRegulating);
        _barrelTaker.Initialize(_secondRegulating);
        _beerGiver.Initialize(_beerRegulating);
        _beerTaker.Initialize(_beerRegulating);
    }
}