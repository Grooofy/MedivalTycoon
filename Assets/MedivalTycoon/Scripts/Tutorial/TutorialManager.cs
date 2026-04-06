using UnityEngine;
using System;
using Events;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Tutorial
{
    public enum TutorialStep
    {
        Welcome,
        BuildTable,
        WaitVisitor,
        Complete
    }

    [Serializable]
    public struct TutorialStepData
    {
        public TutorialStep Step;
        public string Message;
        public Sprite Icon;
        public RectTransform TargetHighlight;
        public bool FullScreenOverlay;
    }

    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private TutorialSpotlight _spotlight;
        [SerializeField] private TutorialUI _tutorialUI;
        [SerializeField] private LoadingGameSettings _loadingGameSettings;
        [SerializeField] private Canvas _mainCanvas;

        [Header("Настройки шагов")]
        [SerializeField] private List<TutorialStepData> _stepsData;

        private TutorialStep _currentStep = TutorialStep.Welcome;

        public void Initialize()
        {
            _spotlight.Initialize(_mainCanvas);
        }

        public void StartTutorial()
        {
            _currentStep = TutorialStep.Welcome;
            ShowStep();
        }

        private void ShowStep()
        {
            TutorialStepData data = _stepsData.Find(s => s.Step == _currentStep);
            
            if (string.IsNullOrEmpty(data.Message) && _currentStep != TutorialStep.Complete)
            {
                Debug.LogWarning($"Tutorial: No data found for step {_currentStep}");
            }

            // Включаем логику шага (подсветку или оверлей) СРАЗУ
            ProcessStepLogic(data);

            if (!string.IsNullOrEmpty(data.Message))
            {
                _tutorialUI.ShowMessage(data.Message, () => 
                {
                    _tutorialUI.Close();
                    
                    // Шаг Welcome всегда переходит к следующему по нажатию кнопки "Далее"
                    if (_currentStep == TutorialStep.Welcome)
                    {
                        NextStep();
                    }
                }, data.Icon);
            }
        }

        private void ProcessStepLogic(TutorialStepData data)
        {
            if (data.FullScreenOverlay)
            {
                _spotlight.ShowFullScreen();
            }
            else if (data.TargetHighlight != null)
            {
                _spotlight.ShowSpotlight(data.TargetHighlight);
            }
            else
            {
                _spotlight.HideSpotlight();
            }

            switch (_currentStep)
            {
                case TutorialStep.BuildTable:
                    EventBus.Subscribe<TableBuilt>(OnTableBuilt);
                    break;

                case TutorialStep.WaitVisitor:
                    EventBus.Subscribe<VisitorLeaveTavern>(OnVisitorLeave);
                    break;
            }
        }

        private void OnTableBuilt(TableBuilt data)
        {
            EventBus.Unsubscribe<TableBuilt>(OnTableBuilt);
            _spotlight.HideSpotlight();
            NextStep();
        }

        private void OnVisitorLeave(VisitorLeaveTavern data)
        {
            EventBus.Unsubscribe<VisitorLeaveTavern>(OnVisitorLeave);
            _spotlight.HideSpotlight();
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
