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
            // YANDERE-DEV IS THAT YOU ?
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                OnScanString("jeremy");
                return;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                OnScanString("marc");
                return;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnScanString("leonardo");
                return;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnScanString("laura");
                return;
            }

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
