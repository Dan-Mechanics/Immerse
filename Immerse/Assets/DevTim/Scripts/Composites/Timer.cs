using System;
using UnityEngine;

namespace Immerse
{
    public class Timer : MonoBehaviour
    {
        public float TotalMinutes { get; private set; }
        [SerializeField] private GameObject listener = default;

        private IReceiver<object> output;
        private DateTime startingPoint;

        private void Awake()
        {
            output = listener.GetComponent<IReceiver<object>>();
        }

        private void OnEnable()
        {
            startingPoint = DateTime.Now;
            CancelInvoke(nameof(Tick));
            InvokeRepeating(nameof(Tick), 0f, 1.1f);
        }

        private void Tick()
        {
            TimeSpan timeSpan = DateTime.Now - startingPoint;
            output.Send($"{timeSpan.Minutes}:{timeSpan.Seconds}");
            TotalMinutes = (float)timeSpan.TotalMinutes;
        }
    }
}
