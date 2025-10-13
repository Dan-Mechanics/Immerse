using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Immerse
{
    public class HoverLerp : StateElement, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private List<Lerper> lerpers = default;
        private bool isHovering;

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovering = true;
        }

        public override void Open()
        {
            base.Open();
            lerpers.ForEach(x => x.Force());
        }

        public override void DoTick()
        {
            base.DoTick();
            lerpers.ForEach(x => x.Send(!isHovering));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovering = false;
        }
    }
}
