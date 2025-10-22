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
        [SerializeField] private GameObject videoState = default;
        [SerializeField] private GameObject endScreenState = default;

        private VideoViewer videoViewer;
        private GameObject doneState;
        private EndScreen endScreen;
        private BlameHandler blame;
        
        private void Awake()
        {
            endScreen = endScreenState.GetComponentInChildren<EndScreen>();
            blame = gameplayState.GetComponentInChildren<BlameHandler>();
            videoViewer = videoState.GetComponentInChildren<VideoViewer>();
            
            blame.OnWinOrLose += OnWinOrLose;
            videoViewer.OnVideoDone += OnVideoDone;
        }

        private void OnDestroy()
        {
            blame.OnWinOrLose               -= OnWinOrLose;
            videoViewer.OnVideoDone         -= OnVideoDone;
        }

        /// <summary>
        /// Called by button.
        /// </summary>
        public void StartGame() 
        {
            PlayVideo(openingVideo, gameplayState);
        }

        private void OnVideoDone()
        {
            stateHandler.Open(doneState);
            doneState = null;
        }

        private void PlayVideo(VideoClip clip, GameObject doneState) 
        {
            this.doneState = doneState;
            stateHandler.Open(videoState);
            videoViewer.Play(clip);
        }

        private void OnWinOrLose(bool won)
        {
            endScreen.SetWon(won);
            PlayVideo(closingVideo, endScreenState);
        }
    }
}
