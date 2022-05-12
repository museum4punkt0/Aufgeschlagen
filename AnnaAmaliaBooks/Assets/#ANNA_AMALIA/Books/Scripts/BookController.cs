using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using com.guinealion.animatedBook;
using com.guidepilot.guidepilotcore;
using DG.Tweening;
using System;

namespace Guidepilot.KSW.FliegendeBuecher
{
    public class BookController : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        [BoxGroup("General")]
        public UnityContent content;

        [BoxGroup("Settings")]
        public bool enableBookToggle = true;

        [BoxGroup("Settings")]
        public bool enablePageSwiping = true;

        [BoxGroup("Settings")]
        [Range(0, 1)]
        [SerializeField]
        private float maxOpenAmount = 1.0f;

        [BoxGroup("References")]
        [SerializeField]
        private LightweightBookHelper bookHelper;

        //[BoxGroup("References")]
        //[SerializeField]
        //private SkinnedMeshRenderer skinnedMeshRenderer;

        [BoxGroup("References")]
        [SerializeField]
        private BoxCollider openBookCollider;

        [BoxGroup("References")]
        [SerializeField]
        private BoxCollider closedBookCollider;

        //[BoxGroup("References")]
        //[SceneObjectsOnly]
        //[SerializeField]
        //private Transform openBookTransform;
        //
        //[BoxGroup("References")]
        //[SceneObjectsOnly]
        //[SerializeField]
        //private Transform closedBookTransform;

        [BoxGroup("Animation")]
        [SerializeField]
        private Ease bookAppearEase = Ease.OutBack;

        [BoxGroup("Animation")]
        [SerializeField]
        private float bookAppearDuration = 0.5f;

        [BoxGroup("Animation")]
        [SerializeField]
        private Ease bookDisappearEase = Ease.OutSine;

        [BoxGroup("Animation")]
        [SerializeField]
        private float bookDisappearDuration = 0.5f;

        [BoxGroup("Animation")]
        [SerializeField]
        private float pageFlipDuration = 0.5f;

        [BoxGroup("Animation")]
        [SerializeField]
        private Ease bookOpenEase;

        [BoxGroup("Animation")]
        [SerializeField]
        private float bookOpenDuration = 0.5f;

        [BoxGroup("Animation")]
        [SerializeField]
        private Ease bookCloseEase;

        [BoxGroup("Animation")]
        [SerializeField]
        private float bookCloseDuration = 0.5f;

        [BoxGroup("Follow Camera")]
        [SerializeField]
        private bool followCamera;

        public bool isVisible { get; private set; }



        public static UnityAction<BookController> OnBookControllerAwakeEvent;
        public static UnityAction<BookController> OnPointerDownEvent;
        public static UnityAction<BookController> OnPointerUpEvent;

        [FoldoutGroup("Events")]
        public UnityEvent OnBookAppearEvent = new UnityEvent();

        [FoldoutGroup("Events")]
        public UnityEvent OnBookDisappearEvent = new UnityEvent();

        [FoldoutGroup("Events")]
        public UnityEvent OnBookOpenEvent = new UnityEvent();

        [FoldoutGroup("Events")]
        public UnityEvent OnBookClosedEvent = new UnityEvent();

        [FoldoutGroup("Debug")]
        [ShowInInspector]
        [ReadOnly]
        private bool bookIsOpen { get => (bookHelper != null && bookHelper.OpenAmmount > 0); }

        private ViewerSceneController sceneController;
        private ViewerCameraController cameraController;
        private ViewerCameraControllerTarget cameraTarget;

        private Quaternion originalRotation;
        private Vector3 originalScale;
        private Vector3 originalPosition;

        private FollowCamera followCameraController;

        private float progressOnDragStart;
        private Vector2 pointerPositionOnDragStart;

        private void Awake()
        {
            BasicInputController.ev_doubletap_started += OnDoubleTouch;
            BasicInputController.ev_drag_started += OnDragStarted;
            BasicInputController.ev_drag_performed += OnDragPerformed;
            BasicInputController.ev_drag_canceled += OnDragCanceled;

            if (openBookCollider != null)
                openBookCollider.enabled = (bookIsOpen);

            if (closedBookCollider != null)
                closedBookCollider.enabled = (!bookIsOpen);

            //bookRootTransform.position = (bookIsOpen) ? openBookTransform.position : closedBookTransform.position;

            sceneController = FindObjectOfType<ViewerSceneController>();
            cameraController = FindObjectOfType<ViewerCameraController>();
            cameraTarget = GetComponent<ViewerCameraControllerTarget>();
            followCameraController = GetComponent<FollowCamera>();

            if (followCameraController != null)
                followCameraController.enabled = followCamera;

            if (cameraTarget != null)
                cameraTarget.collider = (bookIsOpen) ? openBookCollider : closedBookCollider;

            originalScale = transform.localScale;
            originalPosition = transform.position;
            originalRotation = transform.rotation;

            //if (skinnedMeshRenderer != null)
            //    skinnedMeshRenderer.forceMatrixRecalculationPerRender = true;

            OnBookControllerAwakeEvent?.Invoke(this);
        }



        private void OnDestroy()
        {
            BasicInputController.ev_doubletap_started -= OnDoubleTouch;
            BasicInputController.ev_drag_started -= OnDragStarted;
            BasicInputController.ev_drag_performed -= OnDragPerformed;
            BasicInputController.ev_drag_canceled -= OnDragCanceled;
        }



        private void Start()
        {
            //Appear();
        }

