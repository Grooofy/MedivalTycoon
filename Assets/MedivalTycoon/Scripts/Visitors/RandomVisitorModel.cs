using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Visitors
{
    public class RandomVisitorModel : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _visitorsModel;

        public void SpawnRandomModel(Transform position)
        {
            if (_visitorsModel == null) return;
            if (_visitorsModel.Count <= 0) return;
            
           Instantiate(_visitorsModel[Random.Range(0, 1)], position);
        }
    }
}