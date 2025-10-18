using System;
using UnityEngine;

namespace Immerse
{
    public class Scanner : MonoBehaviour
    {
        public event Action<string> OnScanString;
        public event Action<int> OnScanInt;

        [SerializeField] private Holder holder = default;
        private readonly Keyboard keyboard = new Keyboard();

        private void Update()
        {
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

            OnScanString?.Invoke(holder.Dialogue[number].name);
        }
    }
}
