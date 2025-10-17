using UnityEngine;

namespace Immerse
{
    [CreateAssetMenu(fileName = nameof(Object), menuName = nameof(Object))]
    public class Object : ScriptableObject
    {
        public Sprite icon;
        public string description;
        public AudioClip[] interactionSounds;
        public DialogueEvent dialogue;
    }
}
