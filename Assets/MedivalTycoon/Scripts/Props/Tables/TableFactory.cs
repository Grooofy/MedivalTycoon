using MedivalTycoon;
using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

namespace Tables
{
    public class TableFactory 
    {

        private TableTrigger _triggerPrefab;
        private Table _tablePrefab;

        public TableFactory(TableTrigger triggerPrefab, Table tablePrefab)
        {
            _triggerPrefab = triggerPrefab;
            _tablePrefab = tablePrefab;
        }

        public (Table table, ViewTable view) CreateTable(Queue<Point> wayPoint, Vector3 position, Transform parent, ConstructionHandler handler, int startPrice, IPropsPool propsPool, PropsSpawner propsSpawner)
        {
            var triggerZone = Object.Instantiate(_triggerPrefab, position, Quaternion.identity, parent);

            var viewTable = triggerZone.Initialize(handler);
            var tableBuilderAnimation = triggerZone.GetComponent<TableBuilderAnimation>();
            var table = triggerZone.CreateTable(_tablePrefab);

            tableBuilderAnimation.Initialize();
            table.Initialize(startPrice, wayPoint);
            table.InitializeBeerTaker();  
            table.InitializeCoinManager(propsSpawner);
            table.InitializeSeatSystem(propsPool);
            viewTable.Initialize(table, tableBuilderAnimation);

            return (table, viewTable);
        }
    }
}