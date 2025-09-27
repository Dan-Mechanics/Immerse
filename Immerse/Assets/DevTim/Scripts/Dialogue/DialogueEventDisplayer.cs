using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public class DialogueEventDisplayer : MonoBehaviour
    {
        [SerializeField] private Image icon = default;
        [SerializeField] private GameObject textWriterGameObject = default;
        [SerializeField] private GameObject lerperGameObject = default;   
        [SerializeField] private TMP_Text iconText = default;
        [SerializeField] private AudioSource source = default;

        private IReceiver<object> textWriter;
        private IReceiver<bool> rectLerper;
        private float doneTime;

        private void Start()
        {
            textWriter = textWriterGameObject.GetComponent<IReceiver<object>>();
            rectLerper = lerperGameObject.GetComponent<IReceiver<bool>>();

            source.playOnAwake = false;
            source.dopplerLevel = 0f;
            iconText.text = string.Empty;

            textWriter.Send(string.Empty);
            rectLerper.Send(false);

            textWriterGameObject.GetComponent<IEventHolder>().OnEvent += LowerDialogueBox;
        }

        private void FixedUpdate()
        {
            rectLerper.Send(Time.time > doneTime);
        }

        private void LowerDialogueBox()
        {
            doneTime = Time.time + 2f;
        }

        public void Display(DialogueEvent dialogue)
        {
            source.Stop();
            source.PlayOneShot(dialogue.clip);

            icon.sprite = dialogue.actor.icon;
            textWriter.Send(dialogue.script);
            iconText.text = dialogue.actor.name + " > " + "\n" + dialogue.actor.description;
            doneTime = Time.time + 1000f;

            print($"Playing: '{dialogue.name} | {dialogue.clip.name}'");
        }
    }
}
