#if GUIDEPILOT_CORE_AR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace com.guidepilot.guidepilotcore
{
    public class POIController : MonoBehaviour, IPointerClickHandler
    {
        public enum Status { Selected, Deselected };

        public POIController(POI pointOfInterest)
        {
            this.pointOfInterest = pointOfInterest;
        }

        [System.Serializable]
        public class POIControllerEvent : UnityEvent<POIController> { };

        [BoxGroup("Settings")]
        [SerializeField]
        private Status currentStatus = Status.Deselected;

        [BoxGroup("Content")]
        [OnValueChanged("OnPOIChanged")]
        [InlineEditor]
        public POI pointOfInterest = null;

        [BoxGroup("References")]
        [SerializeField]
        private SpriteMask spriteMask = null;

        [BoxGroup("References")]
        [SerializeField]
        private SpriteRenderer spriteRenderer = null;

        [BoxGroup("Animation")]
        [SerializeField]
        private AnimationSettings activationAnimationSettings = new AnimationSettings();

        [BoxGroup("Animation")]
        [SerializeField]
        private AnimationSettings deactivationAnimationSettings = new AnimationSettings();

        [ReadOnly]
        public Vector3 layerOrigin;

        [ReadOnly]
        public Vector3 layerDimensions;

        //public bool editPOIPosition = false;

        public POIControllerEvent OnNewPOIEvent;
        public POIControllerEvent OnPOIClickEvent;
        public POIControllerEvent OnPOISelected;
        public POIControllerEvent OnPOIDeselected;

        private GameObject spriteDisplay = null;
        private GameObject indicator = null;
        private new BoxCollider collider = null;
        private POIIndicatorController poiIndicatorController = null;

        private bool interactable = true;

        private void Awake()
        {
            if (OnNewPOIEvent == null)
                OnNewPOIEvent = new POIControllerEvent();

            if (OnPOIClickEvent == null)
                OnPOIClickEvent = new POIControllerEvent();

            if (OnPOISelected == null)
                OnPOISelected = new POIControllerEvent();

            if (OnPOIDeselected == null)
                OnPOIDeselected = new POIControllerEvent();

            collider = GetComponentInChildren<BoxCollider>();
        }

        private void Update()
        {
            //if (pointOfInterest != null && transform.hasChanged && editPOIPosition)
            //{
            //    transform.hasChanged = false;
            //
            //    Vector2 poiPosition = ((Vector2)transform.localPosition + layerExtents) * 0.5f / layerExtents;
            //    //pointOfInterest.position = poiPosition;
            //}
        }

        public void Initialize(POI pointOfInterest, Status status)
        {
            if (pointOfInterest == null) return;

            this.pointOfInterest = pointOfInterest;
            pointOfInterest.controller = this;
            gameObject.name = pointOfInterest.title;
            gameObject.transform.localPosition = pointOfInterest.position;

            if (pointOfInterest.indicator != null)
            {
                indicator = Instantiate(pointOfInterest.indicator, transform);
                indicator.gameObject.name = pointOfInterest.indicator.name;

                if (collider == null)
                {
                    collider = gameObject.AddComponent<BoxCollider>();
                    collider.size = new Vector3(indicator.transform.localScale.x, indicator.transform.transform.localScale.y, 0.1f);
                }

                poiIndicatorController = indicator.GetComponent<POIIndicatorController>();

                if (pointOfInterest.highlightPOIIndicator)
                    SetPOIIndicatorStatus(POIIndicatorController.Status.Highlight);
            }

            if (pointOfInterest.spriteDisplay != null)
            {
                spriteDisplay = Instantiate(pointOfInterest.spriteDisplay, transform);
                spriteDisplay.gameObject.name = pointOfInterest.spriteDisplay.name;
                spriteMask = spriteDisplay.GetComponentInChildren<SpriteMask>();
                spriteRenderer = spriteDisplay.GetComponentInChildren<SpriteRenderer>();

                if (spriteRenderer != null && pointOfInterest.spriteReference.RuntimeKeyIsValid())
                {
                    Addressables.LoadAssetAsync<Sprite>(pointOfInterest.spriteReference).Completed += (operation) =>
                    {
                        spriteRenderer.sprite = (operation.Status == AsyncOperationStatus.Succeeded) ? operation.Result : pointOfInterest.sprite;
                    };
                }
            }

            activationAnimationSettings = new AnimationSettings();
            deactivationAnimationSettings = new AnimationSettings();

            SetStatus(status, false);
        }

        [Button]
        public void SetStatus(Status status, bool animate = true)
        {
            interactable = false;

            Sequence sequence = DOTween.Sequence();

            TweenCallback callback = () =>
            {
                if (spriteDisplay != null)
                    spriteDisplay.SetActive(status == Status.Selected);
            };

            TweenCallback OnStartCallback = (status == Status.Selected) ? callback : null;
            TweenCallback OnCompleteCallback = (status == Status.Deselected) ? callback : null;

            if (spriteDisplay != null)
            {
                //spriteDisplay.SetActive(status == Status.Selected);

                if (spriteMask != null && spriteRenderer != null && spriteRenderer.sprite != null)
                {

                    Vector3 spriteSize = spriteRenderer.sprite.bounds.size;
                    float startValue = (status == Status.Selected) ? 0.0f : Mathf.Min(spriteSize.x, spriteSize.y);
                    float endValue = (status == Status.Selected) ? Mathf.Min(spriteSize.x, spriteSize.y) : 0.0f;
                    float duration = (status == Status.Selected) ? activationAnimationSettings.duration : deactivationAnimationSettings.duration;
                    duration = (animate) ? duration : 0.0f;
                    Ease ease = (status == Status.Selected) ? activationAnimationSettings.ease : deactivationAnimationSettings.ease;

                    if (DOTween.IsTweening(spriteMask))
                        DOTween.Complete(spriteMask);

                    Tween tween = spriteMask.transform.DOScale(Vector3.one * endValue, duration).From(startValue).SetEase(ease);
                    sequence.Append(tween);
                }

            }

            if (indicator != null)
                indicator.SetActive(status == Status.Deselected);

            sequence.OnStart(OnStartCallback);
            sequence.OnComplete(() =>
            {
                interactable = true;
                OnCompleteCallback?.Invoke();
                OnStatusComplete(status);               
            });
        }

        public void SetPOIIndicatorStatus(POIIndicatorController.Status status)
        {
            if (poiIndicatorController == null || poiIndicatorController.status == status) return;

            poiIndicatorController.SetStatus(status);
        }

        private void OnStatusComplete(Status status)
        {
            currentStatus = status;

            POIControllerEvent OnCompleteEvent = (status == Status.Selected) ? OnPOISelected : OnPOIDeselected;
            OnCompleteEvent?.Invoke(this);
        }

        public void SetActive(bool status)
        {
            gameObject.SetActive(status);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (!interactable) return;

            SetStatus(Status.Selected);

            if (pointOfInterest.highlightPOIIndicator)
                SetPOIIndicatorStatus(POIIndicatorController.Status.Default);

            OnPOIClickEvent?.Invoke(this);
        }

        private void OnPOIChanged()
        {
            Initialize(pointOfInterest, currentStatus);
            OnNewPOIEvent?.Invoke(this);
        }
    }
}

#endif
