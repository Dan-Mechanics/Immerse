using System;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public class BlameHandler : StateElement, IAnswerListener
    {
        public event Action<bool> OnWinOrLose;

        [SerializeField] private GameObject scanResponder = default;
        [SerializeField] private TextWriter textWriter = default;
        [SerializeField] private Timer timer = default;
        [SerializeField] private Prompter prompter = default;
        [SerializeField] private Button blameButton = default;
        [SerializeField] private Holder holder = default;
        [SerializeField] private Actor murderer = default;
        [SerializeField] private Question blameQuestion = default;

        private bool hasStarted;

        private void Awake()
        {
            for (int i = 0; i < holder.Actors.Count; i++)
            {
                blameQuestion.options[i].icon = holder.Actors[i].icon;
                blameQuestion.options[i].text = $"Beschuldig {Utils.CapitilizeFirst(holder.Actors[i].name)}!";
            }
        }

        private void OnNewTime(int minutes, int seconds) => textWriter.Write($"{minutes}:{seconds}");

        public override void Open()
        {
            base.Open();
            blameButton.onClick.AddListener(AskBlame);
            timer.OnNewTime += OnNewTime;
            timer.OnDone += ForceBlame;

            if (hasStarted)
                return;

            hasStarted = true;
            timer.Begin();
        }

        public override void Close()
        {
            base.Close();
            blameButton.onClick.RemoveAllListeners();

            timer.OnNewTime -= OnNewTime;
            timer.OnDone -= ForceBlame;
        }

        private void AskBlame()
        {
            blameQuestion.includeOptional = true;
            prompter.Ask(blameQuestion, this);
        }

        private void StopScanning()
        {
            if (scanResponder == null)
                return;

            scanResponder.SetActive(false);
            Destroy(scanResponder);
            scanResponder = null;
        }

        private void ForceBlame()
        {
            StopScanning();

            blameQuestion.includeOptional = false;
            timer.OnDone -= ForceBlame;

            prompter.Ask(blameQuestion, this);
        }

        /// <summary>
        /// Who did you blame?
        /// </summary>
        public void GetAnswer(int index, Option option)
        {
            if (option.tag == Tag.Cancel)
                return;

            StopScanning();
            OnWinOrLose?.Invoke(holder.Actors[index] == murderer);
        }
    }
}
