using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Guidepilot.KSW.FliegendeBuecher;
using com.guidepilot.guidepilotcore;
using System;

public class StandaloneUIController : MonoBehaviour
{
    [SerializeField]
    private Guidepilot.KSW.FliegendeBuecher.GameManager _gameManager;

    [SerializeField]
    private Button _helpButton;

    [SerializeField]
    private Button _backButton;

    [SerializeField]
    private Dropdown _dropdown;

    [SerializeField]
    private Transform _list;

    [SerializeField]
    private Button[] _listEntries;

    private void Awake()
    {
        if (_backButton != null)
            _backButton.onClick.AddListener(() => _list.gameObject.SetActive(true));

        if (_helpButton != null)
            _helpButton.onClick.AddListener(_gameManager.OnShowHelp);

        foreach (Button entry in _listEntries)
        {
            entry.onClick.AddListener(() => _list.gameObject.SetActive(false));
        }

        ViewerSceneController.OnAwakeEvent += OnViewerAwake;
        ARViewerSceneController.OnAwakeEvent += OnARViewerAwake;
        ViewerTutorialController.OnTutorialStartEvent += OnTutorialStart;
        ViewerTutorialController.OnTutorialCompleteEvent += OnTutorialComplete;
        ViewerTutorialController.OnTutorialStopEvent += OnTutorialComplete;
    }

    private void OnDestroy()
    {
        ViewerSceneController.OnAwakeEvent -= OnViewerAwake;
        ARViewerSceneController.OnAwakeEvent -= OnARViewerAwake;
        ViewerTutorialController.OnTutorialStartEvent -= OnTutorialStart;
        ViewerTutorialController.OnTutorialCompleteEvent -= OnTutorialComplete;
        ViewerTutorialController.OnTutorialStopEvent -= OnTutorialComplete;
    }

    private void OnARViewerAwake(ARViewerSceneController controller)
    {
        if (_helpButton != null)
            _helpButton.gameObject.SetActive(false);
    }

    private void OnViewerAwake(ViewerSceneController controller)
    {
        if (_helpButton != null)
            _helpButton.gameObject.SetActive(true);
    }

    private void OnTutorialStart()
    {
        if (_helpButton != null)
            _helpButton.gameObject.SetActive(false);

        if (_backButton != null)
            _backButton.gameObject.SetActive(false);

        if (_dropdown != null)
            _dropdown.gameObject.SetActive(false);
    }

    private void OnTutorialComplete()
    {
        if (_helpButton != null)
            _helpButton.gameObject.SetActive(true);

        if (_backButton != null)
            _backButton.gameObject.SetActive(true);

        if (_dropdown != null)
            _dropdown.gameObject.SetActive(true);
    }
}
