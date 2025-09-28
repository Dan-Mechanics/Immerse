using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    /// <summary>
    /// Take blame input and read from timer 
    /// to force final blame and all that.
    /// </summary>
    public class GameStateHandler : MonoBehaviour
    {
        [SerializeField] private Timer timer = default;
        [SerializeField] private float forceBlameTimeMinutes = default;
        [SerializeField] private Blame blame = default;

        private void FixedUpdate()
        {
            if (timer.TotalMinutes < forceBlameTimeMinutes)
                return;

            blame.ForceBlame();
        }
    }
}
