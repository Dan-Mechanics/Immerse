using System;
using UnityEngine;

namespace Immerse
{
    public class ScannerResponse : StateElement
    {
        public Action<Question, GameObject> OnDisplayQuestion;
        public Action<int> OnBlameActorIndex;

        [SerializeField] private Holder holder = default;
        [SerializeField] private Scanner scanner = default;
        [SerializeField] private GameObject gameplayState = default;
        [SerializeField] private DialogueEventDisplayer displayer = default;
        [SerializeField] private Prompter prompter = default;
        [SerializeField] private Question interviewQuestion = default;
        [SerializeField] private int reverseBlameIndex = default;
        [SerializeField] private int reverseCancelIndex = default;

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

            if (index < 0)
                return;

            if (scannedActor == null )
                return;

            int lastIndex = interviewQuestion.options.Length - 1;

            if(index == lastIndex - reverseBlameIndex)
            {
                OnBlameActorIndex?.Invoke(scannedActor.index);
                return;
            }

            if (index >= lastIndex - reverseCancelIndex)
                return;

            displayer.Display(scannedActor.dialogue[index]);
        }

        private void InteractWithActor(Actor actor)
        {
            scannedActor = actor;
            interviewQuestion.question = $"Interview {Utils.CapitilizeFirst(scannedActor.name)} ...";
            for (int i = 0; i < interviewQuestion.options.Length - 1 - reverseCancelIndex; i++)
            {
                interviewQuestion.options[i].icon = scannedActor.icon;
            }

            interviewQuestion.clip = scannedActor.interactionNoise;
            OnDisplayQuestion?.Invoke(interviewQuestion, gameplayState);
            prompter.OnAnswer += OnAnswer;
        }

        private void OnScanInt(int index)
        {
            print($"OnScanInt {gameObject} scanned '{index}'.");

            if (index < 0)
                return;

            if (index < holder.Actors.Count)
            {
                InteractWithActor(holder.Actors[index]);
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
                InteractWithActor(holder.ActorsDict[name]);
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
