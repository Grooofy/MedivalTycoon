using System.Collections;
using Characters;
using Propses;
using UnityEngine;

namespace Lever
{
    public class LeverBarrelToBeer : MonoBehaviour
    {
        [SerializeField] private BarrelBuffer _firstSpawn;
        [SerializeField] private Point _barrelPoint;
        [SerializeField] private BeerCreator _beerCreator;
        [SerializeField] private GroundUI _groundUIbartender;
        [SerializeField] private GroundUI _groundUIwaiter;
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem _smoke;
        [SerializeField] private BarrelBuffer barrelBuffer;
        [SerializeField] private SphereCollider _collider;


        private WaitForSeconds _wait = new WaitForSeconds(2f);
        private MoverStoper _moverStoper;
        private IProps _currentBarrel;


        private void Reset(bool value)
        {
            TurnLeverObject(value);
            TurnOffUiObject(_groundUIwaiter);
            TurnOnUiObject(_groundUIbartender);
            _currentBarrel.Reset(_firstSpawn, _barrelPoint);
            _smoke.Play();
        }


        private void TurnLeverObject(bool value)
        {
            _animator.SetBool("IsOn", value);
            _collider.enabled = value;
            TurnOnUiObject(_groundUIbartender);
        }

        private void TurnOffUiObject(GroundUI ui)
        {
            ui.FadeOut();
        }

        private void TurnOnUiObject(GroundUI ui)
        {
            ui.FadeIn();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Bartender bartender) && _barrelPoint.IsFill == false)
            {
                _animator.SetBool("IsOn", false);
                _collider.enabled = false;
                TurnOffUiObject(_groundUIbartender);
                StartCoroutine(FillingPoints());
            }

            if (other.TryGetComponent(out Waiter waiter) && _barrelPoint.IsFill)
            {
                Reset(true);
            }
        }

        private IEnumerator FillingPoints()
        {
            var currentQueueBarrels = barrelBuffer.GetTo(1);
            if (currentQueueBarrels.Count == 0) yield break;

            _currentBarrel = currentQueueBarrels.Peek();
            // _currentBarrel.ScaleUp();

            while (_barrelPoint.IsFill == false)
            {
                if (_currentBarrel == null) yield break;
                currentQueueBarrels.Pop();
                StartCoroutine(_currentBarrel.TryMoveTo(_barrelPoint));
                StartCoroutine(_beerCreator.FillingPoints());
                yield return _wait;
            }

            //_currentBarrel.BarrelEmpty();
            _collider.enabled = true;
            TurnOnUiObject(_groundUIwaiter);
        }
    }
}