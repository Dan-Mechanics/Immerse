using System;
using UnityEngine;

namespace Immerse
{
    public class ScannerResponse : StateElement
    {
        public Action<Prompter.Option[], GameObject> OnRequestPrompt;
        public Action<int> OnBlameActorIndex;

        [SerializeField] private Holder holder = default;
        [SerializeField] private Actor victim = default;
        [SerializeField] private Scanner scanner = default;
        [SerializeField] private GameObject gameplayState = default;
        [SerializeField] private DialogueEventDisplayer displayer = default;
        [SerializeField] private Prompter prompter = default;
        [SerializeField] private Prompter.Option[] interviewQuestions = default;

        private Actor scannedActor;
        private bool hasStarted;

        private void Awake() 
        {
            for (int i = 0; i < interviewQuestions.Length; i++)
            {
                // REPLACE THE NAME TOKEN.
                interviewQuestions[i].text = interviewQuestions[i].text.Replace("[naam]", FirstUpper(victim.name));

                if (i >= interviewQuestions.Length - 1)
                    return;

                Color color = Color.Lerp(Color.red, Color.magenta, (float)i / (interviewQuestions.Length-1));
                interviewQuestions[i].color = Color.Lerp(interviewQuestions[i].color, color, 0.25f);
            }
        }

        private string FirstUpper(string str)
        {
            str = str[0].ToString().ToUpperInvariant() + str.AsSpan(1).ToString();
            return str;
        }

        public override void Open()
        {
            base.Open();
            scanner.OnNewScan += OnNewScan;

            if (!hasStarted)
                OnScanString("Intro");

            hasStarted = true;
        }

        public override void Close()
        {
            base.Close();
            scanner.OnNewScan -= OnNewScan;
        }

        private void OnAnswer(int index)
        {
            prompter.OnAnswer -= OnAnswer;

            if (scannedActor == null)
                return;

            if (index < 0)
                return;

            if (index < scannedActor.dialogue.Length)
            {
                displayer.Display(scannedActor.dialogue[index]);
            }
            else 
            {
                OnBlameActorIndex?.Invoke(scannedActor.index);
            }
        }

        private void OnNewScan(int index)
        {
            print($"{gameObject} scanned '{index}'.");

            foreach (Actor actor in holder.Actors)
            {
                if (actor.index != index)
                    continue;

                scannedActor = actor;

                for (int i = 0; i < interviewQuestions.Length - 1; i++)
                {
                    interviewQuestions[i].icon = scannedActor.icon;
                }

                OnRequestPrompt?.Invoke(interviewQuestions, gameplayState);
                prompter.OnAnswer += OnAnswer;
                return;
            }

            /*foreach (DialogueEvent dialogueEvent in holder.Dialogue)
            {
                if (dialogueEvent.id != index)
                    continue;

                displayer.Display(dialogueEvent);
                return;
            }*/
        }

        private void OnScanString(string name)
        {
            print($"Scanned '{name}'.");

            // WE FOUND ACTOR, LEAVE THE REST.
            if (HasActor(name))
                return;

            //Debug.LogWarning($"222 Scanned '{name}'.");

            if (holder.DialogueDict.ContainsKey(name))
                displayer.Display(holder.DialogueDict[name]);
        }

        private bool HasActor(string name)
        {
            if (!holder.ActorsDict.ContainsKey(name))
                return false;

            scannedActor = holder.ActorsDict[name];
            OnRequestPrompt?.Invoke(interviewQuestions, gameplayState);
            prompter.OnAnswer += OnAnswer;
            return true;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            prompter.OnAnswer -= OnAnswer;
        }
    }
}
