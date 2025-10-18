using UnityEngine;

namespace Immerse
{
    [CreateAssetMenu(fileName = nameof(DialogueEvent), menuName = nameof(DialogueEvent))]
    public class DialogueEvent : ScriptableObject
    {
        public AudioClip clip;
        public string script;

        [HideInInspector] public Actor actor;
        [HideInInspector] public int index;

        public void OnValidate()
        {
            clip = Resources.Load<AudioClip>(name);

            /*if (clip == null)
                Debug.LogWarning($"{nameof(clip)} is null.");*/
        }
    }
}
