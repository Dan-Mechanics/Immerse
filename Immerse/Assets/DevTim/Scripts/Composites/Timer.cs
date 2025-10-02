using System;
using UnityEngine;

namespace Immerse
{
    public class Timer : StateElement
    {
        public event Action<int, int> OnNewTime;

        [SerializeField, Min(0.01f)] private float invokeInterval = default;
        private DateTime startingPoint;

        public void Begin() 
        {
            startingPoint = DateTime.Now;
            Open();
        }

        public override void Open()
        {
            InvokeRepeating(nameof(Tick), 0f, invokeInterval);
        }

        public override void Close()
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
