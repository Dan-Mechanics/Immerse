using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Immerse
{
    public class Timer : MonoBehaviour
    {
        public event Action OnDone;
        
        [SerializeField] private UnityEvent onDone = default;
        [SerializeField] private float doneMinutes = default;
        [SerializeField] private GameObject listener = default;

        private IReceiver<string> writer;
        private DateTime startingPoint;

        private void Awake()
        {
            writer = listener.GetComponent<IReceiver<string>>();
        }

        private void Start()
        {
            startingPoint = DateTime.Now;
            InvokeRepeating(nameof(Tick), 0f, 1.1f);
        }

        private void Tick()
        {
            TimeSpan timeSpan = DateTime.Now - startingPoint;
            writer.Send($"{timeSpan.Minutes}:{timeSpan.Seconds}");

            if(timeSpan.TotalMinutes >= doneMinutes)
            {
                onDone?.Invoke();
                OnDone?.Invoke();
            }
        }
    }
}
