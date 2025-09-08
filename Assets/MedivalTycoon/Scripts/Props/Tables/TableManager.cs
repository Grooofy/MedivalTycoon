using UnityEngine;

namespace Tables
{
    public class TableManager : MonoBehaviour
    {
        [SerializeField] private GridSpawner _gridSpawner;
        [SerializeField] private Table _table;
        [SerializeField] private TableTrigger _tableTrigger;
        [SerializeField] private ConstructionHandler _constructionHandler;
        [SerializeField] private Wallet _wallet;

        public void Initialize(LoadingGameSettings loadingGameSettings)
        {
            _gridSpawner.Initialize(_table, _tableTrigger, loadingGameSettings.GetTableAmount());
            _constructionHandler.Initialize(_wallet);
        }

        public void CreateTables(LoadingGameSettings loadingGameSettings)
        {
            if(_constructionHandler == null) Debug.LogError("ConstructionHandler == null");
            
            _gridSpawner.SpawnGrid(_constructionHandler, loadingGameSettings.GetTableCost());
        }

        public void CheckHits()
        {
            _gridSpawner.CheckHits();
        }
    }
}