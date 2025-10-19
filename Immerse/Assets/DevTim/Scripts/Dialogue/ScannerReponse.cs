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
        [SerializeField] private History history = default;
        [SerializeField] private Prompter prompter = default;
        [SerializeField] private Question question = default;

        private Actor currentActor;
        private bool hasStarted;

        public void Begin() 
        {
            if (hasStarted)
                return;

            scanner.OnScanInt += OnScanInt;
            scanner.OnScanString += OnScanString;
            OnScanString("Verhaal");
            hasStarted = true;
        }

        private void OnScanInt(int index)
        {
            if (index < 0)
                return;

            print($"OnScanInt {gameObject.name} scanned '{index}'.");
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

            print($"OnScanString {gameObject.name} scanned '{name}'.");
            if (holder.ActorsDict.ContainsKey(name))
            {
                InteractWithActor(holder.ActorsDict[name]);
                return;
            }

            if (!holder.DialogueDict.ContainsKey(name))
                return;

            InteractWithDialogue(holder.DialogueDict[name]);
        }

        private void InteractWithActor(Actor actor)
        {
            if (actor == null)
                return;

            if (actor == currentActor)
                return;

            currentActor = actor;
            MatchQuestionToActor(currentActor);

            OnDisplayQuestion?.Invoke(question, gameplayState);
            prompter.OnAnswer += OnAnswer;
        }

        private void MatchQuestionToActor(Actor actor)
        {
            question.question = $"Interview {Utils.CapitilizeFirst(actor.name)} ...";
            question.includeOptional = history.Has(actor.prop);

            for (int i = 0; i < question.options.Length; i++)
            {
                if (question.options[i].tag == Tag.None)
                    question.options[i].icon = actor.icon;
            }

            question.clip = null;
            if (actor.interactionSounds != null && actor.interactionSounds.Length > 0)
                question.clip = actor.interactionSounds[UnityEngine.Random.Range(0, actor.interactionSounds.Length)];
        }

        private void InteractWithDialogue(DialogueEvent dialogue)
        {
            currentActor = null;
            if (dialogue.speaker is Prop prop && !history.Has(prop))
                history.Add(prop);

            if (dialogue.speaker is Actor actor)
                history.Add(actor, dialogue.index);

            OnDialogue?.Invoke();
            displayer.Display(dialogue);
        }

        private void OnAnswer(int index)
        {
            prompter.OnAnswer -= OnAnswer;

            if (index < 0 || currentActor == null)
                return;

            Option option = question.options[index];
            if(option.tag == Tag.Cancel)
            {
                currentActor = null;
                return;
            }

            if (option.tag == Tag.Blame)
            {
                blame.BlameActorIndex(currentActor.index);
                currentActor = null;
                return;
            }

            InteractWithDialogue(currentActor.dialogue[index]);
            currentActor = null;
        }

        private void OnDestroy()
        {
            scanner.OnScanInt -= OnScanInt;
            scanner.OnScanString -= OnScanString;
            prompter.OnAnswer -= OnAnswer;
        }
    }
}
