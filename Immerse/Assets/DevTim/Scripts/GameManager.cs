using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Immerse
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private StateHandler stateHandler = default;
        [SerializeField] private ScannerResponse scannerResponse = default;
        [SerializeField] private Blame blame = default;
        [SerializeField] private VideoClip openingVideo = default;
        [SerializeField] private VideoClip closingVideo = default;

        [SerializeField] private GameObject gameplayState = default;    
        [SerializeField] private GameObject prompterState = default;
        [SerializeField] private GameObject videoState = default;
        [SerializeField] private GameObject wonState = default;
        [SerializeField] private GameObject loseState = default;

        private VideoViewer videoViewer;
        private GameObject doneState;
        private Prompter prompter;

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
            PlayVideo(closingVideo, won ? wonState : loseState);
        }

        /// <summary>
        /// Called by Button.
        /// </summary>
        public void StartGame() 
        {
            PlayVideo(openingVideo, gameplayState);
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
            stateHandler.Open(prompterState);
            prompter.DisplayPrompt(options);
            this.doneState = doneState;
        }

        private void PlayVideo(VideoClip clip, GameObject doneState) 
        {
            stateHandler.Open(videoState);
            videoViewer.Play(clip);
            this.doneState = doneState;
        }
    }
}
