using Mirror;
using ScriptableObjects;
using UnityEngine;

namespace PlayerComponents
{
    public class PlayerLook : NetworkBehaviour
    {
        private const float Threshold = 0.01f;
        [SerializeField] private CameraSettings _cameraSettings;
        [SerializeField] private Transform _cinemachineCameraTarget;
        [SerializeField] private bool _lockCameraPosition;

        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            _cinemachineTargetYaw = _cinemachineCameraTarget.transform.rotation.eulerAngles.y;
        }

        public void MoveLook(Vector2 lookAxis)
        {
            if (!isLocalPlayer)
                return;
            if (lookAxis.sqrMagnitude >= Threshold && !_lockCameraPosition)
            {
                _cinemachineTargetYaw += lookAxis.x;
                _cinemachineTargetPitch += lookAxis.y;
            }
        }

        private void LateUpdate()
        {
            if (!isLocalPlayer)
                return;
            _cinemachineTargetYaw = Extensions.ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = Extensions.ClampAngle(_cinemachineTargetPitch, _cameraSettings.BottomClamp,
                _cameraSettings.TopClamp);

            _cinemachineCameraTarget.rotation = Quaternion.Euler(
                _cinemachineTargetPitch + _cameraSettings.CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }
    }
}