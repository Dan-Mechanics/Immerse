using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    public class MicInput : MonoBehaviour
    {
        #region SingleTon

        public static MicInput Inctance { set; get; }

        #endregion

        public Transform gui;
        public float MicLoudness;
        public float MicLoudnessinDecibels;

        private string _device;

        //mic initialization
        public void InitMic()
        {
            if (_device == null)
            {
                _device = Microphone.devices[0];
            }
            _clipRecord = Microphone.Start(_device, true, 999, 44100);
            _isInitialized = true;
        }

        public void StopMicrophone()
        {
            Microphone.End(_device);
            _isInitialized = false;
        }

        AudioClip _clipRecord;
        int _sampleWindow = 128;

        //get data from microphone into audioclip
        float MicrophoneLevelMax()
        {
            float levelMax = 0;
            float[] waveData = new float[_sampleWindow];
            int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1); // null means the first microphone
            if (micPosition < 0) return 0;
            _clipRecord.GetData(waveData, micPosition);
            // Getting a peak on the last 128 samples
            for (int i = 0; i < _sampleWindow; i++)
            {
                float wavePeak = waveData[i] * waveData[i];
                if (levelMax < wavePeak)
                {
                    levelMax = wavePeak;
                }
            }
            return levelMax;
        }

        //get data from microphone into audioclip
        float MicrophoneLevelMaxDecibels()
        {

            float db = 20 * Mathf.Log10(Mathf.Abs(MicLoudness));

            return db;
        }

        private void FixedUpdate()
        {
            // levelMax equals to the highest normalized value power 2, a small number because < 1
            // pass the value to a static var so we can access it from anywhere
            MicLoudness = MicrophoneLevelMax();
            MicLoudnessinDecibels = -MicrophoneLevelMaxDecibels();
            gui.localScale = MicLoudness *10f* Vector3.one;
        }

        bool _isInitialized;
        // start mic when scene starts
        void OnEnable()
        {
            InitMic();
            _isInitialized = true;
            Inctance = this;
        }

        //stop mic when loading a new level or quit application
        void OnDisable()
        {
            StopMicrophone();
        }

        void OnDestroy()
        {
            StopMicrophone();
        }

        // make sure the mic gets started & stopped when application gets focused
        void OnApplicationFocus(bool focus)
        {
            Debug.Log(focus);
            
            if (focus)
            {
                //Debug.Log("Focus");

                if (!_isInitialized)
                {
                    //Debug.Log("Init Mic");
                    InitMic();
                }
            }
            if (!focus)
            {
                //Debug.Log("Pause");
                StopMicrophone();
                //Debug.Log("Stop Mic");

            }
        }
    }
}

