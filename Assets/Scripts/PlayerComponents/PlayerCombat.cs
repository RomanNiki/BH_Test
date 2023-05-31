using System.Collections;
using Mirror;
using UnityEngine;

namespace PlayerComponents
{
    [RequireComponent(typeof(CombatFx))]
    public class PlayerCombat : NetworkBehaviour
    {
        [SerializeField][SyncVar] private float _invincibilityDuration;
        private CombatFx _combatFx;
        [SyncVar] private bool _isInvincible;

        private void Awake()
        {
            _combatFx = GetComponent<CombatFx>();
        }

        [Server]
        public bool TryGetDamage()
        {
            if (_isInvincible)
                return false;
            
            GetDamage();
            return true;
        }

        [Server]
        private void GetDamage()
        {
            if (_isInvincible)
                return;
            _isInvincible = true;
            _combatFx.RpcPlayerAnimations(_invincibilityDuration);
            StartCoroutine(ResetCombat());
        }

        private IEnumerator ResetCombat()
        {
            yield return new WaitForSeconds(_invincibilityDuration);
            _isInvincible = false;
        }
    }
}