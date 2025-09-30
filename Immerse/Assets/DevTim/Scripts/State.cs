using UnityEngine;

namespace Immerse
{
    /// <summary>
    /// Possibly add in the future
    /// reset feature so you dont have
    /// to reload the scene.
    /// </summary>
    public abstract class State : MonoBehaviour
    {
        public virtual void EnterState() { print($"EnterState() --> [{gameObject.name}]"); }
        //public virtual void ExitState() { print($"ExitState() --> '{gameObject.name}'."); }
        public virtual void ExitState() { }
        public virtual void DoTick() { }
        public virtual void DoFrame() { }
    }
}
