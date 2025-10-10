using UnityEngine;

namespace Immerse
{
    /// <summary>
    /// https://discussions.unity.com/t/check-current-microphone-input-volume/474574/17
    /// https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Microphone.html
    /// 
    /// TODO: make mic always active unless destroy focus etc.
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
        bool micRunning;

        private void Awake()
        {
            foreach (string mic in Microphone.devices)
            {
                print($"{mic}.");
            }

            StartMicrophone();
        }

        private void StartMicrophone()
        {
            if (micRunning)
                return;

            print("starting mic ...");
            micRunning = true;
            clip = Microphone.Start(device, true, 999, 44100);
        }

        private void StopMicrophone()
        {
            if (!micRunning)
                return;

            print("stopping mic ...");
            Microphone.End(device);
            micRunning = false;
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

            if (!micRunning)
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

        public override void OnDestroy()
        {
            base.OnDestroy();
            StopMicrophone();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                StartMicrophone();
            }
            else
            {
                StopMicrophone();
            }
        }
    }
}
