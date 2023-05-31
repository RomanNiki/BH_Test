using System;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;

namespace PlayerComponents
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerRotator))]
    [RequireComponent(typeof(Player))]
    public class PlayerDash : NetworkBehaviour
    {
        [SerializeField] [SyncVar] private float _speed = 15f;
        [SerializeField] [SyncVar] private float _dashDistance = 5f;
        [SerializeField] [SyncVar] private float _cooldown = 1f;
        [SerializeField] [SyncVar] private float _sphereRadius = 1f;
        [SerializeField] private PlayerAnimation _playerAnimation;

        private PlayerRotator _playerRotator;
        private Player _player;
        private CharacterController _controller;
        private bool _isDashing;
        private bool _isCoolDown;
        private float _distance;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _playerRotator = GetComponent<PlayerRotator>();
            _player = GetComponent<Player>();
        }

        public async Task PerformDash(Vector2 moveDirection)
        {
            if (_isDashing || _isCoolDown)
                return;
            var targetRotation = _playerRotator.Rotate(moveDirection);
            var targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
            _distance = _dashDistance;
            if (Physics.SphereCast(transform.position, _sphereRadius, targetDirection, out var hit, _distance))
            {
                var endPoint = hit.point;
                _distance = (endPoint - transform.position).magnitude;
            }
           
            await Dash(targetDirection);
        }

        private async Task Dash(Vector3 dashDirection)
        {
            _playerAnimation.Dash();
            _isDashing = true;
            var dashDistanceTraveled = 0f;
            while (dashDistanceTraveled < _distance)
            {
                if (_controller == null) return;
                dashDistanceTraveled += _speed * Time.deltaTime;
                _controller.Move(dashDirection * _speed * Time.deltaTime);
                await Task.Yield();
            }

            _isDashing = false;
            _playerAnimation.Dash(false);
            await SetDashCooldown();
        }

        private async Task SetDashCooldown()
        {
            _isCoolDown = true;
            await Task.Delay(TimeSpan.FromSeconds(_cooldown));
            _isCoolDown = false;
        }

        [Command]
        private void CmdDamage(PlayerCombat combat)
        {
            if (combat.TryGetDamage())
            {
                _player.CmdIncreaseScore();
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (_isDashing == false)
                return;

            if (hit.collider.TryGetComponent(out PlayerCombat combat))
            {
                CmdDamage(combat);
            }
        }
    }
}