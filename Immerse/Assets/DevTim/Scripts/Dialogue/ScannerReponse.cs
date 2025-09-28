using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    /// <summary>
    /// This might possibly be the place to translate raw data
    /// itno usable data.
    /// 
    /// You can also call this ScannerBridge or something.
    /// </summary>
    public class ScannerResponse : MonoBehaviour
    {
        [SerializeField] private Holder holder = default;
        [SerializeField] private Scanner scanner = default;
        [SerializeField] private DialogueEventDisplayer displayer = default;
        [SerializeField] private Prompter prompter = default;

        [SerializeField] private Prompter.Option[] options = default;

        private void Awake()
        {   
            scanner.OnNewScan += OnNewScan;
        }

        private void OnNewScan(string name)
        {
            if (!TryDialogue(name) && holder.Actors.ContainsKey(name))
            {
                StopAllCoroutines();
                StartCoroutine(WaitForPrompt(holder.Actors[name]));
            }
        }

        /// <summary>
        /// Idk if this works it might crash everything
        /// </summary>
        private IEnumerator WaitForPrompt(Actor actor) 
        {
            while(true)
            {
                int? answer = prompter.GetAnswer(options);
                if (answer != null) 
                {
                    int index = (int)answer;
                    TryDialogue(actor.dialogueEvents[index].name);

                    yield break;
                }

                yield return null;
            }
        }

        private bool TryDialogue(string name) 
        {
            if (holder.DialogueEvents.ContainsKey(name))
            {
                displayer.Display(holder.DialogueEvents[name]);
                return true;
            }

            return false;
        }
    }
}
