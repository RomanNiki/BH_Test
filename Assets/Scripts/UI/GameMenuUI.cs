using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameMenuUI : NetworkBehaviour
    {
        [SerializeField] private GameObject _menuPanel;
        [SerializeField] private Button _backgroundButton;
        [SerializeField] private Button _hideMenuButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private GameMenu _gameMenu;

        public event Action ActiveChanged;
        public bool IsActive => _menuPanel.activeSelf;

        public static GameMenuUI Instance { get; private set; }

        public void Awake()
        {
            if (Instance is null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            if (this == Instance)
            {
                Instance = null;
            }
        }

        private void OnEnable()
        {
            _backgroundButton.onClick.AddListener(HideMenu);
            _hideMenuButton.onClick.AddListener(HideMenu);
            _exitButton.onClick.AddListener(OnExitButtonClick);
        }

        private void OnExitButtonClick()
        {
            _gameMenu.Disconnect();
        }

        private void OnDisable()
        {
            _backgroundButton.onClick.RemoveListener(HideMenu);
            _hideMenuButton.onClick.RemoveListener(HideMenu);
        }

        public void ToggleMenu()
        {
            if (IsActive)
            {
                HideMenu();
            }
            else
            {
                ShowMenu();
            }
        }

        public void ShowMenu()
        {
            _menuPanel.SetActive(true);
            ActiveChanged?.Invoke();
        }

        public void HideMenu()
        {
            _menuPanel.SetActive(false);
            ActiveChanged?.Invoke();
        }
    }
}