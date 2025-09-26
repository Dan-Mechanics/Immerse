using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    public class ScaleLerper : MonoBehaviour, IReceiver<bool>
    {
        public bool showingSecondScale;

        [SerializeField] private Vector3 first = default;
        [SerializeField] private Vector3 second = default;
        [SerializeField] private float lerpSpeed = default;

        private void FixedUpdate()
        {
            transform.localScale = Vector2.Lerp(transform.localScale, !showingSecondScale ? first : second, lerpSpeed);
        }

        public void Toggle()
        {
            showingSecondScale = !showingSecondScale;
        }

        public void Send(bool value)
        {
            showingSecondScale = value;
        }
    }
}
