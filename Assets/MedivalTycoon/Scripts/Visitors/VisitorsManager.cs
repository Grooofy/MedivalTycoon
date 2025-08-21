using System.Collections.Generic;
using UnityEngine;

namespace Visitors
{
    public class VisitorsManager : MonoBehaviour
    {
        [SerializeField] private TavernVisitor _tavernVisitor;
        [SerializeField] private RandomVisitorModel _randomVisitorModel;
        
        private readonly List<VisitorController> _controllers = new List<VisitorController>();

        public void CreateVisitor(Transform position, float speed, int maxBeerCount)
        {
            var currentTavernVisitor = Instantiate(_tavernVisitor, position);
            _randomVisitorModel.SpawnRandomModel(currentTavernVisitor.transform);
            var controller = new VisitorController(currentTavernVisitor, speed, maxBeerCount);
            _controllers.Add(controller);
        }

        public void UpdateState()
        {
            if (_controllers == null) return;

            foreach (var controller in _controllers)
                controller.UpdateState();
        }
    }
}