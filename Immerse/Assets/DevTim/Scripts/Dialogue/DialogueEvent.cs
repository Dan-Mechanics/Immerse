using UnityEngine;

namespace Immerse
{
    [CreateAssetMenu(fileName = nameof(DialogueEvent), menuName = nameof(DialogueEvent))]
    public class DialogueEvent : ScriptableObject
    {
        public Actor actor;
        public AudioClip clip;
        public string script;

        private void OnValidate()
        {
            if (clip != null && clip.name != name)
                Debug.LogWarning($"It might be a good idea to name '{name}' '{clip.name}', or the other way around.");
        }
    }
}
