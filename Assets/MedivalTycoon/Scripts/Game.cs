using Barrels;
using Beers;
using Characters;
using Tables;
using Tutorial;
using UI;
using UnityEngine;
using Visitors;


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
        [SerializeField] private UIController _uiController;
        [SerializeField] private VictoryPanel _victoryPanel;

        private bool _isLevelCompleted;

        // Null until a successful completion; preserve the unrounded timer value.
        public float? RemainingSecondsAtCompletion { get; private set; }
        public float LevelDurationSeconds { get; private set; }




        private void Start()
        {

            _loadingGameSettings.Load();
            LevelDurationSeconds = Mathf.Max(0f, _loadingGameSettings.GetSeconds());
            Time.timeScale = 1f;
            _characterManager.CreateCharacters();
            _gameUIManager.ShowUIInfo(_loadingGameSettings, _uiController.ShowTimeOut);
            _tableManager.Initialize(_loadingGameSettings);
            _tableManager.CreateTables(_loadingGameSettings);
            _chestCoinManager.Initialize();
            _visitorsManager.Initialize(_loadingGameSettings);
            _barrelManager.Initialize();
            _barrelManager.CreatePoints();
            _beerManager.Initialize();
            _beerManager.CreatePoints();
            _tutorialManager.Initialize();
            _uiController.Initialize();
            /* if (!_loadingGameSettings.IsTutorialCompleted())
             {
                 _tutorialManager.StartTutorial();
             }*/
            //ДЛЯ ТЕСТА
            if (IsTutorial)
            {
                _tutorialManager.StartTutorial();
            }

        }

        private void Update()
        {
            if (Time.timeScale == 0f) return;
            _gameUIManager.UpdateUIInfo();
            if (Time.timeScale == 0f) return;
            _characterManager.MoveCharacter();
            _barrelManager.CheckHits();
            _chestCoinManager.CheckHits();
            _beerManager.CheckHits();
            _tableManager.CheckHits();
            _visitorsManager.UpdateState();
            CheckLevelCompletion();
        }

        private void CheckLevelCompletion()
        {
            if (_isLevelCompleted || _visitorsManager.RemainingVisitors > 0 ||
                _tableManager.HasUncollectedCoins || _chestCoinManager.HasUndeliveredCoins)
                return;

            float remainingSeconds = _gameUIManager.RemainingSeconds;
            if (remainingSeconds <= 0f)
                return;

            _isLevelCompleted = true;
            RemainingSecondsAtCompletion = remainingSeconds;
            _gameUIManager.StopTimer();
            int result = LevelRewards.Calculate(remainingSeconds, LevelDurationSeconds);
            int earned = LevelRewards.Grant(_loadingGameSettings.LevelNumber, result);
            _victoryPanel.Show(result, earned);
            Debug.Log($"Уровень пройден! Осталось {remainingSeconds:F2} из {LevelDurationSeconds:F2} секунд.");
        }
    }
}
