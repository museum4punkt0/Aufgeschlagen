#if GUIDEPILOT_CORE_ESSENTIALS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VectorGraphics;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.Localization.Components;
using System.Linq;


namespace com.guidepilot.guidepilotcore
{
    public class UIController : SerializedMonoBehaviour
    {
        public enum Animation { None, Left, Fade }

        public enum MaxVisible { Disabled, Characters, Words, Lines }

        [System.Serializable]
        public class Graphic
        {
            [EnumToggleButtons]
            public enum Type { Sprite, Vector}

            public string identifier;

            public Type type;           

            [DisableIf("type", Type.Vector)]
            public Image spriteImage;

            [DisableIf("type", Type.Sprite)]
            public SVGImage vectorImage;
        }

        [System.Serializable]
        public class CustomButton
        {
            public string identifier;
            public Button button;
        }

        [BoxGroup("Settings")]
        public bool showOnStart = true;

        [BoxGroup("Graphics")]
        [SerializeField]
        private Image background = null;

        [TableList]
        [BoxGroup("Graphics")]
        [SerializeField]
        private List<Graphic> graphics = new List<Graphic>();

        [BoxGroup("Title")]
        [SerializeField]
        private TextMeshProUGUI title = null;

        [BoxGroup("Title")]
        [SerializeField]
        private TextMeshProUGUI subTitle = null;

        [BoxGroup("Description")]
        [SerializeField]
        private TextMeshProUGUI description = null;

        [BoxGroup("Description")]
        [OnValueChanged("OnMaxVisibleUpdate")]
        [SerializeField]
        private MaxVisible maxVisible;

        [BoxGroup("Description")]
        [OnValueChanged("OnMaxVisibleUpdate")]
        [HideIf("maxVisible", MaxVisible.Disabled)]
        [SerializeField]
        private int maxVisibleAmount = 100;

        [BoxGroup("Selectables & Buttons")]
        [SerializeField]
        private GameObject selectablesRoot = null;

        [BoxGroup("Selectables & Buttons")]
        [AssetsOnly]
        [SerializeField]
        private Selectable selectablePrefab = null;

        [TableList]
        [BoxGroup("Selectables & Buttons")]
        [SerializeField]
        private List<CustomButton> customButtons = new List<CustomButton>();

        [BoxGroup("Selectables & Buttons")]
        [SerializeField]
        private Button[] openButtons;

        [BoxGroup("Selectables & Buttons")]
        [SerializeField]
        private Button[] closeButtons;

        [BoxGroup("Animation")]
        [SerializeField]
        private new Animation animation;

        [BoxGroup("Animation")]
        public AnimationSettings enterAnimation = null;

        [BoxGroup("Animation")]
        public AnimationSettings exitAnimation = null;

        [BoxGroup("Debug")]
        [SerializeField]
        [ReadOnly]
        private UnityContent content = null;

        [BoxGroup("Debug")]
        [ShowInInspector]
        [ReadOnly]
        public bool isActive;

        public List<Selectable> instantiatedSelectables = new List<Selectable>();

        public UnityEvent OnActicationEvent = new UnityEvent();
        public UnityEvent OnDeactivationEvent = new UnityEvent();

        private RectTransform rectTransform = null;
        private CanvasGroup canvasGroup = null;
        public ToggleGroup toggleGroup { get; private set; }

        private LocalizeStringEvent titleStringEvent = null;
        private LocalizeStringEvent subTitleStringEvent = null;
        private LocalizeStringEvent descriptionStringEvent = null;

        protected Dictionary<Selectable, UnityAction> selectableActionDictionary = new Dictionary<Selectable, UnityAction>();

