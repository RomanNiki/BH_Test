using UnityEngine;
using UnityEngine.VFX;

namespace PlayerComponents
{
    public class DashFx : MonoBehaviour
    {
        [SerializeField] private VisualEffect _dashVisualEffect;

        public void PlayAnimation()
        {
            if (_dashVisualEffect != null)
            {
                _dashVisualEffect.Play();
            }
        }
    }
}