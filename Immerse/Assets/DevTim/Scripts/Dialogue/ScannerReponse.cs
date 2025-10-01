using System;
using UnityEngine;

namespace Immerse
{
    public class ScannerResponse : Behaviour
    {
        public Action<Prompter.Option[], GameObject> OnRequestPrompt;
        
        [SerializeField] private Holder holder = default;
        [SerializeField] private Actor victim = default;
        [SerializeField] private Scanner scanner = default;
        [SerializeField] private GameObject gameplayState = default;
        [SerializeField] private DialogueEventDisplayer displayer = default;
        [SerializeField] private Prompter prompter = default;
        [SerializeField] private Prompter.Option[] interviewQuestions = default;

        private Actor scannedActor;

        private void Start() 
        {
            OnNewScan("Intro");

            for (int i = 0; i < interviewQuestions.Length; i++)
            {
                interviewQuestions[i].text = interviewQuestions[i].text.Replace("[naam]", FirstUpper(victim.name));
            }
        }

        private string FirstUpper(string str)
        {
            str = str[0].ToString().ToUpperInvariant() + str.AsSpan(1).ToString();
            return str;
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
            if(scannedActor != null)
                CheckDialogue(scannedActor.dialogueEvents[index].name);

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

            scannedActor = holder.ActorsDict[name];
            /*for (int i = 0; i < interviewQuestions.Length; i++)
            {
                interviewQuestions[i].icon = scannedActor.icon;
            }*/

            OnRequestPrompt?.Invoke(interviewQuestions, gameplayState);
            prompter.OnAnswer += OnAnswer;
        }

        private bool CheckDialogue(string name)
        {
            if (!holder.DialogueEventsDict.ContainsKey(name))
                return false;

            displayer.Display(holder.DialogueEventsDict[name]);
            return true;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            prompter.OnAnswer -= OnAnswer;
        }
    }
}
