using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Immerse
{
    public class OverrideVideoHandler : MonoBehaviour
    {
        //[SerializeField] private VideoClip current = default;

        [SerializeField] private VideoPlayer videoPlayer = default;
        [SerializeField] private GameObject preview = default;
        [SerializeField] private GameObject backdrop = default;

        private float doneTime;
        private bool started;

        private void FixedUpdate()
        {
            if (started)
                backdrop.SetActive(Time.time < doneTime);
        }

        public void Play(VideoClip clip)
        {
            Destroy(preview);
            videoPlayer.Stop();
            videoPlayer.clip = clip;
            videoPlayer.Play();
            doneTime = Time.time + (float)clip.length;
            started = true;
        }
    }
}
