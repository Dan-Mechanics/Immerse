using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public class Blame : State
    {
        public event Action<bool> OnBlame;
        public event Action<Prompter.Option[], GameObject> OnPrompt;

        [SerializeField] private GameObject gameplayState = default;
        [SerializeField] private TextWriter textWriter = default;
        [SerializeField] private Timer timer = default;
        [SerializeField] private Prompter prompter = default;
        [SerializeField] private Button blameButton = default;
        [SerializeField] private Holder holder = default;
        [SerializeField] private Actor murderer = default;
        [SerializeField] private float forceBlameMinutes = default;
        [SerializeField] private Prompter.Option template = default;
        [SerializeField] private Prompter.Option cancel = default;

        private Actor[] actors = default;
        private Prompter.Option[] blameOptionsCancel;
        private Prompter.Option[] blameOptions;

        private bool hasStarted;

        private void Awake()
        {
            actors = holder.Actors.Values.ToArray();

            blameOptions = new Prompter.Option[actors.Length];
            blameOptionsCancel = new Prompter.Option[actors.Length + 1];

            for (int i = 0; i < blameOptions.Length; i++)
            {
                blameOptions[i] = template;
                blameOptions[i].text = $"Blame {actors[i].name}!";
                blameOptions[i].icon = actors[i].icon;
                blameOptionsCancel[i] = blameOptions[i];
            }

            blameOptionsCancel[^1] = cancel;

            prompter.OnAnswer += OnAnswer;
            timer.OnNewTime += OnNewTime;
        }

        private void OnDestroy()
        {
            prompter.OnAnswer -= OnAnswer;
            timer.OnNewTime -= OnNewTime;
        }

        private void OnNewTime(int minutes, int seconds)
        {
            textWriter.Write($"{minutes}:{seconds}");

            if (minutes >= forceBlameMinutes)
                ForceBlame();
        }

        public override void EnterState()
        {
            base.EnterState();
            blameButton.onClick.AddListener(AskBlame);

            if (!hasStarted)
            {
                timer.Begin();
                hasStarted = true;
            }
        }

        public override void ExitState()
        {
            base.ExitState();
            blameButton.onClick.RemoveAllListeners();
        }

        private void OnAnswer(int index)
        {
            // CHECK VALID ANSWER.
            if (index >= 0 && index < actors.Length)
                OnBlame?.Invoke(actors[index] == murderer);
        }

        private void AskBlame()
        {
            OnPrompt?.Invoke(blameOptionsCancel, gameplayState);
        }

        private void ForceBlame()
        {
            OnPrompt?.Invoke(blameOptions, null);
        }
    }
}
