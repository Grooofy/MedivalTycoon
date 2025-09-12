using System;
using System.Collections;
using System.Collections.Generic;
using Propses;
using TMPro;
using UnityEngine;
using Visitors;

public class Seat : MonoBehaviour, IPropsMover
{
    [SerializeField] private TextMeshProUGUI _beerText;
    [SerializeField] private Transform _seatTransform;
    public bool IsOccupied  { get; private set; }
    private TavernVisitor _guest;
    private int requiredBeer;
    private WaitForSeconds _delay = new WaitForSeconds(0.5f);
    private int deliveredBeer;

    

    public void Occupy(TavernVisitor guest)
    {
        if (IsOccupied == false && guest != null)
        {
            IsOccupied = true; 
            _guest = guest;
        }
    }

    public Vector3 GetPosition()
    {
        return _seatTransform.position + _seatTransform.localPosition;
    }

    public void Vacate()
    {
        IsOccupied = false;
        _guest = null;
       // SetActiveText(false);
       
    }

    public IEnumerator DeliverBeer(int amount)
    {
        while (deliveredBeer < requiredBeer)
        {
            deliveredBeer += amount;
            var currentAmount = requiredBeer - deliveredBeer;
            //SetActiveText(true, currentAmount);

            if (currentAmount <= 0)
            {
             
            }
            yield return _delay;
        }
    }

    private void SetActiveText(bool value, int remainingBeer = 0)
    {
        _beerText.gameObject.SetActive(value);
        UpdateBeerDisplay(remainingBeer);
    }

    private void UpdateBeerDisplay(int remaining)
    {
        _beerText.text = $"Пиво: {remaining}";
    }

    public void CreatePoints(int cout, float offset, Vector3 spaceSize = new Vector3())
    {
        throw new NotImplementedException();
    }

    public void RegisterProps(Stack<IProps> props)
    {
        throw new NotImplementedException();
    }

    public int GetEmptyPointsCount()
    {
        throw new NotImplementedException();
    }

    public IEnumerator FillingPoints()
    {
        throw new NotImplementedException();
    }

    public Stack<IProps> GetTo(int amount)
    {
        throw new NotImplementedException();
    }
}