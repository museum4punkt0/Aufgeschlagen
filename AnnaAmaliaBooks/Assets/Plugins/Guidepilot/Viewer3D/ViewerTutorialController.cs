#if GUIDEPILOT_CORE_VIEWER3D

using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

namespace com.guidepilot.guidepilotcore
{
    public class ViewerTutorialController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [System.Serializable]

        public class TutorialPart
        {
            public enum Condition { Tap, DoubleTap, OneFingerDrag, TwoFingerDrag, Pinch }
            public enum ConditionRule { OneAfterAnother }

            [GUIColor("color")]
            public UnityContent content;
            [GUIColor("color")]
            public Condition condition;
            [GUIColor("color")]
            public ConditionRule conditionRule;
            [GUIColor("color")]
            public float delay;
            [HideInInspector]
            public bool isFullfilled = false;
            [GUIColor("color")]
            public Color color { get => isFullfilled ? Color.green : Color.red; }

            public UnityAction<BasicInputController.InputEventData> inputAction;
            //public List<Condition> completeConditions = new List<Condition>();
        }

        public enum Status { None, Playing, Awaiting, Completed }

        [EnumToggleButtons]
        [ReadOnly]
        public Status currentStatus;

        public bool startTutorialOnStart = false;
        [Range(0.0f, 3.0f)]
        public float tutorialDelay = 1.0f;

        public UnityAction<ViewerTutorialController> OnTutorialControllerInitializedEvent;
        public UnityAction<TutorialPart> OnTutorialPartStartEvent;
        public UnityAction<TutorialPart> OnTutorialPartCompleteEvent;

        public static UnityAction OnTutorialStartEvent;
        public static UnityAction OnTutorialStopEvent;
        public static UnityAction OnTutorialCompleteEvent;

        [TableList(AlwaysExpanded = true)]
        public List<TutorialPart> tutorialParts = new List<TutorialPart>();

        [ReadOnly]
        public TutorialPart currentTutorialPart;
        private Coroutine currentTutorialCoroutine;

        private UserInput controls;
        private bool userInputActive;

        private bool userSkipped = false;

        // TODO: Use Input System instead
        private UnityAction<BasicInputController.InputEventData> OnPointerDownEvent;
        private UnityAction<BasicInputController.InputEventData> OnPointerUpEvent;

        public bool tutorialActive { get => currentStatus == Status.Awaiting || currentStatus == Status.Playing; }

        private void Awake()
        {
            ViewerSceneController.OnInitializationCompleteEvent += OnViewerInitializationComplete;

            //BasicInputController.ev_tap_started += OnTapStarted;
            //BasicInputController.ev_tap_canceled += OnTapCanceled;
        }

        private void OnDestroy()
        {
            ViewerSceneController.OnInitializationCompleteEvent -= OnViewerInitializationComplete;

            //BasicInputController.ev_tap_started -= OnTapStarted;
            //BasicInputController.ev_tap_canceled -= OnTapCanceled;
        }

        private void Start()
        {
            if (startTutorialOnStart)
                StartTutorial();
        }

        [ButtonGroup]
        [Button("Start", ButtonSizes.Large)]
        public void StartTutorial()
        {
            if (currentStatus == Status.Playing || currentStatus == Status.Awaiting) return;

            foreach (TutorialPart tutorialPart in tutorialParts)
            {
                tutorialPart.isFullfilled = false;
            }

            Debug.Log($"{typeof(ViewerTutorialController)}: Started Tutorial.");

            OnTutorialStartEvent?.Invoke();

            currentTutorialCoroutine = StartCoroutine(TutorialCoroutine());

            currentStatus = Status.Playing;
        }

        [ButtonGroup]
        [Button("Stop", ButtonSizes.Large)]
        public void StopTutorial()
        {
            if (currentTutorialCoroutine != null)
                StopCoroutine(currentTutorialCoroutine);

            if (currentTutorialPart != null)
            {
                Unsubscribe(currentTutorialPart);
                currentTutorialPart = null;
            }

            currentStatus = Status.None;

            Debug.Log($"{typeof(ViewerTutorialController)}: Stopped Tutorial.");

            OnTutorialStopEvent?.Invoke();
        }

