using Barrels;
using Beers;
using Characters;
using Tables;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Visitors;
using Tutorial;

namespace MedivalTycoon
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private CharacterManager _characterManager;
        [SerializeField] private LoadingGameSettings _loadingGameSettings;
        [SerializeField] private GameUIManager _gameUIManager;
        [SerializeField] private TableManager _tableManager;
        [SerializeField] private ChestCoinManager _chestCoinManager;
        [SerializeField] private BarrelManager _barrelManager;
        [SerializeField] private BeerManager _beerManager;
        [SerializeField] private VisitorsManager _visitorsManager;
        [SerializeField] private TutorialManager _tutorialManager;
        
        
        
        private void Start()
        {
            _loadingGameSettings.Load();
            _characterManager.CreateCharacters();
            _gameUIManager.ShowUIInfo(_loadingGameSettings);
            _tableManager.Initialize(_loadingGameSettings);
            _tableManager.CreateTables(_loadingGameSettings);
            _chestCoinManager.Initialize();
            _visitorsManager.Initialize(_loadingGameSettings);
            _barrelManager.Initialize();
            _barrelManager.CreatePoints();
            _beerManager.Initialize();
            _beerManager.CreatePoints();
            _tutorialManager.Initialize();

            if (!_loadingGameSettings.IsTutorialCompleted())
            {
                _tutorialManager.StartTutorial();
            }
        }

        private void Update()
        {
            _gameUIManager.UpdateUIInfo();
            _characterManager.MoveCharacter();
            _barrelManager.CheckHits();
            _chestCoinManager.CheckHits();
            _beerManager.CheckHits();
            _tableManager.CheckHits();
            _visitorsManager.UpdateState();
        }
    }
}