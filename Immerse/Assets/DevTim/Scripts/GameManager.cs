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
            blame.OnBlame += OnBlame;
            
            // scannerResponse = gameplayState.GetComponentInChildren<ScannerResponse>();
            scannerResponse.OnDisplayQuestion += OnDisplayQuestion;
            scannerResponse.OnBlameActorIndex += blame.BlameActorIndex;
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
            scannerResponse.OnBlameActorIndex -= blame.BlameActorIndex;
            scannerResponse.OnDialogue -= OnDialogue;

            blame.OnDisplayQuestion -= OnDisplayQuestion;
            blame.OnBlame -= OnBlame;
        }

        private void OnBlame(bool won)
        {
            endScreen.SetWon(won);
            PlayVideo(closingVideo, endScreenState);
        }

        private void OnDialogue()
        {
            if (prompter.Locked)
                return;

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
            doneState = null;
            scannerResponse.Begin();
        }

        private void OnAnswer(int answer)
        {
            prompter.OnAnswer -= OnAnswer;
            stateHandler.Open(doneState);
        }

        private bool OnDisplayQuestion(Question question, GameObject doneState)
        {
            if (question == null || doneState == null)
                return false;

            if (!prompter.DisplayQuestion(question))
                return false;

            stateHandler.Open(prompterState);
            //prompter.DisplayQuestion(question);
            prompter.OnAnswer += OnAnswer;
            this.doneState = doneState;
            return true;
        }

        private void PlayVideo(VideoClip clip, GameObject doneState) 
        {
            stateHandler.Open(videoState);
            videoViewer.Play(clip);
            this.doneState = doneState;
        }
    }
}
