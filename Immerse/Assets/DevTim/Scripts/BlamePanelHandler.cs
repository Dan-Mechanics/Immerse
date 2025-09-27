using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    /// <summary>
    /// This is also where i wanna add the setup behaviour or i should call that Grid.cs
    /// </summary>
    public class BlamePanelHandler : MonoBehaviour
    {
        [SerializeField] private List<Button> buttons = default;
        [SerializeField] private GameObject panel = default;

        private List<IReceiver<bool>> listeners;
        private bool isBlaming;

        private void Awake()
        {
            listeners = panel.GetComponents<IReceiver<bool>>().ToList();
        }

        private void FixedUpdate()
        {
            listeners.ForEach(x => x.Send(!isBlaming));
            buttons.ForEach(x => x.interactable = isBlaming);
        }

        public void ToggleBlamePanel()
        {
            isBlaming = !isBlaming;
        }
    }
}
