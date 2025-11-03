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
            /*
            Up = handcuffs
            Down = computer
            Left = energy blikjes
            Right = takenlijst
            Space = poster Leonardo
            */

            if (Input.GetKeyDown(KeyCode.UpArrow))
                InteractWithObject(0);

            if (Input.GetKeyDown(KeyCode.DownArrow))
                InteractWithObject(1);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                InteractWithObject(2);

            if (Input.GetKeyDown(KeyCode.RightArrow))
                InteractWithObject(3);

            if (Input.GetKeyDown(KeyCode.Space))
                InteractWithObject(4);

            InteractWithActor(keyboard.GetPressedNumberIndex());
        }

        private void InteractWithObject(int index)
        {
            if (index < 0 || index > holder.Dialogue.Count - 1)
                return;

            OnScanString?.Invoke(holder.Dialogue[index].name);
        }

        private void InteractWithActor(int index)
        {
            if (index < 0 || index > holder.Actors.Count - 1)
                return;

            OnScanString?.Invoke(holder.Actors[index].name);
        }

        public void Jeremy()
        {
            //OnScanString?.Invoke(holder.Actors[0].name);
            OnScanString?.Invoke("jeremy");
        }

        public void Marc()
        {
            //OnScanString?.Invoke(holder.Actors[1].name);
            OnScanString?.Invoke("marc");
        }

        public void Leonardo()
        {
            //OnScanString?.Invoke(holder.Actors[2].name);
            OnScanString?.Invoke("leonardo");
        }

        public void Vivienne()
        {
          //  OnScanString?.Invoke(holder.Actors[3].name);
            OnScanString?.Invoke("vivienne");
        }
    }
}
