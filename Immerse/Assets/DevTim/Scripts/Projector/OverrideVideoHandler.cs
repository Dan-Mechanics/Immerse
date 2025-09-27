using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace Immerse
{
    public class OverrideVideoHandler : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer = default;
        [SerializeField] private GameObject preview = default;
        [SerializeField] private GameObject backdrop = default;
        [SerializeField] private UnityEvent onDone = default;

        private float doneTime;

        private void FixedUpdate()
        {
            if (doneTime <= 0f)
                return;

            if (Time.time < doneTime)
                return;

            // IDEA: MAKE UNIVERSAL MOUSE LOCKER.
            Cursor.visible = true;
            backdrop.SetActive(false);
            onDone?.Invoke();
            doneTime = 0f;
        }

        public void Play(VideoClip clip)
        {
            Destroy(preview);
            Cursor.visible = false;

            videoPlayer.Stop();
            videoPlayer.clip = clip;
            videoPlayer.Play();
            doneTime = Time.time + (float)clip.length;
            backdrop.SetActive(true);
        }
    }
}
