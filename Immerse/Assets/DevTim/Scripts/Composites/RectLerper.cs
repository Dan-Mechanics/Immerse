using UnityEngine;

namespace Immerse
{
    [RequireComponent(typeof(RectTransform))]
    public class RectLerper : LerperBase
    {
        [SerializeField] private Vector2 first = default;
        [SerializeField] private Vector2 second = default;

        private RectTransform rect;

        private void Awake() => rect = GetComponent<RectTransform>();

        private void OnEnable()
        {
            rect.anchoredPosition = showingFirst ? first : second;
        }

        private void FixedUpdate()
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, showingFirst ? first : second, lerpSpeed);
        }
    }
}
