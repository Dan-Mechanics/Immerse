using UnityEngine;

namespace Immerse
{
    /// <summary>
    /// https://discussions.unity.com/t/check-current-microphone-input-volume/474574/17
    /// https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Microphone.html
    /// </summary>
    public class MicrophoneVisual : StateElement
    {
        private const int SAMPLE_WINDOW = 128;

        [SerializeField] private Transform gui = default;
        [SerializeField] private string device = default;

        [Header("Visual")]
        [SerializeField] private float scaleFactor = default;
        [SerializeField] private float rotationFactor = default;
        [SerializeField] private float offset = default;
        [SerializeField] private float lerpFactor = default;

        private AudioClip clip;
        private float volume;
        bool hasStarted;

        private void Awake()
        {
            foreach (string mic in Microphone.devices)
            {
                print($"{mic}.");
            }
        }

        private void StartMicrophone()
        {
            /*if (device == null)
                device = Microphone.devices[0];*/

            clip = Microphone.Start(device, true, 999, 44100);
            hasStarted = true;
            print("Start mic.");
        }

        private void StopMicrophone()
        {
            Microphone.End(device);
            hasStarted = false;
            print("Stop mic.");
        }

        /// <summary>
        /// TODO: use av instead of peak?
        /// </summary>
        private float GetPeakVolume()
        {
            int micPosition = Microphone.GetPosition(null) - (SAMPLE_WINDOW + 1);
            if (micPosition < 0) 
                return 0;

            float[] waveData = new float[SAMPLE_WINDOW];
            float levelMax = 0f;

            clip.GetData(waveData, micPosition);

            for (int i = 0; i < SAMPLE_WINDOW; i++)
            {
                float wavePeak = waveData[i] * waveData[i];
                if (levelMax < wavePeak)
                    levelMax = wavePeak;
            }

            return levelMax;
        }

        public override void DoTick()
        {
            base.DoTick();

            if (!hasStarted)
                return;

            volume = Mathf.Lerp(volume, GetUsableVolume(), lerpFactor);

            gui.localScale = volume * scaleFactor * Vector3.one;
            gui.Rotate(rotationFactor * volume * Vector3.forward, Space.World);
        }

        private float GetUsableVolume() 
        {
            float usable = offset + ConvertToDecibels(GetPeakVolume());
            if (usable < 0f)
                usable = 0f;

            return usable;
        }

        private float ConvertToDecibels(float volume)
        {
            float db = 20f * Mathf.Log10(Mathf.Abs(volume));

            if (float.IsNegativeInfinity(db) || float.IsInfinity(db))
                return 0f;

            return db;
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
            print(focus ? "Focus." : "Unfocus.");
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
