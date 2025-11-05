using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

namespace Immerse
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextWriter : StateElement
    {
        public const float INTERVAL = 0.06f;

        [SerializeField] private string startingMessage = default;

        private readonly StringBuilder builder = new StringBuilder();
        private WaitForSeconds delay;
        private TMP_Text text;
        private string message;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
            delay = new WaitForSeconds(INTERVAL);
        }

        public override void Close()
        {
            base.Close();
            SetToStartingPoint();
        }

        public override void Open()
        {
            base.Open();

            if (!string.IsNullOrEmpty(startingMessage) && !string.IsNullOrWhiteSpace(startingMessage))
                Write(startingMessage);
        }

        /// <summary>
        /// Overrides previous writing.
        /// </summary>
        public void Write(string message)
        {
            if (!gameObject.activeInHierarchy)
                return;

            SetToStartingPoint();

            if (string.IsNullOrEmpty(message))
                return;

            if (string.IsNullOrWhiteSpace(message))
                return;

            this.message = message;
            gameObject.name = message;
            StartCoroutine(WriteDelayed());
        }

        private void SetToStartingPoint()
        {
            StopAllCoroutines();
            builder.Clear();
            message = string.Empty;
            text.text = message;
        }

        private IEnumerator WriteDelayed()
        {
            for (int i = 0; i < message.Length; i++)
            {
                yield return delay;
                builder.Append(message[i]);
                text.text = builder.ToString();
            }

            builder.Clear();
            text.text = message;
            message = string.Empty;
        }

        public void SetColor(Color color) => text.color = color;
        public void SetStartupMessage(string startingMessage) => this.startingMessage = startingMessage;
    }
}
