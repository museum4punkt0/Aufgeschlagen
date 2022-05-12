#if GUIDEPILOT_CORE_AR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using DG.Tweening;

namespace com.guidepilot.guidepilotcore
{
    public class POILayerController : MonoBehaviour
    {
        [System.Serializable]
        public class POILayerControllerEvent : UnityEvent<POILayerController> { };

        [BoxGroup("Animation")]
        public AnimationSettings animationSettings = new AnimationSettings();

        [BoxGroup("Content")]
        [OnValueChanged("OnPOILayerChanged")]
        public POILayer layer = null;

        [ReadOnly]
        public List<POIController> currentPOIs = new List<POIController>();

        public POILayerControllerEvent OnNewPOILayer = new POILayerControllerEvent();
        public UnityAction<POILayerController, POIController> OnPOISelectedAction;
        public UnityAction<POILayerController, POIController> OnPOIDeselectedAction;

        private SpriteRenderer textureDisplay = null;

        //private void Start()
        //{
        //    Initialize();
        //}

        public void Initialize(POILayer layer)
        {
            if (layer == null) return;
            
            this.layer = layer;

            UpdateTextureDisplay();
            UpdatePOIs();
        }

        public void SetActive(bool status, bool animate = true)
        {

            if (animate && textureDisplay != null)
            {
                float startValue = (status) ? 0.0f : 1.0f;
                float endValue = (status) ? 1.0f : 0.0f;
                textureDisplay.DOFade(endValue, animationSettings.duration).From(startValue).SetEase(animationSettings.ease);
            }

                gameObject.SetActive(status);
        }

        private void UpdatePOIs()
        {
            if (layer == null) return;

            if (currentPOIs.Count > 0)
            {
                foreach (POIController controller in currentPOIs)
                {
                    Destroy(controller.gameObject);
                }

                currentPOIs.Clear();
            }

            foreach (POI poi in layer.pointsOfInterest)
            {
                CreatePOI(poi);
            }
        }

        private void CreatePOI(POI pointOfInterest)
        {
            if (pointOfInterest == null) return;

            GameObject poi = new GameObject(pointOfInterest.title);
            poi.transform.parent = transform;
            POIController controller = poi.AddComponent<POIController>();
            controller.Initialize(pointOfInterest, POIController.Status.Deselected);
            controller.OnPOISelected.AddListener(OnPOISelected);
            controller.OnPOIDeselected.AddListener(OnPOIDeselected);
            currentPOIs.Add(controller);

            if (layer != null)
            {
                POITargetController targetController = GetComponentInParent<POITargetController>();

                if (targetController != null && targetController.target != null)
                {
                    Vector3 targetDimensions = targetController.target.dimensions;
                    Vector3 position = (targetController.transform.position - targetDimensions) + new Vector3(targetDimensions.x * pointOfInterest.position.x * 2, targetDimensions.y * pointOfInterest.position.y * 2);
                    poi.transform.localPosition = position;
                    controller.layerDimensions = targetDimensions;
                    controller.layerOrigin = targetController.transform.position;
                    //Debug.Log($"{this.name}: POI Position: {pointOfInterest.position} | Layer Position: {position} | Target Dimensions: {targetDimensions}");
                }

                //Addressables.LoadAssetAsync<Sprite>(layer.spriteReference).Completed += (operation) =>
                //{
                //    if (operation.Status == AsyncOperationStatus.Succeeded)
                //    {
                //        // TODO: Replace GetComponentCall with somethin better
                //        Bounds bounds = (operation.Status == AsyncOperationStatus.Succeeded) ? operation.Result.bounds : GetComponentInParent<POITargetController>().target.sprite.bounds;
                //        Vector3 position = (bounds.center - bounds.extents) + new Vector3(bounds.size.x * pointOfInterest.position.x, bounds.size.y * pointOfInterest.position.y);
                //        poi.transform.localPosition = position;
                //        controller.layerExtents = bounds.extents;
                //        controller.layerOrigin = bounds.center;
                //        Debug.Log($"{this.name}: Created POI at {position}");
                //        return;
                //    }
                //
                //    Debug.Log($"{this.name}: Could not initialize poi because operation status: {operation.Status}");
                //};
            }
        }

        private void UpdateTextureDisplay()
        {
            if (layer == null) return;

            if (textureDisplay == null)
            {
                GameObject textureDisplay = new GameObject();
                textureDisplay.transform.parent = transform;
                this.textureDisplay = textureDisplay.AddComponent<SpriteRenderer>();
            }

            this.textureDisplay.gameObject.name = "TextureDisplay";

            if (layer.spriteReference.RuntimeKeyIsValid())
            {
                Addressables.LoadAssetAsync<Sprite>(layer.spriteReference).Completed += (operation) =>
                {
                    this.textureDisplay.sprite = (operation.Status == AsyncOperationStatus.Succeeded) ? operation.Result : layer.sprite;
                    //Debug.Log($"{layer.title}: {operation.Status}");
                };
            }
            
        }

        private void OnPOILayerChanged()
        {
            Initialize(layer);
            OnNewPOILayer?.Invoke(this);
        }

        private void OnPOISelected(POIController controller)
        {
            OnPOISelectedAction?.Invoke(this, controller);
        }
        private void OnPOIDeselected(POIController controller)
        {
            OnPOIDeselectedAction?.Invoke(this, controller);
        }
    }
}

#endif
