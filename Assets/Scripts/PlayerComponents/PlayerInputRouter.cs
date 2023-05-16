using Mirror;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerComponents
{
    [RequireComponent(typeof(PlayerLook))]
    [RequireComponent(typeof(PlayerMover))]
    [RequireComponent(typeof(PlayerDash))]
    public class PlayerInputRouter : NetworkBehaviour
    {
        private PlayerInput _inputs;
        private PlayerLook _playerLook;
        private PlayerMover _playerMover;
        private PlayerDash _playerDash;
        private Vector2 _moveDirection;

        private void Awake()
        {
            _playerLook = GetComponent<PlayerLook>();
            _playerMover = GetComponent<PlayerMover>();
            _playerDash = GetComponent<PlayerDash>();
            _inputs = new PlayerInput();
        }

        private void OnEnable()
        {
            _inputs.Enable();
            _inputs.Player.Look.started += OnLook;
            _inputs.Player.Look.performed += OnLook;
            _inputs.Player.Look.canceled += OnLook;
            _inputs.Player.Move.started += OnMove;
            _inputs.Player.Move.performed += OnMove;
            _inputs.Player.Move.canceled += OnMove;
            _inputs.Player.Dash.performed += OnDash;
            _inputs.Common.GameMenu.performed += OnGameMenu;
        }

        private void OnDisable()
        {
            _inputs.Player.Look.started -= OnLook;
            _inputs.Player.Look.performed -= OnLook;
            _inputs.Player.Look.canceled -= OnLook;
            _inputs.Player.Move.started -= OnMove;
            _inputs.Player.Move.performed -= OnMove;
            _inputs.Player.Move.canceled -= OnMove;
            _inputs.Player.Dash.performed -= OnDash;
            _inputs.Common.GameMenu.performed -= OnGameMenu;
            _inputs.Disable();
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            if (Application.isFocused)
                _playerMover.Move(_moveDirection);
        }

        private void OnGameMenu(InputAction.CallbackContext obj)
        {
            if (!isLocalPlayer) return;
            if (GameMenuUI.Instance == null) return;
            
            if (obj.performed)
            {
                GameMenuUI.Instance.ToggleMenu();
            }
        }

        private async void OnDash(InputAction.CallbackContext obj)
        {
            if (!isLocalPlayer) return;
            if (Cursor.lockState != CursorLockMode.Locked) return;
            if (Application.isFocused)
            {
                await _playerDash.PerformDash(_moveDirection);
            }
        }

        private void OnLook(InputAction.CallbackContext obj)
        {
            if (!isLocalPlayer) return;
            if (Cursor.lockState != CursorLockMode.Locked) return;
            var lookAxis = obj.ReadValue<Vector2>();
            if (Application.isFocused)
                _playerLook.MoveLook(lookAxis);
        }

        private void OnMove(InputAction.CallbackContext obj)
        {
            if (!isLocalPlayer) return;
            if (Cursor.lockState != CursorLockMode.Locked) return;
            if (Application.isFocused)
                _moveDirection = obj.ReadValue<Vector2>();
        }
    }
}