#if GUIDEPILOT_CORE_AR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using UnityEngine.XR.ARFoundation;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace com.guidepilot.guidepilotcore
{
    public class POITargetController : MonoBehaviour
    {
        [System.Serializable]
        public class POITargetControllerEvent : UnityEvent<POITargetController> { };

        [Title("Unity Events")]
        [PropertyOrder(100)]
        public POITargetControllerEvent OnNewPOITarget = new POITargetControllerEvent();

        public SortingLayer targetTextureSortingLayer;

        [BoxGroup("Content")]
        //[OnValueChanged("OnPOITargetChanged")]
        public POITarget target = null;

        [ReadOnly]
        public POILayerController activeLayer = null;

        [ShowInInspector]
        [ReadOnly]
        public POIController currentSelectedPOI { get; private set; }

        [ReadOnly]
        public List<POILayerController> currentLayers = new List<POILayerController>();

        public static UnityAction<POITargetController> OnInitialization;
        public UnityAction<POITargetController, POILayerController, POIController> OnPOISelectedAction;
        public UnityAction<POITargetController, POILayerController, POIController> OnPOIDeselectedAction;

        private POIController activePOI = null;
        private SpriteRenderer textureDisplay = null;
        private MeshRenderer meshRenderer = null;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void Start()
        {
            //Initialize(target);
        }

        public void Initialize(POITarget target, Vector2 scale, Transform parent)
        {
            if (target == null) return;

            this.target = target;
            gameObject.name = target.name;

            UpdateLayers();

            if (currentLayers.Count > 0)
                SwitchPOILayer(currentLayers[0]);

            UpdateTextureDisplay();

            gameObject.transform.parent = parent;
            transform.localRotation = Quaternion.Euler(transform.right * 90.0f);
            transform.localScale *= scale;

            OnInitialization?.Invoke(this);
        }

        public void SetActive(bool status)
        {
            gameObject.SetActive(status);
        }

        public void SwitchPOILayer(POILayerController controller)
        {
            if (controller == null || !currentLayers.Contains(controller)) return;

            foreach (POILayerController layer in currentLayers)
            {
                layer.gameObject.SetActive(layer == controller);
            }

            SetPOIsActive(true, controller);

            activeLayer = controller;

            Debug.Log($"[{this.name}]: Switched to layer: {controller.layer.name}");
        }

        public void SetPOIsActive(bool status, POILayerController layer)
        {
            foreach (POIController controller in layer.currentPOIs)
            {
                controller.SetActive(status);
            }
        }

        private void UpdateTextureDisplay()
        {
            //if (meshRenderer != null)
            //{
            //    meshRenderer.material.shader = Shader.Find("Unlit/Texture");
            //    meshRenderer.material.SetTexture("_MainTex", target.sprite.texture);
            //}

            if (textureDisplay == null)
            {
                //GameObject textureDisplay = new GameObject("TextureDisplay");
                //textureDisplay.transform.localPosition = Vector3.zero;
                //textureDisplay.transform.parent = transform;
                this.textureDisplay = gameObject.AddComponent<SpriteRenderer>();
            }

            this.textureDisplay.sprite = target.sprite;

            //target.spriteReference.LoadAssetAsync<Sprite>().Completed += (operation) =>
            //{
            //    if (operation.Status == AsyncOperationStatus.Succeeded)
            //    {
            //        this.textureDisplay.sprite = operation.Result;
            //    }
            //};

            // This "trick" is needed to apply the correct scaling of the target in AR
            this.textureDisplay.drawMode = SpriteDrawMode.Sliced;
            this.textureDisplay.size = Vector2.one;
            this.textureDisplay.drawMode = SpriteDrawMode.Simple;
            this.textureDisplay.enabled = false;
        }

        private void UpdateLayers()
        {
            if (target == null) return;

            if (currentLayers.Count > 0)
            {
                foreach (POILayerController controller in currentLayers)
                {
                    if (controller != null)
                        Destroy(controller.gameObject);
                }
            }

            currentLayers.Clear();

            foreach (POILayer layer in target.layers)
            {
                CreateLayer(layer);
            }
        }


        private void CreateLayer(POILayer layer)
        {
            if (layer == null) return;

            GameObject newLayer = new GameObject(layer.title);
            newLayer.transform.parent = transform;
            POILayerController controller = newLayer.AddComponent<POILayerController>();
            controller.OnPOISelectedAction += OnPOISelected;
            controller.OnPOIDeselectedAction += OnPOIDeselected;
            controller.Initialize(layer);
            currentLayers.Add(controller);
        }

        private void OnPOITargetChanged()
        {
            Initialize(target, Vector2.one, transform);
            OnNewPOITarget?.Invoke(this);
        }

        private void OnPOISelected(POILayerController layer, POIController poi)
        {
            currentSelectedPOI = poi;

            Debug.Log("SELECT: Target: " + this.target.title + " | Layer: " + layer.layer.title + " | POI: " + poi.pointOfInterest.title);
            OnPOISelectedAction?.Invoke(this, layer, poi);
        }

        private void OnPOIDeselected(POILayerController layer, POIController poi)
        {
            currentSelectedPOI = null;

            Debug.Log("DESELECT: Target: " + this.target.title + " | Layer: " + layer.layer.title + " | POI: " + poi.pointOfInterest.title);
            OnPOIDeselectedAction?.Invoke(this, layer, poi);
        }
    }
}

#endif
