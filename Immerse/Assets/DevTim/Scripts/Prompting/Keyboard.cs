using UnityEngine;

namespace Immerse
{
    public class Keyboard
    {
        private readonly char[] alpha = { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };

        public int Update(char[] alpha)
        {
            for (int i = 0; i < alpha.Length; i++)
            {
                if (Input.GetKeyDown(alpha[i].ToString()))
                    return i;
            }

            return -1;
        }
    }
}
