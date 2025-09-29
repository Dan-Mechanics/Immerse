using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Immerse
{
    /// <summary>
    /// This is also where i wanna add the setup behaviour or i should call that Grid.cs
    /// </summary>
    public class Blame : State
    {
        public event Action<bool> OnBlame;
        
        [SerializeField] private Prompter prompter = default;
        [SerializeField] private Button blameButton = default;
        [SerializeField] private Holder holder = default;
        [SerializeField] private Actor murderer = default;
        [SerializeField] private Prompter.Option template = default;
        [SerializeField] private Prompter.Option cancel = default;

        private Actor[] actors = default;
        private Prompter.Option[] blameOptionsCancel;
        private Prompter.Option[] blameOptions;

        private void Start()
        {
            actors = holder.Actors.Values.ToArray();
            
            blameOptions = new Prompter.Option[actors.Length];
            blameOptionsCancel = new Prompter.Option[actors.Length + 1];

            for (int i = 0; i < blameOptions.Length; i++)
            {
                blameOptions[i] = template;
                blameOptions[i].text = $"Blame {actors[i].name}!";
                blameOptions[i].icon = actors[i].icon;
                blameOptionsCancel[i] = blameOptions[i];
            }

            blameOptionsCancel[blameOptionsCancel.Length - 1] = cancel;
        }

        /// <summary>
        /// But sohuld be called by blame button
        /// </summary>
        public void AskBlame()
        {
            StopAllCoroutines();
            prompter.ForceStop();
            StartCoroutine(WaitForPrompt(blameOptionsCancel));
        }

        private IEnumerator WaitForPrompt(Prompter.Option[] options)
        {
            while (true)
            {
                int? answer = prompter.DisplayPrompt(options);
                if (answer != null)
                {
                    int index = (int)answer;

                    if (index >= 0 && index < actors.Length)
                        (actors[index] == murderer ? blamedCorrectly : blamedIncorrectly)?.Invoke();

                    yield break;
                }

                yield return null;
            }
        }

        /// <summary>
        /// This should be called by the timer.
        /// </summary>
        public void ForceBlame()
        {
            StopAllCoroutines();
            prompter.ForceStop();
            StartCoroutine(WaitForPrompt(blameOptions));
        }
    }
}
