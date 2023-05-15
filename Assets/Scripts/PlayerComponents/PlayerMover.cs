using Mirror;
using UnityEngine;

namespace PlayerComponents
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerRotator))]
    public class PlayerMover : NetworkBehaviour
    {
        [SerializeField] [SyncVar] private float _moveSpeed = 5f;
        [SerializeField] [SyncVar] private float _speedRateChange = 10f;
        private CharacterController _controller;
        private PlayerRotator _playerRotator;
        private float _targetRotation;
        private float _speed;
        private const float SpeedOffset = 0.1f;

        public void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _playerRotator = GetComponent<PlayerRotator>();
        }

        public void Move(Vector2 moveDirection)
        {
            var targetSpeed = moveDirection.normalized.magnitude * _moveSpeed;
            if (moveDirection != Vector2.zero)
            {
                var velocity = _controller.velocity;
                var currentHorizontalSpeed = new Vector3(velocity.x, 0.0f, velocity.z).magnitude;

                if (currentHorizontalSpeed < targetSpeed - SpeedOffset ||
                    currentHorizontalSpeed > targetSpeed + SpeedOffset)
                {
                    _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed,
                        Time.deltaTime * _speedRateChange);
                    _speed = Mathf.Round(_speed * 1000f) / 1000f;
                }
                else
                {
                    _speed = targetSpeed;
                }

                _targetRotation = _playerRotator.Rotate(moveDirection);
            }
            else
            {
                _speed = targetSpeed;
            }

            var targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            _controller.Move(targetDirection.normalized * (targetSpeed * Time.deltaTime));
        }
    }
}