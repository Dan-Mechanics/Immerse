using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Immerse
{
    public class NotepadHandler : StateElement
    {
        [SerializeField] private TMP_InputField inputField = default;
        [SerializeField] private EventSystem eventSystem = default;
        [SerializeField] private Button toggleNotepadButton = default;
        [SerializeField] private List<Lerper> lerpers = default;

        private bool showingNotes;

        public override void Open()
        {
            base.Open();
            toggleNotepadButton.onClick.AddListener(ToggleNotepad);
        }

        public override void Close()
        {
            base.Close();
            toggleNotepadButton.onClick.RemoveAllListeners();
        }

        public override void DoTick()
        {
            base.DoTick();
            inputField.interactable = showingNotes;
            lerpers.ForEach(x => x.Send(!showingNotes));
        }

        public void ToggleNotepad() 
        {
            showingNotes = !showingNotes;

            if (!showingNotes)
                eventSystem.SetSelectedGameObject(null);
        }
    }
}
