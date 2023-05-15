using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Misc
{
    public class MouseLock : MonoBehaviour
    {
        [FormerlySerializedAs("_gameMenu")] [SerializeField] private GameMenuUI _gameMenuUI;

        private void OnEnable()
        {
            _gameMenuUI.ActiveChanged += OnActiveChanged;
        }

        private void OnDisable()
        {
            _gameMenuUI.ActiveChanged -= OnActiveChanged; 
        }

        private void Start()
        {
            LockMouse();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                ToggleLockCursor();
            }
            else
            {
                UnlockMouse();
            }
        }

        private void ToggleLockCursor()
        {
            if (_gameMenuUI.IsActive)
            {
                UnlockMouse();
            }
            else
            {
                LockMouse();
            }
        }
        
        private void OnActiveChanged()
        {
            ToggleLockCursor();
        }

        private static void LockMouse()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private static void UnlockMouse()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}