using Barrels;
using Characters;
using Tables;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Visitors;

namespace MedivalTycoon
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private CharacterManager _characterManager;
        [SerializeField] private LoadingGameSettings _loadingGameSettings;
        [SerializeField] private GameUIManager _gameUIManager;
        [SerializeField] private TableManager _tableManager;
        [SerializeField] private BarrelManager _barrelManager;
        [SerializeField] private QueueVisitor queueVisitor;
        
        
        private void Start()
        {
            _loadingGameSettings.Load();
            _characterManager.CreateCharacters();
            _gameUIManager.ShowUIInfo(_loadingGameSettings);
            _tableManager.Initialize(_loadingGameSettings);
            _tableManager.CreateTables(_loadingGameSettings);
            _barrelManager.Initialize();
            _barrelManager.CreatePoints();
            queueVisitor.Initialize(10, 0.5f, 0.2f,10);
            queueVisitor.SpawnVisitorsInLine(queueVisitor.transform.position);
        }

        private void Update()
        {
            _gameUIManager.UpdateUIInfo();
            _characterManager.MoveCharacter();
            _barrelManager.CheckHits();
            queueVisitor.UpdateState();
        }
    }
}