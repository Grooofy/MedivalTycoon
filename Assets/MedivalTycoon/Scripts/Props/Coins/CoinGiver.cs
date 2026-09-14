using System.Collections;
using Characters;
using Events;
using Tutorial;
using UnityEngine;

namespace Money
{
    public class CoinGiver : MonoBehaviour
    {
        private LayerMask _waiterLayer;
        private float _detectionRadius = 0.35f;
        private CoinBuffer _buffer;
        private bool _isTransferring;

        public void Initialize(IPropsMover regulating, LayerMask waiterLayer)
        {
            _buffer = (CoinBuffer)regulating;
            _waiterLayer = waiterLayer;
            gameObject.SetActive(false);
        }

        public void CheckHits()
        {
            if (_buffer == null) return;
            // The table polls this even while the marker is hidden.
            bool hasCoins = _buffer.HasAvailableCoins;
            if (gameObject.activeSelf != hasCoins)
                gameObject.SetActive(hasCoins);
            if (!hasCoins || _isTransferring) return;

            foreach (var hit in Physics.OverlapSphere(transform.position, _detectionRadius, _waiterLayer))
            {
                if (!hit.TryGetComponent(out Hand hand) || !hand.CanAccept(_buffer.Type)
                    || hand.IsFull || hand.GetEmptyPointsCount() == 0)
                    continue;

                _isTransferring = true;
                hand.RegisterProps(_buffer);
                EventBus.Raise(new TutorialStepCompleted { Step = TutorialStep.TakeMoney });
                // Complete reserved coins even when the player leaves or the marker hides.
                hand.StartCoroutine(TransferTo(hand));
                break;
            }
        }

        private IEnumerator TransferTo(Hand hand)
        {
            try
            {
                yield return hand.FillingPoints();
            }
            finally
            {
                _isTransferring = false;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }
#endif
    }
}
