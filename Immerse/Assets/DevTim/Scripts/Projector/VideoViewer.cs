using System;
using UnityEngine;
using UnityEngine.Video;

namespace Immerse
{
    public class VideoViewer : StateElement
    {
        public event Action OnVideoDone;
        
        [SerializeField] private VideoPlayer videoPlayer = default;
        [SerializeField] private GameObject preview = default;
        [SerializeField] private GameObject rawImage = default;

        private float doneTime;

        public override void DoTick()
        {
            base.DoTick();

            rawImage.SetActive(doneTime > 0f && videoPlayer.isPrepared);

            if (doneTime <= 0f)
                return;

            if (Time.time < doneTime && !Input.GetKey(KeyCode.Return))
                return;

            OnVideoDone?.Invoke();
            doneTime = -1f;
        }

        public void Play(VideoClip clip)
        {
            Cursor.visible = false;
            rawImage.SetActive(false);
            videoPlayer.clip = clip;
            videoPlayer.Play();
            doneTime = Time.time + (float)clip.length;
            Destroy(preview);
        }

        public override void Close()
        {
            base.Close();
            rawImage.SetActive(false);
            Cursor.visible = true;
            videoPlayer.Stop();
            doneTime = -1f;
        }
    }
}
