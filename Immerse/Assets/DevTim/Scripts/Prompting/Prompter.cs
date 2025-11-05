using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public class Prompter : StateElement
    {
        [SerializeField] private GameObject gameplayState = default;
        [SerializeField] private GameObject promptState = default;
        [SerializeField] private StateHandler stateHandler = default;

        [SerializeField] private Transform promptHolder = default;
        [SerializeField] private AudioSource source = default;
        [SerializeField] private GameObject promptPrefab = default;
        [SerializeField] private TMP_Text questionText = default;
        [SerializeField] private float verticalSpacing = default;

        private readonly List<StateElement> promptBehaviours = new List<StateElement>();
        private readonly List<GameObject> spawned = new List<GameObject>();
        private readonly Keyboard keyboard = new Keyboard();

        private Current current;

        private class Current
        {
            public IAnswerListener listener;
            public Question question;
        }

        /// <summary>
        /// Init prompt.
        /// </summary>
        public void Ask(Question question, IAnswerListener listener) 
        {
            if (question == null || listener == null)
                return;

            stateHandler.Open(promptState);
            DestroyPrompts();
            source.Stop();

            current = new Current() { listener = listener, question = question };

            questionText.text = question.message;
            if (question.clip != null)
                source.PlayOneShot(question.clip);

            for (int i = 0; i < question.options.Length; i++)
            {
                SpawnOption(question.options[i], i, question.includeOptional || !question.options[i].optional);

                /*if (question.includeOptional || !question.options[i].optional)
                    SpawnOption(question.options[i], i);*/
            }

            promptBehaviours.ForEach(x => x.Open());
        }

        private void GetAnswer(int answer) 
        {
            if (answer < 0)
                return;

            if (current == null)
                return;

            if (answer >= current.question.options.Length)
                return;

            // THIS IS HACKEY
            if (!spawned[answer].GetComponent<Button>().interactable)
                return;

            // NOTE: THE ORDER OF ALL THIS IS VERY IMPORTANT.
            stateHandler.Open(gameplayState);
            current.listener?.GetAnswer(answer, current.question.options[answer]);
            current = null;
        }

        public override void Close()
        {
            base.Close();

            //current = null;
            DestroyPrompts();
            source.Stop();
        }

        public override void DoFrame()
        {
            base.DoFrame();

            if (current == null)
                return;

            GetAnswer(keyboard.GetPressedLetterIndex());
            promptBehaviours.ForEach(x => x.DoFrame());
        }

        public override void DoTick()
        {
            base.DoTick();
            promptBehaviours.ForEach(x => x.DoTick());
        }

        private void SpawnOption(Option option, int index, bool interactable)
        {
            GameObject go = Instantiate(promptPrefab, promptHolder);
            RectTransform rect = go.GetComponent<RectTransform>();
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            rect.anchoredPosition = Vector2.up * verticalSpacing + (index * verticalSpacing * Vector2.down);

            go.GetComponent<Image>().color = option.color;
            go.GetComponentInChildren<TMP_Text>().text = Utils.alphabet[index].ToString().ToUpperInvariant() + ": " + option.text;
            go.GetComponentsInChildren<Image>()[1].sprite = option.icon;
            go.GetComponent<Button>().onClick.AddListener(delegate { GetAnswer(index); });
            go.GetComponent<Button>().interactable = interactable;
            go.GetComponent<Lerper>().Send(false);

            promptBehaviours.AddRange(go.GetComponentsInChildren<StateElement>());

            spawned.Add(go);
        }

        /// <summary>
        /// Remove all previous.
        /// </summary>
        private void DestroyPrompts() 
        {
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