        private void Update()
        {
            if (followCameraController != null)
                followCameraController.enabled = followCamera;

            //if (Input.GetMouseButtonDown(0))
            //{
            //    previousMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            //}
            //
            //if (Input.GetMouseButton(0))
            //{
            //    Vector3 currentMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            //
            //    Vector3 mousePosDelta = currentMousePos - previousMousePos;
            //    //Debug.Log(mousePosDelta);
            //
            //    if (bookIsOpen)
            //        bookHelper.Progress += -mousePosDelta.x;
            //
            //    previousMousePos = currentMousePos;
            //}
            //
            //if (Input.GetMouseButtonUp(0))
            //{
            //    if (bookIsOpen)
            //    {
            //        int nearestPage = Mathf.RoundToInt(bookHelper.Progress);
            //        bookHelper.GoToPage(nearestPage, true, pageFlipDuration);
            //    }
            //}
        }

        [Button(ButtonSizes.Large)]
        public void ToggleBook()
        {
            if (!enableBookToggle) return;

            if (!bookIsOpen)
            {
                OpenBook();
            }
            else
            {
                CloseBook();
            }
        }

        [Button(ButtonSizes.Large)]
        public void Appear(bool followCamera = false) => Appear(originalScale, followCamera);

        public void Appear(Vector3 scale, bool followCamera = false)
        {
            if (DOTween.IsTweening(transform))
                DOTween.Kill(transform);

            transform.DORotate(transform.rotation.eulerAngles, bookAppearDuration).SetEase(bookAppearEase).From(new Vector3(45, 45, 45));
            transform.DOScale(scale, bookAppearDuration).SetEase(bookAppearEase).From(Vector3.zero).OnStart(() => this.followCamera = false).OnComplete(() => this.followCamera = followCamera);
            //transform.DOLocalMoveZ(originalPosition.z, bookAppearDuration).SetEase(bookAppearEase).From(originalPosition - transform.forward * 0.25f);
            //transform.DOLocalRotate(originalRotation.eulerAngles, bookAppearDuration).SetEase(bookAppearEase).From(transform.rotation.eulerAngles - transform.up * 90f - transform.right * 30f);

            //this.followCamera = followCamera;

            //if (followCamera)
            //    transform.DOLookAt(Camera.main.transform.position, bookAppearDuration).OnComplete(() => this.followCamera = true);

            isVisible = true;

            OnBookAppearEvent?.Invoke();
        }

        [Button(ButtonSizes.Large)]
        public void Disappear(bool animate = true)
        {
            if (DOTween.IsTweening(transform))
                DOTween.Kill(transform);

            float duration = (animate) ? bookDisappearDuration : 0.0f;

            transform.DOScale(Vector3.zero, duration).SetEase(bookDisappearEase).From(originalScale);

            isVisible = false;

            OnBookDisappearEvent?.Invoke();
        }
        //=> DOTween.Restart(gameObject, "OnDisable");

        private void OpenBook()
        {
            bookHelper.Open(bookOpenDuration, maxOpenAmount);

            openBookCollider.enabled = true;
            closedBookCollider.enabled = false;

            if (cameraTarget != null)
            {
                cameraTarget.collider = openBookCollider;
                cameraTarget.cameraAngleRestrictions.constrainHorizontalAngles = true;
                cameraTarget.cameraAngleRestrictions.maxHorizontalAngle = 90.0f;
                cameraTarget.cameraAngleRestrictions.minHorizontalAngle = 90.0f;
                cameraTarget.initialCameraAngle = new Vector2(-90, 0);
            }


            if (cameraController != null)
            {
                cameraController.enableRotation = false;
                //cameraController.enablePan = false;
                cameraController.ResetPositionAndRotation();
            }

            OnBookOpenEvent?.Invoke();
            //bookRootTransform.DOMove(openBookTransform.position, bookOpenDuration).SetEase(bookOpenEase);
        }

        private void CloseBook()
        {
            bookHelper.Close(bookCloseDuration);

            openBookCollider.enabled = false;
            closedBookCollider.enabled = true;

            if (cameraTarget != null)
            {
                cameraTarget.collider = closedBookCollider;
                cameraTarget.cameraAngleRestrictions.constrainHorizontalAngles = false;
                cameraTarget.cameraAngleRestrictions.maxHorizontalAngle = 180.0f;
                cameraTarget.cameraAngleRestrictions.minHorizontalAngle = 180.0f;
                cameraTarget.initialCameraAngle = new Vector2(180, 0);
            }

            if (cameraController != null)
            {
                cameraController.enableRotation = true;
                //cameraController.enablePan = true;
                cameraController.ResetPositionAndRotation();
            }

            OnBookClosedEvent?.Invoke();
            //bookRootTransform.DOMove(closedBookTransform.position, bookCloseDuration).SetEase(bookCloseEase);
        }

        public void OnPointerUp(PointerEventData eventData) => OnPointerUpEvent?.Invoke(this);

        public void OnPointerDown(PointerEventData eventData) => OnPointerDownEvent?.Invoke(this);

        private void OnDoubleTouch(BasicInputController.InputEventData eventData) => ToggleBook();

        private void OnDragStarted(BasicInputController.InputEventData eventData)
        {
            if (followCameraController != null)
                followCameraController.StopRotating();

            progressOnDragStart = bookHelper.Progress;
            pointerPositionOnDragStart = eventData.pointerPosition;
        }

        private void OnDragCanceled()
        {
            if (!bookIsOpen || !enablePageSwiping) return;

            int nearestPage = Mathf.RoundToInt(bookHelper.Progress);
            bookHelper.GoToPage(nearestPage, true, pageFlipDuration);
        }

        private void OnDragPerformed(BasicInputController.InputEventData eventData)
        {
            if (!bookIsOpen || !enablePageSwiping) return;
            
            float value = (pointerPositionOnDragStart.x - eventData.pointerPosition.x) / Screen.width;
            bookHelper.Progress = progressOnDragStart + value;
        }
    }
}

