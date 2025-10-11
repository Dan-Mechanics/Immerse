using System;
using UnityEngine;

namespace Immerse
{
    public class Scanner : MonoBehaviour
    {
        public event Action<string> OnScanString;
        public event Action<int> OnScanInt;

        private readonly Keyboard keyboard = new Keyboard();

        private void Update()
        {
            int number = keyboard.GetPressedNumberIndex();
            if (number < 0)
                return;

            OnScanInt?.Invoke(number);

            /*if (Input.GetKeyDown(KeyCode.Alpha1))
                OnNewScan?.Invoke("accountant");

            if (Input.GetKeyDown(KeyCode.Alpha2))
                OnNewScan?.Invoke("janitor");

            if (Input.GetKeyDown(KeyCode.Alpha3))
                OnNewScan?.Invoke("Intro");*/
        }
    }
}
