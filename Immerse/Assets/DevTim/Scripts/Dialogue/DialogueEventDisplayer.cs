using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public class DialogueEventDisplayer : MonoBehaviour
    {
        [SerializeField] private Image icon = default;
        [SerializeField] private TextWriter dialogueText = default;
        [SerializeField] private TMP_Text iconText = default;
        [SerializeField] private AudioSource source = default;

        private void Start()
        {
            source.playOnAwake = false;
            source.dopplerLevel = 0f;
        }

        public void Display(DialogueEvent dialogue)
        {
            source.Stop();
            source.PlayOneShot(dialogue.clip);

            icon.sprite = dialogue.actor.icon;
            dialogueText.Write(dialogue.script);
            iconText.text = dialogue.actor.desc + "\n" + dialogue.name;

            print($"Playing: '{dialogue.name} | {dialogue.clip.name}'");
        }
    }
}
