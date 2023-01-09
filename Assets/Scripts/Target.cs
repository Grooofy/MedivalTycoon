using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private List<CharacterInputController> _controllers;

    public Transform Get()
    {
        foreach (var controller in _controllers)
        {
            if (controller.enabled)
            {
                return controller.transform;
            }
        }
        return null;
    }
    
}