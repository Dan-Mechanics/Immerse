using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

namespace Immerse
{
    /// <summary>
    /// https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Microphone.html
    /// </summary>
    public class MicrophoneVisual : StateElement
    {
        public AudioSource source;
        public float volume;
        public Transform gui;

        void Start()
        {
            foreach (var item in Microphone.devices)
            {
                print(item);
            }
            
            
            AudioSource audioSource = GetComponent<AudioSource>();
            source.clip = Microphone.Start(Microphone.devices[0], true, 1, 44100);
            source.Play();
        }

        private void FixedUpdate()
        {
            float[] data = new float[735];
            source.GetOutputData(data, 0);
            //take the median of the recorded samples
            ArrayList s = new ArrayList();
            foreach (float f in data)
            {
                s.Add(Mathf.Abs(f));
            }
            s.Sort();
            volume = (float)s[735 / 2];
            gui.localScale = 10f * volume * Vector3.one;
        }
    }
}
