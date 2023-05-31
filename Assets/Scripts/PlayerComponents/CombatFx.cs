using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.VFX;

namespace PlayerComponents
{
    public class CombatFx : NetworkBehaviour
    {
        [SerializeField] private VisualEffect _damageVisualEffect;
        [SerializeField] private Renderer _renderer;
        private Material _material;

        private void Awake()
        {
            var sharedMaterial = new Material(_renderer.material);
            _renderer.material = sharedMaterial;
            _material = sharedMaterial;
        }

        [ClientRpc]
        public void RpcPlayerAnimations(float duration)
        {
            if (_damageVisualEffect != null)
            {
                _damageVisualEffect.Play();
            }
            _material.SetFloat(PlayerShader.IsInvisible, 1f);
            StartCoroutine(ChangeColorBack(duration));
        }

        private IEnumerator ChangeColorBack(float duration)
        {
            yield return new WaitForSeconds(duration);
            _material.SetFloat(PlayerShader.IsInvisible, 0f);;
        }
    }
}