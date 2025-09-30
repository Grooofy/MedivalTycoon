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

        public void Initialize(LoadingGameSettings loadingGameSettings)
        {
            _queueVisitor.Initialize(loadingGameSettings.GetVisitors(), _spacing, _speed, _maxBeerCount, _maxWaitTime);
            _queueVisitor.SpawnVisitorsInLine(_queueVisitor.transform.position);
        }

        public void UpdateState()
        {
            _queueVisitor.UpdateState();
        }
    }
}