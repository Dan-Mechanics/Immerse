using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    /// <summary>
    /// Or we use indices but it all depends on what the output is from the scanner basically.
    /// Doesnt really matter.
    /// 
    /// If we use ints as ID then this might be the place where you need to convert index to thing.
    /// </summary>
    public class ScannerResponse : MonoBehaviour
    {
        [SerializeField] private DialogueEventHolder holder = default;
        
        /// <summary>
        /// Input.
        /// </summary>
        [SerializeField] private Scanner scanner = default;

        /// <summary>
        /// Output.
        /// </summary>
        [SerializeField] private DialogueEventDisplayer displayer = default;

        // This is good too:
        //private readonly Dictionary<int, DialogueEvent> dict = new Dictionary<int, DialogueEvent>();

        private void Awake()
        {
            scanner.OnNewScan += OnNewScan;
        }

        private void OnNewScan(string name)
        {
            if (holder.Has(name))
                displayer.Display(holder.GetByName(name));
        }
    }
}
