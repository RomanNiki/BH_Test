using Mirror;
using UnityEngine;

namespace PlayerComponents
{
    [RequireComponent(typeof(PlayerCamera))]
    public class PlayerRotator : NetworkBehaviour
    {
        [Range(0.0f, 0.3f)] [SerializeField] private float _rotationSmoothTime = 0.12f;
        private PlayerCamera _camera;
        private Transform _transform;
        private float _targetRotation;
        private float _rotationVelocity;

        private void Awake()
        {
            _camera = GetComponent<PlayerCamera>();
            _transform = transform;
        }

        public float Rotate(Vector2 moveDirection)
        {
            if (!isLocalPlayer) return _targetRotation;
            var inputDirection = new Vector3(moveDirection.x, 0.0f, moveDirection.y).normalized;
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _camera.GetCameraEulerAngles().y;

            if (moveDirection == Vector2.zero)
            {
                _transform.rotation = Quaternion.Euler(0.0f, _targetRotation, 0.0f);
                return _targetRotation;
            }

            var rotation = Mathf.SmoothDampAngle(_transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                _rotationSmoothTime);
            _transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            return _targetRotation;
        }
    }
}