using UnityEngine;

namespace Immerse
{
    public abstract class Entity : ScriptableObject
    {
        public Sprite icon;
        public string description;
        public AudioClip[] interactionSounds;

        [HideInInspector] public int index;

        public virtual void Setup() { }
    }
}
