using UnityEngine;
using UnityEngine.UI;

namespace Visitors
{
    public class WaitTimerUI : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
       
        private Camera _mainCamera;

        public void Initialize()
        {
            _mainCamera = Camera.main;
        }

        public void SetFill(float normalizedTime)
        {
            if (_mainCamera != null && _fillImage != null)
            {
                transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward,
                                 _mainCamera.transform.rotation * Vector3.up);

                _fillImage.fillAmount = normalizedTime;
            }
        }

        public void SetActive(bool active)
        {
            if (_fillImage != null)
            {
                _fillImage.gameObject.SetActive(active);
            }
        }
    }
}