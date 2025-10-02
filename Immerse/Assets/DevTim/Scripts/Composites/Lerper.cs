using UnityEngine;

namespace Immerse
{
    public abstract class Lerper : StateElement
    {
        [SerializeField] protected float lerpSpeed = default;
        [SerializeField] protected bool showingFirst = default;

        public void Toggle() => showingFirst = !showingFirst;
        public void Send(bool value) => showingFirst = value;
        public virtual void Force() { showingFirst = true; }
    }
}
