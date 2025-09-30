using System;
using UnityEngine;

namespace Immerse
{
    public class ScannerResponse : Behaviour
    {
        public Action<Prompter.Option[], GameObject> OnPrompt;
        
        [SerializeField] private Holder holder = default;
        [SerializeField] private Scanner scanner = default;
        [SerializeField] private GameObject gameplayState = default;
        [SerializeField] private DialogueEventDisplayer displayer = default;
        [SerializeField] private Prompter prompter = default;
        [SerializeField] private Prompter.Option[] interviewQuestions = default;

        private Actor actor;

        private void Start() => OnNewScan("Intro");

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
            prompter.OnAnswer -= OnAnswer;
        }

        private void OnNewScan(string name)
        {
            print($"Scanned '{scanner}'.");

            // WE FOUND DIALOGUE, LEAVE THE REST.
            if (CheckDialogue(name))
                return;

            if (!holder.ActorsDict.ContainsKey(name))
                return;

            actor = holder.ActorsDict[name];
            for (int i = 0; i < interviewQuestions.Length; i++)
            {
                interviewQuestions[i].icon = actor.icon;
            }

            OnPrompt?.Invoke(interviewQuestions, gameplayState);
            prompter.OnAnswer += OnAnswer;
        }

        private bool CheckDialogue(string name)
        {
            if (!holder.DialogueDict.ContainsKey(name))
                return false;

            displayer.Display(holder.DialogueDict[name]);
            return true;
        }

        private void OnDestroy()
        {
            prompter.OnAnswer -= OnAnswer;
        }
    }
}
