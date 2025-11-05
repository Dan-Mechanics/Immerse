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
        [SerializeField, Min(0.1f)] private float endLag = default;

        private DialogueEvent currentDialogue;
        private bool isShowing;

        public override void Open()
        {
            base.Open();
            notepadHandler.OnOpen += HideDialogue;
        }

        public override void Close()
        {
            base.Close();
            iconText.text = string.Empty;
            //doneTime = 0f;
            currentDialogue = null;
            lerper.Force();

            textWriter.Write(string.Empty);

            source.playOnAwake = false;
            source.dopplerLevel = 0f;
            source.Stop();

            notepadHandler.OnOpen -= HideDialogue;
        }

        public override void DoFrame()
        {
            base.DoFrame();
            if(Input.GetKeyDown(KeyCode.Mouse0))
                HideDialogue();
        }

        public override void DoTick()
        {
            base.DoTick();

            lerper.Send(!isShowing);
            if (!isShowing)
                currentDialogue = null;
        }

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
            isShowing = true;
        }

        public void HideDialogue()
        {
            isShowing = false;
            currentDialogue = null;
            source.Stop();
        }
    }
}
