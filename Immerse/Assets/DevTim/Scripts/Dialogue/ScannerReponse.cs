using System;
using UnityEngine;

namespace Immerse
{
    public class ScannerResponse : MonoBehaviour
    {
        public Action<Question, GameObject> OnDisplayQuestion;
        public Action OnDialogue;

        [SerializeField] private Holder holder = default;
        [SerializeField] private Blame blame = default;
        [SerializeField] private Scanner scanner = default;
        [SerializeField] private GameObject gameplayState = default;
        [SerializeField] private DialogueEventDisplayer displayer = default;
        [SerializeField] private Prompter prompter = default;
        [SerializeField] private Question interviewQuestion = default;
        [SerializeField] private int reverseBlameIndex = default;
        [SerializeField] private int reverseCancelIndex = default;

        private Actor currentActor;
        private DialogueEvent currentDialogue;
        private bool hasStarted;

        public void Begin() 
        {
            if (hasStarted)
                return;

            scanner.OnScanInt += OnScanInt;
            scanner.OnScanString += OnScanString;
            OnScanString("Intro");
            hasStarted = true;
        }

        private void OnAnswer(int index)
        {
            prompter.OnAnswer -= OnAnswer;

            if (index < 0 || currentActor == null)
                return;

            int lastIndex = interviewQuestion.options.Length - 1;
            if (index == lastIndex - reverseCancelIndex)
            {
                currentActor = null;
                currentDialogue = null;
                return;
            }

            if (index == lastIndex - reverseBlameIndex)
            {
                blame.BlameActorIndex(currentActor.index);
                currentActor = null;
                currentDialogue = null;
                return;
            }

            InteractWithDialogue(currentActor.dialogue[index]);
            currentActor = null;
        }

        private void InteractWithActor(Actor actor)
        {
            if (actor == null)
                return;

            if (actor == currentActor)
                return;

            currentActor = actor;
            currentDialogue = null;
            interviewQuestion.question = $"Interview {Utils.CapitilizeFirst(currentActor.name)} ...";
            for (int i = 0; i < interviewQuestion.options.Length - 1 - reverseCancelIndex; i++)
            {
                interviewQuestion.options[i].icon = currentActor.icon;
            }

            // PLAY A RANDOM INTERACTION NOISE IF 
            // THERE ARE ANY.
            if (currentActor.interactionSounds != null && currentActor.interactionSounds.Length > 0)
            {
                interviewQuestion.clip = currentActor.interactionSounds[UnityEngine.Random.Range(0, currentActor.interactionSounds.Length)];
            }
            else
            {
                interviewQuestion.clip = null;
            }

            OnDisplayQuestion?.Invoke(interviewQuestion, gameplayState);
            prompter.OnAnswer += OnAnswer;
        }

        private void InteractWithDialogue(DialogueEvent dialogue)
        {
            if (dialogue == null)
                return;

            if (dialogue == currentDialogue)
                return;

            currentDialogue = dialogue;
            currentActor = null;
            OnDialogue?.Invoke();
            displayer.Display(currentDialogue);
        }

        private void OnScanInt(int index)
        {
            if (index < 0)
                return;

            print($"OnScanInt {gameObject} scanned '{index}'.");
            if (index < holder.Actors.Count)
            {
                InteractWithActor(holder.Actors[index]);
                return;
            }

            index -= holder.Actors.Count;
            if (index >= holder.Dialogue.Count)
                return;

            InteractWithDialogue(holder.Dialogue[index]);
        }

        private void OnScanString(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                return;
            
            print($"OnScanString {gameObject} scanned '{name}'.");
            if (holder.ActorsDict.ContainsKey(name))
            {
                InteractWithActor(holder.ActorsDict[name]);
                return;
            }

            if (!holder.DialogueDict.ContainsKey(name))
                return;

            InteractWithDialogue(holder.DialogueDict[name]);
        }

        private void OnDestroy()
        {
            scanner.OnScanInt -= OnScanInt;
            scanner.OnScanString -= OnScanString;
            prompter.OnAnswer -= OnAnswer;
        }
    }
}
