using UnityEngine;

namespace Immerse
{
    [RequireComponent(typeof(CanvasGroup))]
    public class AlphaLerp : Lerper
    {
        [SerializeField] private float firstAlpha = default;
        [SerializeField] private float secondAlpha = default;

        private CanvasGroup canvasGroup;

        private void Awake() => canvasGroup = GetComponent<CanvasGroup>();

        public override void EnterState()
        {
            base.EnterState();
            canvasGroup.alpha = 1f;
        }

        public override void DoTick()
        {
            base.DoTick();
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, showingFirst ? firstAlpha : secondAlpha, lerpSpeed);
        }
    }
}
