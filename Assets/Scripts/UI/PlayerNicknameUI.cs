using PlayerComponents;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerNicknameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nicknameText;
        [SerializeField] private Player _player;

        private void OnEnable()
        {
            _player.NicknameChanged += OnNicknameChanged;
        }

        private void OnDisable()
        {
            _player.NicknameChanged -= OnNicknameChanged;
        }
        
        public void OnNicknameChanged(string nickname)
        {
            _nicknameText.text = nickname;
        }
    }
}