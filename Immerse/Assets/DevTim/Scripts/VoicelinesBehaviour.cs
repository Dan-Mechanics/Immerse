using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    public class VoicelinesBehaviour : MonoBehaviour, IVoicelineService
    {
        [SerializeField] private List<AudioClip> clips = default;
        private readonly Voicelines voicelines = new Voicelines(':');

        private void Awake() 
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.dopplerLevel = 0f;

            voicelines.Setup(source, clips);
            InvokeRepeating(nameof(Tick), 0.1f, 0.1f);

            ServiceLocator<IVoicelineService>.Provide(this);
        }

        private void Tick() => voicelines.Update();
        public void Play(string name) => voicelines.Filter(name);
    }
}
