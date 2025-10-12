using UnityEngine;

namespace Immerse
{
    [CreateAssetMenu(fileName = nameof(DialogueEvent), menuName = nameof(DialogueEvent))]
    public class DialogueEvent : ScriptableObject
    {
        public AudioClip clip;
        public string script;
        [HideInInspector] public Actor actor;

        private void OnValidate()
        {
            if (clip != null && clip.name != name)
                Debug.LogWarning($"It might be a good idea to name '{name}' '{clip.name}', or the other way around.");
        }
    }
}
