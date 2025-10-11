using UnityEngine;
using UnityEngine.Video;

namespace Immerse
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private StateHandler stateHandler = default;
        [SerializeField] private ScannerResponse scannerResponse;
        
        [SerializeField] private VideoClip openingVideo = default;
        [SerializeField] private VideoClip closingVideo = default;
        [SerializeField] private GameObject gameplayState = default;    
        [SerializeField] private GameObject prompterState = default;
        [SerializeField] private GameObject videoState = default;
        [SerializeField] private GameObject endScreenState = default;

        private VideoViewer videoViewer;
        private GameObject doneState;
        private EndScreen endScreen;
        private Prompter prompter;
        private Blame blame;
        
        private void Awake()
        {
            endScreen = endScreenState.GetComponentInChildren<EndScreen>();
            
            blame = gameplayState.GetComponentInChildren<Blame>();
            blame.OnDisplayQuestion += OnDisplayQuestion;
            blame.OnWinOrLose += OnWinOrLose;
            
            scannerResponse.OnDisplayQuestion += OnDisplayQuestion;
            scannerResponse.OnDialogue += OnDialogue;

            videoViewer = videoState.GetComponentInChildren<VideoViewer>();
            videoViewer.OnVideoDone += OnVideoDone;

            prompter = prompterState.GetComponentInChildren<Prompter>();
        }

        private void OnDestroy()
        {
            videoViewer.OnVideoDone -= OnVideoDone;
            prompter.OnAnswer -= OnAnswer;
            scannerResponse.OnDisplayQuestion -= OnDisplayQuestion;
            scannerResponse.OnDialogue -= OnDialogue;

            blame.OnDisplayQuestion -= OnDisplayQuestion;
            blame.OnWinOrLose -= OnWinOrLose;
        }

        private void OnWinOrLose(bool won)
        {
            endScreen.SetWon(won);
            PlayVideo(closingVideo, endScreenState);
        }

        private void OnDialogue()
        {
            // THIS AUTO-DESTROYS PROMPTS.
            stateHandler.Open(gameplayState);
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
            stateHandler.Open(doneState);
            //doneState = null;
            scannerResponse.Begin();
        }

        private void OnAnswer(int answer)
        {
            prompter.OnAnswer -= OnAnswer;
            stateHandler.Open(doneState);
        }

        private void OnDisplayQuestion(Question question, GameObject doneState)
        {
            // NULL DONESTATE IS ALLOWED BEHAVIOUR.
            if (question == null)
                return;

            this.doneState = doneState;
            stateHandler.Open(prompterState);
            prompter.DisplayQuestion(question);
            prompter.OnAnswer += OnAnswer;
        }

        private void PlayVideo(VideoClip clip, GameObject doneState) 
        {
            this.doneState = doneState;
            stateHandler.Open(videoState);
            videoViewer.Play(clip);
        }
    }
}
