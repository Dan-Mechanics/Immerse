using UnityEngine;

namespace Immerse
{
    public class Keyboard
    {
        public int GetPressedLetterIndex()
        {
            for (int i = 0; i < Utils.alphabet.Length; i++)
            {
                if (Input.GetKeyDown(Utils.alphabet[i].ToString()))
                    return i;
            }

            return -1;
        }

        public int GetPressedNumberIndex()
        {
            for (int i = 1; i < 10; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                    return i - 1;
            }

            return -1;
        }
    }
}
