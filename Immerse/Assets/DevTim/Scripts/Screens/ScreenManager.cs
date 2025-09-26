using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] private List<Screen> screens = default;
        [SerializeField] private GameObject startingScreen = default;

        private Screen current;

        private void Start()
        {
            screens.ForEach(x => x.Exit());
            Open(startingScreen);
        }

        public void Open(GameObject go)
        {
            current?.Exit();
            current = go.GetComponent<Screen>();
            current.Enter();
        }

        private void Update()
        {
            current?.DoFrame();
        }

        private void FixedUpdate()
        {
            current?.DoTick();
        }
    }
}
