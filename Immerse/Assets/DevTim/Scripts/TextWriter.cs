using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

namespace Immerse
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextWriter : MonoBehaviour, IInvoker, IReceiver<string>
    {
        public event Action OnInvoke;
        private const float INTERVAL = 0.05f;
        
        private readonly StringBuilder builder = new StringBuilder();
        private WaitForSeconds delay;
        private TMP_Text text;
        private string message;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
            text.text = string.Empty;
            delay = new WaitForSeconds(INTERVAL);
        }

        private IEnumerator WriteDelayed() 
        {
            for (int i = 0; i < message.Length; i++)
            {
                builder.Append(message[i]);
                text.text = builder.ToString();
                yield return delay;
            }

            builder.Clear();
            text.text = message;
            OnInvoke?.Invoke();
        }

        public void Write(string message)
        {
            this.message = message;
            builder.Clear();
            StopAllCoroutines();
            
            if(message == string.Empty)
            {
                text.text = message;
                OnInvoke?.Invoke();
                return;
            }
            
            StartCoroutine(WriteDelayed());
        }

        public void Send(string value) => Write(value);
    }
}
