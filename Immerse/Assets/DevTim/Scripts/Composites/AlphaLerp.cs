using UnityEngine;

namespace Immerse
{
    [RequireComponent(typeof(CanvasGroup))]
    public class AlphaLerp : LerperBase
    {
        [SerializeField] private float firstAlpha = default;
        [SerializeField] private float secondAlpha = default;

        private CanvasGroup canvasGroup;

        private void Awake() => canvasGroup = GetComponent<CanvasGroup>();

        private void FixedUpdate()
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, showingFirst ? firstAlpha : secondAlpha, lerpSpeed);
        }
    }
}
