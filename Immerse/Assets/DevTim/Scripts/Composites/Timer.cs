using System;
using UnityEngine;

namespace Immerse
{
    public class Timer : State
    {
        public event Action<TimeSpan> OnNewTime;

        [SerializeField, Min(0.01f)] private float interval = default;
        private DateTime startingPoint;

        public void Begin() 
        {
            startingPoint = DateTime.Now;
            EnterState();
        }

        public override void EnterState()
        {
            InvokeRepeating(nameof(Tick), 0f, interval);
        }

        public override void ExitState()
        {
            CancelInvoke(nameof(Tick));
        }

        private void Tick()
        {
            TimeSpan timeSpan = DateTime.Now - startingPoint;
            OnNewTime?.Invoke(timeSpan);
        }
    }
}
