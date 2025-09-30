using System;
using UnityEngine;
using UnityEngine.Video;

namespace Immerse
{
    public class VideoViewer : Behaviour
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

            if (Time.time < doneTime)
                return;

            OnVideoDone?.Invoke();
        }

        public void Play(VideoClip clip)
        {
            // WE WOULD MERELY DISABLE IT IF WE HAD RESET FUNCTIONALITY. 
            Destroy(preview);
            videoPlayer.clip = clip;
            videoPlayer.Play();
            doneTime = Time.time + (float)clip.length;
        }

        public override void EnterState()
        {
            base.EnterState();
            Cursor.visible = false;
            doneTime = 0f;
            videoPlayer.Stop();
        }

        public override void ExitState()
        {
            base.ExitState();
            Cursor.visible = true;

            if (videoPlayer != null)
                videoPlayer.Stop();
        }
    }
}
