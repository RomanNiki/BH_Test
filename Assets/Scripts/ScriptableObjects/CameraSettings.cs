using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "Settings/Player/Camera")]
    public class CameraSettings : ScriptableObject
    {
        [SerializeField] private float _topClamp = 70.0f;
        [SerializeField] private float _bottomClamp = -30.0f;
        [SerializeField] private float _cameraAngleOverride;
        
        public float TopClamp => _topClamp;
        public float BottomClamp => _bottomClamp;
        public float CameraAngleOverride => _cameraAngleOverride;
    }
}