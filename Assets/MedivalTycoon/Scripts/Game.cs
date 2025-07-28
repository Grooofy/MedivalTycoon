using Barrels;
using Characters;
using Tables;
using UI;
using UnityEngine;

namespace MedivalTycoon
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private CharacterManager _characterManager;
        [SerializeField] private LoadingGameSettings _loadingGameSettings;
        [SerializeField] private GameUIManager _gameUIManager;
        [SerializeField] private TableManager _tableManager;
        [SerializeField] private BarrelManager _barrelManager;
        
        
        private void Start()
        {
            _loadingGameSettings.Load();
            _characterManager.CreateCharacters();
            _gameUIManager.ShowUIInfo(_loadingGameSettings);
            _tableManager.Initialize(_loadingGameSettings);
            _tableManager.CreateTables(_loadingGameSettings);
            _barrelManager.Initialize();
            _barrelManager.CreatePoints();
        }

        private void Update()
        {
            _gameUIManager.UpdateUIInfo();
            _characterManager.MoveCharacter();
        }
    }
}