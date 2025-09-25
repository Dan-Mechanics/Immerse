using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public class BlameButton : MonoBehaviour
    {
        public bool isBlaming;

        [SerializeField] private RectLerper rectLerper = default;
        [SerializeField] private ScaleLerper scaleLerper = default;
        [SerializeField] private List<Button> buttons = default;

        private void FixedUpdate()
        {
            rectLerper.showingSecondPosition = isBlaming;
            scaleLerper.showingSecondScale = isBlaming;
            buttons.ForEach(x => x.interactable = isBlaming);
        }

        public void ToggleNotepad()
        {
            isBlaming = !isBlaming;
        }
    }
}
