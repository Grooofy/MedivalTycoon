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
        private IPropsMover _regulating;
        private Hand _currentHand;
        private Coroutine _activeCoroutine;
        private bool _isActive;
        private bool _isReady;
        private bool _isActiveRequested;

        public void Initialize(IPropsMover regulating, LayerMask waiterLayer)
        {
            _regulating = regulating;
            _waiterLayer = waiterLayer;
            _isReady = false;
            _isActiveRequested = false;

            // Если регулятор - это CoinBuffer, подпишемся на событие создания всех монет
            if (regulating is Money.CoinBuffer coinBuffer)
            {
                coinBuffer.AllCoinsCreated += OnAllCoinsCreated;
            }
        }

        public void CheckHits()
        {
            if (_isActive == false) return;

            Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius, _waiterLayer);

            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    if (_currentHand != null) break;

                    if (hit.TryGetComponent(out Hand hand))
                    {
                        if (hand.CanAccept(_regulating.Type))
                        {
                            _currentHand = hand;
                            _currentHand.RegisterProps(_regulating);
                            EventBus.Raise(new TutorialStepCompleted { Step = TutorialStep.TakeMoney });
                            _activeCoroutine = StartCoroutine(_currentHand.FillingPoints());
                        }
                            
                    }
                }
            }
            else if (_currentHand != null)
            {
                if (_activeCoroutine != null)
                {
                    StopCoroutine(_activeCoroutine);
                    _activeCoroutine = null;
                }

                _currentHand = null;
            }
        }

        public void SetActiveGameObject(bool value)
        {
            // Запрашиваем видимость; реальная видимость зависит также от готовности (коинов)
            _isActiveRequested = value;

            var shouldBeActive = _isActiveRequested && _isReady;
            _isActive = shouldBeActive;
            gameObject.SetActive(shouldBeActive);
        }

        private void OnAllCoinsCreated()
        {
            _isReady = true;

            // Если уже был запрос на активацию — включаем
            if (_isActiveRequested)
            {
                _isActive = true;
                gameObject.SetActive(true);
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
