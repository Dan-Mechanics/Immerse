using UnityEngine;

namespace Immerse
{
    public class Screen : MonoBehaviour
    {
        protected bool isOpen;
        
        public virtual void DoTick() { }
        public virtual void DoFrame() { }

        public virtual void Enter()
        { 
            isOpen = true;
            gameObject.SetActive(true);
        }

        public virtual void Exit() 
        { 
            isOpen = false;
            gameObject.SetActive(false);
        }
    }
}
