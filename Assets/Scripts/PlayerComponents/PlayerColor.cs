using Mirror;
using UnityEngine;

namespace PlayerComponents
{
    public class PlayerColor : NetworkBehaviour
    {
        [SerializeField] private Renderer _renderer;
        private Color _startColor;
        private bool _isInvincible;
        private float _invincibilityTimer;

        private void Awake()
        {
            _startColor = _renderer.material.color;
        }

        [ClientRpc]
        public void RpcChangeColor(Color newColor)
        {
            var playerMaterialClone = new Material(_renderer.material)
            {
                color = newColor
            };
            _renderer.material = playerMaterialClone;
        }

        public void SetDefaultColor()
        {
            RpcChangeColor(_startColor);
        }
    }
}