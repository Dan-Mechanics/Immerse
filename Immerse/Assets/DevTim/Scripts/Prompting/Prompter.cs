using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public partial class Prompter : StateElement
    {
        public event Action<int> OnAnswer;
        
        [SerializeField] private Transform promptHolder = default;
        [SerializeField] private AudioSource source = default;
        [SerializeField] private GameObject promptPrefab = default;
        [SerializeField] private TMP_Text questionText = default;
        [SerializeField] private float verticalSpacing = default;

        private readonly List<StateElement> promptBehaviours = new List<StateElement>();
        private readonly List<GameObject> spawned = new List<GameObject>();
        private Keyboard keyboard;
        private int optionsLength;

        public bool DisplayQuestion(Question question) 
        {
            DestroyPrompts();

            questionText.text = question.question;
            source.Stop();
            if (question.clip != null)
                source.PlayOneShot(question.clip);

            for (int i = 0; i < question.options.Length; i++)
            {
                if (question.includeOptional)
                {
                    SpawnOption(question.options[i], i, question);
                }
                else
                {
                    if (!question.options[i].optional)
                        SpawnOption(question.options[i], i, question);
                }
            }

            promptBehaviours.ForEach(x => x.Open());

            optionsLength = question.options.Length;
            keyboard = new Keyboard();
            return true;
        }

        private void GiveAnswer(int answer) 
        {
            if (answer < 0)
                return;

            if (answer >= optionsLength)
                return;
            
            keyboard = null;
            OnAnswer?.Invoke(answer);
        }

        public override void Close()
        {
            base.Close();
            DestroyPrompts();
            source.Stop();
        }

        public override void DoFrame()
        {
            base.DoFrame();
            
            if (keyboard != null)
                GiveAnswer(keyboard.GetPressedLetterIndex());

            promptBehaviours.ForEach(x => x.DoFrame());
        }

        public override void DoTick()
        {
            base.DoTick();
            promptBehaviours.ForEach(x => x.DoTick());
        }

        private void SpawnOption(Option option, int i, Question question)
        {
            GameObject go = Instantiate(promptPrefab, promptHolder);
            RectTransform rect = go.GetComponent<RectTransform>();
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            rect.anchoredPosition = Vector2.up * verticalSpacing + (i * verticalSpacing * Vector2.down);

            go.GetComponent<Image>().color = option.color;
            if (question.saturation > 0f && i < question.processLength)
            {
                float lerp = 0f;
                if (question.options.Length > 1)
                    lerp = (float)i / (question.options.Length - 1);

                go.GetComponent<Image>().color = Color.Lerp(option.color, Color.Lerp(question.a, question.b, lerp), question.saturation);
            }

            go.GetComponentInChildren<TMP_Text>().text = Utils.alphabet[i].ToString().ToUpperInvariant() + ": " + option.text;
            go.GetComponentsInChildren<Image>()[1].sprite = option.icon;
            go.GetComponent<Button>().onClick.AddListener(delegate { GiveAnswer(i); });
            go.GetComponent<Lerper>().Send(false);

            promptBehaviours.AddRange(go.GetComponentsInChildren<StateElement>());

            spawned.Add(go);
        }

        /// <summary>
        /// Remove all previous.
        /// </summary>
        private void DestroyPrompts() 
        {
            OnAnswer?.GetInvocationList().ToList().ForEach(x => OnAnswer -= (Action<int>)x);
            keyboard = null;

            foreach (GameObject go in spawned)
            {
                go.GetComponent<Button>().onClick.RemoveAllListeners();
                Destroy(go);
            }

            promptBehaviours.Clear();
            spawned.Clear();
        }
    }
}
