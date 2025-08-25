using System.Collections.Generic;
using UnityEngine;

namespace Visitors
{
    public class VisitorsManager : MonoBehaviour
    {
        [SerializeField] private TavernVisitor _tavernVisitor;
        [SerializeField] private RandomVisitorModel _randomVisitorModel;
        
        private readonly List<TavernVisitor> _tavernVisitors = new List<TavernVisitor>();

        public TavernVisitor CreateVisitor(Transform position, float speed, int maxBeerCount)
        {
            var currentTavernVisitor = Instantiate(_tavernVisitor, position);
            _randomVisitorModel.SpawnRandomModel(currentTavernVisitor.transform);
            currentTavernVisitor.Initialize(speed, maxBeerCount);
            _tavernVisitors.Add(currentTavernVisitor);
            return currentTavernVisitor;
        }
        
        public void SetAllVisitorsState(StateEvent newState)
        {
            foreach (var controller in _tavernVisitors)
                controller.ChangeState(newState);
        }

        public void UpdateState()
        {
            if (_tavernVisitors == null) return;

            foreach (var tavernVisitor in _tavernVisitors)
                tavernVisitor.UpdateState();
        }
    }
}