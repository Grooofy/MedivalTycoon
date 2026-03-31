using UnityEngine;
using UI.MainMenu;

namespace MedivalTycoon
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private MenuUIManager _menuUIManager;
        [SerializeField] private LevelButtonCreater _levelButtonCreater;
        
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _menuUIManager.Initialize();
            _levelButtonCreater.Initialize();
        }

        private void Update()
        {
            _menuUIManager.UpdateUI();
        }
    }
}
