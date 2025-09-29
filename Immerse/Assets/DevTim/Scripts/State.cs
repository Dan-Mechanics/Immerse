using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    public abstract class State : MonoBehaviour
    {
        public virtual void EnterState() { }
        public virtual void ExitState() { }
        public virtual void DoTick() { }
        public virtual void DoFrame() { }
    }
}
