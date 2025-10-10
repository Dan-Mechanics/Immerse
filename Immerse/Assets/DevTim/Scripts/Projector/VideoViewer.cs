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

        private float doneTime;

        public override void DoTick()
        {
            base.DoTick();

            if (doneTime <= 0f)
                return;

            if (Time.time < doneTime && !Input.GetKey(KeyCode.Return))
                return;

            OnVideoDone?.Invoke();
            doneTime = -1f;
        }

        public void Play(VideoClip clip)
        {
            // WE WOULD MERELY DISABLE IT IF WE HAD RESET FUNCTIONALITY. 
            videoPlayer.clip = clip;
            videoPlayer.Play();
            Cursor.visible = false;
            doneTime = Time.time + (float)clip.length;
            Destroy(preview);
        }

        public override void Close()
        {
            base.Close();
            Cursor.visible = true;
            videoPlayer.Stop();
            doneTime = -1f;
        }
    }
}
