using Mirror;
using Networking;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;

        private void OnEnable()
        {
            _exitButton.onClick.AddListener(ExitGameRoom);
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(ExitGameRoom);
        }
        
        private void ExitGameRoom()
        {
            var networkManager = NetworkManager.singleton;
            switch (networkManager.mode)
            {
                case NetworkManagerMode.Host:
                    networkManager.StopHost();
                    break;
                case NetworkManagerMode.ClientOnly:
                    networkManager.StopClient();
                    break;
            }
        }
    }
}