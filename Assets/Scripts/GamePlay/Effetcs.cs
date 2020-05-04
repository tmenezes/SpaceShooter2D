using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public static class Effetcs
    {
        private static Shader _whiteShader = null;

        public static void Load()
        {
            _whiteShader = _whiteShader ?? (_whiteShader = Shader.Find("GUI/Text Shader"));
        }

        public static Sequence StartInvulnerableEffect(SpriteRenderer renderer, float duration = 1f)
        {
            var interval = 0.2f;
            var loops = Mathf.CeilToInt(duration / interval / 2);
            var originalShader = renderer.material.shader;

            return DOTween.Sequence()
                   .AppendCallback(() => renderer.material.shader = _whiteShader)
                   .AppendInterval(interval)
                   .AppendCallback(() => renderer.material.shader = originalShader)
                   .AppendInterval(interval)
                   .SetLoops(loops)
                   .SetEase(Ease.Flash)
                   .Play();
        }
    }
}
