using System;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public class Blame : StateElement
    {
        public event Action<bool> OnWinOrLose;
        public event Action<Question, GameObject> OnDisplayQuestion;

        [SerializeField] private GameObject scanner = default;
        [SerializeField] private GameObject gameplayState = default;
        [SerializeField] private TextWriter textWriter = default;
        [SerializeField] private Timer timer = default;
        [SerializeField] private Prompter prompter = default;
        [SerializeField] private Button blameButton = default;
        [SerializeField] private Holder holder = default;
        [SerializeField] private Actor murderer = default;
        [SerializeField] private Question softBlame = default;
        [SerializeField] private Question forceBlame = default;

        private bool hasStarted;

        private void Awake()
        {
            for (int i = 0; i < holder.Actors.Count; i++)
            {
                softBlame.options[i].icon = holder.Actors[i].icon;
                softBlame.options[i].text = $"Beschuldig {Utils.CapitilizeFirst(holder.Actors[i].name)}!";

                forceBlame.options[i].icon = softBlame.options[i].icon;
                forceBlame.options[i].text = softBlame.options[i].text;
            }
        }

        private void OnNewTime(int minutes, int seconds) => textWriter.Write($"{minutes}:{seconds}");

        public override void Open()
        {
            base.Open();
            blameButton.onClick.AddListener(AskBlame);
            timer.OnNewTime += OnNewTime;
            timer.OnDone += ForceBlame;

            if (!hasStarted)
                timer.Begin();

            hasStarted = true;
        }

        public override void Close()
        {
            base.Close();
            blameButton.onClick.RemoveAllListeners();
            prompter.OnAnswer -= BlameActorIndex;
            timer.OnNewTime -= OnNewTime;
            timer.OnDone -= ForceBlame;
        }

        public void BlameActorIndex(int index)
        {
            if (index >= 0 && index < holder.Actors.Count)
            {
                StopScanning();
                OnWinOrLose?.Invoke(holder.Actors[index] == murderer);
            }

            prompter.OnAnswer -= BlameActorIndex;
        }

        private void AskBlame()
        {
            OnDisplayQuestion?.Invoke(softBlame, gameplayState);
            prompter.OnAnswer += BlameActorIndex;
        }

        private void StopScanning()
        {
            if (scanner == null)
                return;

            scanner.SetActive(false);
            Destroy(scanner);
        }

        private void ForceBlame()
        {
            StopScanning();
            
            // TIMER CALLS ITS OWN CLOSE(),
            // SO I THINK THIS MAKES SENSE HERE.
            timer.OnDone -= ForceBlame;
            OnDisplayQuestion?.Invoke(forceBlame, null);
            prompter.OnAnswer += BlameActorIndex;
        }
    }
}
