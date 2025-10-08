using System;
using UnityEngine;

namespace Immerse
{
    public class Scanner : MonoBehaviour
    {
        public event Action<string> OnScanString;
        public event Action<int> OnScanInt;

        private void Update()
        {
            for (int i = 1; i < 10; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                    OnScanInt?.Invoke(i - 1);
            }

            /*if (Input.GetKeyDown(KeyCode.Alpha1))
                OnNewScan?.Invoke("accountant");

            if (Input.GetKeyDown(KeyCode.Alpha2))
                OnNewScan?.Invoke("janitor");

            if (Input.GetKeyDown(KeyCode.Alpha3))
                OnNewScan?.Invoke("Intro");*/
        }
    }
}
