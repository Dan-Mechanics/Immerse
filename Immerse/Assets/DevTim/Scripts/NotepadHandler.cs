using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Immerse
{
    public class NotepadHandler : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField = default;
        [SerializeField] private EventSystem eventSystem = default;
        [SerializeField] private List<GameObject> interested = default;

        private readonly List<IReceiver<bool>> listeners = new List<IReceiver<bool>>();
        private bool showingNotes;

        private void Awake()
        {
            interested.ForEach(x => listeners.Add(x.GetComponent<IReceiver<bool>>()));
        }

        private void FixedUpdate()
        {
            listeners.ForEach(x => x.Send(!showingNotes));
            inputField.interactable = showingNotes;
        } 

        public void ToggleNotepad() 
        {
            showingNotes = !showingNotes;

            if (!showingNotes)
                eventSystem.SetSelectedGameObject(null);
        }
    }
}
