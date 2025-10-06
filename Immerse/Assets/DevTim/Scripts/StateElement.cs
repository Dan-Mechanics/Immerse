using UnityEngine;

namespace Immerse
{
    /// <summary>
    /// Possibly add in the future
    /// reset feature so you dont have
    /// to reload the scene.
    /// </summary>
    public abstract class StateElement : MonoBehaviour
    {
        public virtual void Open() { } //=> print($"<b><color=green>Open() --> [{gameObject.name}].</color></b>");
        public virtual void Close() { } //=> print($"<b><color=red>Close() --> [{gameObject.name}].</color></b>");
        public virtual void DoTick() { }
        public virtual void DoFrame() { }
        public virtual void OnDestroy() => Close();
    }
}
