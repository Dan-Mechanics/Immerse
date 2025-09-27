using UnityEngine;

namespace Immerse
{
    public class ScaleLerper : LerperBase
    {
        [SerializeField] private Vector3 first = default;
        [SerializeField] private Vector3 second = default;

        private void OnEnable()
        {
            transform.localScale = showingFirst ? first : second;
        }

        private void FixedUpdate()
        {
            transform.localScale = Vector2.Lerp(transform.localScale, showingFirst ? first : second, lerpSpeed);
        }
    }
}
