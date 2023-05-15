using UnityEngine;

namespace UI
{
    public class NicknameController : MonoBehaviour
    {
        [SerializeField] private Transform _nicknameTransform;

        private Transform _mainCameraTransform;

        private void Start()
        {
            _mainCameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            _nicknameTransform.rotation =
                Quaternion.LookRotation(_nicknameTransform.position - _mainCameraTransform.position);
        }
    }
}