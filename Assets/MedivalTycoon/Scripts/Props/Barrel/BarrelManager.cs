using System;
using Lever;
using UnityEngine;

namespace Barrels
{
    public class BarrelManager : MonoBehaviour
    {
        [SerializeField] private LeverGetBarrel _leverGetBarrel;
        [SerializeField] private BarrelBuffer _barrelBuffer;

       /* private void Awake()
        {
            _barrelBuffer.Initialize("Barrel", new Vector3(2,2,2));
            _barrelBuffer.CreatePoints(10, 1);
        }*/
    }
}