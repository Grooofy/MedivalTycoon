using UnityEngine;

namespace UI
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private Wallet _wallet;
        [SerializeField] private Timer _timer;
        [SerializeField] private MoneyUI _moneyUI;
        [SerializeField] private VisitorsUI _visitorsUI;


        public void ShowUIInfo(LoadingGameSettings loadingGameSettings)
        {
            _wallet.Initialize(loadingGameSettings);
            _timer.Initialize(loadingGameSettings);
            _visitorsUI.Initialize(loadingGameSettings);
            _moneyUI.Initialize(_wallet);
        }

        public void UpdateUIInfo()
        {
            _timer.UpdateTimer();
        }
    }
}