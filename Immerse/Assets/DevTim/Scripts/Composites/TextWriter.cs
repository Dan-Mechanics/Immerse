using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

namespace Immerse
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextWriter : Behaviour
    {
        public const float INTERVAL = 0.05f;

        [SerializeField] private bool simpleReadFromSetup = default;

        private readonly StringBuilder builder = new StringBuilder();
        private WaitForSeconds delay;
        private TMP_Text text;

        private string currentlyWritingMessage;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
            delay = new WaitForSeconds(INTERVAL);
        }

        private void Start()
        {
            if (simpleReadFromSetup)
            {
                Debug.LogWarning($"Writing '{text.text}' on {gameObject.name}.");
                Write(text.text);
                return;
            }

            SetToStartingPoint();
        }

        public override void ExitState()
        {
            base.ExitState();
            SetToStartingPoint();
        }

        /// <summary>
        /// Overrides previous writing.
        /// </summary>
        public void Write(string message)
        {
            if (!gameObject.activeInHierarchy)
            {
                Debug.LogWarning($"Tried to write '{message}' on {gameObject.name} while it was inactive ...");
                return;
            }

            SetToStartingPoint();

            if (string.IsNullOrEmpty(message))
                return;

            if (string.IsNullOrWhiteSpace(message))
                return;

            currentlyWritingMessage = message;
            StartCoroutine(WriteDelayed());
        }

        private void SetToStartingPoint()
        {
            StopAllCoroutines();
            builder.Clear();
            currentlyWritingMessage = string.Empty;
            text.text = string.Empty;
        }

        private IEnumerator WriteDelayed()
        {
            for (int i = 0; i < currentlyWritingMessage.Length; i++)
            {
                yield return delay;
                builder.Append(currentlyWritingMessage[i]);
                text.text = builder.ToString();
            }

            builder.Clear();
            text.text = currentlyWritingMessage;
            currentlyWritingMessage = string.Empty;
        }
    }
}
