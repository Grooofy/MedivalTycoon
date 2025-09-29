using UnityEngine;
using UnityEngine.UI;

namespace Visitors
{
    public class WaitTimerUI : MonoBehaviour
    {
       [SerializeField] private Image _fillImage;         

        public void Initialize()
        {
            _fillImage = GetComponentInChildren<Image>();
        }      

        public void SetFill(float normalizedTime)
        {
            if (_fillImage != null)
            {
                _fillImage.fillAmount = normalizedTime; 
            }
        }
       
        public void SetActive(bool active)
        {
            Debug.Log("SETACTIVE_NOT");
            if (_fillImage != null)
            {
                Debug.Log("SETACTIVE");
                _fillImage.gameObject.SetActive(active);
            }
        }
    }
}