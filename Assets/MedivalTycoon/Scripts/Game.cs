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
        [SerializeField] private bool IsTutorial;
        [SerializeField] private bool IsPause;

        [SerializeField] private CharacterManager _characterManager;
        [SerializeField] private LoadingGameSettings _loadingGameSettings;
        [SerializeField] private GameUIManager _gameUIManager;
        [SerializeField] private TableManager _tableManager;
        [SerializeField] private ChestCoinManager _chestCoinManager;
        [SerializeField] private BarrelManager _barrelManager;
        [SerializeField] private BeerManager _beerManager;
        [SerializeField] private VisitorsManager _visitorsManager;
        [SerializeField] private TutorialManager _tutorialManager;
        [SerializeField] private UIAnimation _uiAnimation;
        
        
        
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
            _uiAnimation.Initialize();
            /* if (!_loadingGameSettings.IsTutorialCompleted())
             {
                 _tutorialManager.StartTutorial();
             }*/
            //ДЛЯ ТЕСТА
            if (IsTutorial)
            {
                _tutorialManager.StartTutorial();
            }
            _uiAnimation.Play();

        }

        private void Update()
        {
            

            if (Input.GetKey(KeyCode.Escape))
            {
                IsPause = !IsPause;
                Time.timeScale = 0;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                IsPause = !IsPause;
                Time.timeScale = 1;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _uiAnimation.Lift();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                _uiAnimation.Play();
            }

            if (IsPause) return;

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