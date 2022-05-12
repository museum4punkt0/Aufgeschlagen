#if GUIDEPILOT_CORE_VIEWER3D

using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

namespace com.guidepilot.guidepilotcore
{
    [RequireComponent(typeof(ViewerTutorialController))]
    public class ViewerTutorialUIController : com.guidepilot.guidepilotcore.UIController, IPointerDownHandler, IPointerUpHandler
    {
        private ViewerTutorialController viewerTutorialController;

        [BoxGroup("Selectables & Buttons")]
        [SerializeField]
        private Button startButton;

        [BoxGroup("Selectables & Buttons")]
        [SerializeField]
        private Button nextButton;

        [BoxGroup("Selectables & Buttons")]
        [SerializeField]
        private Button stopButton;

        private Coroutine currentCoroutine;

        protected override void Awake()
        {
            base.Awake();

            viewerTutorialController = GetComponent<ViewerTutorialController>();

            //BasicInputController.ev_touch_started += OnTouchStarted;
            //BasicInputController.ev_touch_canceled += OnTouchEnded;

            viewerTutorialController.OnTutorialPartStartEvent += OnTutorialPartStart;
            viewerTutorialController.OnTutorialPartCompleteEvent += OnTutorialPartComplete;

            ViewerTutorialController.OnTutorialStartEvent += OnTutorialStart;
            ViewerTutorialController.OnTutorialStopEvent += OnTutorialStop;
            ViewerTutorialController.OnTutorialCompleteEvent += OnTutorialComplete;

            

            viewerTutorialController.OnTutorialControllerInitializedEvent += Initialize;

            if (startButton != null)
                startButton.onClick.AddListener(viewerTutorialController.StartTutorial);

            if (nextButton != null)
                nextButton.onClick.AddListener(viewerTutorialController.NextTutorialPart);

            if (stopButton != null)
                stopButton.onClick.AddListener(viewerTutorialController.StopTutorial);
        }

        private void OnDestroy()
        {
            //BasicInputController.ev_touch_started -= OnTouchStarted;
            //BasicInputController.ev_touch_canceled -= OnTouchEnded;

            viewerTutorialController.OnTutorialPartStartEvent -= OnTutorialPartStart;
            viewerTutorialController.OnTutorialPartCompleteEvent -= OnTutorialPartComplete;

            ViewerTutorialController.OnTutorialStartEvent -= OnTutorialStart;
            ViewerTutorialController.OnTutorialStopEvent -= OnTutorialStop;
            ViewerTutorialController.OnTutorialCompleteEvent -= OnTutorialComplete;

            

            viewerTutorialController.OnTutorialControllerInitializedEvent -= Initialize;

            if (startButton != null)
                startButton.onClick.RemoveListener(viewerTutorialController.StartTutorial);

            if (nextButton != null)
                nextButton.onClick.RemoveListener(viewerTutorialController.NextTutorialPart);

            if (stopButton != null)
                stopButton.onClick.RemoveListener(viewerTutorialController.StopTutorial);
        }

        private void Initialize(ViewerTutorialController controller)
        {
            List<SelectableConfig> selectables = new List<SelectableConfig>();

            foreach (var tutorialPart in viewerTutorialController.tutorialParts)
            {
                SelectableConfig selectableConfig = new SelectableConfig(tutorialPart.content.name, null);
                selectables.Add(selectableConfig);
            }

            UpdateButtons(selectables);
        }

        private void OnTutorialStart()
        {
            

            SetActiveWithDelay(true, 0.0f);
        }

        private void OnTouchStarted()
        {
            if (viewerTutorialController.tutorialActive && viewerTutorialController.currentTutorialPart.condition != ViewerTutorialController.TutorialPart.Condition.Tap)
            {
                SetActiveWithDelay(false, 0.25f);
            }
                
        }

        private void OnTouchEnded()
        {
            if (viewerTutorialController.tutorialActive && viewerTutorialController.currentTutorialPart.condition != ViewerTutorialController.TutorialPart.Condition.Tap)
            {
                SetActiveWithDelay(true, 0.0f);
            }
        }

        public void SetActiveWithDelay(bool status, float delay)
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            currentCoroutine = StartCoroutine(SetActiveWithDelayCoroutine(status, delay));
        }

        private IEnumerator SetActiveWithDelayCoroutine(bool status, float delay)
        {
            yield return new WaitForSeconds(delay);
            SetActive(status);
        }


        private void OnTutorialPartStart(ViewerTutorialController.TutorialPart tutorialPart)
        {
            if (tutorialPart.content == null) return;

            SetActiveWithDelay(true, 0.0f);

            if (tutorialPart.content.sprite != null)
                UpdateGraphic("Icon", tutorialPart.content.sprite);

            Toggle toggle = instantiatedSelectables.FirstOrDefault((x) => x.name == tutorialPart.content.name) as Toggle;
            toggle.isOn = true;

            UpdateTextDisplays(tutorialPart.content);
        }

        private void OnTutorialPartComplete(ViewerTutorialController.TutorialPart tutorialPart)
        {
            SetActiveWithDelay(false, 0.0f);
            //TODO
        }

        private void OnTutorialStop() => OnTutorialComplete();

        private void OnTutorialComplete()
        {
            SetActiveWithDelay(false, 0.0f);
        }

        public void OnPointerDown(PointerEventData eventData) => OnTouchStarted();

        public void OnPointerUp(PointerEventData eventData) => OnTouchEnded();
    }
}

#endif