        protected virtual void Awake()
        {
            foreach (Button button in openButtons)
            {
                if (button != null)
                    button.onClick.AddListener(() => SetActive(true));
            }

            foreach (Button button in closeButtons)
            {
                if (button != null)
                    button.onClick.AddListener(() => SetActive(false));
            }

            rectTransform = GetComponent<RectTransform>();

            if (selectablesRoot != null)
                instantiatedSelectables = selectablesRoot.GetComponentsInChildren<Selectable>().ToList();

            SetMaxVisible(maxVisible, maxVisibleAmount);
        }

        private void Start()
        {
            //isActive = showOnStart;
            SetActive(showOnStart, false);
        }
        [Button]
        public void SetActive(bool status)
        {
            SetActive(status, true);
        }


        public void SetActive(bool status, bool animate = true)
        {
            //if (status == isActive) return;

            //bool animateUI = status != isActive && animate;

            AnimateUI(status, animate);

            //gameObject.SetActive(status);

            if (status != isActive)
            {
                UnityEvent OnCompleteEvent = (status) ? OnActicationEvent : OnDeactivationEvent;
                OnCompleteEvent?.Invoke();
            }
            

            isActive = status;
        }

        [Button]
        public void UpdateGraphic(string identifier, Sprite sprite)
        {
            Graphic graphic = graphics.Find((x) => x.identifier == identifier);

            if (graphic == null)
            {
                Debug.LogWarning($"{this.name}: Can't update graphic because {this.name} does not contain a graphic with identifier: {identifier}.");
                return;
            }

            switch (graphic.type)
            {
                case Graphic.Type.Sprite:

                    if (graphic.spriteImage == null)
                    {
                        Debug.LogWarning($"{this.name}: Can't update graphic because {identifier} has no Sprite Image assigned.");
                        return;
                    }

                    graphic.spriteImage.sprite = sprite;

                    break;
                case Graphic.Type.Vector:

                    if (graphic.vectorImage == null)
                    {
                        Debug.LogWarning($"{this.name}: Can't update graphic because {identifier} has no SVG Image assigned.");
                        return;
                    }

                    graphic.vectorImage.sprite = sprite;

                    break;
                default:
                    break;
            }
        }

        [Button]
        public void UpdateTextDisplays(UnityContent content)
        {
            if (content == null) return;
            this.content = content;

            if (content.enableLocalization)
            {
                if (!content.titleReference.IsEmpty)
                {
                    if (titleStringEvent == null)
                    {
                        if (LocalizationHelperClass.InitializeStringLocalizationEvent(title, out LocalizeStringEvent stringEvent))
                        {
                            this.titleStringEvent = stringEvent;
                        }
                    }

                    titleStringEvent.StringReference = content.titleReference;

                    title.gameObject.SetActive(titleStringEvent.StringReference.TableEntryReference != "");
                }

                if (!content.subtitleReference.IsEmpty)
                {
                    if (subTitleStringEvent == null)
                    {
                        if (LocalizationHelperClass.InitializeStringLocalizationEvent(subTitle, out LocalizeStringEvent stringEvent))
                        {
                            this.subTitleStringEvent = stringEvent;
                        }
                    }

                    subTitleStringEvent.StringReference = content.subtitleReference;

                    subTitle.gameObject.SetActive(subTitleStringEvent.StringReference.TableEntryReference != "");
                }

                if (!content.descriptionReference.IsEmpty)
                {
                    if (descriptionStringEvent == null)
                    {
                        if (LocalizationHelperClass.InitializeStringLocalizationEvent(description, out LocalizeStringEvent stringEvent))
                        {
                            this.descriptionStringEvent = stringEvent;
                        }
                    }

                    descriptionStringEvent.StringReference = content.descriptionReference;

                    description.gameObject.SetActive(descriptionStringEvent.StringReference.TableEntryReference != "");
                }
            }
            else
            {
                if (title != null)
                {
                    title.text = content.title;
                }

                if (subTitle != null)
                {
                    subTitle.text = content.subtitle;
                }

                if (description != null)
                {
                    description.text = content.description;
                }
            }
        }

        public void UpdateBackground(Sprite sprite)
        {
            if (background == null) return;

            background.sprite = sprite;
        }

