using System;
using UnityEngine;

namespace Immerse
{
    public class ScannerResponse : MonoBehaviour
    {
        public Func<Question, GameObject, bool> OnDisplayQuestion;
        public Action<int> OnBlameActorIndex;
        public Action OnDialogue;

        [SerializeField] private Holder holder = default;
        [SerializeField] private Scanner scanner = default;
        [SerializeField] private GameObject gameplayState = default;
        [SerializeField] private DialogueEventDisplayer displayer = default;
        [SerializeField] private Prompter prompter = default;
        [SerializeField] private Question interviewQuestion = default;
        [SerializeField] private int reverseBlameIndex = default;
        [SerializeField] private int reverseCancelIndex = default;

        private Actor scannedActor;
        private bool hasBegunAlready;

        private void Awake()
        {
            scanner.OnScanInt += OnScanInt;
            scanner.OnScanString += OnScanString;
        }

        public void Begin() 
        {
            if (hasBegunAlready)
                return;

            OnScanString("Intro");
            hasBegunAlready = true;
        }

        private void OnAnswer(int index)
        {
            prompter.OnAnswer -= OnAnswer;

            if (index < 0 || scannedActor == null)
                return;

            int lastIndex = interviewQuestion.options.Length - 1;
            if(index == lastIndex - reverseBlameIndex)
            {
                OnBlameActorIndex?.Invoke(scannedActor.index);
                return;
            }

            if (index == lastIndex - reverseCancelIndex)
                return;

            displayer.Display(scannedActor.dialogue[index]);
        }

        private void InteractWithActor(Actor actor)
        {
            if (actor == null)
                return;
            
            scannedActor = actor;
            interviewQuestion.question = $"Interview {Utils.CapitilizeFirst(scannedActor.name)} ...";
            for (int i = 0; i < interviewQuestion.options.Length - 1 - reverseCancelIndex; i++)
            {
                interviewQuestion.options[i].icon = scannedActor.icon;
            }

            interviewQuestion.clip = scannedActor.interactionNoise;
            if (OnDisplayQuestion.Invoke(interviewQuestion, gameplayState))
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

            OnDialogue?.Invoke();
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

            if (!holder.DialogueDict.ContainsKey(name))
                return;

            displayer.Display(holder.DialogueDict[name]);
            OnDialogue?.Invoke();
        }

        private void OnDestroy()
        {
            scanner.OnScanInt -= OnScanInt;
            scanner.OnScanString -= OnScanString;
            prompter.OnAnswer -= OnAnswer;
        }
    }
}
