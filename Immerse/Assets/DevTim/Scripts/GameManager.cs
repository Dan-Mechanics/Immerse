using System;
using TMPro;
using UnityEngine;
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
        [SerializeField] private ScannerResponse scannerResponse = default;
        [SerializeField] private Blame blame = default;
        [SerializeField] private VideoClip gameOverVideo = default;

        [SerializeField] private GameObject gameplayState = default;    
        [SerializeField] private GameObject prompterState = default;
        [SerializeField] private GameObject videoState = default;
        [SerializeField] private GameObject wonState = default;
        [SerializeField] private GameObject loseState = default;

        private VideoViewer videoViewer;
        private Prompter prompter;
        private GameObject doneState;

        private void Awake()
        {
            videoViewer = videoState.GetComponentInChildren<VideoViewer>();
            videoViewer.OnVideoDone += OnVideoDone;

            prompter = prompterState.GetComponentInChildren<Prompter>();
            prompter.OnAnswer += OnAnswer;

            scannerResponse.OnPrompt += OnPrompt;
            blame.OnPrompt += OnPrompt;
            blame.OnBlame += OnBlame;
        }

        private void OnDestroy()
        {
            videoViewer.OnVideoDone -= OnVideoDone;
            prompter.OnAnswer -= OnAnswer;
            scannerResponse.OnPrompt -= OnPrompt;
            blame.OnPrompt -= OnPrompt;
            blame.OnBlame -= OnBlame;
        }

        private void OnBlame(bool won)
        {
            PlayVideo(gameOverVideo, won ? wonState : loseState);
        }

        private void OnVideoDone()
        {
            if (doneState == null)
                return;

            stateHandler.Open(doneState);
        }

        private void OnAnswer(int answer)
        {
            if (doneState == null)
                return;

            stateHandler.Open(doneState);
        }

        private void OnPrompt(Prompter.Option[] options, GameObject doneState)
        {
            stateHandler.Open(prompterState.gameObject);
            this.doneState = doneState;
            prompter.DisplayPrompt(options);
        }

        private void PlayVideo(VideoClip clip, GameObject doneState) 
        {
            stateHandler.Open(videoState.gameObject);
            videoViewer.Play(clip);
            this.doneState = doneState;
        }
    }
}
