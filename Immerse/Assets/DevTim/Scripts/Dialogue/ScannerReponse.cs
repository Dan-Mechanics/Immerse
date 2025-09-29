using System.Collections;
using UnityEngine;

namespace Immerse
{
    public class ScannerResponse : State
    {
        [SerializeField] private Holder holder = default;
        [SerializeField] private Scanner scanner = default;
        [SerializeField] private DialogueEventDisplayer displayer = default;
        [SerializeField] private GameManager gameStateHandler = default;
        [SerializeField] private Prompter prompter = default;

        [SerializeField] private Prompter.Option[] interviewQuestions = default;

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

        private void OnNewScan(string name)
        {
            print($"Scanned '{scanner}'.");

            // WE FOUND DIALOGUE, LEAVE THE REST.
            if (CheckDialogue(name))
                return;

            if (!holder.Actors.ContainsKey(name))
                return;

            StopAllCoroutines();
            prompter.ForceStop();

            Actor actor = holder.Actors[name];
            for (int i = 0; i < interviewQuestions.Length; i++)
            {
                interviewQuestions[i].icon = actor.icon;
            }

            StartCoroutine(WaitForPrompt(actor));
        }

        /// <summary>
        /// Idk if this works it might crash everything
        /// </summary>
        private IEnumerator WaitForPrompt(Actor actor) 
        {
            while(true)
            {
                int? answer = prompter.DisplayPrompt(interviewQuestions);
                if (answer != null)
                {
                    int index = (int)answer;
                    CheckDialogue(actor.dialogueEvents[index].name);

                    yield break;
                }

                yield return null;
            }
        }

        private bool CheckDialogue(string name)
        {
            if (!holder.DialogueEvents.ContainsKey(name))
                return false;

            StopAllCoroutines();
            prompter.ForceStop();
            displayer.Display(holder.DialogueEvents[name]);

            return true;
        }
    }
}
