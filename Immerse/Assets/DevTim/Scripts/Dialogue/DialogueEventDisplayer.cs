using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public class DialogueEventDisplayer : MonoBehaviour
    {
        [SerializeField] private Image icon = default;

        /// <summary>
        /// This should call TextWriter.cs but I dont have that yet...
        /// </summary>
        [SerializeField] private Text dialogueText = default;
        [SerializeField] private Text iconText = default;
        [SerializeField] private AudioSource source = default;

        public void Display(DialogueEvent dial)
        {
            source.Stop();
            source.PlayOneShot(dial.clip);

            icon.sprite = dial.actor.icon;
            dialogueText.text = dial.script;
            iconText.text = dial.name + "\n" + dial.actor.desc;

            print($"Playing: '{dial.name} | {dial.clip.name}'");
        }
    }
}
