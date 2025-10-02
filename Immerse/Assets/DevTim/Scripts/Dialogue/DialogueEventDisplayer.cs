using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public class DialogueEventDisplayer : StateElement
    {
        [SerializeField] private Image icon = default;
        [SerializeField] private TextWriter textWriter = default;
        [SerializeField] private Lerper lerper = default;   
        [SerializeField] private TMP_Text iconText = default;
        [SerializeField] private AudioSource source = default;
        [SerializeField, Min(0.1f)] private float textBoxTrailTime = default;

        private float doneTime;
        
        public override void Close()
        {
            base.Close();
            iconText.text = string.Empty;
            doneTime = 0f;
            lerper.Force();

            textWriter.Write(string.Empty);

            source.playOnAwake = false;
            source.dopplerLevel = 0f;
            source.Stop();
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
            doneTime = Time.time + textBoxTrailTime + dialogue.script.Length * TextWriter.INTERVAL;

            print($"Playing: '{dialogue.name} | {dialogue.clip.name}'");
        }
    }
}
