using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Immerse
{
    public class NotepadHandler : StateElement
    {
        public Action OnOpen;
        public Action OnClose;
        
        [SerializeField] private TMP_InputField inputField = default;
        [SerializeField] private EventSystem eventSystem = default;
        [SerializeField] private Button toggleNotepadButton = default;
        [SerializeField] private KeyCode showKey = default;
        [SerializeField] private KeyCode hidekey = default;
        [SerializeField] private List<Lerper> lerpers = default;

        private bool showingNotes;

        public override void Open()
        {
            base.Open();
            toggleNotepadButton.onClick.AddListener(ToggleNotepad);
            SetVisible(false);
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

        public override void DoFrame()
        {
            base.DoFrame();
            if (Input.GetKeyDown(showKey))
                SetVisible(true);

            if (Input.GetKeyDown(hidekey))
                SetVisible(false);
        }

        public void ToggleNotepad() => SetVisible(!showingNotes);

        private void SetVisible(bool visible)
        {
            if (showingNotes != visible)
                (showingNotes ? OnOpen : OnClose)?.Invoke();

            showingNotes = visible;

            if (!showingNotes)
                eventSystem.SetSelectedGameObject(null);
        }
    }
}
