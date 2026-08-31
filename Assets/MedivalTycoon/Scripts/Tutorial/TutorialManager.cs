using UnityEngine;
using System;
using Events;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using Money;
using Beers;


namespace Tutorial
{
    public enum TutorialStep
    {
        Welcome,
        Characters,
        ShowUITimer,
        CreateBarrel,
        TakeBarrel,
        MoveBarrel,
        ShowUIVisitorAmount,
        BuildTable,
        WaitVisitor,
        SelectedWaiter,
        CreateBeer,
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
        LastStep,
        Complete

    }

    [Serializable]
    public struct TutorialStepData
    {
        public TutorialStep Step;
        public string Message;
        public Sprite Icon;
        public RectTransform TargetHighlight;
        public Button TargetButton;
        public bool FullScreenOverlay;
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
        private bool _pointerActiveStep = false;
        private Transform _pointerTarget = null;
        private Coroutine _popupCoroutine = null;
        private Transform _popupTarget = null;
        private Vector3 _popupOriginalScale = Vector3.one;
        private Button _subscribedButton = null;
        private UnityAction _subscribedAction = null;

        public void Initialize()
        {
            _spotlight.Initialize(_mainCanvas);
            if (_pointer != null) _pointer.Initialize(_mainCanvas);
            // Подписка будет выполняться для текущего шага при его показе
            _switcherSelectedCharacter.Activate += OnCharacterSelected;
            EventBus.Subscribe<TutorialStepCompleted>(OnTutorialStepCompleted);
        }

        private void OnDisable()
        {
            if (_switcherSelectedCharacter != null)
                _switcherSelectedCharacter.Activate -= OnCharacterSelected;

            EventBus.Unsubscribe<TutorialStepCompleted>(OnTutorialStepCompleted);
            ClearSubscribedButton();
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

            ProcessStepLogic(data);

            if (!string.IsNullOrEmpty(data.Message))
            {
                var autoAdvanceSteps = new HashSet<TutorialStep>
                {
                    TutorialStep.Welcome,
                    TutorialStep.Characters,
                    TutorialStep.ShowUITimer,
                    TutorialStep.ShowUIVisitorAmount,
                    TutorialStep.ShowUIMoneyAmout
                };

                _tutorialUI.ShowMessage(data.Message, () =>
                {
                    _tutorialUI.Close();
                    if (data.TargetButton != null || autoAdvanceSteps.Contains(_currentStep))
                    {
                        NextStep();
                    }
                }, data.Icon);
            }
        }

        private void ProcessStepLogic(TutorialStepData data)
        {
            if (data.Step == TutorialStep.BuildTable || data.Step == TutorialStep.TakeVisitor && data.PathTarget == null)
            {
                var trigger = FindObjectOfType<TableTrigger>();

                if (trigger != null)
                {
                    int idx = _stepsData.FindIndex(s => s.Step == _currentStep);
                    if (idx >= 0)
                    {
                        var newData = _stepsData[idx];
                        newData.PathTarget = trigger.transform;
                        _stepsData[idx] = newData;
                        data = newData;
                    }
                }
            }

            if ((data.Step == TutorialStep.ServesesVisitor || data.Step == TutorialStep.TakeMoney) && data.PathTarget == null)
            {

                var table = FindObjectOfType<TableTrigger>();
                Component trigger = table?.GetComponentInChildren<BeerTaker>();
                
                if(trigger == null) trigger = table?.GetComponentInChildren<CoinTaker>();

                if (trigger != null)
                {
                    int idx = _stepsData.FindIndex(s => s.Step == _currentStep);
                    if (idx >= 0)
                    {
                        var newData = _stepsData[idx];
                        newData.PathTarget = trigger.transform;
                        _stepsData[idx] = newData;
                        data = newData;
                    }
                }
            }

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
            // Подписываемся только на кнопку текущего шага
            SetSubscribedButton(data.TargetButton);

            // Запускаем анимацию "попап" для целевой кнопки
            StopPopupAnimation();
            if (data.TargetButton != null)
            {
                _popupTarget = data.TargetButton.transform;
                _popupOriginalScale = _popupTarget.localScale;
                _popupCoroutine = StartCoroutine(PopupAnimation(_popupTarget));
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
        }

        private void OnCharacterSelected(int id)
        {
            if (_currentStep == TutorialStep.SelectedWaiter && id == 1) NextStep();
            else if (_currentStep == TutorialStep.SelectedSecuryte && id == 2) NextStep();

            if (_pointerActiveStep && _pointerTarget != null && _pointer != null)
            {
                var current = _switcherSelectedCharacter.CurrentCharacter;
                if (current != null)
                {
                    _pointer.ShowPointer(current, _pointerTarget);
                }
            }
        }

        private void OnAnyButtonClicked(Button clicked)
        {
            if (_stepsData == null) return;
            TutorialStepData data = _stepsData.Find(s => s.Step == _currentStep);
            if (data.TargetButton == clicked)
            {
                StopPopupAnimation();
                NextStep();
            }
        }
        private void SetSubscribedButton(Button btn)
        {
            // Снимаем старую подписку
            ClearSubscribedButton();

            if (btn == null) return;

            _subscribedButton = btn;
            _subscribedAction = () => OnAnyButtonClicked(btn);
            _subscribedButton.onClick.AddListener(_subscribedAction);
        }

        private void ClearSubscribedButton()
        {
            if (_subscribedButton != null && _subscribedAction != null)
            {
                _subscribedButton.onClick.RemoveListener(_subscribedAction);
            }
            _subscribedButton = null;
            _subscribedAction = null;
        }

        private void StopPopupAnimation()
        {
            if (_popupCoroutine != null)
            {
                StopCoroutine(_popupCoroutine);
                _popupCoroutine = null;
            }
            if (_popupTarget != null)
            {
                _popupTarget.localScale = _popupOriginalScale;
                _popupTarget = null;
                _popupOriginalScale = Vector3.one;
            }
        }

        private IEnumerator PopupAnimation(Transform target)
        {
            if (target == null) yield break;

            Vector3 originalScale = _popupOriginalScale;

            float upScale = 1.15f;
            float duration = 0.35f;

            while (true)
            {
                // scale up
                float t = 0f;
                while (t < duration)
                {
                    t += Time.deltaTime;
                    float v = Mathf.SmoothStep(1f, upScale, t / duration);
                    if (target == null) yield break;
                    target.localScale = originalScale * v;
                    yield return null;
                }

                // small pause
                yield return new WaitForSeconds(0.12f);

                // scale down
                t = 0f;
                while (t < duration)
                {
                    t += Time.deltaTime;
                    float v = Mathf.SmoothStep(upScale, 1f, t / duration);
                    if (target == null) yield break;
                    target.localScale = originalScale * v;
                    yield return null;
                }

                // pause before next pulse
                yield return new WaitForSeconds(0.6f);
            }
        }

        private void OnTutorialStepCompleted(TutorialStepCompleted data)
        {
            if (data.Step == _currentStep)
            {
                _spotlight.HideSpotlight();
                if (_pointer != null) _pointer.HidePointer();
                NextStep();
            }
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
