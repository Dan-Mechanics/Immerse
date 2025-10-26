using System;
using UnityEngine;

namespace Immerse
{
    /// <summary>
    /// Listens to ScanRespondder.cs and calls 
    /// other methods.
    /// </summary>
    public class InteractionHandler : MonoBehaviour, IAnswerListener
    {
        public Action<DialogueEvent> OnInteractWithAnyDialogue;

        [SerializeField] private BlameHandler blame = default;
        [SerializeField] private ScanResponder scanResponder = default;
        [SerializeField] private GameObject gameplayState = default;
        [SerializeField] private DialogueEventDisplayer displayer = default;
        [SerializeField] private StateHandler stateHandler = default;
        [SerializeField] private History history = default;
        [SerializeField] private Prompter prompter = default;
        [SerializeField] private Question interviewQuestion = default;

        private Actor currentActor;

        private void Awake()
        {
            scanResponder.OnInteractWithActor += OnInteractWithActor;
            scanResponder.OnDirectDialogue += InteractWithDialogue;
        }

        private void OnDestroy()
        {
            scanResponder.OnInteractWithActor -= OnInteractWithActor;
            scanResponder.OnDirectDialogue -= InteractWithDialogue;
        }

        private void MatchInterviewQuestionToActor(Actor actor, ref Question question)
        {
            question.message = $"Interview {Utils.CapitilizeFirst(actor.name)} ...";
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

        private void OnInteractWithActor(Actor actor)
        {
            if (actor == currentActor)
                return;

            currentActor = actor;
            MatchInterviewQuestionToActor(currentActor, ref interviewQuestion);

            prompter.Ask(interviewQuestion, this);
        }

        private void InteractWithDialogue(DialogueEvent dialogue)
        {
            currentActor = null;
            stateHandler.Open(gameplayState);
            displayer.Display(dialogue);

            OnInteractWithAnyDialogue?.Invoke(dialogue);
        }

        public void GetAnswer(int index, Option option)
        {
            if (currentActor == null)
                return;

            switch (option.tag)
            {
                case Tag.None:
                    InteractWithDialogue(currentActor.dialogue[index]);
                    break;
                case Tag.Blame:
                    blame.GetAnswer(currentActor.index, option);
                    break;
                default:
                    break;
            }

            currentActor = null;
        }
    }
}