        public virtual void UpdateButtons(List<SelectableConfig> configurations)
        {
            if (selectablesRoot == null || selectablePrefab == null) return;

            for (int i = 0; i < configurations.Count; i++)
            {
                Selectable selectable = null;

                if (i < instantiatedSelectables.Count)
                {
                    selectable = instantiatedSelectables[i];
                }
                else
                {
                    selectable = Instantiate(selectablePrefab, selectablesRoot.transform);
                    instantiatedSelectables.Add(selectable);
                }

                selectable.name = configurations[i].title;

                TextMeshProUGUI textDisplay = selectable.GetComponentInChildren<TextMeshProUGUI>();

                if (textDisplay != null)
                {
                    if (configurations[i].titleReference != null && LocalizationHelperClass.StringReferenceExists(configurations[i].titleReference))
                    {
                        LocalizationHelperClass.InitializeStringLocalizationEvent(textDisplay, out LocalizeStringEvent stringEvent);
                        stringEvent.StringReference = configurations[i].titleReference;
                    }
                    else
                    {
                        textDisplay.text = configurations[i].title;
                    }
                }


                if (selectable is Button)
                {
                    Button button = (Button)selectable;
                    InitSelectable(button, configurations[i].action);
                }
                else if (selectable is Toggle)
                {
                    Toggle toggle = (Toggle)selectable;
                    InitSelectable(toggle, configurations[i].action);
                }

                if (!instantiatedSelectables[i].gameObject.activeSelf)
                    instantiatedSelectables[i].gameObject.SetActive(true);
            }

            int difference = instantiatedSelectables.Count - configurations.Count;

            if (difference > 0)
            {
                for (int i = 1; i <= difference; i++)
                {
                    instantiatedSelectables[instantiatedSelectables.Count - i].gameObject.SetActive(false);
                }
            }

            Debug.Log($"{this.name}: Update Buttons");
        }

        public void UpdateCustomButton(string identifier, UnityAction action)
        {
            CustomButton customButton = customButtons.Find((x) => x.identifier == identifier);

            if (customButton == null)
            {
                Debug.LogWarning($"{this.name}: Can't update custom button because {this.name} does not contain a custom button with identifier: {identifier}.");
                return;
            }

            if (customButton.button == null)
            {
                Debug.LogWarning($"{this.name}: Can't update custom button because button with identifier {identifier} has no button assigned.");
                return;
            }

            customButton.button.onClick.RemoveAllListeners();
            customButton.button.onClick.AddListener(action);
        }

        private void InitSelectable(Button button, UnityAction action)
        {
            if (selectableActionDictionary.ContainsKey(button))
                button.onClick.RemoveListener(selectableActionDictionary[button]);

            button.onClick.AddListener(action);
            selectableActionDictionary[button] = action;
        }

        private void InitSelectable(Toggle toggle, UnityAction action)
        {
            //toggle.onValueChanged.RemoveListener(selectableActionDictionary[toggle]);
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener((value) =>
            {
                Debug.Log($"{toggle.name}: {value}");

                if (value)
                    action?.Invoke();
            });

            if (this.toggleGroup == null)
            {
                if (TryGetComponent(out ToggleGroup toggleGroup))
                {
                    this.toggleGroup = toggleGroup;
                }
                else
                {
                    this.toggleGroup = gameObject.AddComponent<ToggleGroup>();
                }
            }

            toggle.group = this.toggleGroup;
            this.toggleGroup.RegisterToggle(toggle);

            //selectableActionDictionary[toggle] = action;
        }

