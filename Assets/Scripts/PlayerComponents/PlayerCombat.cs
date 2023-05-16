using System.Collections;
using Mirror;
using UnityEngine;

namespace PlayerComponents
{
    [RequireComponent(typeof(PlayerColor))]
    public class PlayerCombat : NetworkBehaviour
    {
        [SerializeField][SyncVar] private float _invincibilityDuration;
        [SerializeField] private Color _invincibilityColor;
        private PlayerColor _playerColor;
        [SyncVar] private bool _isInvincible;

        private void Awake()
        {
            _playerColor = GetComponent<PlayerColor>();
        }

        public bool TryGetDamage()
        {
            if (_isInvincible)
                return false;
            CmdGetDamage();
            return true;
        }
        
        [Command(requiresAuthority = false)]
        public void CmdGetDamage()
        {
            GetDamage();
        }

        [Server]
        private void GetDamage()
        {
            if (_isInvincible)
                return;
            _isInvincible = true;
            ChangeColor();
        }
        
        private void ChangeColor()
        {
            _playerColor.RpcChangeColor(_invincibilityColor);
            StartCoroutine(ResetCombat());
        }

        private IEnumerator ResetCombat()
        {
            yield return new WaitForSeconds(_invincibilityDuration);
            _isInvincible = false;
            _playerColor.SetDefaultColor();
        }
    }
}