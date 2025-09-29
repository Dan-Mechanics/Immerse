using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Immerse
{
    /// <summary>
    /// Take blame input and read from timer 
    /// to force final blame and all that.
    /// </summary>
    public class GameStateHandler : MonoBehaviour
    {
        [SerializeField] private List<State> states = default;
        
        
        public event Action<string> OnStart;
        
        [SerializeField] private Timer timer = default;
        [SerializeField] private Blame blame = default;
        [SerializeField] private float forceBlameTimeMinutes = default;
        [SerializeField] private List<GameObject> destroyOnGameOver = default;
        [SerializeField] private UnityEvent blamedCorrectly = default;
        [SerializeField] private UnityEvent blamedIncorrectly = default;

        private bool won;
        private bool gameOver;
        private bool hasStarted;

        private void FixedUpdate()
        {
            if (gameOver)
                return;
            
            if (timer.TotalMinutes < forceBlameTimeMinutes)
                return;

            destroyOnGameOver.ForEach(x => Destroy(x));
            gameOver = true;
            blame.ForceBlame();
        }

        public void CheckGameOver() 
        {
            if (!hasStarted)
            {
                OnStart?.Invoke("Intro");
                hasStarted = true;
            }

            if (!gameOver)
                return;

            (won ? blamedCorrectly : blamedIncorrectly)?.Invoke();
        }

        public void SetWon(bool won) => this.won = won;
    }
}
