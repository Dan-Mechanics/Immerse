using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    /// <summary>
    /// This might possibly be the place to translate raw data
    /// itno usable data.
    /// 
    /// You can also call this ScannerBridge or something.
    /// </summary>
    public class ScannerResponse : MonoBehaviour
    {
        [SerializeField] private DialogueEventHolder holder = default;
        [SerializeField] private Scanner scanner = default;
        [SerializeField] private DialogueEventDisplayer displayer = default;

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
