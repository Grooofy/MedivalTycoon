using Beers;
using MedivalTycoon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tables
{
    public class GridSpawner : MonoBehaviour
    {
        [SerializeField] private WayPointsCreater _wayPointsCreater;
        private Vector2 areaSize = new Vector2(2f, 3f);
        private Vector3 _prefabOffset = new Vector3(0.25f, 0.02f, 0.25f);
        private float spacing = 1f;

        private readonly List<ViewTable> _viewTables = new();
        private readonly List<Table> _tables = new();
        private List<GridCell> cells;

        private TableFactory _factory;
        private int _objectsToSpawn;
        private Vector3 _origin;

        public void Initialize(Table table, TableTrigger prefabTableZone, int objectsToSpawn)
        {
            _factory = new TableFactory(prefabTableZone, table);
            _objectsToSpawn = objectsToSpawn;
            _origin = transform.position;
        }



        public void SpawnGrid(ConstructionHandler handler, int[] startPrice, IPropsPool propsPool)
        {
            var entries = GridCalculator.GetGridEntries(_origin, areaSize, spacing, _prefabOffset);
            entries = entries.OrderBy(_ => Random.value).ToList(); 

            int spawnCount = Mathf.Min(_objectsToSpawn, entries.Count);

            for (int i = 0; i < spawnCount; i++)
            {
                var entry = entries[i];

                var way = _wayPointsCreater.CreatePoints(entry.cell);
                var (table, viewTable) = _factory.CreateTable(way,entry.position,transform,handler,startPrice[i],propsPool);
                _viewTables.Add(viewTable);
                _tables.Add(table);
            }
        }

        public void CheckHits()
        {
            foreach (var table in _tables)
                table.CheckHits();
        }
    }
}