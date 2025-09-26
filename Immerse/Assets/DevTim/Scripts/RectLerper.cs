using UnityEngine;

namespace Immerse
{
    [RequireComponent(typeof(RectTransform))]
    public class RectLerper : MonoBehaviour, IReceiver<bool>
    {
        public bool showingSecondPosition;
        
        [SerializeField] private Vector2 first = default;
        [SerializeField] private Vector2 second = default;
        [SerializeField] private float lerpSpeed = default;

        private RectTransform rect;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        private void FixedUpdate()
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, !showingSecondPosition ? first : second, lerpSpeed);
        }

        public void Toggle()
        {
            showingSecondPosition = !showingSecondPosition;
        }

        public void Send(bool value)
        {
            showingSecondPosition = value;
        }
    }
}
