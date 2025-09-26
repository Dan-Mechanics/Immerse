using UnityEngine;

namespace Immerse
{
    [CreateAssetMenu(fileName = "VideoEvent", menuName = "VideoEvent")]
    public class VideoEvent : ScriptableObject
    {
        public Actor actor;
        public AudioClip clip;
        public string script = "Sample text.";

        private void OnValidate()
        {
            if (clip != null && clip.name != name)
                Debug.LogWarning($"It might be a good idea to name '{name}' '{clip.name}', or the other way around.");
        }
    }
}
