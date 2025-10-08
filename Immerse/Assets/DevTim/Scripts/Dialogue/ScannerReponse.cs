using System;
using UnityEngine;

namespace Immerse
{
    public class ScannerResponse : StateElement
    {
        public Action<Question, GameObject> OnRequestPrompt;
        public Action<int> OnBlameActorIndex;

        [SerializeField] private Holder holder = default;
        [SerializeField] private Scanner scanner = default;
        [SerializeField] private GameObject gameplayState = default;
        [SerializeField] private DialogueEventDisplayer displayer = default;
        [SerializeField] private Prompter prompter = default;
        [SerializeField] private Question interviewQuestion = default;

        private Actor scannedActor;
        private bool hasStarted;

        public override void Open()
        {
            base.Open();
            scanner.OnScanInt += OnScanInt;
            scanner.OnScanString += OnScanString;

            if (!hasStarted)
                OnScanString("Intro");

            hasStarted = true;
        }

        public override void Close()
        {
            base.Close();
            scanner.OnScanInt -= OnScanInt;
            scanner.OnScanString -= OnScanString;
        }

        private void OnAnswer(int index)
        {
            prompter.OnAnswer -= OnAnswer;

            if (scannedActor == null || index < 0)
                return;

            if (index == interviewQuestion.options.Length - 1)
            {
                // BLAME.
                OnBlameActorIndex?.Invoke(scannedActor.index);
            }
            else 
            {
                displayer.Display(scannedActor.dialogue[index]);
            }
        }

        private void OnScanInt(int index)
        {
            print($"OnScanInt {gameObject} scanned '{index}'.");

            if (index < 0)
                return;

            if (index < holder.Actors.Count)
            {
                scannedActor = holder.Actors[index];
                for (int i = 0; i < interviewQuestion.options.Length - 1; i++)
                {
                    interviewQuestion.options[i].icon = scannedActor.icon;
                }

                OnRequestPrompt?.Invoke(interviewQuestion, gameplayState);
                prompter.OnAnswer += OnAnswer;
                return;
            }

            index -= holder.Actors.Count;
            if (index >= holder.Dialogue.Count)
                return;

            displayer.Display(holder.Dialogue[index]);
        }

        private void OnScanString(string name)
        {
            print($"OnScanString {gameObject} scanned '{name}'.");

            if (holder.ActorsDict.ContainsKey(name))
            {
                scannedActor = holder.ActorsDict[name];
                for (int i = 0; i < interviewQuestion.options.Length - 1; i++)
                {
                    interviewQuestion.options[i].icon = scannedActor.icon;
                }

                OnRequestPrompt?.Invoke(interviewQuestion, gameplayState);
                prompter.OnAnswer += OnAnswer;
                return;
            }

            if (holder.DialogueDict.ContainsKey(name))
                displayer.Display(holder.DialogueDict[name]);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            prompter.OnAnswer -= OnAnswer;
        }
    }
}
