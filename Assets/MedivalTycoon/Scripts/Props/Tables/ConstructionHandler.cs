using System.Collections;
using UnityEngine;

namespace Tables
{
    public class ConstructionHandler : MonoBehaviour
    {
        private Wallet _wallet;
        private float _stepCooldown = 0.003f;
        private int _step = 1;

        private Coroutine _buildRoutine;

        public void Initialize(Wallet wallet)
        {
            _wallet = wallet;
        }

        public void StartBuilding(Table table)
        {
            if (_buildRoutine == null)
                _buildRoutine = StartCoroutine(BuildRoutine(table));
        }

        public void StopBuilding()
        {
            if (_buildRoutine != null)
            {
                StopCoroutine(_buildRoutine);
                _buildRoutine = null;
            }
        }

        private IEnumerator BuildRoutine(Table table)
        {
            while (!table.IsBuilt)
            {
                if (_wallet.TryRemoveCoin(_step))
                {
                    _wallet.StartRemoveCoins(1, _step);
                    table.ReducePrice(_step);
                }

                yield return new WaitForSeconds(_stepCooldown);
            }

            StopBuilding();
        }
    }
}