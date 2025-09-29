using UnityEngine;

namespace Immerse
{
    public class ScaleLerper : Lerper
    {
        [SerializeField] private Vector3 first = default;
        [SerializeField] private Vector3 second = default;

        public override void DoTick()
        {
            base.DoTick();
            transform.localScale = Vector2.Lerp(transform.localScale, showingFirst ? first : second, lerpSpeed);
        }
    }
}
