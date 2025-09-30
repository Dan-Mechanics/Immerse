using System;
using UnityEngine;

namespace Immerse
{
    public class Timer : Behaviour
    {
        public event Action<int, int> OnNewTime;

        [SerializeField, Min(0.01f)] private float invokeInterval = default;
        private DateTime startingPoint;

        public void Begin() 
        {
            startingPoint = DateTime.Now;
            EnterState();
        }

        public override void EnterState()
        {
            InvokeRepeating(nameof(Tick), 0f, invokeInterval);
        }

        public override void ExitState()
        {
            CancelInvoke(nameof(Tick));
        }

        private void Tick()
        {
            TimeSpan timeSpan = DateTime.Now - startingPoint;
            OnNewTime?.Invoke((int)timeSpan.TotalMinutes, (int)timeSpan.TotalSeconds);
        }
    }
}
