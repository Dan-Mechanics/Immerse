using UnityEngine;
using UnityEngine.Video;

namespace Immerse
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private StateHandler stateHandler = default;
        
        [SerializeField] private VideoClip openingVideo = default;
        [SerializeField] private VideoClip closingVideo = default;
        [SerializeField] private GameObject gameplayState = default;    
        [SerializeField] private GameObject prompterState = default;
        [SerializeField] private GameObject videoState = default;
        [SerializeField] private GameObject endScreenState = default;

        private ScannerResponse scannerResponse;
        private VideoViewer videoViewer;
        private GameObject doneState;
        private EndScreen endScreen;
        private Prompter prompter;
        private Blame blame;
        
        private void Awake()
        {
            endScreen = endScreenState.GetComponentInChildren<EndScreen>();
            
            blame = gameplayState.GetComponentInChildren<Blame>();
            blame.OnRequestPrompt += OnRequestPrompt;
            blame.OnBlame += OnBlame;
            
            scannerResponse = gameplayState.GetComponentInChildren<ScannerResponse>();
            scannerResponse.OnRequestPrompt += OnRequestPrompt;

            videoViewer = videoState.GetComponentInChildren<VideoViewer>();
            videoViewer.OnVideoDone += OnVideoDone;

            prompter = prompterState.GetComponentInChildren<Prompter>();
        }

        private void OnDestroy()
        {
            videoViewer.OnVideoDone -= OnVideoDone;
            prompter.OnAnswer -= OnAnswer;
            scannerResponse.OnRequestPrompt -= OnRequestPrompt;
            blame.OnRequestPrompt -= OnRequestPrompt;
            blame.OnBlame -= OnBlame;
        }

        private void OnBlame(bool won)
        {
            endScreen.SetWon(won);
            PlayVideo(closingVideo, endScreenState);
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
            /*if (doneState == null)
                return;*/

            stateHandler.Open(doneState);
        }

        private void OnAnswer(int answer)
        {
            prompter.OnAnswer -= OnAnswer;

            /*if (doneState == null)
                return;*/

            stateHandler.Open(doneState);
        }

        private void OnRequestPrompt(Prompter.Option[] options, GameObject doneState)
        {
            stateHandler.Open(prompterState);
            prompter.DisplayPrompt(options);
            prompter.OnAnswer += OnAnswer;
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
