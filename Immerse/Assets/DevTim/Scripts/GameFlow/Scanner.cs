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
                OnScanInt?.Invoke(0);

            if (Input.GetKeyDown(KeyCode.DownArrow))
                OnScanInt?.Invoke(1);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                OnScanInt?.Invoke(2);

            if (Input.GetKeyDown(KeyCode.RightArrow))
                OnScanInt?.Invoke(3);

            if (Input.GetKeyDown(KeyCode.Space))
                OnScanInt?.Invoke(4);

            int number = keyboard.GetPressedNumberIndex();
            if (number < 0)
                return;

            OnScanString?.Invoke(holder.Dialogue[number].name);
        }

        public void Jeremy()
        {
            OnScanString?.Invoke("jeremy");
        }

        public void Marc()
        {
            OnScanString?.Invoke("marc");
        }

        public void Leonardo()
        {
            OnScanString?.Invoke("leonardo");
        }

        public void Vivienne()
        {
            OnScanString?.Invoke("vivienne");
        }
    }
}
