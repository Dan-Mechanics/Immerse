using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

namespace Immerse
{
    /// <summary>
    /// https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Microphone.html
    /// https://discussions.unity.com/t/check-current-microphone-input-volume/474574/17
    /// </summary>
    public class MicrophoneVisual : StateElement
    {
        [SerializeField] private Transform gui = default;
        [SerializeField] private float scaleFactor = default;
        
        private float volume;
        private const int SAMPLE_WINDOW = 128;
        private AudioClip clip;

        /// <summary>
        /// NULL means first microphone.
        /// </summary>
        private readonly string device = null;
        bool hasStarted;

        public void StartMicrophone()
        {
            /*if (device == null)
                device = Microphone.devices[0];*/

            clip = Microphone.Start(device, true, 999, 44100);
            hasStarted = true;
        }

        public void StopMicrophone()
        {
            Microphone.End(device);
            hasStarted = false;
        }

        private float GetVolume()
        {
            float levelMax = 0f;
            float[] waveData = new float[SAMPLE_WINDOW];
            int micPosition = Microphone.GetPosition(null) - (SAMPLE_WINDOW + 1);

            if (micPosition < 0) 
                return 0;

            clip.GetData(waveData, micPosition);

            for (int i = 0; i < SAMPLE_WINDOW; i++)
            {
                float wavePeak = waveData[i] * waveData[i];
                if (levelMax < wavePeak)
                {
                    levelMax = wavePeak;
                }
            }

            return levelMax;
        }

        public override void DoTick()
        {
            base.DoTick();

            if (!hasStarted)
                return;

            volume = GetVolume();
            gui.localScale = volume * scaleFactor * Vector3.one;
        }

        public override void Open()
        {
            base.Open();
            StartMicrophone();
        }

        public override void Close()
        {
            base.Close();
            StopMicrophone();
        }

        private void OnApplicationFocus(bool focus)
        {
            print(focus ? "focus" : "unfocus");

            if (!focus)
            {
                StopMicrophone();
                return;
            }

            if (!hasStarted)
                StartMicrophone();
        }
    }
}
