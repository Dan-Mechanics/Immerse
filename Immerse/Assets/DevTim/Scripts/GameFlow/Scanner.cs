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

        /// <summary>
        /// ALL OF THIS IS FOR DEBUG !!
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                OnScanString?.Invoke("jeremy");
                return;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                OnScanString?.Invoke("marc");
                return;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnScanString?.Invoke("leonardo");
                return;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnScanString?.Invoke("vivienne");
                return;
            }

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
