using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Immerse
{
    public class NotepadHandler : MonoBehaviour
    {
        public bool showingNotes;

        [SerializeField] private TMP_InputField inputField = default;
        [SerializeField] private List<GameObject> gameObjectListeners = default;
        //[SerializeField] private ScaleLerper scaleLerper = default;
        [SerializeField] private float lerpSpeed = default;

        private readonly List<IReceiver<bool>> listeners = new List<IReceiver<bool>>();

        private void Awake()
        {
            gameObjectListeners.ForEach(x => listeners.Add(x.GetComponent<IReceiver<bool>>()));
        }

        private void FixedUpdate()
        {
            listeners.ForEach(x => x.Send(showingNotes));
            inputField.interactable = showingNotes;
        } 

        public void ToggleNotepad() 
        {
            showingNotes = !showingNotes;
        }
    }
}
