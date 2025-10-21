using UnityEngine;

namespace Immerse
{
    [CreateAssetMenu(fileName = nameof(Question), menuName = nameof(Question))]
    public class Question : ScriptableObject
    {
        public string message;
        public AudioClip clip;
        public bool includeOptional;
        public Option[] options;
        [Header("Tools")]
        [Min(0)] public int start;
        [Min(0)] public int end;
        [Range(0f, 1f)] public float saturation;
        public Color a = Color.white;
        public Color b = Color.white;

        [ContextMenu(nameof(UpdateColorTooling))]
        private void UpdateColorTooling() 
        {
            if (saturation <= 0f)
                return;

            if (end < 2)
                return;

            if (start < 0 || end < 0)
                return;

            if (start >= end)
                return;

            if (end >= options.Length)
                return;

            Debug.Log($"coloring {nameof(Question)} ...");
            for (int i = start; i < end; i++)
            {
                float lerp = (float)i / (end - 1);
                options[i].color = Color.Lerp(options[i].color, Color.Lerp(a, b, lerp), saturation);
            }
        }
    }
}
