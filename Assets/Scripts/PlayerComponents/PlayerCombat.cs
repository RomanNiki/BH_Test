using System;
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
        private bool _isInvincible;

        private void Awake()
        {
            _playerColor = GetComponent<PlayerColor>();
        }
        
        [Command(requiresAuthority = false)]
        public void CmdGetDamage(Player damager)
        {
            if (_isInvincible)
                return;
            damager.CmdIncreaseScore();
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