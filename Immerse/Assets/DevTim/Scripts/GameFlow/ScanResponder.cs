using System;
using UnityEngine;

namespace Immerse
{
    /// <summary>
    /// Responsibility:
    /// Validates output from Scanner.cs.
    /// </summary>
    public class ScanResponder : StateElement
    {
        public Action<DialogueEvent> OnInteractWithDialogue;
        public Action<Actor> OnInteractWithActor;
      
        [SerializeField] private Holder holder = default;
        [SerializeField] private Scanner scanner = default;
        private bool hasStarted;

        public override void Open()
        {
            base.Open();
            if (hasStarted)
                return;

            hasStarted = true;

            scanner.OnScanInt += OnScanInt;
            scanner.OnScanString += OnScanString;
            OnScanString("Verhaal");
        }

        private void OnScanInt(int index)
        {
            if (!gameObject.activeSelf)
                return;
            
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
            if (!gameObject.activeSelf)
                return;

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

        private void InteractWithDialogue(DialogueEvent dialogue)
        {
            if (dialogue == null)
                return;

            if (!holder.Dialogue.Contains(dialogue))
                return;

            OnInteractWithDialogue?.Invoke(dialogue);
        }

        private void InteractWithActor(Actor actor)
        {
            if (actor == null)
                return;

            if (!holder.Actors.Contains(actor))
                return;

            OnInteractWithActor?.Invoke(actor);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            scanner.OnScanInt -= OnScanInt;
            scanner.OnScanString -= OnScanString;
        }
    }
}
