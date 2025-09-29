using UnityEngine;

namespace Immerse
{
    public abstract class Lerper : State
    {
        [SerializeField] protected float lerpSpeed = default;
        [SerializeField] protected bool showingFirst = default;

        public void Toggle() => showingFirst = !showingFirst;
        public void Send(bool value) => showingFirst = value;
    }
}
