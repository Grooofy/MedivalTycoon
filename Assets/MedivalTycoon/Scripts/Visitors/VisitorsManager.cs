using System.Collections;
using UnityEngine;

namespace Visitors
{
    public class VisitorsManager : MonoBehaviour
    {
        [SerializeField] private QueueVisitor _queueVisitor;

        [SerializeField] private float _spacing;
        [SerializeField] private float _speed;
        [SerializeField] private float _maxWaitTime;
        [SerializeField] private int _maxBeerCount;

        public int RemainingVisitors { get; private set; }

        public void Initialize(LoadingGameSettings loadingGameSettings)
        {
            RemainingVisitors = loadingGameSettings.GetVisitors();
            EventBus.Unsubscribe<Events.VisitorLeaveTavern>(OnVisitorLeft);
            EventBus.Subscribe<Events.VisitorLeaveTavern>(OnVisitorLeft);
            _queueVisitor.Initialize(loadingGameSettings.GetVisitors(), _spacing, _speed, _maxBeerCount, _maxWaitTime);
            _queueVisitor.SpawnVisitorsInLine(_queueVisitor.transform.position);
        }

        public void UpdateState()
        {
            _queueVisitor.UpdateState();
        }

        private void OnVisitorLeft(Events.VisitorLeaveTavern visitorLeaveTavern)
        {
            RemainingVisitors = Mathf.Max(0, RemainingVisitors - 1);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<Events.VisitorLeaveTavern>(OnVisitorLeft);
        }
    }
}
