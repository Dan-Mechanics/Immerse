using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace Immerse
{
    /// <summary>
    /// Take blame input and read from timer 
    /// to force final blame and all that.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private StateHandler stateHandler = default;
        [SerializeField] private Transform videoState = default;
        private VideoViewer videoViewer;

        private void Awake()
        {
            videoViewer = videoState.GetComponentInChildren<VideoViewer>();
        }

        public void PlayVideo(VideoClip clip) 
        {
            stateHandler.Open(videoState.gameObject);
            videoViewer.Play(clip);
        }
    }
}
