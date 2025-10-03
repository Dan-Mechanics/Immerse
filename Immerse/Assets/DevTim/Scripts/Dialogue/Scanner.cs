using System;
using UnityEngine;
// using UnityEngine.Windows.Speech; 

namespace Immerse
{
    /// <summary>
    /// If bjdr8 could implement this that would be good.
    /// </summary>
    public class Scanner : MonoBehaviour
    {
        /// <summary>
        /// I would prefer if the int inputs
        /// would be first translated to names
        /// but it would work either way.
        /// </summary>
        public event Action<string> OnNewScan;

        /// <summary>
        /// Example code for invoking OnNewScan.
        /// </summary>
        private void Update()
        {   
            if (!Input.GetKey(KeyCode.Mouse3) && !Input.GetKey(KeyCode.Mouse4))
                return;

            if (Input.GetKeyDown(KeyCode.Alpha1))
                OnNewScan?.Invoke("accountant");

            if (Input.GetKeyDown(KeyCode.Alpha2))
                OnNewScan?.Invoke("janitor");

            if (Input.GetKeyDown(KeyCode.Alpha3))
                OnNewScan?.Invoke("Intro");
        }
    }
}
