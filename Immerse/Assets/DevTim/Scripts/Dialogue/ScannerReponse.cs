using System;
using UnityEngine;

namespace Immerse
{
    public class ScannerResponse : State
    {
        public Action<Prompter.Option[], GameObject> OnPrompt;
        
        [SerializeField] private Holder holder = default;
        [SerializeField] private Scanner scanner = default;
        [SerializeField] private GameObject gameplayState = default;
        [SerializeField] private DialogueEventDisplayer displayer = default;
        [SerializeField] private Prompter prompter = default;
        [SerializeField] private Prompter.Option[] interviewQuestions = default;

        private Actor actor;

        private void Awake()
        {
            prompter.OnAnswer += OnAnswer;
        }

        private void OnDestroy()
        {
            prompter.OnAnswer -= OnAnswer;
        }

        public override void EnterState()
        {
            base.EnterState();
            scanner.OnNewScan += OnNewScan;
        }

        public override void ExitState()
        {
            base.ExitState();
            scanner.OnNewScan -= OnNewScan;
        }

        private void OnAnswer(int index)
        {
            CheckDialogue(actor.dialogueEvents[index].name);
        }

        private void OnNewScan(string name)
        {
            print($"Scanned '{scanner}'.");

            // WE FOUND DIALOGUE, LEAVE THE REST.
            if (CheckDialogue(name))
                return;

            if (!holder.Actors.ContainsKey(name))
                return;

            actor = holder.Actors[name];
            for (int i = 0; i < interviewQuestions.Length; i++)
            {
                interviewQuestions[i].icon = actor.icon;
            }

            OnPrompt?.Invoke(interviewQuestions, gameplayState);
        }

        private bool CheckDialogue(string name)
        {
            if (!holder.DialogueEvents.ContainsKey(name))
                return false;

            displayer.Display(holder.DialogueEvents[name]);
            return true;
        }
    }
}
