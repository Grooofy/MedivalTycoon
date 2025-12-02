using System.Collections.Generic;
using UnityEngine;

namespace Visitors
{
    public class VisitorsSpawner : MonoBehaviour
    {
        [SerializeField] private TavernVisitor _tavernVisitor;
        [SerializeField] private RandomVisitorModel _randomVisitorModel;
        
        private readonly List<TavernVisitor> _tavernVisitors = new List<TavernVisitor>();

        public TavernVisitor CreateVisitor(Transform position, float speed, int maxBeerCount, float maxWaitTime, Vector3 exitPoint)
        {
            var currentTavernVisitor = Instantiate(_tavernVisitor, position);
            _randomVisitorModel.SpawnRandomModel(currentTavernVisitor.transform);
            currentTavernVisitor.Initialize(speed, maxBeerCount, maxWaitTime, exitPoint);
            _tavernVisitors.Add(currentTavernVisitor);
            return currentTavernVisitor;
        }
        
        public void UpdateState()
        {
            if (_tavernVisitors == null) return;

            foreach (var tavernVisitor in _tavernVisitors)
                tavernVisitor.UpdateState();
        }
    }
}