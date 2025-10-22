using UnityEngine;

namespace Immerse
{
    /// <summary>
    /// Possibly add in the future:
    /// reset feature so you don't have
    /// to reload the scene.
    /// </summary>
    public abstract class StateElement : MonoBehaviour
    {
        public virtual void Open() { }
        public virtual void Close() { }
        public virtual void DoTick() { }
        public virtual void DoFrame() { }
        public virtual void OnDestroy() => Close();
    }
}
