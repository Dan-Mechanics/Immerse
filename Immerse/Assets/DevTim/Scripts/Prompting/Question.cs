using UnityEngine;

namespace Immerse
{
    [CreateAssetMenu(fileName = nameof(Question), menuName = nameof(Question))]
    public class Question : ScriptableObject
    {
        public string question;
        public int processLength;
        public float saturation;
        public Color a;
        public Color b;
        public AudioClip clip;
        public bool includeOptional;
        public Option[] options;
    }
}
