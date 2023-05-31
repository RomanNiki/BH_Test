using UnityEngine;

namespace PlayerComponents
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _speedRateChange = 5f;
        [SerializeField] private DashFx _dashFx;

        private float _velocity;

        public void SetMove(float velocity)
        {
            var clampedVelocity = Mathf.Clamp01(velocity);
            _velocity = Mathf.Lerp(_velocity, clampedVelocity, Time.deltaTime * _speedRateChange);
            _animator.SetFloat(PlayerAnimator.Velocity, _velocity);
        }

        public void Dash(bool isDash = true)
        {
            _animator.SetBool(PlayerAnimator.Dash, isDash);
            if (isDash)
            {
                _dashFx.PlayAnimation();
            }
        }
    }
}