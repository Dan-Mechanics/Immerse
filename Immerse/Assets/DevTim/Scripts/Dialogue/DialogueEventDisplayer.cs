using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public class DialogueEventDisplayer : MonoBehaviour
    {
        [SerializeField] private Image icon = default;
        [SerializeField] private GameObject textWriterGO = default;
        [SerializeField] private GameObject rectLerperGO = default;   
        [SerializeField] private TMP_Text iconText = default;
        [SerializeField] private AudioSource source = default;

        private IReceiver<string> textWriter;
        private IReceiver<bool> rectLerper;

        private void Start()
        {
            textWriter = textWriterGO.GetComponent<IReceiver<string>>();
            rectLerper = rectLerperGO.GetComponent<IReceiver<bool>>();

            source.playOnAwake = false;
            source.dopplerLevel = 0f;
            iconText.text = string.Empty;

            textWriter.Send(string.Empty);
            rectLerper.Send(false);

            textWriterGO.GetComponent<IInvoker>().OnInvoke += OnTextWriterDone;
        }

        private void OnTextWriterDone()
        {
            Invoke(nameof(HackDelay), 2f);
        }

        private void HackDelay() 
        {
            rectLerper.Send(false);
        }

        public void Display(DialogueEvent dialogue)
        {
            source.Stop();
            source.PlayOneShot(dialogue.clip);

            icon.sprite = dialogue.actor.icon;
            textWriter.Send(dialogue.script);
            rectLerper.Send(true);
            iconText.text = dialogue.actor.name + " > " + "\n" + dialogue.actor.description;

            print($"Playing: '{dialogue.name} | {dialogue.clip.name}'");
        }
    }
}
