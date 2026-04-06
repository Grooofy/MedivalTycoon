using UnityEngine;
using System;
using Events;

namespace Tutorial
{
    public enum TutorialStep
    {
        Welcome,
        BuildTable,
        WaitVisitor,
        Complete
    }

    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private TutorialSpotlight _spotlight;
        [SerializeField] private LoadingGameSettings _loadingGameSettings;
        private TutorialStep _currentStep = TutorialStep.Welcome;

        public void StartTutorial()
        {
            _currentStep = TutorialStep.Welcome;
            ShowStep();
        }

        private void ShowStep()
        {
            switch (_currentStep)
            {
                case TutorialStep.Welcome:
                    Debug.Log("Tutorial: Welcome!");
                    // В реальном проекте здесь будет вызов UI окна
                    // Для теста перейдем к следующему шагу через 2 секунды
                    Invoke(nameof(NextStep), 2f);
                    break;
                case TutorialStep.BuildTable:
                    Debug.Log("Tutorial: Build a table!");
                    EventBus.Subscribe<TableBuilt>(OnTableBuilt);
                    break;
                case TutorialStep.WaitVisitor:
                    Debug.Log("Tutorial: Wait for visitor to leave!");
                    EventBus.Subscribe<VisitorLeaveTavern>(OnVisitorLeave);
                    break;
            }
        }

        private void OnTableBuilt(TableBuilt data)
        {
            EventBus.Unsubscribe<TableBuilt>(OnTableBuilt);
            NextStep();
        }

        private void OnVisitorLeave(VisitorLeaveTavern data)
        {
            EventBus.Unsubscribe<VisitorLeaveTavern>(OnVisitorLeave);
            NextStep();
        }

        private void NextStep()
        {
            _currentStep++;
            if (_currentStep == TutorialStep.Complete)
            {
                FinishTutorial();
            }
            else
            {
                ShowStep();
            }
        }

        private void FinishTutorial()
        {
            Debug.Log("Tutorial: Completed!");
            _loadingGameSettings.SaveTutorialStatus(true);
        }
    }
}
