using System;
using UnityEngine;
using TMPro;

namespace Immerse
{
    public class Timer : StateElement
    {
        public event Action<int, int> OnNewTime;
        public event Action OnDone;

        [SerializeField] private float doneMinutes = default;
        [SerializeField] private float doneSeconds = default;
        [SerializeField] private TMP_Text doneWhenOutput = default;

        [SerializeField, Min(0.01f)] private float invokeInterval = default;
        private DateTime startingPoint;

        private void Awake()
        {
            if (doneWhenOutput == null)
                return;

            doneWhenOutput.text = $"{doneMinutes}:{doneSeconds}";
        }

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
            int min = timeSpan.Minutes;
            int sec = timeSpan.Seconds;

            OnNewTime?.Invoke(min, sec);
            if (min >= doneMinutes && sec >= doneSeconds)
            {
                OnDone?.Invoke();
                Close();
            }
        }
    }
}
