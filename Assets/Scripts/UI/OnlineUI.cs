using Mirror;
using Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public class OnlineUI : MonoBehaviour
    {
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _joinButton;
        [SerializeField] private TMP_InputField _nicknameInputField;
        [SerializeField] private string _defaultNickname = "Guest";
        
        
        private void OnEnable()
        {
            _hostButton.onClick.AddListener(StartHost);
            _joinButton.onClick.AddListener(Join);
        }
    
        private void OnDisable()
        {
            _hostButton.onClick.AddListener(StartHost);
            _joinButton.onClick.AddListener(Join);
        }

        private void Join()
        {
            SetName();
            var networkManager = NetworkManager.singleton;
            networkManager.StartClient();
        }

        private void StartHost()
        {
            SetName();
            var networkManager = NetworkManager.singleton;
            networkManager.StartHost();
        }

        private void SetName()
        {
            PlayerSettings.Nickname = _nicknameInputField.text != "" ? _nicknameInputField.text : _defaultNickname;
        }
    }
}
