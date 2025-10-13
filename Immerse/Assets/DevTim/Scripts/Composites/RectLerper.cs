using UnityEngine;

namespace Immerse
{
    [RequireComponent(typeof(RectTransform))]
    public class RectLerper : Lerper
    {
        [SerializeField] private Vector2 first = default;
        [SerializeField] private Vector2 second = default;

        private RectTransform rect;

        private void Awake() => rect = GetComponent<RectTransform>();

        public override void DoTick()
        {
            base.DoTick();
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, showingFirst ? first : second, lerpSpeed);
        }

        public override void Force()
        {
            base.Force();
            rect.anchoredPosition = showingFirst ? first : second;
        }
    }
}
