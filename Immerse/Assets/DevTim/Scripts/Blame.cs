using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    /// <summary>
    /// This is also where i wanna add the setup behaviour or i should call that Grid.cs
    /// </summary>
    public class Blame : MonoBehaviour
    {
        [SerializeField] private Prompter prompter = default;

        [SerializeField] private List<Prompter.Option> options = default;
        [SerializeField] private Prompter.Option cancel = default;

        public void BlameButton()
        {

        }

        private IEnumerator WaitForPrompt(Prompter.Option[] options)
        {
            while (true)
            {
                int? answer = prompter.GetAnswer(options);
                if (answer != null)
                {
                    int index = (int)answer;
                    // Code here..
                    //TryDialogue(actor.dialogueEvents[index].name);

                    yield break;
                }

                yield return null;
            }
        }

        // Feeds into win condition.
        public void ForceBlame()
        {

        }
    }
}
