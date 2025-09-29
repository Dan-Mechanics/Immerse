using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public class DialogueEventDisplayer : State
    {
        [SerializeField] private Image icon = default;
        [SerializeField] private TextWriter textWriter = default;
        [SerializeField] private Lerper lerper = default;   
        [SerializeField] private TMP_Text iconText = default;
        [SerializeField] private AudioSource source = default;

        private float doneTime;

        /// <summary>
        /// ??
        /// </summary>
        private void Start() => ExitState();

        public override void ExitState()
        {
            base.ExitState();
            source.playOnAwake = false;
            source.dopplerLevel = 0f;
            source.Stop();
            iconText.text = string.Empty;

            lerper.Send(true);
            textWriter.Write(string.Empty);
        }

        public override void DoTick()
        {
            base.DoTick();
            lerper.Send(Time.time >= doneTime);
        }

        public void Display(DialogueEvent dialogue)
        {
            source.Stop();
            source.PlayOneShot(dialogue.clip);

            icon.sprite = dialogue.actor.icon;
            textWriter.Write(dialogue.script);
            iconText.text = dialogue.actor.name + " > " + "\n" + dialogue.actor.description;
            doneTime = Time.time + dialogue.script.Length * TextWriter.INTERVAL;

            print($"Playing: '{dialogue.name} | {dialogue.clip.name}'");
        }
    }
}