        [ButtonGroup()]
        [Button("Next", ButtonSizes.Large)]
        public void NextTutorialPart()
        {
            if (currentStatus == Status.None || currentStatus == Status.Completed) return;

            currentTutorialPart.isFullfilled = true;
            userSkipped = true;
            //TutorialPart nextTutorialPart = tutorialParts[tutorialParts.IndexOf(currentTutorialPart) + 1];
        }



        private IEnumerator TutorialCoroutine()
        {
            foreach (TutorialPart tutorialPart in tutorialParts)
            {
                yield return StartCoroutine(TutorialPartCoroutine(tutorialPart));
            }

            currentStatus = Status.Completed;

            Debug.Log($"{typeof(ViewerTutorialController)}: Tutorial completed!");

            OnTutorialCompleteEvent?.Invoke();
        }

        private IEnumerator TutorialPartCoroutine(TutorialPart tutorialPart)
        {
            float delay = (userSkipped) ? 0.0f : tutorialPart.delay;
            userSkipped = false;

            yield return new WaitForSeconds(delay);

            Debug.Log($"{typeof(ViewerTutorialController)}: Started Tutorial Part {tutorialParts.IndexOf(tutorialPart)}.");

            currentTutorialPart = tutorialPart;
            OnTutorialPartStartEvent?.Invoke(tutorialPart);

            Subscribe(tutorialPart);

            yield return new WaitUntil(() => tutorialPart.isFullfilled && !userInputActive);

            Unsubscribe(tutorialPart);

            OnTutorialPartCompleteEvent?.Invoke(tutorialPart);

            //yield return null;

            Debug.Log($"{typeof(ViewerTutorialController)}: Completed Tutorial Part {tutorialParts.IndexOf(tutorialPart)}.");
        }

        private void Subscribe(TutorialPart tutorialPart)
        {
            tutorialPart.inputAction = (BasicInputController.InputEventData ev) => tutorialPart.isFullfilled = true;

            switch (tutorialPart.condition)
            {
                case TutorialPart.Condition.Tap:
                    // TODO: Use Input System instead
                    OnPointerUpEvent += tutorialPart.inputAction;
                    break;
                case TutorialPart.Condition.DoubleTap:
                    BasicInputController.ev_doubletap_started += tutorialPart.inputAction;
                    break;
                case TutorialPart.Condition.OneFingerDrag:
                    BasicInputController.ev_drag_started += tutorialPart.inputAction;
                    break;
                case TutorialPart.Condition.TwoFingerDrag:
                    BasicInputController.ev_twofingerdrag_started += tutorialPart.inputAction;
                    break;
                case TutorialPart.Condition.Pinch:
                    BasicInputController.ev_pinch_started += tutorialPart.inputAction;
                    break;
                default:
                    break;
            }


        }

        private void Unsubscribe(TutorialPart tutorialPart)
        {
            switch (tutorialPart.condition)
            {
                case TutorialPart.Condition.Tap:
                    // TODO: Use Input System instead
                    OnPointerUpEvent -= tutorialPart.inputAction;
                    break;
                case TutorialPart.Condition.DoubleTap:
                    BasicInputController.ev_doubletap_started -= tutorialPart.inputAction;
                    break;
                case TutorialPart.Condition.OneFingerDrag:
                    BasicInputController.ev_drag_started -= tutorialPart.inputAction;
                    break;
                case TutorialPart.Condition.TwoFingerDrag:
                    BasicInputController.ev_twofingerdrag_started -= tutorialPart.inputAction;
                    break;
                case TutorialPart.Condition.Pinch:
                    BasicInputController.ev_pinch_started -= tutorialPart.inputAction;
                    break;
                default:
                    break;
            }
        }

        private void OnViewerInitializationComplete(ViewerSceneController controller)
        {
            tutorialParts = controller.configuration.tutorialParts;

            OnTutorialControllerInitializedEvent?.Invoke(this);
        }

        //private void OnTapStarted() => userInputActive = true;

        //private void OnTapCanceled() => userInputActive = false;

        // TODO: Use Input System instead
        public void OnPointerDown(PointerEventData eventData)
        {
            userInputActive = true;
            OnPointerDownEvent?.Invoke(new BasicInputController.InputEventData());
        }

        // TODO: Use Input System instead
        public void OnPointerUp(PointerEventData eventData)
        {
            userInputActive = false;
            OnPointerUpEvent?.Invoke(new BasicInputController.InputEventData());
        }
    }
}

#endif
