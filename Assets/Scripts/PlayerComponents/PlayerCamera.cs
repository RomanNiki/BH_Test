using Cinemachine;
using Mirror;
using UnityEngine;

namespace PlayerComponents
{
    public class PlayerCamera : NetworkBehaviour
    {
        [SerializeField] private Transform _cinemachineCameraTarget;
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        private Camera _camera;
        
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            _camera = Camera.main;
            _cinemachineVirtualCamera.Follow = _cinemachineCameraTarget;
        }
        
        public Vector3 GetCameraEulerAngles()
        {
            return _camera.transform.eulerAngles;
        }
    }
}