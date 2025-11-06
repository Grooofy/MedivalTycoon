using Events;
using System.Collections;
using UnityEngine;

namespace Tables
{
    public class TableBuilderAnimation : MonoBehaviour
    {
        private Table _target;
        private float _buildDuration = 1f;
        private ParticleSystem _buildParticles;

        private Vector3 _initialScale = new Vector3(1f, 1f, 1f);
        private Coroutine _animationRoutine;
        private bool _isBuilding;

        public void Initialize()
        {
            _buildParticles = GetComponentInChildren<ParticleSystem>();
        }

        public void Play(Table table, Seat seat)
        {  
            if (_isBuilding) return; 

            _isBuilding = true;
            if (_animationRoutine != null)
                StopCoroutine(_animationRoutine);

            _animationRoutine = StartCoroutine(AnimateBuild(table, seat));
        }

        private IEnumerator AnimateBuild(Table table, Seat seat)
        {
            float elapsed = 0f;

            while (elapsed < _buildDuration)
            {
                if (_buildParticles != null && !_buildParticles.isPlaying)
                {
                    _buildParticles.Play();
                }

                float time = elapsed / _buildDuration;
                table.transform.localScale = Vector3.Lerp(Vector3.zero, _initialScale, time);
                elapsed += Time.deltaTime;
                yield return null;
            }

            table.transform.localScale = _initialScale;
            EventBus.Raise(new TableBuilt(seat));
        }
    }
}