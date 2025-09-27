using UnityEngine;

namespace Immerse
{
    public abstract class LerperBase : MonoBehaviour, IReceiver<bool>
    {
        [SerializeField] protected float lerpSpeed = default;
        [SerializeField] protected bool showingFirst = true;

        public void Toggle() => showingFirst = !showingFirst;
        public void Send(bool value) => showingFirst = value;
    }
}