        public virtual void UpdateButtons(List<UnityContent> content, List<UnityAction> actions)
        {
            if (selectablesRoot == null || selectablePrefab == null) return;

            for (int i = 0; i < content.Count; i++)
            {
                if (i > instantiatedSelectables.Count - 1)
                {
                    Selectable newSelectable = Instantiate(selectablePrefab, selectablesRoot.transform).GetComponent<Selectable>();
                    newSelectable.name = content[i].title;
                    instantiatedSelectables.Add(newSelectable);
                }

                TextMeshProUGUI textDisplay = instantiatedSelectables[i].GetComponentInChildren<TextMeshProUGUI>();

                if (textDisplay != null)
                    textDisplay.text = content[i].title;

                if (!instantiatedSelectables[i].gameObject.activeSelf)
                    instantiatedSelectables[i].gameObject.SetActive(true);

                if (actions.Count > 0)
                {
                    if (actions[i] != null)
                    {
                        if (instantiatedSelectables[i].TryGetComponent(out Button button))
                        {
                            button.onClick.RemoveAllListeners();
                            button.onClick.AddListener(actions[i]);
                        }
                    }
                }

                selectableActionDictionary[instantiatedSelectables[i]] = actions[i];

            }

            int difference = instantiatedSelectables.Count - content.Count;

            if (difference > 0)
            {
                for (int i = 1; i <= difference; i++)
                {
                    instantiatedSelectables[instantiatedSelectables.Count - i].gameObject.SetActive(false);
                }
            }
        }

        private void AnimateUI(bool status, bool animate = true)
        {
            //if (!gameObject.activeSelf)
            //    gameObject.SetActive(true);

            Tween tween = null;

            float duration = (status) ? enterAnimation.duration : exitAnimation.duration;
            duration = (animate) ? duration : 0.0f;

            Ease ease = (status) ? enterAnimation.ease : exitAnimation.ease;

            switch (animation)
            {
                case Animation.None:
                    break;
                case Animation.Left:

                    if (rectTransform == null)
                    {
                        if (TryGetComponent(out RectTransform rectTransform))
                        {
                            this.rectTransform = rectTransform;
                        }
                        else
                        {
                            this.rectTransform = gameObject.AddComponent<RectTransform>();
                        }
                    }

                    if (DOTween.IsTweening(rectTransform))
                        DOTween.Kill(rectTransform);

                    float leftEndValue = (status) ? 0.0f : rectTransform.rect.width;

                    tween = rectTransform.DOAnchorPosX(leftEndValue, duration).SetEase(ease);


                    break;
                case Animation.Fade:

                    if (canvasGroup == null)
                    {
                        if (TryGetComponent(out CanvasGroup canvasGroup))
                        {
                            this.canvasGroup = canvasGroup;
                        }
                        else
                        {
                            this.canvasGroup = gameObject.AddComponent<CanvasGroup>();
                        }
                    }

                    if (DOTween.IsTweening(canvasGroup))
                        DOTween.Kill(canvasGroup);

                    float fadeEndValue = (status) ? 1.0f : 0.0f;
                    float fadeStartValue = canvasGroup.alpha;

                    tween = canvasGroup.DOFade(fadeEndValue, duration).From(fadeStartValue).SetEase(ease).OnStart(() => canvasGroup.blocksRaycasts = status);

                    break;
                default:
                    break;
            }

            //if (!status)
            //    tween.OnComplete(() => gameObject.SetActive(false));
        }

        private void SetMaxVisible(MaxVisible maxVisible, int amount)
        {
            if (description == null) return;

            switch (maxVisible)
            {
                case MaxVisible.Disabled:
                    description.maxVisibleCharacters = 9999;
                    description.maxVisibleWords = 9999;
                    description.maxVisibleLines = 9999;
                    break;
                case MaxVisible.Characters:
                    description.maxVisibleCharacters = amount;
                    break;
                case MaxVisible.Words:
                    description.maxVisibleWords = amount;
                    break;
                case MaxVisible.Lines:
                    description.maxVisibleLines = amount;
                    break;
                default:
                    break;
            }
        }

        private void OnMaxVisibleUpdate()
        {
            SetMaxVisible(maxVisible, maxVisibleAmount);
        }
    }
}

#endif