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
        Characters,        
        ShowUITimer,
        CreateBarrel,
        TakeBarrel,
        
        ShowUIVisitorAmount,
        BuildTable,
        WaitVisitor,
        Visitor,    
        SelectedWaiter,
        TakeBeer,
        ServesesVisitor,
        GiveBeer,
        TakeMoney,
        GiveMoney,
        ShowUIMoneyAmout,
        SleepVisitors,
        SelectedSecuryte,
        TakeVisitor,
        GiveVisitor,
        Complete
    }

    [Serializable]
    public struct TutorialStepData
    {
        public TutorialStep Step;
        public string Message;
        public Sprite Icon;
        public RectTransform TargetHighlight;
        public Button TargetButton; // Кнопка для завершения шага
        public bool FullScreenOverlay;
        // Для указателя пути: цель в мировых координатах и флаг отображения
        public Transform PathTarget;
        public bool ShowPathFromSelected;
    }

    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private TutorialSpotlight _spotlight;
        [SerializeField] private TutorialPointer _pointer;
        [SerializeField] private TutorialUI _tutorialUI;
        [SerializeField] private LoadingGameSettings _loadingGameSettings;
        [SerializeField] private Canvas _mainCanvas;

        [SerializeField] private Characters.SwitcherSelectedCharacter _switcherSelectedCharacter;
        [SerializeField] private Wallet _wallet;

        [Header("Настройки шагов")]
        [SerializeField] private List<TutorialStepData> _stepsData;
        private TutorialStep _currentStep = TutorialStep.Welcome;
        // Fields used for path pointer step
        private bool _pointerActiveStep = false;
        private Transform _pointerTarget = null;

        public void Initialize()
        {
            _spotlight.Initialize(_mainCanvas);
            if (_pointer != null) _pointer.Initialize(_mainCanvas);
            _switcherSelectedCharacter.Activate += OnCharacterSelected;            
            _wallet.CoinsChanged += OnCoinsChanged;
            // Подписываемся на событие завершения шага, чтобы внешний код мог его триггерить через EventBus
            EventBus.Subscribe<Events.TutorialStepCompleted>(OnTutorialStepCompleted);
        }

        private void OnDisable()
        {
            if (_switcherSelectedCharacter != null)
                _switcherSelectedCharacter.Activate -= OnCharacterSelected;                
            
            if (_wallet != null)
                _wallet.CoinsChanged -= OnCoinsChanged;
            EventBus.Unsubscribe<Events.TutorialStepCompleted>(OnTutorialStepCompleted);
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
                    
                    // Шаги, которые переходят по нажатию кнопки "Далее" в UI
                    if (_currentStep == TutorialStep.Welcome || 
                        _currentStep == TutorialStep.Characters ||
                        _currentStep == TutorialStep.ShowUITimer ||
                        _currentStep == TutorialStep.ShowUIVisitorAmount ||
                        _currentStep == TutorialStep.ShowUIMoneyAmout)
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
                if (_spotlight.gameObject.activeSelf)
                {
                    _spotlight.MoveSpotlight(data.TargetHighlight);
                }
                else
                {
                    _spotlight.ShowSpotlight(data.TargetHighlight);
                }
            }
            else
            {
                _spotlight.HideSpotlight();
            }
            // Подписка на кнопку, если она указана
            if (data.TargetButton != null)
            {
                data.TargetButton.onClick.AddListener(OnTargetButtonClicked);
            }

            // Обработка указателя пути
            if (data.ShowPathFromSelected && data.PathTarget != null)
            {
                _pointerActiveStep = true;
                _pointerTarget = data.PathTarget;
                if (_switcherSelectedCharacter.CurrentCharacter != null && _pointer != null)
                {
                    _pointer.ShowPointer(_switcherSelectedCharacter.CurrentCharacter, _pointerTarget);
                }
            }
            else
            {
                _pointerActiveStep = false;
                _pointerTarget = null;
                if (_pointer != null) _pointer.HidePointer();
            }

            switch (_currentStep)
            {
                case TutorialStep.CreateBarrel:
                    EventBus.Subscribe<BeerCreated>(OnBeerCreated);
                    break;

                case TutorialStep.TakeBarrel:
                    EventBus.Subscribe<BeerBufferOpen>(OnBeerBufferOpen);
                    break;

                case TutorialStep.BuildTable:
                    EventBus.Subscribe<TableBuilt>(OnTableBuilt);
                    break;

                case TutorialStep.WaitVisitor:
                    EventBus.Subscribe<VisitorLeaveTavern>(OnVisitorLeave);
                    break;
                
                case TutorialStep.Visitor:
                    EventBus.Subscribe<SeatTaken>(OnSeatTaken);
                    break;

                case TutorialStep.SleepVisitors:
                    EventBus.Subscribe<SeatFreed>(OnSeatFreed);                    
                    break;
            }
        }

        private void OnCharacterSelected(int id)
        {
            // id: 0 - Бармен, 1 - Официант, 2 - Охранник
            if (_currentStep == TutorialStep.SelectedWaiter && id == 1) NextStep();            
            else if (_currentStep == TutorialStep.SelectedSecuryte && id == 2) NextStep();

            // Обновляем указатель, если шаг требует отображения пути
            if (_pointerActiveStep && _pointerTarget != null && _pointer != null)
            {
                var current = _switcherSelectedCharacter.CurrentCharacter;
                if (current != null)
                {
                    _pointer.ShowPointer(current, _pointerTarget);
                }
            }
        }

        private void OnCoinsChanged(int amount)
        {
            if (_currentStep == TutorialStep.TakeMoney || _currentStep == TutorialStep.GiveMoney)
            {
                NextStep();
            }
        }

        private void OnTargetButtonClicked()
        {
            TutorialStepData data = _stepsData.Find(s => s.Step == _currentStep);
            if (data.TargetButton != null)
            {
                data.TargetButton.onClick.RemoveListener(OnTargetButtonClicked);
            }
            NextStep();
        }

        private void OnBeerCreated(BeerCreated data)
        {
            EventBus.Unsubscribe<BeerCreated>(OnBeerCreated);
            NextStep();
        }

        private void OnBeerBufferOpen(BeerBufferOpen data)
        {
            EventBus.Unsubscribe<BeerBufferOpen>(OnBeerBufferOpen);
            NextStep();
        }

        private void OnSeatTaken(SeatTaken data)
        {
            EventBus.Unsubscribe<SeatTaken>(OnSeatTaken);
            NextStep();
        }

        private void OnSeatFreed(SeatFreed data)
        {
            EventBus.Unsubscribe<SeatFreed>(OnSeatFreed);
            NextStep();
        }

        private void OnTableBuilt(TableBuilt data)
        {
            EventBus.Unsubscribe<TableBuilt>(OnTableBuilt);
            _spotlight.HideSpotlight();
            if (_pointer != null) _pointer.HidePointer();
            NextStep();
        }

        private void OnTutorialStepCompleted(Events.TutorialStepCompleted data)
        {
            // Защита: только если событие для текущего шага
            if (data.Step == _currentStep)
            {
                // Снимаем подсветки/указатель и переходим дальше
                _spotlight.HideSpotlight();
                if (_pointer != null) _pointer.HidePointer();
                NextStep();
            }
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
