using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    /// <summary>
    /// If bjdr8 could implement this that would be good.
    /// </summary>
    public class Scanner : MonoBehaviour
    {
        /// <summary>
        /// Or make this an int, it doesn't really matter.
        /// The string here is the name of the thing being scanned.
        /// </summary>
        public event Action<string> OnNewScan;

        // Code here that invokes OnNewScan.

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha2))
                OnNewScan?.Invoke("Intro");

            if (Input.GetKeyDown(KeyCode.Alpha3))
                OnNewScan?.Invoke("Accountant_interview");

            if (Input.GetKeyDown(KeyCode.Alpha4))
                OnNewScan?.Invoke("Janitor_interview");
        }
    }
}
