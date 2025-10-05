using UnityEngine;

namespace Immerse
{
    [CreateAssetMenu(fileName = nameof(Actor), menuName = nameof(Actor))]
    public class Actor : ScriptableObject
    {
        public Sprite icon;
        public string description;
        public DialogueEvent[] dialogue;

        [HideInInspector] public int index;
    }
}
