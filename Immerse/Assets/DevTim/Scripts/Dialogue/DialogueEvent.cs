using UnityEngine;

namespace Immerse
{
    [CreateAssetMenu(fileName = nameof(DialogueEvent), menuName = nameof(DialogueEvent))]
    public class DialogueEvent : ScriptableObject
    {
        public AudioClip clip;
        public string script;

        [HideInInspector] public Entity owner;
        [HideInInspector] public int index;

        public void Setup()
        {
            clip = Resources.Load<AudioClip>($"Sounds/{name}");
        }
    }
}
