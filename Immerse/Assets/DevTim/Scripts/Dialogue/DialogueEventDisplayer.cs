using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public class DialogueEventDisplayer : StateElement
    {
        [SerializeField] private Image icon = default;
        [SerializeField] private TextWriter textWriter = default;
        [SerializeField] private NotepadHandler notepadHandler = default;
        [SerializeField] private Lerper lerper = default;   
        [SerializeField] private TMP_Text iconText = default;
        [SerializeField] private AudioSource source = default;
        [SerializeField, Min(0.1f)] private float textBoxTrailTime = default;

        private DialogueEvent currentDialogue;
        private float doneTime;

        public override void Open()
        {
            base.Open();
            notepadHandler.OnOpen += HideDialogue;
           // notepadHandler.OnClose += HideDialogue;
        }

        public override void Close()
        {
            base.Close();
            iconText.text = string.Empty;
            doneTime = 0f;
            currentDialogue = null;
            lerper.Force();

            textWriter.Write(string.Empty);

            source.playOnAwake = false;
            source.dopplerLevel = 0f;
            source.Stop();

            notepadHandler.OnOpen -= HideDialogue;
            //notepadHandler.OnClose -= HideDialogue;
        }

        public override void DoTick()
        {
            base.DoTick();

            bool done = Time.time > doneTime;
            lerper.Send(done);
            if (done)
                currentDialogue = null;
        }

        /// <summary>
        /// Possible todo: make it so that when you press O or L that the shit goes away.
        /// </summary>
        /// <param name="dialogue"></param>
        public void Display(DialogueEvent dialogue)
        {
            if (dialogue == null)
                return;

            if (currentDialogue != null && currentDialogue == dialogue)
                return;
            
            source.Stop();

            if (dialogue.clip != null)
            {
                source.PlayOneShot(dialogue.clip);
                print($"Playing: '{dialogue.name} | {dialogue.clip.name}'");
            }

            icon.sprite = dialogue.owner.icon;
            textWriter.Write(dialogue.script);
            iconText.text = dialogue.owner.name + " > " + "\n" + dialogue.owner.description;

            currentDialogue = dialogue;
            doneTime = Time.time + textBoxTrailTime + dialogue.script.Length * TextWriter.INTERVAL;
        }

        public void HideDialogue()
        {
            doneTime = 0f;
            currentDialogue = null;
            source.Stop();
        }
    }
}
