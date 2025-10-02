using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public class Blame : StateElement
    {
        public event Action<bool> OnBlame;
        public event Action<Prompter.Option[], GameObject> OnRequestPrompt;
        private List<Actor> Actors => holder.Actors;

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

        private Prompter.Option[] blameOptionsCancel;
        private Prompter.Option[] blameOptions;
        private bool hasStarted;

        private void Awake()
        {
            blameOptions = new Prompter.Option[Actors.Count];
            blameOptionsCancel = new Prompter.Option[Actors.Count + 1];

            for (int i = 0; i < blameOptions.Length; i++)
            {
                blameOptions[i] = template;
                blameOptions[i].text = $"Blame {Actors[i].name}!";
                blameOptions[i].icon = Actors[i].icon;
                blameOptionsCancel[i] = blameOptions[i];
            }

            blameOptionsCancel[^1] = cancel;
        }

        private void OnNewTime(int minutes, int seconds)
        {
            textWriter.Write($"{minutes}:{seconds}");

            if (minutes >= forceBlameMinutes)
                ForceBlame();
        }

        public override void Open()
        {
            base.Open();
            blameButton.onClick.AddListener(AskBlame);
            timer.OnNewTime += OnNewTime;

            if (!hasStarted)
                timer.Begin();

            hasStarted = true;
        }

        public override void Close()
        {
            base.Close();
            blameButton.onClick.RemoveAllListeners();
            prompter.OnAnswer -= OnAnswer;
            timer.OnNewTime -= OnNewTime;
        }

        private void OnAnswer(int index)
        {
            // CHECK VALID ANSWER.
            if (index >= 0 && index < Actors.Count)
                OnBlame?.Invoke(Actors[index] == murderer);

            prompter.OnAnswer -= OnAnswer;
        }

        private void AskBlame()
        {
            OnRequestPrompt?.Invoke(blameOptionsCancel, gameplayState);
            prompter.OnAnswer += OnAnswer;
        }

        private void ForceBlame()
        {
            OnRequestPrompt?.Invoke(blameOptions, null);
            prompter.OnAnswer += OnAnswer;
        }
    }
}
